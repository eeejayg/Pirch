using Microsoft.SqlServer.Server;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using Dapper;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

using SalesAdvisorSharedClasses.Communication;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWebRole.Adapters;
using SalesAdvisorWebRole.Models;
using Winnovative;

namespace SalesAdvisorWebRole.Controllers
{
    public class TestbedController : BaseRequirementsController
    {
        //
        // GET: /Testbed/
        public ActionResult Index()
        {
            /* Example
            SqlConnection conn = this.openConnection();
            //
            SqlCommand command = new SqlCommand("teststoredproc", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@testtext", "foobar"));
            command.ExecuteNonQuery();
            //
            conn.Close();
             * */
            HttpCookie test = new HttpCookie("testCookie");
            test.Values["testValue"] = "This is a test value";
            test.Path = "/";
            test.Expires = DateTime.MaxValue;
            Response.Cookies.Add(test);
            return View();
        }

        [AllowAnonymous]
        public ActionResult Picturetest()
        {
            HttpCookie testCookie;
            if ((testCookie = Request.Cookies["testCookie"]) != null) {
                ViewBag.cookieValue = testCookie.Values["testValue"];
            }
            return View();
        }

        public ActionResult AddUser()
        {
            /*
            DBAdapter.getInstance().getConnection();
            ViewBag.newid = DBAdapter.getInstance().AddUser("Bob", "Dobbs", "bobdobbs", "foobar");
            DBAdapter.getInstance().closeConnection();
             * */
            return View();
        }

        public ActionResult GetUser()
        {
            SalesAdvisorSharedClasses.Models.User user;
            UserService client = WebServiceUtils.GetEndpointService<UserService>(UserServiceInfo.ENDPOINT_NAME);
            user = client.GetUserByUsername("Foobarbaz");
            List<User> list = client.GetUsersByStoreCode("10004");
            ViewBag.username = String.Format("List of users count: {0}", list.Count);
            return View();
        }
        
        public ActionResult UpdateProductType()
        {
            /*
            SqlConnection conn = this.openConnection();
            //
            SqlCommand command = new SqlCommand("UpdateProductType", conn);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Id", 1));
            command.Parameters.Add(new SqlParameter("@Name", "TestUpdate"));
            command.ExecuteNonQuery();
             * */
            //
            return View();
        }


        public ActionResult ProductInstanceCreate()
        {
            return View();
        }

        public ActionResult ProductInstanceDelete()
        {
            return View();
        }

        public ActionResult ProductInstanceUpdate(){
            return View();
        }

        
        public ActionResult CreateQuote(){

            return View();
        }

        public ActionResult StyleGuide()
        {
            return View();
        }

        public ActionResult SendMessage()
        {
            TestService client = WebServiceUtils.GetEndpointService<TestService>(TestServiceInfo.ENDPOINT_NAME);
            client.TestCall();
            return View();
        }

        public ActionResult AddressAddToCustomer(Address address)
        {
            return View("PassFail");
        }

        public ActionResult GimmePDF()
        {
            MemoryStream stream = new MemoryStream();
            Cookie c = new Cookie("StoreCookie", "StoreCookieStoreCode=10003", "/");
            CookieCollection collection = new CookieCollection();
            collection.Add(c);
            PdfConverter converter = new PdfConverter();
            converter.HttpRequestCookies.Add("StoreCookie", "StoreCookieStoreCode=10003");
            return File(converter.GetPdfBytesFromHtmlFile("https://cf0779ee14574ebeab5b1a52a87d8265.cloudapp.net/Login/Index"), "application/pdf");
        }

        public ActionResult GetSiteAreas()
        {
            ProductService serv = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            List<SiteArea> siteAreas = serv.SiteAreasForStoreCode(SessionAdapter.getInstance().GetCurrentStore(this.Request));
            ViewBag.siteAreas = siteAreas;
            return View();
        }

        [HttpGet]
        public ActionResult GetProductForSiteArea(String siteArea)
        {
            ProductService serv = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            SiteArea area = serv.ProductsForSiteArea(siteArea);
            ViewBag.siteArea = area;
            return View();
        }

        public ActionResult GetKitchenCategories()
        {
            ProductService serv = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            RoomsCategory room = new RoomsCategory();
            room.Name = "kitchen";
            RoomsCategory cats = serv.CategoriesForRoomsCategory(room);
            ViewBag.cats = cats;
            return View();
        }

        public ActionResult ProductsForCategory(String id)
        {
            ProductService serv = WebServiceUtils.GetEndpointService<ProductService>(ProductServiceInfo.ENDPOINT_NAME);
            ProductCategory cat = serv.ProductCategoryForGuid(id);
            cat = serv.ProductsForCategory(cat);
            ViewBag.category = cat;
            return View();
        }
    }
}
