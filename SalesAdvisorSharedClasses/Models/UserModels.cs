using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{
    [DataContract]
    public class User
    {
        [DataMember]
        public int id = 0;
        [DataMember]
        public String userguid;
        [DataMember]
        public string firstName { get; set; }
        [DataMember]
        public string lastName { get; set; }
        [DataMember]
        public int roleid = 1;
        [DataMember]
        public string title;
        [DataMember]
        public string userName { get; set; }
        [DataMember]
        public string storeCode;
        [DataMember]
        public String primaryphone;
        [DataMember]
        public String email;
        [DataMember]
        public string imagePath;
        public string ImageString()
        {
            return this.imagePath == null ? "/Images/people/"+this.firstName+".png" : this.imagePath;
        }
    }

    [DataContract]
    public class UserRole
    {
        [DataMember]
        public String name;
        [DataMember]
        public String roleId;
    }
}