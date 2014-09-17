using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{
    public class Project : HasLists
    {
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public DateTime Deadline { get; set; }
        [DataMember]
        public List<Room> Rooms {set; get;}
        [DataMember]
        public ProjectType ProjectType { get; set; }
        [DataMember]
        public int FKParentProject { get; set; }
        [DataMember]
        public string Sunset{ get; set; }
        [DataMember]
        public int FKStatus{ get; set; }
        [DataMember]
        public int FKProjectType { get; set; }
        [DataMember]
        public DateTime Created { get; set; }
        
        
        [DataMember]
        public List<ProductInstance> ProductInstances {get;set;}
        [DataMember]
        public ProjectStatus ProjectStatus { get; set; }

        /// <summary>
        /// This function returns a project of type Project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public Project CloneToProject(Project ProjectToClone)
        {
            Project Project = new Project();
            Project.Id= ProjectToClone.Id;
            Project.Name = ProjectToClone.Name;
            Project.Deadline = ProjectToClone.Deadline;
            Project.Created = ProjectToClone.Created;
            Project.Sunset = ProjectToClone.Sunset;
            Project.FKParentProject = ProjectToClone.FKParentProject;
            Project.FKProjectType = ProjectToClone.FKProjectType;
            Project.FKStatus = ProjectToClone.FKStatus;

            return Project;
        }


        public int RoomIndexOrAdd(Room room)
        {
            if (room == null || room.Id == 0) { return -1; }
            if (this.Rooms == null)
            {
                this.Rooms= new List<Room>();
            }

            if (!this.Rooms.Any(item => item.Id == room.Id))
            {
                this.Rooms.Add(room);
            }
            return this.Rooms.FindIndex(p => p.Id == room.Id);
        }


        public int ProductInstancesIndexOrAdd(ProductInstance productInstance)
        {
            if (productInstance == null || productInstance.Id == 0) { return -1; }
            //  Create our projects list if it doesn't exist
            if (this.ProductInstances == null)
            {
                this.ProductInstances = new List<ProductInstance>();
            }
                
            if (!this.ProductInstances.Any(item => item.Id == productInstance.Id))
            {
                this.ProductInstances.Add(productInstance);
            }
            return this.ProductInstances.FindIndex(p => p.Id == productInstance.Id);
        }



    }



    
    
    //  This is a helper model built to collect POST data from the UI.
    public class CreateProjectFromUI
    {
        [DataMember]
        public int customerID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int projectType { get; set; }
        [DataMember]
        public string dateComplete { get; set; }
        [DataMember]
        public int kitchens { get; set; }
        [DataMember]
        public int bathrooms { get; set; }
        [DataMember]
        public int laundry { get; set; }
        [DataMember]
        public int outdoor { get; set; }
        
        
        
    }

    public class ProjectStatus : HasLists
    {
        [DataMember]
        public String Name { set; get; }
    
    }


    public class ProjectType
    {
        [DataMember]
        public int Id { set; get; }

        [DataMember]
        public string Name { set; get; }
    }


    public enum ProjectTypeEnum
    {
        Remodel = 1,
        NewHome = 2,
        Product = 3,
        JustBrowsing = 4
    }
    
    public enum Status {
        Project = 1,
	    Proposal = 2,
        Quote = 3
    };
}