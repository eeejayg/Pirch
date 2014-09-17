using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;

using Dapper;
using Microsoft.WindowsAzure.ServiceRuntime;
using SalesAdvisorSharedClasses.Communication;
using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorWebRole.Adapters
{
    /**
     * This class is being used as an abstraction layer.  All database calls should be here, so that if we want to use
     * something besides the worker role we only have to change this.
     */
    public class DBAdapter
    {
        private static DBAdapter instance = new DBAdapter();

        public static DBAdapter getInstance()
        {
            return instance;
        }

        // instance stuff

        private DBAdapter()
        {
        }

        /**
         * Gets the appropriate Sql connection. If you're not using Dapper, you need to open the connection yourself.
         */
        public SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(RoleEnvironment.GetConfigurationSettingValue("SADBConnectionString"));
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Execute a stored procedure that returns an Id
        /// </summary>
        /// <param name="proc">The stored procedure to execute</param>
        /// <param name="procParams">The proc parameters</param>
        /// <param name="returnName">the rreturn name of the </param>
        /// <returns></returns>
        public int StoredProcReturningId(string proc, Dictionary<string, object> procParams, string returnName)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                foreach (KeyValuePair<string, object> p in procParams)
                {
                    dp.Add(p.Key, p.Value);
                }

                dp.Add("@newid", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute(proc, dp, commandType: CommandType.StoredProcedure);
                return dp.Get<int>("@newid");
            }
        }

        // Data commands

        // Store info

        public List<StoreInfo> GetAllStoreCodes()
        {
            using (var connection = this.getConnection())
            {
                return connection.Query<StoreInfo>("StoreCodes_Read", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public List<StoreInfo> GetStoreInfoByStoreCode(String storeCode)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@storeCode", storeCode);
                return connection.Query<StoreInfo>("StoreCode_ReadByCode", p, commandType: CommandType.StoredProcedure).ToList();
            }
        }

        //================================================================================
        // Site Areas
        //================================================================================

        public List<SiteArea> SiteAreasForStoreCode(String storecode)
        {
            List<SiteArea> result = null;
            List<StoreInfo> stores = this.GetStoreInfoByStoreCode(storecode);
            if (stores.Count > 0) {
                StoreInfo info = stores.First();
                ProductService productService = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
                result = productService.SiteAreasForStoreCode(info);
            }
            return result;
        }

        public SiteArea SiteAreaById(String guid)
        {
            ProductService productService = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            return productService.GetSiteArea(guid);
        }

        //================================================================================
        // Addresses
        //================================================================================
        /// <summary>
        /// Creates an address
        /// </summary>
        /// <returns>ID of the new address</returns>
        public int AddressCreate(Address address)
        {
            try
            {
                CustomerService client = WebServiceUtils.GetEndpointService<CustomerService>(CustomerServiceInfo.ENDPOINT_NAME);
                return client.AddressCreateOrUpdate(address);
            } catch {
                return -1;
            }
        }
        
        
        
        
        //================================================================================
        // Users
        //================================================================================
        public List<User> GetAllUsers()
        {
            try
            {
                UserService client = WebServiceUtils.GetEndpointService<UserService>(UserServiceInfo.ENDPOINT_NAME);
                return client.GetAllUsers();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public User GetUserByUsername(String username)
        {
            try
            {
                UserService client = WebServiceUtils.GetEndpointService<UserService>(UserServiceInfo.ENDPOINT_NAME);
                return client.GetUserByUsername(username);
            } 
            catch (Exception e) 
            {
                return null;
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                UserService client = WebServiceUtils.GetEndpointService<UserService>(UserServiceInfo.ENDPOINT_NAME);
                return client.GetUserById(id);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<User> GetUsersByStoreCode(String storeCode)
        {
            try
            {
                UserService client = WebServiceUtils.GetEndpointService<UserService>(UserServiceInfo.ENDPOINT_NAME);
                return client.GetUsersByStoreCode(storeCode);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int AddUser(String firstname, String lastname, String username, String storecode, int role = 1)
        {
            try
            {
                User newUser = new User();
                newUser.firstName = firstname;
                newUser.lastName = lastname;
                newUser.userName = username;
                newUser.storeCode = storecode;
                newUser.roleid = role;
                UserService client = WebServiceUtils.GetEndpointService<UserService>(UserServiceInfo.ENDPOINT_NAME);
                return client.AddUser(newUser);
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        //========================================================================================
        // Emails
        //========================================================================================
        internal int EmailCreate(Emails email)
        {
            using (var connection = this.getConnection())
            {
                if (email.Address == null || email.Address == "") { return -1; }
                var p = new DynamicParameters();
                p.Add("@Address", email.Address);
                p.Add("@FKCustomer", email.FKCustomer);
                p.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute("Email_Create", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@newid");
            }        
        }

        internal void EmailDeleteById(int id)
        {
              using (var connection = this.getConnection())
              {
                    var dp = new DynamicParameters();
                    dp.Add("@Id", id);
                    connection.Execute("EmailDeleteById", dp, commandType: CommandType.StoredProcedure);
              }
        }

        internal void EmailUpdateById(Emails email)
        {
             using (var connection = this.getConnection()){
                 var dp = new DynamicParameters();
                 dp.Add("@Id", email.Id);
                 dp.Add("@Address", email.Address);
                 dp.Add("@FKCustomer", email.FKCustomer);
                 connection.Execute("Email_UpdateById", dp, commandType: CommandType.StoredProcedure);
             }
        }
             

        //========================================================================================
        // CUSTOMERS
        //========================================================================================

        public List<Customers> CustomerListGetByAssociate(int Id)
        {
            try
            {
                CustomerService customerService = WebServiceUtils.GetEndpointService<CustomerService>(CustomerServiceInfo.ENDPOINT_NAME);
                List<Customers> customerList = customerService.GetCustomerListBySalesAssociate(Id);
                return customerList;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        //  Updates  or adds customer an existing customer
        public int CustomerUpdateOrAdd(Customers customer)
        {
            CustomerService customerService = WebServiceUtils.GetEndpointService<CustomerService>(CustomerServiceInfo.ENDPOINT_NAME);
            return customerService.CustomerUpdateOrAdd(customer);

        }


        /// <summary>
        /// This attaches creates an AddressToOwner row attaching the customer and the address with the OwnerId representing the owner typ
        /// </summary>
        /// <param name="CustomerId"></param>
        /// <param name="AddressId"></param>
        /// <returns></returns>
        public int AddressAttachOwner(int CustomerId, int AddressId, int OwnerId)
        {
            return 1;
        
        }

        public bool CustomerUpdateDefaultAddress(int AddressId, int CustomerId)
        {
            try
            {
                CustomerService customerService = WebServiceUtils.GetEndpointService<CustomerService>(CustomerServiceInfo.ENDPOINT_NAME);
                return customerService.CustomerUpdateDefaultAddress(AddressId, CustomerId);

            }
            catch
            {
                return false;
            }
        }

        ///<summary>
        ///  This collects a JSON object that can power the entire projects/customers section of the app.  It returns by ID
        ///</summary>
        public Customers CustomerGetById(int id)
        {
            CustomerService customerService = WebServiceUtils.GetEndpointService<CustomerService>(CustomerServiceInfo.ENDPOINT_NAME);
            return customerService.CustomerGetById(id);
        }

        public Customers CustomerGetByGUID(String CustomerGUID)
        {
            CustomerService customerService = WebServiceUtils.GetEndpointService<CustomerService>(CustomerServiceInfo.ENDPOINT_NAME);
            return customerService.CustomerGetByGUID(CustomerGUID);
        }



        //========================================================================================
        // Phone Numbers
        //========================================================================================
        internal int PhoneNumberCreate(PhoneNumbers pn)
        {
            using (var connection = this.getConnection())
            {
                if (pn.PhoneNumber == null || pn.PhoneNumber == "") { return -1; }
                var p = new DynamicParameters();
                p.Add("@PhoneNumber", pn.PhoneNumber);
                p.Add("@FKType", pn.FKType);
                p.Add("@FKCustomer", pn.FKCustomer);
                p.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute("PhoneNumber_Create", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@newid");
            }
        }



        internal void PhoneNumberDeleteById(int id)
        {
            using (var connection = this.getConnection())
            {
                var dp = new DynamicParameters();
                dp.Add("@Id", id);
                connection.Execute("PhoneNumber_DeleteById", dp, commandType: CommandType.StoredProcedure);
            }
        }

        internal void PhoneNumberUpdateById(PhoneNumbers phone)
        {
            using (var connection = this.getConnection())
            {
                var dp = new DynamicParameters();
                dp.Add("@Id", phone.Id);
                dp.Add("@PhoneNumber", phone.PhoneNumber);
                dp.Add("@FKType", phone.FKType);
                dp.Add("@FKCustomer", phone.FKCustomer);
                connection.Execute("PhoneNumber_UpdateById", dp, commandType: CommandType.StoredProcedure);
            }
        }


        //==============================================================================================
        //  Product Types
        //==============================================================================================
        
        internal Dictionary<string, ProductType> ProductsGetAll()
        {
            ProductService productService = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            return productService.ProductGetAll();
        }

        internal SiteArea ProductsForSiteArea(SiteArea siteArea)
        {
            ProductService productService = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            return productService.ProductsForSiteArea(siteArea);
        }

        //==============================================================================================
        //  Product Instances
        //==============================================================================================

        /// <summary>
        ///  Creates or updates a product instance depending on whether or not we have an Id.
        /// </summary>
        /// <param name="ProductInstance">ProductInstance</param>
        /// <returns></returns>
        internal int ProductInstanceCreateOrUpdate(ProductInstance ProductInstance)
        {
            ProductService productService = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            return productService.ProductInstanceCreateOrUpdate((ProductInstance)ProductInstance);

        }

        internal bool ProductInstanceDelete(int id)
        {
            ProductService productService = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            return productService.ProductInstanceDelete(id);

        }
        //============================================================================================
        //  Projects
        //============================================================================================

        /// <summary>
        /// This creates a record attaching a project and a customer.
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        internal int ProjectAttachCustomer(int CustomerId, int ProjectId){
            ProjectService projectService = WebServiceUtils.GetEndpointService<ProjectService>(ProjectServiceInfo.ENDPOINT_NAME);
            return projectService.ProjectAttachCustomer(CustomerId, ProjectId);

        }
        
        internal Project ProjectCollectFirstIfExists(int CustomerId, int StatusId)
        {
            ProjectService projectService = WebServiceUtils.GetEndpointService<ProjectService>(ProjectServiceInfo.ENDPOINT_NAME);
            return projectService.ProjectCollectFirstIfExists(CustomerId, StatusId);
        
        }
        
        internal List<Project> ProjectFindChildren(int ProjectId, int StatusId){
            ProjectService projectService = WebServiceUtils.GetEndpointService<ProjectService>(ProjectServiceInfo.ENDPOINT_NAME);
            return projectService.ProjectFindChildren(ProjectId, StatusId);
        }
        
        /// <summary>
        ///  Feed this an existing project ID and we create the whole quote.
        /// </summary>
        /// <param name="project">Project ID to Turn into a Quote</param>
        /// <returns></returns>
        ///  Here we copy an entire project into a quote.  To do so we need to
        ///  1.  Copy the project row itself, setting a new FKParent and updating the ProjectStatus
        ///  2.  Copy all the rooms, setting new FKParentRoom and new FKProject
        ///  3.  Copy all the ProductInstances, setting a new FKParentInstance and FKRoom
        internal int ProjectCreateFromProject(Project project, int statusId)
        {
            ProjectService projectService = WebServiceUtils.GetEndpointService<ProjectService>(ProjectServiceInfo.ENDPOINT_NAME);
            return projectService.ProjectCreateFromProject(project,statusId);
        }


        internal bool ProjectDeleteFromUsers(int ProjectId)
        {
            ProjectService projectService = WebServiceUtils.GetEndpointService<ProjectService>(ProjectServiceInfo.ENDPOINT_NAME);
            return projectService.ProjectDeleteFromUsers(ProjectId);
        
        }
        
        /// <summary>
        /// Creates a new project in the database and returns the Id.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        internal int ProjectCreateNew(Project project)
        {
            ProjectService projectService = WebServiceUtils.GetEndpointService<ProjectService>(ProjectServiceInfo.ENDPOINT_NAME);
            return projectService.ProjectCreateNew(project);
        }

        //============================================================================================
        //  Categories
        //============================================================================================

        internal List<RoomsCategory> RoomsCategoryReadAll()
        {
            ProductService productService = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            return productService.GetAllRoomsCategories();
        }

        internal RoomsCategory CategoriesForRoomsCategory(String name)
        {
            ProductService productService = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            return productService.CategoriesForRoomsCategory(name);
        }

        internal ProductCategory ProductsForCategory(String guid)
        {
            ProductService productService = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            return productService.ProductsForCategory(guid);
        }
    }

}