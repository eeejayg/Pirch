using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SalesAdvisorWebRole
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "RoomCollection",
                url: "Customers/{customerId}/Projects/{projectId}/Rooms/{roomId}/Products",
                defaults: new { controller = "Collections", action = "ShowByRoom", customerId = "", projectId = "", roomId = "" }
            );

            routes.MapRoute(
                name: "QuoteCollection",
                url: "Customers/{customerId}/Quotes/{quoteId}/Products",
                defaults: new { controller = "Collections", action = "ShowByQuote", customerId = "", quoteId = "" }
            );

            routes.MapRoute(
                name: "ProposalCollection",
                url: "Customers/{customerId}/Proposals/{proposalId}/Products",
                defaults: new { controller = "Collections", action = "ShowByProposal", customerId = "", proposalId = "" }
            );

            routes.MapRoute(
                name: "DefaultForCustomer",
                url: "Customers/{customerId}/{controller}/{action}/{id}",
                defaults: new { controller = "Logon", action = "Index", id = UrlParameter.Optional }
            );

            // Room categories tree

            routes.MapRoute(
                name: "JSONAllRoomCategories",
                url: "JSON/RoomsCategory/",
                defaults: new { controller = "JSON", action = "RoomCategoriesAll" }
            );

            // defaults

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "DefaultNoId",
                url: "{controller}/{action}",
                defaults: new { controller = "Login", action = "Index" }
            );
        }
    }
}