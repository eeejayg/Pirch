using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorWebRole.Models
{
    public class ProjectWithCustomer : Project
    {
        public int CustomerId {set; get;}
    }
}