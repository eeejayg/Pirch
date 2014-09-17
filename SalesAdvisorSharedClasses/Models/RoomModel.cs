using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{

    [DataContract]
    public class Room
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int FKRoomsCategory { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int FKParentRoom { get; set; }
        [DataMember]
        public int FKProject { get; set; }
        [DataMember]
        public RoomsCategory RoomCategory { get; set; }
    }
}