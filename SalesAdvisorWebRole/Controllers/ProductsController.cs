using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Dapper;
using SalesAdvisorWebRole.Adapters;
using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorWebRole.Controllers
{
    public class ProductsController : BaseRequirementsController
    {
        //
        // POST: /Products/Instances
        [HttpPost]
        public ActionResult Instances(ProductInstance instance)
        {
            return View();
        }

        //
        // GET: /Products/TestInstanceCreate
        [HttpGet]
        public ActionResult CreateInstance()
        {
            using (SqlConnection conn = DBAdapter.getInstance().getConnection()) {

                int id = DBAdapter.getInstance().StoredProcReturningId("ProductInstance_Create", new Dictionary<string, object>()
                {
                    { "@FKProductType", 1 },
                    { "@FKContainer", 1 },
                    { "@ContainerType", 3 },
                    { "@Rating", 2 }
                }, "@newid");

                return Json(new { success = true, id = id }, JsonRequestBehavior.AllowGet);
            }
        }

        //
        // GET: /Products
        public ActionResult Index()
        {
            @ViewBag.user = SessionAdapter.getInstance().LoggedInUser;
            return View();
        }

    }
}
