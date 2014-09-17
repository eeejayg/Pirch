using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorWebRole.Adapters
{

    public class CustomerModelBinder : DefaultModelBinder
    {
//   I ended up not needing this data binder, but left it here for a reference for the future. - BMW
//        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
//            Customers PostedCustomer = new Customers();
//            var test = this;
//            return new Customers();
//        }
    }

}