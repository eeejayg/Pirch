using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorWebRole.Models
{
    /// <summary>
    ///   This adds in a customer to a product instance.  It is not needed at all
    ///   times, but can be convenient for automatically casting posts that shouldn't
    ///   be needlessly complicated.  
    /// </summary>
    public class ProductInstanceWithCustomer : ProductInstance
    {

        public int CustomerId {set; get;}
    }
}