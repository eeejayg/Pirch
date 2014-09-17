using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{
    [DataContract]
    public class HasId
    {
        [DataMember]
        public int Id { set; get; }
    }

    public class HasLists : HasId{
    

        //  TODO - There's going to be a bunch of XXXIndexOrAdds that are almost identical.
        //  We need to figure out how to implement this on the parent.  I timeboxed it, but didn't 
        //  get it figured out -BMW
        internal int IndexOrAdd(List<HasId> list, HasId toAdd)
        {
            if (toAdd == null || toAdd.Id == 0) { return -1; }
            return 1;
        }
    }
}