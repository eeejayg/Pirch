using SalesAdvisorWebRole.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.WindowsAzure.ServiceRuntime;

using SalesAdvisorWebRole.Adapters;
using SalesAdvisorWebRole.Filters;
using System.Data.Common;

namespace SalesAdvisorWebRole.Controllers
{
    /*
     * The entire application needs to be locked down several ways, so we need all controllers to inherit from this
     * base class to enforce security requirements.
     */
    [Authorize]
    /*[AuthorizeIPAddress]*/
    [RequireHttps]
    public class BaseRequirementsController : Controller
    {
        public static String GetCDNDomain()
        {
            return RoleEnvironment.GetConfigurationSettingValue("CDNDomain");
        }

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
 	        base.Initialize(requestContext);
            SessionAdapter.setSessionStateBase(Session);
            //SessionAdapter.getInstance().LoggedInUser = DBAdapter.getInstance().GetUserById(1);
        }

        /**
         * Gets the appropriate Sql connection. If you're not using Dapper, you need to open the connection yourself.
         */
        public SqlConnection getConnection(){
            return DBAdapter.getInstance().getConnection();
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        //  Simply returns a list of the different customer types available
        protected List<String> getCustomerTypes()
        {
            List<String> customerTypes = new List<String>();
            customerTypes.Add("Owner");
            customerTypes.Add("Architect");
            customerTypes.Add("Designer");
            customerTypes.Add("Builder");
            customerTypes.Add("Vendor");
            customerTypes.Add("Realtor");
            return customerTypes;
        }
        #endregion
    
    }

    



}
