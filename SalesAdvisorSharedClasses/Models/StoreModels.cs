using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{
    [DataContract]
    public class StoreInfo
    {
        [DataMember]
        public String StoreName;
        [DataMember]
        public String StoreCode;
    }
}