using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Reflection;

using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWebRole.Models;

using Dapper;


namespace SalesAdvisorWebRole.Controllers
{
    public class ProjectsController : BaseRequirementsController
    {

        //  This holds the responsive images for the rooms
        private Dictionary<String, ResponsiveImage> roomImages = new Dictionary<String, ResponsiveImage>();

        //
        // GET: /Projects/
        [HttpGet]
        public ActionResult Index(int customerId)
        {
            @ViewBag.custID = customerId;
            using (var connection = this.getConnection())
            {
                @ViewBag.projectTypes = connection.Query<ProjectType>("Select * from ProjectType").ToList();
                @ViewBag.rooms = connection.Query<RoomsCategory>("Select * from RoomsCategory").ToList();
                @ViewBag.images = this.CreateResponsiveImages();
                connection.Close();
            }

            return View();
        }
        [HttpPost]
        public ActionResult Index(int customerId, CreateProjectFromUI project)
        {
            using (var connection = this.getConnection())
            {
                try
                {

                    int newProjectId = this.CreateProject(customerId, project, connection);
                    this.CreateRooms(customerId, newProjectId, connection, project);
                    this.AttachProjectToCustomer(customerId, newProjectId, connection);

                    //  This creates a quote as well.  This needs to stop post beta
                    Project Quote = new Project();
                    Quote.FKParentProject = newProjectId;
                    JSONController jc = new JSONController();
                    jc.ProjectCopy(Quote, (int)Status.Quote);

                    @ViewBag.success = true;
                    @ViewBag.message = "Project created!";
                }
                catch(SqlException e)
                {
                    @ViewBag.success = false;
                    @ViewBag.message = e.Message;

                }

            
            }

            return View("passFail");
        }



        //  TODO - this should use the project service doing the same. 
        //  example in the JSONController
        private void AttachProjectToCustomer(int customerId, int newProjectId, SqlConnection connection)
        {
            var p = new DynamicParameters();
            p.Add("@FKCustomer", customerId);
            p.Add("@FKProject", newProjectId);
            connection.Execute("ProjectToCustomers_Create", p, commandType: CommandType.StoredProcedure);
        }


        //  There are four possible room types.  This iterates through the room types, figures out
        //  how many are on the project, and creates the room instances where need be.
        //  This could be done more elegantly, but I didn't figure that out in my timebox, so quick and 
        //  dirty it is - BMW.
        private void CreateRooms(int customerId, int newProjectId, SqlConnection connection, CreateProjectFromUI project)
        {
            RoomsCategory rc = new RoomsCategory();
            rc.Name = "laundry";
            for (var i = 0; i < project.laundry; i++)
            {
                var p = new DynamicParameters();
                p.Add("@FKRoomsCategory", rc.SpaceType);
                p.Add("@FKProject", newProjectId);
                connection.Execute("Room_Create", p, commandType: CommandType.StoredProcedure);
            }
            rc.Name = "bathroom";
            for (var i = 0; i < project.bathrooms; i++)
            {
                var p = new DynamicParameters();
                p.Add("@FKRoomsCategory", rc.SpaceType);
                p.Add("@FKProject", newProjectId);
                connection.Execute("Room_Create", p, commandType: CommandType.StoredProcedure);
            }
            rc.Name = "kitchen";
            for (var i = 0; i < project.kitchens; i++)
            {
                var p = new DynamicParameters();
                p.Add("@FKRoomsCategory", rc.SpaceType);
                p.Add("@FKProject", newProjectId);
                connection.Execute("Room_Create", p, commandType: CommandType.StoredProcedure);
            }
            rc.Name = "outdoor";
            for (var i = 0; i < project.outdoor; i++)
            {
                var p = new DynamicParameters();
                p.Add("@FKRoomsCategory", rc.SpaceType);
                p.Add("@FKProject", newProjectId);
                connection.Execute("Room_Create", p, commandType: CommandType.StoredProcedure);
            }  
        }


        private int CreateProject(int customerId, CreateProjectFromUI project, SqlConnection connection)
        {
            //  TODO - standardize this using the Projects Service.
            var p = new DynamicParameters();
            p.Add("@Deadline", project.dateComplete);
            p.Add("@Name", project.Name);
            p.Add("@FKProjectType", project.projectType);
            p.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
            p.Add("@FKStatus", (int)Status.Project);

            connection.Execute("Project_Create", p, commandType: CommandType.StoredProcedure);
           return p.Get<int>("@newid");
        }


        public Dictionary<String, ResponsiveImage> CreateResponsiveImages()
        {
            String mq = "media query?";
            ResponsiveImage imgKitchen = new ResponsiveImage();
            imgKitchen.src = "/Images/kitchen.jpg";
            imgKitchen.alt = "Kitchen";
            imgKitchen.srcset.Add(new ResponsiveImageSrc("/Images/kitchen.jpg", mq));
            roomImages.Add("kitchen", imgKitchen);

            ResponsiveImage imgBath = new ResponsiveImage();
            imgBath.src = "/Images/bathroom.jpg";
            imgBath.alt = "Bathroom";
            imgBath.srcset.Add(new ResponsiveImageSrc("/Images/bathroom.jpg", mq));
            roomImages.Add("bathroom", imgBath);

            ResponsiveImage Outdoor = new ResponsiveImage();
            Outdoor.src = "/Images/outdoor.jpg";
            Outdoor.alt = "Laundry";
            Outdoor.srcset.Add(new ResponsiveImageSrc("/Images/outdoor.jpg", mq));
            roomImages.Add("outdoor", Outdoor);

            ResponsiveImage  Laundry = new ResponsiveImage();
            Laundry.src = "/Images/laundry.jpg";
            Laundry.alt = "Laundry";
            Laundry.srcset.Add(new ResponsiveImageSrc("/Images/laundry.jpg", mq));
            roomImages.Add("laundry", Laundry);
            return roomImages;
        }

    }
}
