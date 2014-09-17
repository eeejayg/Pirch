using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SalesAdvisorSharedClasses.Models
{
    public class ProductTypeAddOn:HasLists
    {
        [DataMember]
        public string GUID { set; get; }
        [DataMember]
        public string Name { set; get; }
        [DataMember]
        public string Description { set; get; }
    }
}
