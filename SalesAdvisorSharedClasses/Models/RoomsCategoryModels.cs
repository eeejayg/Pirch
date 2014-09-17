using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{
    public class RoomsCategory : HasLists
    {
        public enum Categories
        {
            nothing = 0,
            kitchen = 1,
            bathroom = 2,
            outdoor = 3,
            laundry = 4
        }


        public RoomsCategory.Categories Category { set; get; }
        public int SpaceType {
            set
            {
                try
                {
                    this.Category = (Categories)Enum.ToObject(typeof(Categories), value);
                }
                catch (Exception e) 
                {
                    // do nothing
                }
            }
            get
            {
                return (int)this.Category;
            }
        }

        [DataMember]
        public string Name
        {
            set
            {
                try
                {
                    this.Category = (Categories)Enum.Parse(typeof(Categories), value.ToLower());
                }
                catch (Exception e)
                {
                    // Do nothing
                }
            }
            get
            {
                return Enum.GetName(typeof(RoomsCategory.Categories), this.Category);
            }
        }

        [DataMember]
        public List<ProductCategory> ProductCategories { set; get; }


        public int ProductCategoriesIndexOrAdd(ProductCategory ProductCategory)
        {
            if (ProductCategory.Id == 0) { return -1; }
            if (this.ProductCategories == null)
            {
                this.ProductCategories = new List<ProductCategory>();
            }

            if (!this.ProductCategories.Any(item => item.Id == ProductCategory.Id))
            {
                this.ProductCategories.Add(ProductCategory);
            }
            return this.ProductCategories.FindIndex(p => p.Id == ProductCategory.Id);
        }

        public List<String> GetCategories()
        {
            return Enum.GetNames(typeof(Categories)).ToList<String>();
        }
    }
}