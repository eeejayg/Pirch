using SalesAdvisorSharedClasses.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace SalesAdvisorSharedClasses.Models
{
    ///===============================================================================
    /// <summary>
    /// 
    /// This is our master product model.  It pulls all of our various product types/ options/ instances into a logical whole.
    ///  This is NOT a product instance.  This represents a single product available for sale before it has 
    ///  been added to a collection 
    ///  
    /// The data for this comes mostly from the products db
    ///  </summary>
    ///  
    ///   !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    ///   !!!!!!!!!!!!!!!!!!!! NOT PRODUCT INSTANCE!!!!  If you're unsure of the difference, please ask
    ///   !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //===============================================================================
    public class ProductType : HasLists
    {
        [DataMember]
        public String SKU {set;get;}
        [DataMember]
        public String Description { set; get; }
        [DataMember]
        public String Name { set; get; }
        [DataMember]
        public String ProductTypeGUID { set; get; }
        [DataMember]
        public double ListPrice { set; get; }
        [DataMember]
        public double FullyLoaded { set; get; }
        [DataMember]
        public double MinimumPrice { set; get; }
        [DataMember]
        public DateTime EstimatedDelivery { set; get; }

        [DataMember]
        public List<String> Images { set; get; }

        [DataMember]
        public int FKRoomsCategory { set; get; }


        [DataMember]
        public List<ProductTypeOptionGroup> OptionGroups { set; get; }
        [DataMember]
        public List<ProductTypeAddOn> AddOns { set; get; }
        [DataMember]
        public RoomsCategory RoomCategory { set; get; }
        [DataMember]
        public Manufacturer Manufacturer { set; get; }
        [DataMember]
        public List<ProductCategory> ProductCategories { set; get; }

        public int ProductCategoryIndexOrAdd(ProductCategory category)
        {

            if (category == null || category.Id == 0) { return -1; }


            if (this.ProductCategories == null)
            {
                this.ProductCategories = new List<ProductCategory>();
            }

            if (!this.ProductCategories.Any(item => item.Id == category.Id))
            {
                this.ProductCategories.Add(category);
            }
            return this.ProductCategories.FindIndex(p => p.Id == category.Id);
        }




        public bool ImageAdd(String imgSrc)
        {

            if (imgSrc == null || imgSrc == "") { return false; }


            if (this.Images == null)
            {
                this.Images = new List<String>();
            }

            if (!this.Images.Contains(imgSrc))
            {
                this.Images.Add(imgSrc);
                return true;
            }
            return false;
        }



        public int ProductTypeOptionGroupIndexOrAdd(ProductTypeOptionGroup productTypeOptionGroup)
        {

            if (productTypeOptionGroup == null || productTypeOptionGroup.Id == 0) { return -1; }


            if (this.OptionGroups == null)
            {
                this.OptionGroups = new List<ProductTypeOptionGroup>();
            }

            if (!this.OptionGroups.Any(item => item.Id == productTypeOptionGroup.Id))
            {
                this.OptionGroups.Add(productTypeOptionGroup);
            }
            return this.OptionGroups.FindIndex(p => p.Id == productTypeOptionGroup.Id);
        }

        public int AddOnIndexOrAdd(ProductTypeAddOn addOn)
        {

            if (addOn == null || addOn.Id == 0) { return -1; }


            if (this.AddOns == null)
            {
                this.AddOns = new List<ProductTypeAddOn>();
            }

            if (!this.AddOns.Any(item => item.Id == addOn.Id))
            {
                this.AddOns.Add(addOn);
            }
            return this.OptionGroups.FindIndex(p => p.Id == addOn.Id);
        }

        public int OptionGroupIndexOrAdd(ProductTypeOptionGroup optionGroup){

             if (optionGroup == null || optionGroup.Id == 0) { return -1; }


            if (this.OptionGroups == null)
            {
                this.OptionGroups = new List<ProductTypeOptionGroup>();
            }

            if (!this.OptionGroups.Any(item => item.Id == optionGroup.Id))
            {
                this.OptionGroups.Add(optionGroup);
            }
            return this.OptionGroups.FindIndex(p => p.Id == optionGroup.Id);
        }
    
    }


    public class ProductTypeAddOns: HasLists {

    }

    /// <summary>
    ///     <para>Product Instance add ons are options additions to a project.  A project instance either does or does not have </para>
    /// </summary>
    public class ProductInstanceAddOn : HasLists
    {
        public String Name { set; get; }
        public int FKProductInstance { set; get; }
        public String GUID { set; get; }
    }


    public class ProductInstance : HasLists
    {
        public int FKProductType { set; get; }
        // TODO -- This is a legacy foreign key.  Clean this up... post beta -BMW
        [DataMember]
        public int FKParentInstance { set; get; }
        [DataMember]
        public int FKProject { set; get; }
        [DataMember]
        public int FKRoom { set; get; }
        [DataMember]
        public byte Rating { set; get; }
        [DataMember]
        public string ProductTypeGUID { set; get; }
        [DataMember]
        public double Price { set; get; }  //  Note.  This price is where we hold the specific, negotiated price for this product instance.  It will have to be checked back against actual Product Price.
 
        [DataMember]
        public List<ProductTypeOptions> ProductTypeOptions { set; get; }
        [DataMember]
        public List<ProductInstanceAddOn> ProductInstanceAddOns { set; get; }
        public int ProductTypeOptionsIndexOrAdd(ProductTypeOptions productTypeOption)
        {
 
            if (productTypeOption == null || productTypeOption.Id == 0) { return -1; }


            if (this.ProductTypeOptions == null)
            {
                this.ProductTypeOptions = new List<ProductTypeOptions>();
            }
 
            if (!this.ProductTypeOptions.Any(item => item.Id == productTypeOption.Id))
            {
                this.ProductTypeOptions.Add(productTypeOption);
            }
            return this.ProductTypeOptions.FindIndex(p => p.Id == productTypeOption.Id);
        }


        /// <summary>
        /// This object returns a clone of this product instance.  It was built because
        /// we couldn't properly serialize a child in the web role inheriting from the shared classes.
        /// </summary>
        /// <returns></returns>
        public ProductInstance Clone()
        {
            ProductInstance clonedProductInstance = new ProductInstance();
            clonedProductInstance.Id = this.Id;
            clonedProductInstance.FKParentInstance = this.FKParentInstance;
            clonedProductInstance.FKProject = this.FKProject;
            clonedProductInstance.FKRoom = this.FKRoom;
            clonedProductInstance.Rating = this.Rating;
            clonedProductInstance.ProductTypeGUID = this.ProductTypeGUID;
            return clonedProductInstance;
        }


        public int ProductAddOnIndexOrAdd(ProductInstanceAddOn productAddOn)
        {
            if (productAddOn == null || productAddOn.Id == 0) { return -1; }
            if (this.ProductInstanceAddOns== null)
            {
                this.ProductInstanceAddOns = new List<ProductInstanceAddOn>();
            }
            if (!this.ProductInstanceAddOns.Any(item => item.Id == productAddOn.Id))
            {
                this.ProductInstanceAddOns.Add(productAddOn);
            }
            return this.ProductInstanceAddOns.FindIndex(p => p.Id == productAddOn.Id);

        }
    }


    public class ProductTypeOptions : HasLists
    {
        [DataMember]
        public string Name { set; get; }
        [DataMember]
        public string Description { set; get; }
        [DataMember]
        public string GUID { get; set; }
    }

    public class ProductTypeOptionGroup : HasLists
    {
        [DataMember]
        public List<ProductTypeOptions> Options { set; get; }

        [DataMember]
        public Boolean AllowMultiples = true;

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool AllowMultiple { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string GUID { get; set; }

        public int OptionIndexOrAdd(ProductTypeOptions productTypeOption)
        {
            if (productTypeOption == null || productTypeOption.Id == 0) { return -1; }
            if (this.Options== null)
            {
                this.Options = new List<ProductTypeOptions>();
            }
            if (!this.Options.Any(item => item.Id == productTypeOption.Id))
            {
                this.Options.Add(productTypeOption);
            }
            return this.Options.FindIndex(p => p.Id == productTypeOption.Id);
        }
    }

    public class ProductCategory : HasLists
    {
        public String CategoryGuid
        {
            set
            {
                try
                {
                    this.CategoryId = Guid.Parse(value);
                }
                catch (Exception e)
                {
                    // do nothing
                }
            }
            get
            {
                return this.CategoryId.ToString();
            }
        }
        [DataMember]
        public Guid CategoryId { set; get; }
        [DataMember]
        public string Name{set;get;}
        [DataMember]
        public string Image { set; get; }
        
        [DataMember]
        public List<ProductType> ProductTypes { set; get; }


        public int ProductTypeIndexOrAddById(ProductType ProductType)
        {
            if (ProductType.Id == 0) { return -1; }
            if (this.ProductTypes == null)
            {
                this.ProductTypes = new List<ProductType>();
            }
            if (!this.ProductTypes.Any(item => item.Id == ProductType.Id))
            {
                this.ProductTypes.Add(ProductType);
            }
            return this.ProductTypes.FindIndex(p => p.Id == ProductType.Id);

        }

        public int ProductTypeIndexOrAddByGuid(ProductType productType)
        {
            if (productType.ProductTypeGUID == null) { return -1; }
            if (this.ProductTypes == null)
            {
                this.ProductTypes = new List<ProductType>();
            }
            int index = this.ProductTypes.FindIndex(item => item.ProductTypeGUID == productType.ProductTypeGUID);
            if (index == -1)
            {
                this.ProductTypes.Add(productType);
                index = this.ProductTypes.Count - 1;
            }
            return index;

        }
    }

    public class SiteArea
    {
        public enum AreaTypes {
            Vignette = 1
        };

        [DataMember]
        public Guid SiteAreaId {set; get;}
        [DataMember]
        public Guid ParentSiteAreaId { set; get; }
        [DataMember]
        public int AreaType { set; get; }
        [DataMember]
        public String AreaCode { set; get; }
        private String name;
        [DataMember]
        public String Name {
            set 
            {
                this.name = value == null ? null : value.Trim();
            }
            get 
            {
                return this.name;
            }
        }
        [DataMember]
        public List<ProductType> ProductTypes { set; get; }
        [DataMember]
        public List<String> ImageUrls { set; get; }

        public int ProductTypeIndexOrAddById(ProductType productType)
        {
            if (this.ProductTypes == null)
            {
                this.ProductTypes = new List<ProductType>();
            }
            int index = this.ProductTypes.FindIndex(p => p.Id == productType.Id);
            if (index == -1)
            {
                this.ProductTypes.Add(productType);
                index = this.ProductTypes.Count - 1;
            }
            return index;
        }

        public int ProductTypeIndexOrAddByGuid(ProductType productType)
        {
            if (this.ProductTypes == null)
            {
                this.ProductTypes = new List<ProductType>();
            }
            int index = this.ProductTypes.FindIndex(p => p.ProductTypeGUID == productType.ProductTypeGUID);
            if (index == -1)
            {
                this.ProductTypes.Add(productType);
                index = this.ProductTypes.Count - 1;
            }
            return index;
        }

        public int ImageIndexOrAdd(String imageUrl)
        {
            if (this.ImageUrls == null) {
                this.ImageUrls = new List<String>();
            }
            int index = this.ImageUrls.IndexOf(imageUrl);
            if (index == -1) {
                this.ImageUrls.Add(imageUrl.Trim());
                index = this.ImageUrls.Count;
            }
            return index;
        }
    }
}