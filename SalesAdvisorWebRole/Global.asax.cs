
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWebRole.Adapters;

namespace SalesAdvisorWebRole
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();        
       

            //  THis adds in a model data binder...  it allows us to perform some plumbing before type-casting the posted model to the controller
            //  They're in the WebRole/Adapters
//            AreaRegistration.RegisterAllAreas();
//            ModelBinders.Binders.Add(typeof(Customers), new CustomerModelBinder());
//  This is from the example code I found.  I'm unsure what it does so commented and left it - BMW
//            RegisterRoutes(RouteTable.Routes);

        }
    }


}