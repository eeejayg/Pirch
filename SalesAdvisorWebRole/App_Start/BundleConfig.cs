using System.Web;
using System.Web.Optimization;

namespace SalesAdvisorWebRole
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            //------------------------- GLOBAL BUNDLES ----------------------------------
            bundles.Add(new ScriptBundle("~/bundles/global-scripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bb/underscore-1.4.4.js",
                        "~/Scripts/bb/underscore.strings.0.1.js",
                        "~/Scripts/bb/backbone-1.0.0-min.js",
                        "~/Scripts/bb/backbone-1.0.0.js",
                         "~/Scripts/bb/Backbone.BabySitter.0.0.6.js",
                         "~/Scripts/bb/backbone.wreqr.0.2.0.js",
                         "~/Scripts/bb/backbone.marionette.1.0.3.js",
                         "~/Scripts/bb/bb-init.js",
                        "~/Scripts/fsa-ui.js",
                        "~/Scripts/picturefill.js",
                        "~/Scripts/jquery.colorbox.js",
                        "~/Scripts/jquery.cookie.js"
                        ));
            //  The Base Styles for the site
            bundles.Add(new StyleBundle("~/base-styles/css").Include(
                "~/Content/htmldoctor.reset-1.6.1.css",
                "~/Content/colorbox.css",
                "~/Content/fonts/pirchFonts.css"
            ));


            //----------------- HomePage Bundles  -----------------------
            bundles.Add(new StyleBundle("~/homepage/css").Include(
                "~/Content/page-styles/homepage.css"
            ));
           

            //----------------- Login Bundles  -----------------------
            bundles.Add(new StyleBundle("~/login/css").Include(
                "~/Content/page-styles/login.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
               "~/Scripts/bb/models/login.js",
                "~/Scripts/bb/routers/login.js",
                "~/Scripts/bb/views/login/chooseFace.js",
                "~/Scripts/bb/views/login/loginScreen.js",
                "~/Scripts/bb/login.js"
            ));


            //----------------- Customers Bundles  -----------------------
            bundles.Add(new StyleBundle("~/customers/css").Include(
                "~/Content/page-styles/customers.css"
            ));
            bundles.Add(new ScriptBundle("~/bundles/customers").Include(
                "~/Scripts/bb/models/products.js",
                "~/Scripts/bb/models/customer.js",
                "~/Scripts/bb/models/user.js",
                "~/Scripts/bb/views/shared/tabBar.js",
                "~/Scripts/bb/views/shared/toolBar.js",
                "~/Scripts/bb/views/shared/layouts.js",
                "~/Scripts/bb/views/shared/headerBar.js",
                "~/Scripts/bb/views/customers/listCustomers.js",
                "~/Scripts/bb/views/customers/newCustomer.js",
                "~/Scripts/bb/views/customers/detailCustomer.js",
                "~/Scripts/bb/views/customers/customerProjects.js",
                "~/Scripts/bb/views/customers/layouts.js",
                "~/Scripts/bb/views/customers/editQuote.js",
                "~/Scripts/bb/views/project/singleCollection.js",
                "~/Scripts/bb/views/project/allCollections.js",
                "~/Scripts/bb/collections/customers.js",
                "~/Scripts/bb/routers/customers.js",
                "~/Scripts/bb/controllers/customers.js",
                "~/Scripts/bb/customers.js"
                ));

            //----------------- Products Bundles  -----------------------
            bundles.Add(new StyleBundle("~/products/css").Include(
                "~/Content/page-styles/products.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/products").Include(
                "~/Scripts/bb/models/products.js",
                "~/Scripts/bb/models/customer.js",
                "~/Scripts/bb/models/user.js",
                "~/Scripts/bb/views/shared/headerBar.js",
                "~/Scripts/bb/views/shared/searchBar.js",
                "~/Scripts/bb/views/shared/grid.js",
                "~/Scripts/bb/views/shared/layouts.js",
                "~/Scripts/bb/views/products/products.js",
                "~/Scripts/bb/views/products/siteAreasList.js",
                "~/Scripts/bb/routers/products.js",
                "~/Scripts/bb/controllers/products.js"
            ));

            //----------------- Projects Bundles  -----------------------
            bundles.Add(new ScriptBundle("~/bundles/projects").Include(
                "~/Scripts/bb/models/products.js",
                "~/Scripts/bb/models/customer.js",
                "~/Scripts/bb/models/project-add-rooms.js",
                "~/Scripts/bb/views/project/chooseProjectType.js",
                "~/Scripts/bb/views/project/chooseRooms.js",
                "~/Scripts/bb/views/project/chooseDate.js",
                "~/Scripts/bb/routers/projects.js"
                ));
            bundles.Add(new StyleBundle("~/projects/css").Include(
                 "~/Content/page-styles/projects.css"
            ));
        }
    }
}