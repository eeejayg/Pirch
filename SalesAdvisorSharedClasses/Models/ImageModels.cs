using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{
    [DataContract]
    public class ResponsiveImage
    {
        [DataMember]
        public String src;
        [DataMember]
        public String alt;
        [DataMember]
        public List<ResponsiveImageSrc> srcset;
        [DataMember]
        public String classname;

        public ResponsiveImage()
        {
            this.init();
        }

        public ResponsiveImage(String src)
        {
            this.init(src);
        }

        public ResponsiveImage(String src, String alt)
        {
            this.init(src, alt);
        }

        public ResponsiveImage(String src, String alt, String classname)
        {
            this.init(src, alt, classname);
        }

        private void init(String src = "", String alt = "", String classname = "")
        {
            this.src = src;
            this.alt = alt;
            this.classname = classname;
            this.srcset = new List<ResponsiveImageSrc>();
        }
    }

    public class ResponsiveImageSrc
    {
        public String src;
        public String media;
        public String classname;

        public ResponsiveImageSrc(String src = null, String media = null, String classname = null)
        {
            this.src = src;
            this.media = media;
            this.classname = classname;
        }
    }
}