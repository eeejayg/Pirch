using SalesAdvisorWebRole.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWebRole.Adapters;

namespace SalesAdvisorWebRole.Controllers
{
    public class HomeController : BaseRequirementsController
    {

        public User user = new User();


        //
        // GET: /Home/

        public ActionResult Index()
        {
            @ViewBag.user = SessionAdapter.getInstance().LoggedInUser;
            return View();
        }
    }
}
