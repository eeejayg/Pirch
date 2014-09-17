using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{
    public class PhoneNumbers
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public int FKType { get; set; }
        public int FKCustomer { get; set; }
    }
}