using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Net.Http;

using SalesAdvisorSharedClasses.Models;

using SalesAdvisorWebRole.Models;
using SalesAdvisorWebRole.Adapters;

namespace SalesAdvisorWebRole.Controllers
{
    //  This is the controller for the entire "Client Management 
    public class CustomersController : BaseRequirementsController
    {

        private List<Customers> customers = new List<Customers>();

        //
        // GET: /Customers/
        //  This is our list view.  It is the only one intended to be 
        //  rendered as a traditional page as opposed to as a single page app
        [HttpGet]
        public ActionResult Index()
        {
            @ViewBag.customerTypes = this.getCustomerTypes();
            @ViewBag.user = SessionAdapter.getInstance().LoggedInUser;
            @ViewBag.associates = DBAdapter.getInstance().GetAllUsers();
            @ViewBag.associateImages = this.PrepareAssociateImages(@ViewBag.associates);
            return View();
        }



        //  Foreach of the associates, lets attach a responsive image
        private Dictionary<int, ResponsiveImage> PrepareAssociateImages(List<User> associates)
        {
            Dictionary<int, ResponsiveImage> imgs = new Dictionary<int, ResponsiveImage>();
            for (int i = 0; i < associates.Count(); i++)
            {
                String mq = "TODO - What is this for?";
                ResponsiveImage img = new ResponsiveImage();
                img.src = associates[i].ImageString();
                img.alt = associates[i].firstName;
                img.srcset.Add(new ResponsiveImageSrc(img.src, mq));
                imgs.Add(associates[i].id, img);
            }
            return imgs;
        }



    
    }



}
