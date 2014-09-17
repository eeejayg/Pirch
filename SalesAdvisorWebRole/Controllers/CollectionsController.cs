using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesAdvisorSharedClasses.Models;
using Dapper;

namespace SalesAdvisorWebRole.Controllers
{
    public class CollectionsController : BaseRequirementsController
    {
        private List<Customers> customers = new List<Customers>();

        //
        // GET: /Customers/customerId/Projects/projectId/Rooms/roomId/Products
        public ActionResult ShowByRoom(int customerId, int projectId, int roomId)
        {
            /*
            List<Collections> collections;
            using (var connection = this.openConnection())
            {
                collections = connection.Query<Collections>("Select * from collection").ToList();
                connection.Close();
            }
            */
            @ViewBag.Type = "room";

            return View("Show");
        }

        //
        // GET: /Customers/customerId/Quotes/quoteId/Products
        public ActionResult ShowByQuote(int customerId, int quoteId)
        {
            @ViewBag.Type = "quote";

            return View("Show");
        }

        //
        // GET: /Customers/customerId/Proposals/proposalId/Products
        public ActionResult ShowByProposal(int customerId, int proposalId)
        {
            @ViewBag.Type = "proposal";

            return View("Show");
        }

        //private void GetCollections()
        //{
        //    using (var connection = this.openConnection())
        //    {
        //        this.collections = connection.Query<Customers>("Select * from customers").ToList();
        //        connection.Close();
        //    }
        //}
    }
}
