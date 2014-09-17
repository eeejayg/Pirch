using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{
    public class Emails
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public int FKCustomer { get; set; }
    }
}