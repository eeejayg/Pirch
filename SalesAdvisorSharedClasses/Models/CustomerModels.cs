using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{

    public class Address : HasLists
    {
        [DataMember]
        public string AddressLine1 { get; set; }
        [DataMember]
        public string AddressLine2 { get; set; }
        [DataMember]
        public string AddressLine3 { get; set; }
        [DataMember]
        public string PostalCode { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Country { get; set; }
    }

    public enum OrganizationTypes
    {
        Customer = 1,
        Company = 2
    };


    [KnownType(typeof(CreateCustomer))]
    public class Customers : HasLists
    {
        [DataMember]
        public String FirstName { get; set; }
        [DataMember]
        public String LastName { get; set; }
        [DataMember]
        public Boolean Owner { get; set; }
        [DataMember]
        public Boolean Architect { get; set; }
        [DataMember]
        public Boolean Designer { get; set; }
        [DataMember]
        public Boolean Builder { get; set; }
        [DataMember]
        public Boolean Vendor { get; set; }
        [DataMember]
        public Boolean Realtor { get; set; }
        [DataMember]
        public int FKDefaultShipping{ get; set; }
        [DataMember]
        public String CustomerGuid;

        [DataMember]
        public List<Project> Projects { set; get; }

        [DataMember]
        public List<Emails> Emails { set; get; }
        [DataMember]
        public List<PhoneNumbers> PhoneNumbers { set; get; }
        [DataMember]
        public List<Address> Addresses { set; get; }
        [DataMember]
        public Address DefaultShippingAddress { set; get; }

        public string Name()
        {   
            return this.FirstName + ' ' + this.LastName;
        }
        
        //  This checks to see if the project is already attached to this customer
        //  It returns -1 for fail (bad project data)
        //  or returns the id of the project -BMW
        public int ProjectIndexOrAdd(Project project)
        {
            if (project == null || project.Id == 0){return -1;}

            if (this.Projects == null)
            {
                this.Projects = new List<Project>();
            }
            if (!this.Projects.Any(item => item.Id == project.Id))
            {
                this.Projects.Add(project);
            }
            return this.Projects.FindIndex(p => p.Id == project.Id);
        }

        public int PhoneNumberIndexOrAdd(PhoneNumbers pn)
        {
            if (pn == null || pn.Id == 0) { return -1; }
            if (this.PhoneNumbers == null) 
            { 
                this.PhoneNumbers = new List<PhoneNumbers>();
            }
            if (!this.PhoneNumbers.Any(item => item.Id == pn.Id))
            {
                this.PhoneNumbers.Add(pn);
            }
            return this.PhoneNumbers.FindIndex(p => p.Id == pn.Id);
        }


        public int EmailIndexOrAdd(Emails Email)
        {
            if (Email == null || Email.Id == 0) { return -1; }
            
            {
                this.Emails= new List<Emails>();
            }
            if (!this.Emails.Any(item => item.Id == Email.Id))
            {
                this.Emails.Add(Email);
            }
            return this.Emails.FindIndex(p => p.Id == Email.Id);
        }

        public int AddressIndexOrAdd(Address Addy)
        {
            if (Addy == null || Addy.Id == 0) { return -1; }
            if (this.Addresses == null)
            {
                this.Addresses = new List<Address>();
            }
            if (!this.Addresses.Any(item => item.Id == Addy.Id))
            {
                this.Addresses.Add(Addy);
            }
            return this.Addresses.FindIndex(p => p.Id == Addy.Id);
        }

    }



    //  This extends our base Customers model
    //  for easy use with a post form  -BMW
    public class CreateCustomer : Customers
    {
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public Boolean Mobile { get; set; }
        public int EmailID { get; set; }
        public int PhoneID { get; set; }
    }

}