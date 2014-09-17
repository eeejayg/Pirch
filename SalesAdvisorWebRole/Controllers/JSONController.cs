using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using Dapper;
using Newtonsoft.Json;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWebRole.Adapters;
using SalesAdvisorWebRole.Models;

using SalesAdvisorSharedClasses.Communication;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace SalesAdvisorWebRole.Controllers
{
    /// <summary>
    /// <para>THis JSON controller could also be called /api/ controller.  This is the main restpoint for all
    /// calls to the database that want a response via json.  </para>
    /// </summary>
    public class JSONController : BaseRequirementsController
    {
        private ActionResult GetSQL<T>(string query)
        {
            using (var connection = this.getConnection())
            {
                var data = connection.Query<T>(query).ToList();
                connection.Close();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }


        //=============================================================================
        // JSON/address
        //=============================================================================

        [HttpPost]
        public ActionResult Address(Address address)
        {
            int addressId = DBAdapter.getInstance().AddressCreate(address);
            return View("PassFail");
        }



        //=============================================================================
        // JSON/CUSTOMERS
        //=============================================================================
        // It creates the customer and potentially a phone number and an email address

        //
        // GET: /JSON/CustomersByAssociate
        public ActionResult CustomersByAssociate(int Id)
        {

             List<Customers> customerList = DBAdapter.getInstance().CustomerListGetByAssociate(Id);
             return Json(customerList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CustomerGetById(int id)
        {

            Customers customer = DBAdapter.getInstance().CustomerGetById(id);
            return Json(customer, JsonRequestBehavior.AllowGet);
         
        }






        //  Collect all customers
        // GET: /JSON/Customers
        //public ActionResult Customers()
        //{
        //    return GetSQL<Customers>("Select * from customers ORDER BY FirstName asc");
        //}


        //  Collect one customer
        // GET: /JSON/Customers/:id
        public ActionResult Customers(int id)
        {
            Customers cust = DBAdapter.getInstance().CustomerGetById(id);
            return Json(cust, JsonRequestBehavior.AllowGet);
        }

        // GET: /JSON/customersguid/:id
        /// <summary>
        ///   Collects a customer by guid as opposed to id
        /// </summary>
        /// <param name="id">This Id is actually a GUID</param>
        /// <returns></returns>
        public ActionResult CustomersGUID(String id)
        {
            Customers cust = DBAdapter.getInstance().CustomerGetByGUID(id);
            return Json(cust, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Put is the update method.  We have nested Emails and PhoneNumbers
        /// We need to update the user, then create or update both 
        /// an email and a phone number
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult Customers(int id, Customers customer)
        {
            int customerId = DBAdapter.getInstance().CustomerUpdateOrAdd(customer);
            customer.Id = customerId;
            @ViewBag.message = "Customer updated";
            @ViewBag.success = true;
            @ViewBag.custID = customer.Id;
            @ViewBag.message = "Customer Created";
            return View("JSONReturnCustomerID");
        }



        // This is handling a POST from the new customer form.
        [HttpPost]
        public ActionResult Customers(Customers customer)
        {
            customer.Id = DBAdapter.getInstance().CustomerUpdateOrAdd(customer);
//            this.HandleEmailForCustomer(customer);
//            this.HandlePhoneForCustomer(customer);
            @ViewBag.message = "Customer created";
            @ViewBag.success = true;
            @ViewBag.custID = customer.Id;
            @ViewBag.message = "Customer Created";
            return View("JSONReturnCustomerID");
        }


        /// <summary>
        /// This is a utility function for handling custemer email creation.
        ///  The cases we need to deal with are..
        ///  1.  No address, No Id.... return
        ///  2.  Id no address... delete
        ///  2.  Email address, no Id... create
        ///  2.  Email address, Id... update
        /// </summary>
        /// <param name="customer">CreateCustomer</param>
        /// <returns></returns>
        public bool HandleEmailForCustomer(CreateCustomer customer)
        {
            if (customer.EmailID == 0 && customer.Email == null)
            {
                return false;
            }


            //  Now we know we have an email address...
            //  Check for delete
            if (customer.EmailID > 0 && (customer.Email == null || customer.Email == ""))
            {
                DBAdapter.getInstance().EmailDeleteById(customer.EmailID);
                return true;
            }
            //  Now, construct our email address..
            Emails email = new Emails();
            email.Address = customer.Email;
            email.FKCustomer = customer.Id;
            // Check for create
            if (customer.EmailID == 0 && (customer.Email != "" || customer.Email != null))
            {
                DBAdapter.getInstance().EmailCreate(email);
                return true;
            }
            //  add in the ID
            email.Id = customer.EmailID;
            //  All the other cases are done, so update
            DBAdapter.getInstance().EmailUpdateById(email);
            return true;
        }

        /// <summary>
        /// This is a utility function for handling custemer email creation.
        ///  The cases we need to deal with are..
        ///  1.  No address, No Id.... return
        ///  2.  Id no address... delete
        ///  2.  Email address, no Id... create
        ///  2.  Email address, Id... update
        /// </summary>
        /// <param name="customer">CreateCustomer</param>
        /// <returns></returns>
        public bool HandlePhoneForCustomer(CreateCustomer customer)
        {
            if (customer.PhoneID == 0 && customer.PhoneNumber == null)
            {
                return false;
            }


            //  Now we know we have an email address...
            //  Check for delete
            if (customer.PhoneID > 0 && (customer.PhoneNumber == null || customer.PhoneNumber == ""))
            {
                DBAdapter.getInstance().PhoneNumberDeleteById(customer.PhoneID);
                return true;
            }
            //  Now, construct our email address..
            PhoneNumbers phone = new PhoneNumbers();
            phone.PhoneNumber = customer.PhoneNumber;
            phone.FKCustomer = customer.Id;
            if (customer.Mobile)
            {
                phone.FKType = 1;
            }
            else
            {
                phone.FKType = 2;
            }

            // Check for create
            if (customer.PhoneID == 0 && (customer.PhoneNumber != "" || customer.PhoneNumber != null))
            {
                DBAdapter.getInstance().PhoneNumberCreate(phone);
                return true;
            }
            //  add in the ID
            phone.Id = customer.PhoneID;
            //  All the other cases are done, so update
            DBAdapter.getInstance().PhoneNumberUpdateById(phone);
            return true;
        }









        //=============================================================================
        // JSON/PRODUCTS
        //
        //  Please note... This is products, not product instances.
        //=============================================================================


        /// <summary>
        /// This function returns a JSON object of all products, including available options
        /// Its contains the product key.  It should be relatively easy to reference this object 
        /// from other JSON objects to get the product details needed.
        /// </summary>
        /// <returns>A JSON View</returns>
        public ActionResult ProductsGetAll()
        {

            Dictionary<string, ProductType> products = DBAdapter.getInstance().ProductsGetAll();

            return Json(products, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ProductsGetAllMock() {
            Dictionary<string, ProductType> products = new Dictionary<string, ProductType>();

            //=======================================================================
            // First mock product
            //=======================================================================
            ProductType product1 = new ProductType();
            product1.Id = 1;
            product1.ProductTypeGUID = "CABINETGUID";
            product1.Name = "Cabinet";
            product1.SKU = "AAABBBCCC111";
            product1.Description = "This cabinet is made of the finest foo";
            product1.ImageAdd("/Images/products/faucet1.jpg");

            ProductCategory ProductCategory1 = new ProductCategory();
            ProductCategory1.Id = 1;
            ProductCategory1.Name = "Cabinetry";
            product1.ProductCategoryIndexOrAdd(ProductCategory1);

            
            
            //  Create Room Category
            product1.FKRoomsCategory = 1; /// We are unsure if this is a 1=1 relationship at this point.

            // Create Line
            Line MeileLine = new Line();
            MeileLine.Id = 1;
            MeileLine.Name = "Modernist";
            Manufacturer Manufacturer1 = new Manufacturer();

            // Create Manufacturer
            Manufacturer1.Id = 1;
            Manufacturer1.Name = "Meile";
            Manufacturer1.LineIndexOrAdd(MeileLine);
            product1.Manufacturer = Manufacturer1;
            product1.ListPrice = 323.23;
            product1.FullyLoaded = 2323.23;
            product1.MinimumPrice = 100.23;


            //

            ProductTypeOptionGroup ProductTypeOptionGroup1 = new ProductTypeOptionGroup();
            ProductTypeOptionGroup1.Id = 1;
            ProductTypeOptionGroup1.Name = "Wood";
            ProductTypeOptionGroup1.Description = "Choose what kind of wood you'd like for this cabinet";
            ProductTypeOptionGroup1.AllowMultiple = false;
            ProductTypeOptionGroup1.GUID = "WOODOPTIONGROUPGUID";


            // Product Options
            ProductTypeOptions WoodOption1 = new ProductTypeOptions();
            WoodOption1.Id = 1;
            WoodOption1.Name = "Oak";
            WoodOption1.Description = "Oak is great!";
            WoodOption1.GUID = "OAKOPTIONGUID";

            ProductTypeOptions WoodOption2 = new ProductTypeOptions();
            WoodOption2.Id = 2;
            WoodOption2.Name = "Walnut";
            WoodOption2.Description = "Walnut is great!";
            WoodOption2.GUID = "WALNUTOPTIONGUID";

            ProductTypeOptionGroup1.OptionIndexOrAdd(WoodOption1);
            ProductTypeOptionGroup1.OptionIndexOrAdd(WoodOption2);

            ProductTypeOptionGroup ProductTypeOptionGroup2 = new ProductTypeOptionGroup();
            ProductTypeOptionGroup2.Id = 2;
            ProductTypeOptionGroup2.Name = "Chrome";
            ProductTypeOptionGroup2.Description = "Choose what kind of chrome you'd like for this cabinet";
            ProductTypeOptionGroup2.AllowMultiple = false;
            ProductTypeOptionGroup2.GUID = "CHROMEOPTIONGROUPGUID";

            ProductTypeOptions ChromeOption1 = new ProductTypeOptions();
            ChromeOption1.Id = 3;
            ChromeOption1.Name = "Silver";
            ChromeOption1.Description = "Silver is great!";
            ChromeOption1.GUID = "SILVEROPTIONGUID";

            ProductTypeOptions ChromeOption2 = new ProductTypeOptions();
            ChromeOption2.Id = 4;
            ChromeOption2.Name = "Gold";
            ChromeOption2.Description = "Gold is great!";
            ChromeOption2.GUID = "GoldOPTIONGUID";

            ProductTypeOptionGroup2.OptionIndexOrAdd(ChromeOption1);
            ProductTypeOptionGroup2.OptionIndexOrAdd(ChromeOption2);

            product1.OptionGroupIndexOrAdd(ProductTypeOptionGroup1);
            product1.OptionGroupIndexOrAdd(ProductTypeOptionGroup2);
            product1.Manufacturer = Manufacturer1;
            


            //  ProductAddOns
            ProductTypeAddOn AddOn1 = new ProductTypeAddOn();
            AddOn1.Id = 1;
            AddOn1.Name = "Extra Drawer";
            AddOn1.GUID = "EXTRADRAWERGUID";
            AddOn1.Description = "This is an extra drawer if you'd like it";
            product1.AddOnIndexOrAdd(AddOn1);


            // Finally add the product
            products.Add(product1.ProductTypeGUID, product1);


            

            //==============================================================================
            // Second mock product
            //==============================================================================
            ProductType product2 = new ProductType();
            product2.Id = 2;
            product2.ProductTypeGUID = "TABLENOOPTIONS";
            product2.Name = "Stone Table";
            product2.SKU = "DDDEEEFFF222";
            product2.Description = "This table is a stone table with no options or add ons";
            product2.ListPrice = 565.76;
            product2.FullyLoaded = 565.76;
            product2.FKRoomsCategory = 2;
            product2.ImageAdd("/Images/products/range1.jpg");
            product2.MinimumPrice = 90.23;



            ProductCategory ProductCategory2 = new ProductCategory();
            ProductCategory2.Id = 2;
            ProductCategory2.Name = "Stone Furniture";
            product2.ProductCategoryIndexOrAdd(ProductCategory2);
            
            ProductCategory ProductCategory3 = new ProductCategory();
            ProductCategory3.Id = 3;
            ProductCategory3.Name = "Dining Tables";
            product2.ProductCategoryIndexOrAdd(ProductCategory3);


            Line Claw = new Line();
            Claw.Id = 2;
            Claw.Name = "Claw";



            Manufacturer Manufacturer2 = new Manufacturer();
            Manufacturer2.Id = 2;
            Manufacturer2.Name = "Cougar";
            Manufacturer2.LineIndexOrAdd(Claw);
            product2.Manufacturer = Manufacturer2;

            products.Add(product2.ProductTypeGUID, product2);





            // Kitchen Mock
            RoomsCategory kitchen = new RoomsCategory();
            kitchen.Name = "Kitchen";

            ProductCategory Cooktops = new ProductCategory();
            Cooktops.Id = 1;
            Cooktops.Name = "Cooktops";

            ProductType GasCooktop = new ProductType();
            GasCooktop.Id = 1;
            GasCooktop.ProductTypeGUID = "GASCOOKTOP";
            GasCooktop.Name = "Gas cooktop";
            GasCooktop.RoomCategory = kitchen;
            GasCooktop.ProductCategoryIndexOrAdd(Cooktops);
            GasCooktop.Manufacturer = Manufacturer1;
            GasCooktop.ListPrice = 323.23;
            GasCooktop.FullyLoaded = 2323.23;
            GasCooktop.MinimumPrice = 90.23;


            products.Add(GasCooktop.ProductTypeGUID, GasCooktop);

            ProductType ElectricCooktop = new ProductType();
            ElectricCooktop.Id = 2;
            ElectricCooktop.ProductTypeGUID = "ELECTRICCOOKTOP";
            ElectricCooktop.Name = "Electric cooktop";
            ElectricCooktop.RoomCategory = kitchen;
            ElectricCooktop.ProductCategoryIndexOrAdd(Cooktops);
            ElectricCooktop.Manufacturer = Manufacturer1;
            ElectricCooktop.ImageAdd("/Images/products/faucet2.jpg");
            ElectricCooktop.ListPrice = 323.23;
            ElectricCooktop.FullyLoaded = 2323.23;
            ElectricCooktop.MinimumPrice = 90.23;
            products.Add(ElectricCooktop.ProductTypeGUID, ElectricCooktop);


            ProductCategory Dishwashers = new ProductCategory();
            Dishwashers.Id = 2;
            Dishwashers.Name = "Dishwasher";

            ProductType BigDish = new ProductType();
            BigDish.ProductTypeGUID = "BIGDISHWASHER";
            BigDish.Id = 3;
            BigDish.Name = "World's biggest dishwasher";
            BigDish.RoomCategory = kitchen;
            BigDish.ProductCategoryIndexOrAdd(Dishwashers);
            BigDish.Manufacturer = Manufacturer1;
            BigDish.ImageAdd("/Images/products/showerhead1.jpg");
            BigDish.ListPrice = 323.23;
            BigDish.FullyLoaded = 2323.23;
            BigDish.MinimumPrice = 90.23;
            products.Add(BigDish.ProductTypeGUID, BigDish);

            ProductType SmallDish = new ProductType();
            SmallDish.ProductTypeGUID = "SMALLDISHWASHER";
            SmallDish.Id = 4;
            SmallDish.Name = "Boutique Dish washer";
            SmallDish.RoomCategory = kitchen;
            SmallDish.ProductCategoryIndexOrAdd(Dishwashers);
            SmallDish.Manufacturer = Manufacturer1;
            SmallDish.ImageAdd("/Images/products/range1.jpg");
            SmallDish.ListPrice = 323.23;
            SmallDish.FullyLoaded = 2323.23;
            SmallDish.MinimumPrice = 90.23; 
            products.Add(SmallDish.ProductTypeGUID, SmallDish);


            // Bathroom Mock
            RoomsCategory bathroom = new RoomsCategory();
            bathroom.Name = "bathroom";

            ProductCategory Sinks = new ProductCategory();
            Sinks.Id = 11;
            Sinks.Name = "Sinks";

            ProductType WhiteSink = new ProductType();
            WhiteSink.ProductTypeGUID = "WHITESINK";
            WhiteSink.Id = 11;
            WhiteSink.Name = "Porcelain Sink";
            WhiteSink.RoomCategory = bathroom;
            WhiteSink.ProductCategoryIndexOrAdd(Sinks);
            WhiteSink.Manufacturer = Manufacturer1;
            WhiteSink.ImageAdd("/Images/products/faucet1.jpg");
            WhiteSink.ListPrice = 323.23;
            WhiteSink.FullyLoaded = 2323.23;
            WhiteSink.MinimumPrice = 90.23; 
            products.Add(WhiteSink.ProductTypeGUID, WhiteSink);


            ProductType BlackSink = new ProductType();
            BlackSink.ProductTypeGUID = "BLACKSINK";
            BlackSink.Id = 12;
            BlackSink.Name = "Obsidian Sink";
            BlackSink.RoomCategory = bathroom;
            BlackSink.ProductCategoryIndexOrAdd(Sinks);
            BlackSink.Manufacturer = Manufacturer1;
            BlackSink.Manufacturer = Manufacturer1;
            BlackSink.ImageAdd("/Images/products/faucet2.jpg");
            BlackSink.ListPrice = 323.23;
            BlackSink.FullyLoaded = 2323.23;
            BlackSink.MinimumPrice = 90.23; 
            products.Add(BlackSink.ProductTypeGUID, BlackSink);


            ProductCategory Showerheads = new ProductCategory();
            Showerheads.Id = 12;
            Showerheads.Name = "Showerheads";

            ProductType Rainmaker = new ProductType();
            Rainmaker.Id = 13;
            Rainmaker.ProductTypeGUID = "RAINMAKER";
            Rainmaker.Name = "Rainmaker";
            Rainmaker.RoomCategory = bathroom;
            Rainmaker.ProductCategoryIndexOrAdd(Showerheads);
            Rainmaker.Manufacturer = Manufacturer1;
            Rainmaker.ImageAdd("/Images/products/showerhead1.jpg");
            Rainmaker.ListPrice = 323.23;
            Rainmaker.MinimumPrice = 90.23; 
            Rainmaker.FullyLoaded = 2323.23;

            products.Add(Rainmaker.ProductTypeGUID, Rainmaker);

            ProductType Waterfall = new ProductType();
            Waterfall.Id = 14;
            Waterfall.ProductTypeGUID = "WATERFALL";
            Waterfall.Name = "Boutique Dish washer";
            Waterfall.RoomCategory = bathroom;
            Waterfall.ProductCategoryIndexOrAdd(Showerheads);
            Waterfall.Manufacturer = Manufacturer1;
            Waterfall.ImageAdd("/Images/products/showerhead1.jpg");
            Waterfall.ListPrice = 323.23;
            Waterfall.FullyLoaded = 2323.23;
            Waterfall.MinimumPrice = 90.23; 
            products.Add(Waterfall.ProductTypeGUID, Waterfall);


            // Outdoor Mock
            RoomsCategory Outdoor = new RoomsCategory();
            Outdoor.Name = "Outdoor";

            ProductCategory Grills = new ProductCategory();
            Grills.Id = 21;
            Grills.Name = "Grills";

            ProductType Propane = new ProductType();
            Propane.ProductTypeGUID = "PROPANE";
            Propane.Id = 21;
            Propane.Name = "Propane Grill";
            Propane.ProductCategoryIndexOrAdd(Grills);
            Propane.RoomCategory = Outdoor;
            Propane.Manufacturer = Manufacturer1;
            Propane.ImageAdd("/Images/products/range1.jpg");
            Propane.ListPrice = 323.23;
            Propane.FullyLoaded = 2323.23;
            Propane.MinimumPrice = 90.23; 
            products.Add(Propane.ProductTypeGUID, Propane);

            ProductType Charcoal = new ProductType();
            Charcoal.Id = 22;
            Charcoal.ProductTypeGUID = "CHARCOAL";
            Charcoal.Name = "Charcoal Grill";
            Charcoal.RoomCategory = Outdoor;
            Charcoal.ProductCategoryIndexOrAdd(Grills);
            Charcoal.Manufacturer = Manufacturer1;
            Charcoal.ImageAdd("/Images/products/faucet2.jpg");
            Charcoal.ListPrice = 323.23;
            Charcoal.FullyLoaded = 2323.23;
            Charcoal.MinimumPrice = 90.23; 
            products.Add(Charcoal.ProductTypeGUID, Charcoal);

            ProductCategory PatioFurniture = new ProductCategory();
            PatioFurniture.Id = 22;
            PatioFurniture.Name = "Patio Furniture";

            ProductType Chair = new ProductType();
            Chair.ProductTypeGUID = "PATIOCHAIR";
            Chair.Id = 23;
            Chair.Name = "Patio Chair";
            Chair.RoomCategory = Outdoor;
            Chair.ProductCategoryIndexOrAdd(PatioFurniture);
            Chair.Manufacturer = Manufacturer1;
            Chair.Manufacturer = Manufacturer1;
            Chair.ImageAdd("/Images/products/faucet1.jpg");
            Chair.ListPrice = 323.23;
            Chair.FullyLoaded = 2323.23;
            Chair.MinimumPrice = 90.23; 
            products.Add(Chair.ProductTypeGUID, Chair);

            ProductType Table = new ProductType();
            Table.Id = 14;
            Table.ProductTypeGUID = "PATIOTABLE";
            Table.Name = "Patio Table";
            Table.RoomCategory = Outdoor;
            Table.ProductCategoryIndexOrAdd(PatioFurniture);
            Table.Manufacturer = Manufacturer1;
            Table.ImageAdd("/Images/products/showerhead1.jpg");
            Table.ListPrice = 323.23;
            Table.FullyLoaded = 2323.23;
            Table.MinimumPrice = 90.23; 
            products.Add(Table.ProductTypeGUID, Table);



            // Laundry Mock
            RoomsCategory Laundry = new RoomsCategory();
            Laundry.Name = "Laundry";

            ProductCategory Washer = new ProductCategory();
            Washer.Id = 31;
            Washer.Name = "Grills";

            ProductType EcoFriendly = new ProductType();
            EcoFriendly.Id = 31;
            EcoFriendly.ProductTypeGUID = "ECOFRIENDLY";
            EcoFriendly.Name = "Eco-Friendly Washer";
            EcoFriendly.ProductCategoryIndexOrAdd(Washer);
            EcoFriendly.RoomCategory = Laundry;
            EcoFriendly.Manufacturer = Manufacturer1;
            EcoFriendly.ImageAdd("/Images/products/range1.jpg");
            EcoFriendly.ListPrice = 323.23;
            EcoFriendly.FullyLoaded = 2323.23;
            EcoFriendly.MinimumPrice = 90.23; 


            products.Add(EcoFriendly.ProductTypeGUID, EcoFriendly);


            ProductType SuperClean = new ProductType();
            SuperClean.Id = 32;
            SuperClean.ProductTypeGUID = "SUPERCLEAN";
            SuperClean.Name = "SuperClean Washer";
            SuperClean.ProductCategoryIndexOrAdd(Washer);
            SuperClean.RoomCategory = Laundry;
            SuperClean.Manufacturer = Manufacturer1;
            SuperClean.ImageAdd("/Images/products/faucet1.jpg");
            SuperClean.ListPrice = 323.23;
            SuperClean.FullyLoaded = 2323.23;
            SuperClean.MinimumPrice = 90.23; 
            products.Add(SuperClean.ProductTypeGUID, SuperClean);


            ProductCategory Dryer = new ProductCategory();
            Dryer.Id = 32;
            Dryer.Name = "Driers";

            ProductType Huge = new ProductType();
            Huge.Id = 33;
            Huge.ProductTypeGUID = "HUGEDRIER";
            Huge.Name = "Huge Drier";
            Huge.ProductCategoryIndexOrAdd(Dryer);
            Huge.RoomCategory = Laundry;
            Huge.Manufacturer = Manufacturer1;
            Huge.ImageAdd("/Images/products/faucet2.jpg");
            Huge.ListPrice = 323.23;
            Huge.FullyLoaded = 2323.23;
            Huge.MinimumPrice = 90.23; 

            products.Add(Huge.ProductTypeGUID, Huge);

            ProductType Fast = new ProductType();
            Fast.Id = 34;
            Fast.ProductTypeGUID = "FASTDRIER";
            Fast.Name = "Super Fast";
            Fast.ProductCategoryIndexOrAdd(Dryer);
            Fast.RoomCategory = Laundry;
            Fast.Manufacturer = Manufacturer1;
            Fast.ImageAdd("/Images/products/showerhead1.jpg");
            Fast.ListPrice = 323.23;
            Fast.FullyLoaded = 2323.23;
            Fast.MinimumPrice = 90.23; 

            products.Add(Fast.ProductTypeGUID, Fast);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        //===============================================================================
        //  JSON/SiteAreas
        //===============================================================================   

        [HttpGet]
        public ActionResult SiteArea(String id)
        {
            if (id == null)
            {
                List<SiteArea> result = DBAdapter.getInstance().SiteAreasForStoreCode(SessionAdapter.getInstance().GetCurrentStore(this.Request).StoreCode);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                SiteArea siteArea = DBAdapter.getInstance().SiteAreaById(id);
                return Json(DBAdapter.getInstance().ProductsForSiteArea(siteArea), JsonRequestBehavior.AllowGet);
            }
        }

        // GET: /JSON/ProductTypes
        public ActionResult ProductTypes()
        {
            return GetSQL<ProductType>("Select * from producttype ORDER BY Id asc");
        }

        //===============================================================================
        //  JSON/ProductInstances
        //  
        //  Please note, this is product instances, not products
        //===============================================================================        


        /// <summary>
        /// This deletes the identified product instance
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult ProductInstance(int id)
        {
            if (DBAdapter.getInstance().ProductInstanceDelete(id))
            {
                @ViewBag.success = true;
                @ViewBag.message = "Product Instance deleted";
            }
            else
            {
                @ViewBag.success = false;
                @ViewBag.message = "Product Instance failed to delete";
            }
            return View("passFail");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ProductInstance"></param>
        /// <returns></returns>
        [HttpPut]
        public ActionResult ProductInstance(int id,ProductInstance ProductInstance)
        {
            if (DBAdapter.getInstance().ProductInstanceCreateOrUpdate(ProductInstance) > 0)
            {
                @ViewBag.success = true;
                @ViewBag.message = "Product Instance updated";                
            }
            else
            {
                @ViewBag.success = false;
                @ViewBag.message = "Product Instance failed to update";
            
            }
            return View("passFail");
        }


        /// <summary>
        /// This creates a product instance.  We must accept products..
        ///    1.  With and without addons
        ///    2.  With and without projects
        ///    3.  With and without options
        ///    Remember, this is built to accept native backbone functionality in a RESTful manner.
        ///   For beta, the functionality is as follows, we will actually be making sure there are 4 records here
        ///   1.   There needs to be one project per customer
        ///   2.   There needs to be on quote per customer with the parent of a project
        ///   3.   There needs to be one project instance per project
        ///   4.   There needs to be one project instance per quote.
        /// </summary>
        /// <param name="productInstance"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ProductInstance(ProductInstanceWithCustomer productInstanceWithCustomer)
        {
            try
            {
                ProductInstance productInstance = productInstanceWithCustomer.Clone();
                //=========================================================================
                //  A little bit of sanity testing...
                //=========================================================================
                if (productInstance.ProductTypeGUID == "" || productInstance.ProductTypeGUID == null)
                {
                    @ViewBag.success = false;
                    @ViewBag.message = "You tried to create a product instance no GUID";
                    return View("passFail");
                }

                if (productInstanceWithCustomer.CustomerId == 0 && productInstanceWithCustomer.FKProject == 0)
                {
                    @ViewBag.success = false;
                    @ViewBag.message = "'CustomerId' OR 'FKProject' is mandatory to create a product instance";
                    return View("passFail");

                }
                // TODO - Validate that the GUID exists....
                // TODO - Respect a second project
//                if (productInstance.FKProject == 0)
//                {
                    
                    // BETA1 - This will likely be ripped out... 
                    // First, Collect or find the top level project
                    Project Project = this.ProjectCreateOrFindFirstProject(productInstanceWithCustomer.CustomerId, (int)Status.Project);
                    // Now attach the incoming productInstance to the project
                    productInstance.FKProject = Project.Id;
                    productInstance.Id = DBAdapter.getInstance().ProductInstanceCreateOrUpdate(productInstance);
                    //  At this point we return if we're just creating a new collection, but for beta,
                    // we need to make a quote.  
//                }

                // Lets see if this project has any child quotes....
                Project Quote;
                List<Project> quotes = this.ProjectFindChildren(Project.Id, (int)Status.Quote);
                if (quotes.Count == 0)
                {
                    //  Nope!  Lets make a new quote.
                    Quote = new Project();
                    Quote.FKStatus = (int)Status.Quote;
                    Quote.FKProjectType = Project.FKProjectType;
                    Quote.FKParentProject = Project.Id;
                    Quote.Name = "Your Proposal";
                    Quote.Id = this.ProjectCreateNew(Quote);
                    this.ProjectAttachCustomer(productInstanceWithCustomer.CustomerId, Quote.Id);
                }
                else
                {
                    //  We have at least one quote.  Lets use the first.
                    Quote = quotes.First();
                }

                ProductInstance QuotesProductInstance = new ProductInstance();
                //  Now set the parent and the appropriate project id
                QuotesProductInstance.FKParentInstance = productInstance.Id;
                QuotesProductInstance.FKProject = Quote.Id;
                QuotesProductInstance.ProductTypeGUID = productInstance.ProductTypeGUID;
                QuotesProductInstance.Id = DBAdapter.getInstance().ProductInstanceCreateOrUpdate(QuotesProductInstance);

                @ViewBag.success = true;
                @ViewBag.message = "Product Instance added";

            }
            catch (Exception e)
            {
                @ViewBag.success = false;productInstanceWithCustomer.Clone();
                @ViewBag.message = "Product Instance failed creation";
            }
            return View("passFail");
            
        }




        //============================================================================
        //  JSON PROJECTS
        //  Quotes, projects  and propsals have different API endpoints, but in the backend they are
        //  all projects.  This has combined functions
        //  
        //============================================================================
        /// <summary>
        /// This is a deep copy of a project to a different project.  All you need to pass it is a project object with FKParentProject set to the parent
        /// and a statusId of the new project.  You'll create all the instances/options/rooms/etc.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public int ProjectCopy(Project project, int statusId)
        {

            if (project.FKParentProject == 0)
            {
                @ViewBag.message = "Oops!  You tried to copy a non-existent product.";
                @ViewBag.success = false;
                return -1;
            }
            try
            {
                return  DBAdapter.getInstance().ProjectCreateFromProject(project, statusId); 
            }
            catch (SqlException error)
            {
                @ViewBag.message = error.Message;
                @ViewBag.success = false;
                return -1;
            }
        }

        /// <summary>
        /// This creates a new project from the posted project and returns the new project Id.
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        private int ProjectCreateNew(Project project){
            return DBAdapter.getInstance().ProjectCreateNew(project);
        }


        /// <summary>
        /// This project either returns the first existing project of a specific type
        /// or creates a project of that type and returns it.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="CustomerId"></param>
        /// <returns></returns>
        private Project ProjectCreateOrFindFirstProject(int CustomerId, int StatusId)
        {

            var project = DBAdapter.getInstance().ProjectCollectFirstIfExists(CustomerId, StatusId);
            if(project != null){
                 return project;
            }
            //  We didn't find a project.  Lets make one!
            Project newProject = new Project();
            newProject.FKStatus = StatusId;
            newProject.FKProjectType = (int)ProjectTypeEnum.Product;
            newProject.Id  = this.ProjectCreateNew(newProject);
            //  The project is created new... lets connect the project to the current user
            DBAdapter.getInstance().ProjectAttachCustomer(CustomerId, newProject.Id);
            return newProject;
        }

        /// <summary>
        /// This returns all the child projects of the project id.  For example, feeding this a project id and the status "quote" will return all quotes for the child id.
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="StatusId"></param>
        /// <returns></returns>
        private List<Project> ProjectFindChildren(int ProjectId, int StatusId)
        {
            return DBAdapter.getInstance().ProjectFindChildren(ProjectId, StatusId);
        
        }



        /// <summary>
        ///   This attaches a customer and a project by creating a record in the swing table.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="newProjectId"></param>
        /// <param name="connection"></param>
        private int ProjectAttachCustomer(int customerId, int newProjectId)
        {
            return DBAdapter.getInstance().ProjectAttachCustomer(customerId, newProjectId);
        }



        /// <summary>
        /// This function is called by any project delete function.  This is so we can have different endpoints
        /// for projects, quotes and deletes but perform the backend logic the same. Note...  
        /// This function doesn't actually delete a project, but it deletes the swing table connecting users and projects.
        /// This is done to preserve the possibility of rollbacks.  
        /// </summary>
        /// TODO - Delete from one user, not everyone.  The UI doesn't call for this ... yet.
        /// <param name="Id">The Project to delete from the user</param>
        /// <param name="ProjectType">The name of the project type.  This is just used for messaging, not logic</param>
        /// <returns></returns>
        private bool ProjectDeleteFromUsers(int ProjectId, String ProjectType)
        {

            if(DBAdapter.getInstance().ProjectDeleteFromUsers(ProjectId)){
                @ViewBag.message = ProjectType+ " " +ProjectId.ToString()+" deleted";
                return true;
            }
            else
            {
                @ViewBag.message = ProjectType+" failed to delete";
                return false;
            }
        }


        /// <summary>
        /// "Deletes a project"... in actuality, it deletes the foreign key between a project with a type proposal and 
        /// any users.  In this way, we still have info for the proposal for rollback/analytic purposes.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Project(int Id)
        {
            @ViewBag.success = this.ProjectDeleteFromUsers(Id, "Project");
            return View("PassFail");
        }


       
        //============================================================================
        //  PROPOSAL
        //============================================================================
        //  Creates proposal
        // POST /json/proposal
        [HttpPost]
        public ActionResult Proposal(Project PostedProject)
        {

            int PossibleNewId = this.ProjectCopy(PostedProject, (int)Status.Proposal);
            if (PossibleNewId > 0)
            {
                @ViewBag.success = "true";
                @ViewBag.IdType = "Id";
                @ViewBag.message = "New Proposal Created";
                @ViewBag.Id = PossibleNewId;
            }
            else
            {
                @ViewBag.message = "Oops!  Something went wrong";
                @ViewBag.success = "false"; 
            }
            return View("PassFail");
        }

        /// <summary>
        /// "Deletes a proposal"... in actuality, it deletes the foreign key between a project with a type proposal and 
        /// any users.  In this way, we still have info for the proposal for rollback/analytic purposes.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Proposal(int Id)
        {
            @ViewBag.success = this.ProjectDeleteFromUsers(Id, "Proposal");
            return View("PassFail");
        }


        //============================================================================
        //  QUOTES
        //============================================================================

        /// <summary>
        /// This creates a quote.  
        /// </summary>
        /// <param name="Project"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Quote(ProjectWithCustomer ProjectWithCustomer)
        {
            if(ProjectWithCustomer.CustomerId == 0){
               @ViewBag.message = "You need to post 'CustomerID' along with the quote model to create a quote this way.";
               return View("passFail");
            }
            Project Project = ProjectWithCustomer.CloneToProject(ProjectWithCustomer);
            int NewQuoteId = this.ProjectCreateNew(Project);
            this.ProjectAttachCustomer(ProjectWithCustomer.CustomerId, NewQuoteId);
            if (NewQuoteId > 0)
            {
                @ViewBag.success = "true";
                @ViewBag.IdType = "Id";
                @ViewBag.message = "New Quote Created";
                @ViewBag.Id = NewQuoteId;
            }
            else
            {
               @ViewBag.message = "Oops!  Something went wrong";
            }
            return View("PassFail");
        }
        
        /// <summary>
        ///     This creates a quote from a parent.  It doesn't matter if the parent is a proposal or a project. 
        ///     Its a simple copy of everything 
        /// </summary>
        // Create Quote
        // POST: /json/quote
        [HttpPost]
        public ActionResult QuoteCopy(Project PostedProject)
        {
            int PossibleNewId = this.ProjectCopy(PostedProject, (int)Status.Quote);
            if (PossibleNewId > 0)
            {
                @ViewBag.success = "true";
                @ViewBag.IdType = "Id";
                @ViewBag.message = "New Quote Created";
                @ViewBag.Id = PossibleNewId;
            }
            else
            {
                if (RoleEnvironment.GetConfigurationSettingValue("SADBConnectionString") == "production")
                {
                    @ViewBag.message = "Oops!  Something went wrong";
                }
            }
            return View("PassFail");
        }



        /// <summary>
        /// "Deletes a quote"... in actuality, it deletes the foreign key between a project with a type proposal and 
        /// any users.  In this way, we still have info for the proposal for rollback/analytic purposes.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Quote(int Id)
        {
            @ViewBag.success = this.ProjectDeleteFromUsers(Id, "Quote");
            return View("PassFail");
        }






        //=============================================================================
        //  JSON/RoomsCategory
        //=============================================================================

        /// <summary>
        /// This returns a Mock JSON object of RoomsCategories.  It needs nested Categories and products
        /// </summary>
        /// <returns></returns>
        public ActionResult RoomsCategoryMock()
        {
            List<RoomsCategory> rooms = new List<RoomsCategory>();
            
            // Kitchen Mock
            RoomsCategory kitchen = new RoomsCategory();
            kitchen.Name = "Kitchen";
            rooms.Add(kitchen);

            ProductCategory Cooktops = new ProductCategory();
            Cooktops.Id = 1;
            Cooktops.Name = "Cooktops";

            ProductType GasCooktop = new ProductType();
            GasCooktop.Id = 1;
            GasCooktop.ProductTypeGUID = "GASCOOKTOP";
            GasCooktop.Name = "Gas cooktop";
            Cooktops.ProductTypeIndexOrAddById(GasCooktop);
            
            ProductType ElectricCooktop = new ProductType();
            ElectricCooktop.Id = 2;
            ElectricCooktop.ProductTypeGUID = "ELECTRICCOOKTOP";
            ElectricCooktop.Name = "Electric cooktop";
            Cooktops.ProductTypeIndexOrAddById(ElectricCooktop);
            kitchen.ProductCategoriesIndexOrAdd(Cooktops);

            ProductCategory Dishwashers = new ProductCategory();
            Dishwashers.Id = 2;
            Dishwashers.Name = "Dishwasher";

            ProductType BigDish = new ProductType();
            BigDish.ProductTypeGUID = "BIGDISHWASHER";
            BigDish.Id = 3;
            BigDish.Name = "World's biggest dishwasher";
            Dishwashers.ProductTypeIndexOrAddById(BigDish);

            ProductType SmallDish = new ProductType();
            SmallDish.ProductTypeGUID = "SMALLDISHWASHER";
            SmallDish.Id = 4;
            SmallDish.Name = "Boutique Dish washer";
            Dishwashers.ProductTypeIndexOrAddById(SmallDish);
            kitchen.ProductCategoriesIndexOrAdd(Dishwashers);


            // Bathroom Mock
            RoomsCategory bathroom = new RoomsCategory();
            bathroom.Name = "bathroom";
            rooms.Add(bathroom);

            ProductCategory Sinks = new ProductCategory();
            Sinks.Id = 11;
            Sinks.Name = "Sinks";

            ProductType WhiteSink = new ProductType();
            WhiteSink.ProductTypeGUID = "WHITESINK";
            WhiteSink.Id = 11;
            WhiteSink.Name = "Porcelain Sink";
            Sinks.ProductTypeIndexOrAddById(WhiteSink);

            ProductType BlackSink = new ProductType();
            BlackSink.ProductTypeGUID = "BLACKSINK";
            BlackSink.Id = 12;
            BlackSink.Name = "Obsidian Sink";
            Sinks.ProductTypeIndexOrAddById(BlackSink);
            bathroom.ProductCategoriesIndexOrAdd(Sinks);

            ProductCategory Showerheads = new ProductCategory();
            Showerheads.Id = 12;
            Showerheads.Name = "Showerheads";

            ProductType Rainmaker = new ProductType();
            Rainmaker.Id = 13;
            Rainmaker.ProductTypeGUID = "RAINMAKER";
            Rainmaker.Name = "Rainmaker";
            Showerheads.ProductTypeIndexOrAddById(Rainmaker);

            ProductType Waterfall = new ProductType();
            Waterfall.Id = 14;
            Waterfall.ProductTypeGUID = "WATERFALL";
            Waterfall.Name = "Boutique Dish washer";
            Showerheads.ProductTypeIndexOrAddById(Waterfall);
            bathroom.ProductCategoriesIndexOrAdd(Showerheads);


            // Outdoor Mock
            RoomsCategory Outdoor = new RoomsCategory();
            Outdoor.Name = "Outdoor";
            rooms.Add(Outdoor);

            ProductCategory Grills = new ProductCategory();
            Grills.Id = 21;
            Grills.Name = "Grills";

            ProductType Propane = new ProductType();
            Propane.ProductTypeGUID = "PROPANE";
            Propane.Id = 21;
            Propane.Name = "Propane Grill";
            Grills.ProductTypeIndexOrAddById(Propane);

            ProductType Charcoal = new ProductType();
            Charcoal.Id = 22;
            Charcoal.ProductTypeGUID = "CHARCOAL";
            Charcoal.Name = "Charcoal Grill";
            Grills.ProductTypeIndexOrAddById(Charcoal);
            Outdoor.ProductCategoriesIndexOrAdd(Grills);

            ProductCategory PatioFurniture = new ProductCategory();
            PatioFurniture.Id = 22;
            PatioFurniture.Name = "Patio Furniture";

            ProductType Chair = new ProductType();
            Chair.ProductTypeGUID = "PATIOCHAIR";
            Chair.Id = 23;
            Chair.Name = "Patio Chair";
            PatioFurniture.ProductTypeIndexOrAddById(Chair);

            ProductType Table = new ProductType();
            Table.Id = 14;
            Table.ProductTypeGUID = "PATIOTABLE";
            Table.Name = "Patio Table";
            PatioFurniture.ProductTypeIndexOrAddById(Table);
            Outdoor.ProductCategoriesIndexOrAdd(PatioFurniture);




            // Laundry Mock
            RoomsCategory Laundry = new RoomsCategory();
            Laundry.Name = "Laundry";
            rooms.Add(Laundry);

            ProductCategory Washer = new ProductCategory();
            Washer.Id = 31;
            Washer.Name = "Grills";

            ProductType EcoFriendly = new ProductType();
            EcoFriendly.Id = 31;
            EcoFriendly.ProductTypeGUID = "ECOFRIENDLY";
            EcoFriendly.Name = "Eco-Friendly Washer";
            Grills.ProductTypeIndexOrAddById(EcoFriendly);

            ProductType SuperClean = new ProductType();
            SuperClean.Id = 32;
            SuperClean.ProductTypeGUID = "SUPERCLEAN";
            SuperClean.Name = "SuperClean Washer";
            Washer.ProductTypeIndexOrAddById(SuperClean);
            Laundry.ProductCategoriesIndexOrAdd(Washer);

            ProductCategory Dryer = new ProductCategory();
            Dryer.Id = 32;
            Dryer.Name = "Driers";

            ProductType Huge = new ProductType();
            Huge.Id = 33;
            Huge.ProductTypeGUID = "HUGEDRIER";
            Huge.Name = "Huge Drier";
            Dryer.ProductTypeIndexOrAddById(Huge);

            ProductType Fast = new ProductType();
            Fast.Id = 34;
            Fast.ProductTypeGUID = "HUGEDRIER";
            Fast.Name = "Super Fast";
            Dryer.ProductTypeIndexOrAddById(Table);
            Laundry.ProductCategoriesIndexOrAdd(Dryer);

            
            return Json(rooms, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RoomCategoriesAll()
        {
            return Json(DBAdapter.getInstance().RoomsCategoryReadAll(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">This is actually the name of the category. I may add the ability to look up by numeric id later.</param>
        /// <returns></returns>
        public ActionResult RoomsCategory(String id)
        {
            return Json(DBAdapter.getInstance().CategoriesForRoomsCategory(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Category(String id)
        {
            return Json(DBAdapter.getInstance().ProductsForCategory(id), JsonRequestBehavior.AllowGet);
        }

        //============================================================================
        //  Users  (Users)
        //============================================================================

        // Get: JSON
        public ActionResult users(User userFromUI)
        {
            User user = DBAdapter.getInstance().GetUserById(userFromUI.id);
            return Json(user, JsonRequestBehavior.AllowGet);

        }


        //  This function takes a C# object, turns it into a JSON string
        //  then returns it to a C# dynamic object representing JSON.  This is only useful if you need 
        //  to deal with dictionaries as nested objects.  If  all you're using is Lists, then disregard
        //  this -BMW
        private dynamic JSONifyDictionary(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            var jss = new JavaScriptSerializer();
            dynamic data = jss.Deserialize<dynamic>(json);
            return data;
        }


        public ActionResult SalesAssociatesGetByStoreCode(int id)
        {
            List<User> users = DBAdapter.getInstance().GetUsersByStoreCode(id.ToString());  
            return Json(users, JsonRequestBehavior.AllowGet);
        }

    }
}
