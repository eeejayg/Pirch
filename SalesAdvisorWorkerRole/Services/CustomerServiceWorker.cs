using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using SalesAdvisorSharedClasses.Communication;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWorkerRole.Adapters;
using SalesAdvisorWorkerRole.MessageHandlers;

namespace SalesAdvisorWorkerRole.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single
        )]
    class CustomerServiceWorker : CustomerService
    {
        private SqlConnection getConnection()
        {
            return DBAdapter.getInstance().getConnection();
        }


        //========================================================================================
        // This holds Customers and Customer related items for the service.  The primary DB objects it 
        // interacts with are..
        //  CUSTOMER
        //  ADDRESS
        //========================================================================================
        public enum AddressOwner
        {
            Customer = 1,
            Company = 2
        }




        /// <summary>
        /// Pass this an address object and it will create or update the address.
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public int AddressCreateOrUpdate(Address address)
        {
            if (address.City == null && address.AddressLine1 == null)
            {
                return -1;
            }
            using (var connection = this.getConnection())
            {
                DynamicParameters ParamsForProc = new DynamicParameters();
                ParamsForProc.Add("@AddressLine1", address.AddressLine1);
                ParamsForProc.Add("@AddressLine2", address.AddressLine2);
                ParamsForProc.Add("@AddressLine3", address.AddressLine3);
                ParamsForProc.Add("@PostalCode", address.PostalCode);
                ParamsForProc.Add("@City", address.City);
                ParamsForProc.Add("@State", address.State);
                ParamsForProc.Add("@Country", address.Country);
                if (address.Id != 0)
                {
                    ParamsForProc.Add("@Id", address.Id);
                    connection.Execute("Address_Update", ParamsForProc, commandType: CommandType.StoredProcedure);
                    return address.Id;                
                
                }
                else
                {
                    ParamsForProc.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
                    connection.Execute("Address_Create", ParamsForProc, commandType: CommandType.StoredProcedure);
                    int NewAddressId = ParamsForProc.Get<int>("@newid");
                    return NewAddressId;  
                }
            }
        }

        /// <summary>
        /// This creates the record that ties a customer to an address.  It is neither the customer
        /// nor the address record, it just ties the two together.
        //  We already assume that the record needs to be created.  
        //  -TODO force this to be unique in the DB as a double-check.  Alpha is not time for double-check.
        /// </summary>
        /// <param name="CustomerId">Customer Id</param>
        /// <param name="AddressId">Address Id</param>
        /// <returns></returns>
        public int CustomerConnectWithAddress(int CustomerId, int AddressId)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters ParamsForProc = new DynamicParameters();
                ParamsForProc.Add("@OwnerId", CustomerId);
                ParamsForProc.Add("@AddressId", AddressId);
                ParamsForProc.Add("@OrganizationId", (int)OrganizationTypes.Customer);
                ParamsForProc.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute("Address_ConnectWithOwner", ParamsForProc, commandType: CommandType.StoredProcedure);
                return ParamsForProc.Get<int>("@newid");
            }
        }




        /// <summary>
        /// Customers all have a single default shipping address stored as a foreign key on their record.
        /// This updates that foreign key.
        /// </summary>
        /// <param name="AddressId">Address ID</param>
        /// <param name="CustomerId">Customer ID</param>
        /// <returns></returns>
        public bool CustomerUpdateDefaultAddress(int AddressId, int CustomerId)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters ParamsForProc = new DynamicParameters();
                ParamsForProc.Add("@AddressId", AddressId);
                ParamsForProc.Add("@CustomerId", CustomerId);
                connection.Execute("Customer_UpdateDefaultAddress", ParamsForProc, commandType: CommandType.StoredProcedure);
                return true;
            }

        }


        public List<Customers> GetCustomerListBySalesAssociate(int Id)
        {
            List<Customers> CL = new List<Customers>();
            using (var connection = this.getConnection())
            {
                connection.Query<Customers, Project, Room, RoomsCategory, ProjectStatus, int>("CustomerList_ReadAll", map: (customer, project, room, rc, projectStatus) =>
                {
                    int CustomerIndex = CL.FindIndex(p => p.Id == customer.Id); // Check if our customer already exists in our list
                    if (CustomerIndex == -1)  //  They don't, so lets add the customer and collect the  index.
                    {
                        CL.Add(customer);
                        CustomerIndex = CL.FindIndex(p => p.Id == customer.Id);
                    }
                    // Project must have a project status
                    project.ProjectStatus = projectStatus;
                    int ProjectIndex = CL[CustomerIndex].ProjectIndexOrAdd(project);
                    if (ProjectIndex >= 0)  //  Only try and add rooms if there's Product rows being return.  The LEFT JOIN can create NULL
                    {
                        //rooms are optional...
                        if (room.Id != 0)
                        {
                            int RoomIndex = CL[CustomerIndex].Projects[ProjectIndex].RoomIndexOrAdd(room);
                            CL[CustomerIndex].Projects[ProjectIndex].Rooms[RoomIndex].RoomCategory = rc;
                            CL[CustomerIndex].Projects[ProjectIndex].ProjectStatus = projectStatus;
                        }
                    }
                    return 1;
                }, splitOn: "ProjectSplit,RoomSplit, RCSplit, PSSplit");
            }
            return CL;
        }


       

        //  Simple utility function for common customer params
        private DynamicParameters ConvertCustomerToParams(Customers customer)
        {
            DynamicParameters DynamicParameters = new DynamicParameters();
            DynamicParameters.Add("@FirstName", customer.FirstName);
            DynamicParameters.Add("@LastName", customer.LastName);
            DynamicParameters.Add("@Owner", customer.Owner);
            DynamicParameters.Add("@Architect", customer.Architect);
            DynamicParameters.Add("@Designer", customer.Designer);
            DynamicParameters.Add("@Builder", customer.Builder);
            DynamicParameters.Add("@Vendor", customer.Vendor);
            DynamicParameters.Add("@Realtor", customer.Realtor);
            DynamicParameters.Add("@CustomerGuid", customer.CustomerGuid);
            return DynamicParameters;
        }


        //  Creates a new customer
        /* Unused, CustomerUpdateOrAdd does what we want.
        public int CustomerCreate(Customers customer)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters ParamsForProc = ConvertCustomerToParams(customer);
                ParamsForProc.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute("Customer_Create", ParamsForProc, commandType: CommandType.StoredProcedure);
                return ParamsForProc.Get<int>("@newid");
            }
        }
         */

        /// <summary>
        ///     Updates an existing customer
        /// </summary>
        /// <param name="customer">Customer Object To Update</param>
        /// <returns>Returns TRUE</returns>
        public int CustomerUpdateOrAdd(Customers customer)
        {
            int newId = this.CustomerUpdateOrAddLocal(customer);
            // Put a customer create message into the queue
            if (customer.CustomerGuid == null)
            {
                // New customer!
                CustomerCreateHandler msg = new CustomerCreateHandler();
                msg.customer = customer;
                msg.EnqueueSelfAsMessage(WorkerRole.privateQueue);
            }
            else
            {
                CustomerUpdateHandler msg = new CustomerUpdateHandler();
                msg.customer = customer;
                msg.EnqueueSelfAsMessage(WorkerRole.privateQueue);
            }
            //
            return newId;
        }

        public int CustomerUpdateOrAddLocal(Customers customer)
        {
            using (var connection = this.getConnection())
            {
                if (customer.Id == 0)
                {

                    DynamicParameters ParamsForProc = ConvertCustomerToParams(customer);
                    ParamsForProc.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
                    connection.Execute("Customer_Create", ParamsForProc, commandType: CommandType.StoredProcedure);
                    customer.Id = ParamsForProc.Get<int>("@newid");
                }
                else
                {
                    DynamicParameters ParamsForProc = this.ConvertCustomerToParams(customer);
                    ParamsForProc.Add("@Id", customer.Id);
                    connection.Execute("Customer_Update", ParamsForProc, commandType: CommandType.StoredProcedure);
                }

                if (customer.Addresses != null)
                {
                    foreach (Address Addy in customer.Addresses)
                    {
                        int NewAddressId = this.AddressCreateOrUpdate(Addy);
                        //  We can use this key to see if this is a new address.  If its new, then we need 
                        //  to add a record connection the address to the customer
                        if (Addy.Id == 0)
                        {
                            Addy.Id = NewAddressId;
                            this.CustomerConnectWithAddress(customer.Id, NewAddressId);
                        }
                    }
                }
                if (customer.Emails != null)
                {
                    foreach (Emails Email in customer.Emails)
                    {
                        int NewEmailId = this.EmailCreateOrUpdate(Email, customer.Id);
                    }
                }
                if (customer.PhoneNumbers != null)
                {
                    foreach (PhoneNumbers PhoneNumber in customer.PhoneNumbers)
                    {
                        int NewPhoneNumber = this.PhoneNumberCreateOrUpdate(PhoneNumber, customer.Id);
                    }
                }

                return customer.Id;
            }
        }

        /// <summary>
        /// This function takes a stored procedure, likely "Customer_ReadBy{Identifier}" and returns the basic information (including contact info) for the customer.  
        /// </summary>
        /// <param name="StoredProcedure">Mandatory.  How do we want to collect our customer info?  Likely by GUID or ID</param>
        /// <param name="DynamicParameters">The dynamic parameters for the Proc.</param>
        /// <returns>Customer</returns>
        private Customers CustomersGetBasicInformationByProc(String StoredProcedure, DynamicParameters DynamicParameters)
        {
            Customers Customer = new Customers();
            using (var connection = this.getConnection())
            {
                connection.Query<Customers, Emails, PhoneNumbers, PhoneTypes, Address, int>(StoredProcedure, map: (customerFromDB, email, phone, phoneType, address) =>
                {
                    //Create our customer
                    if (Customer.Id == 0) { Customer = customerFromDB; }
                    Customer.PhoneNumberIndexOrAdd(phone);
                    Customer.EmailIndexOrAdd(email);
                    Customer.AddressIndexOrAdd(address);
                    return 1;
                }, param: DynamicParameters, commandType: CommandType.StoredProcedure, splitOn: "LastCustomer,LastEmail, LastPhoneNumber, LastPhoneType, LastAddress");
            }
            return Customer;
        }




        ///<summary>
        ///  <para>This collects a JSON object that can power the entire projects/customers section of the app. This includes
        ///  product, productInstances, etc... </para>
        ///</summary>
        ///
        ///  Unfortunately, dapper cannot handle more than seven different types.
        ///  I broke this up so we can build the JSON object with seperate queries.  This should make it
        ///  easier to understand and easier to get partial bits of data.
        public Customers CustomerGetById(int id)
        {
            Customers customerRet = new Customers();

            //  First, set our Dynamic Parameters for stored procedures...
            var DynamicParameters = new DynamicParameters();
            DynamicParameters.Add("@Id", id);

            // Now go get the basic customer information 
            Customers OurCustomer = this.CustomersGetBasicInformationByProc("Customer_ReadById", DynamicParameters);
            
            //  Now add in the project info
            OurCustomer = this.CustomersGetProjectInformationByProc("Projects_CollectByCustomerId", DynamicParameters, OurCustomer);

            // Finally, lets add in our rooms to the projects for this customer
            OurCustomer = this.CustomersGetProjectRoomInformationByProc("Rooms_CollectByCustomerId", DynamicParameters, OurCustomer);
            return OurCustomer;
        }

        public Customers CustomerGetByGUID(String CustomerGUID)
        {
            Customers customerRet = new Customers();


            //TODO make this block and the corresponding block in CustomerGetById DRYer
            //  First, set our Dynamic Parameters for stored procedures...
            var DynamicParameters = new DynamicParameters();
            DynamicParameters.Add("@CustomerGUID", CustomerGUID);

            // Now go get the basic customer information 
            Customers OurCustomer = this.CustomersGetBasicInformationByProc("Customer_ReadByGUID", DynamicParameters);

            //  Now add in the project info
            OurCustomer = this.CustomersGetProjectInformationByProc("Projects_CollectByCustomerGUID", DynamicParameters, OurCustomer);

            // Finally, lets add in our rooms to the projects for this customer
            OurCustomer = this.CustomersGetProjectRoomInformationByProc("Rooms_CollectByCustomerGUID", DynamicParameters, OurCustomer);
            return OurCustomer;
        }



        /// <summary>
        /// Takes an existing customer with projects and adds room data to the projects using the identified stored proc.  The stored proc is likely something like "Rooms_CollectByCustomer{Identifier}"
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="DynamicParameters"></param>
        /// <param name="Customer"></param>
        /// <returns></returns>
        private Customers CustomersGetProjectRoomInformationByProc(String StoredProcedure, DynamicParameters DynamicParameters, Customers Customer){
            using (var connection = this.getConnection())
            {
                //======================================================================================================
                //  Next, we add in Rooms Data
                //======================================================================================================
                connection.Query<Project, Room, RoomsCategory, int>(StoredProcedure, map: (project, room, rc) =>
                {
                    //  rooms Create and add children
                    int ProjectId = Customer.Projects.FindIndex(ListItem => ListItem.Id == project.Id);
                    int RoomIndex = Customer.Projects[ProjectId].RoomIndexOrAdd(room);
                    if (RoomIndex >= 0)
                    {
                        Customer.Projects[ProjectId].Rooms[RoomIndex].RoomCategory = rc;
                    }
                    return 1;
                }, param: DynamicParameters, commandType: CommandType.StoredProcedure, splitOn: "LastProject, LastRoom, LastRoomCategory");
                return Customer;
            }
        }


        /// <summary>
        /// Takes an existing customer and adds in project data (including nested product instances) using the identified stored proc.  Likely "Customer_ReadBy{Identifier}"
        /// </summary>
        /// <param name="StoredProcedure"></param>
        /// <param name="DynamicParameters"></param>
        /// <param name="Customer"></param>
        /// <returns></returns>
        private Customers CustomersGetProjectInformationByProc(String StoredProcedure, DynamicParameters DynamicParameters, Customers Customer)
        {
            if (Customer.Id == 0 && Customer.CustomerGuid == "")
            {
                throw new Exception("CustomersGetProjectInformationByProc requires an existing customer");
            }
            using (var connection = this.getConnection())
            {
                connection.Query<Project, ProjectType, ProductInstance, ProductTypeOptions, ProjectStatus, ProductInstanceAddOn, int>(StoredProcedure, map: (project, projectType, productInstance, productTypeOption, projectStatus, productAddOn) =>
                {
                    //  Project create and add children
                    int ProjectIndex = Customer.ProjectIndexOrAdd(project);
                    Customer.Projects[ProjectIndex].ProjectType = projectType;
                    Customer.Projects[ProjectIndex].ProjectStatus = projectStatus;

                    //  product related stuff
                    int ProductInstanceIndex = Customer.Projects[ProjectIndex].ProductInstancesIndexOrAdd(productInstance);
                    if (ProductInstanceIndex >= 0)
                    {
                        //  Do things that only make sense if the product is there...
                        Customer.Projects[ProjectIndex].ProductInstances[ProductInstanceIndex].ProductTypeOptionsIndexOrAdd(productTypeOption);
                        Customer.Projects[ProjectIndex].ProductInstances[ProductInstanceIndex].ProductAddOnIndexOrAdd(productAddOn);
                    }
                    return 1;
                }, param: DynamicParameters, commandType: CommandType.StoredProcedure, splitOn: "LastProject, LastProjectType, LastProductInstance, LastProductTypeOption, LastProjectStatus");
            }


            return Customer;
        }


        //===================================================================================================
        //   Emails
        //===================================================================================================
        public int EmailCreateOrUpdate(Emails email, int CustomerId)
        {
            if (CustomerId == 0)
            {
                return -1;
            }
            if (email.Address == null || email.Address == "")
            {
                return -1;
            }
            using (var connection = this.getConnection())
            {

                DynamicParameters ParamsForProc = new DynamicParameters();
                ParamsForProc.Add("@Address", email.Address);
                ParamsForProc.Add("@FKCustomer", CustomerId);
                if (email.Id == 0)
                {                   
                    ParamsForProc.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
                    connection.Execute("Email_Create", ParamsForProc, commandType: CommandType.StoredProcedure);
                    email.Id = ParamsForProc.Get<int>("@newid");
                    return email.Id;

                }
                ParamsForProc.Add("@Id", email.Id);
                connection.Execute("Email_UpdateById", ParamsForProc, commandType: CommandType.StoredProcedure);
                return email.Id; 
            }
        }


        //===================================================================================================
        //   PhoneNumbers
        //===================================================================================================
        public int PhoneNumberCreateOrUpdate(PhoneNumbers phoneNumber, int CustomerId)
        {
            if (CustomerId == 0)
            {
                return -1;
            }
            if (phoneNumber.PhoneNumber == null || phoneNumber.PhoneNumber == "")
            {
                return -1;
            }
            using (var connection = this.getConnection())
            {

                DynamicParameters ParamsForProc = new DynamicParameters();
                ParamsForProc.Add("@PhoneNumber", phoneNumber.PhoneNumber);
                ParamsForProc.Add("@FKCustomer", CustomerId);
                ParamsForProc.Add("@FKType", phoneNumber.FKType);
                if (phoneNumber.Id == 0)
                {
                    ParamsForProc.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
                    connection.Execute("PhoneNumber_Create", ParamsForProc, commandType: CommandType.StoredProcedure);
                    phoneNumber.Id = ParamsForProc.Get<int>("@newid");
                    return phoneNumber.Id;

                }
                ParamsForProc.Add("@Id", phoneNumber.Id);
                connection.Execute("PhoneNumber_UpdateById", ParamsForProc, commandType: CommandType.StoredProcedure);
                return phoneNumber.Id; 
            }
        }
    }
}
