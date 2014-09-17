using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SalesAdvisorSharedClasses.Models
{
    /// <summary>
    /// This manufacturer class's data is in the Products DB
    /// </summary>
    public class Manufacturer : HasLists
    {
        [DataMember]
        public String ManufacturerGuid { set; get; }
        [DataMember]
        public int Id { set; get; }
        [DataMember]
        public String Name { set; get; }
        [DataMember]
        public List<Line> Lines { set; get; }

        public int LineIndexOrAdd(Line Line)
        {

            if (Line == null) { return -1; }

            if (this.Lines == null)
            {
                this.Lines = new List<Line>();
            }

            if (!this.Lines.Any(item => item.LineGuid == Line.LineGuid))
            {
                this.Lines.Add(Line);
            }
            return this.Lines.FindIndex(p => p.LineGuid == Line.LineGuid);
        }

    
    }


    /// <summary>
    /// This Line class's data is in the Products DB
    /// </summary>
    public class Line : HasLists
    {
        [DataMember]
        public String LineGuid { set; get; }
        [DataMember]
        public int Id { set; get; }
        [DataMember]
        public string Name { set; get; }        
        [DataMember]
        public List<ProductType> ProductTypes { set; get; }
    }


    /// <summary>
    /// The data to populate this class is in the Products DB
    /// </summary>

}
