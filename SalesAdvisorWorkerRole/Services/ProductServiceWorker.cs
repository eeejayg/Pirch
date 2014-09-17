using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using SalesAdvisorSharedClasses.Communication;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWorkerRole.Adapters;
using Dapper;

namespace SalesAdvisorWorkerRole.Services
{
    [ServiceBehavior(
        InstanceContextMode = InstanceContextMode.Single
        )]
    class ProductServiceWorker : ProductService
    {
        private SqlConnection getConnection()
        {
            return DBAdapter.getInstance().getConnection();
        }

        private SqlConnection getProductConnection()
        {
            return ProductDBAdapter.getInstance().getConnection();
        }

        /// <summary>
        /// The dictionary is indexed by ProductType guid
        /// </summary>
        /// <returns></returns>
        public Dictionary<String, ProductType> ProductGetAll()
        {
            Dictionary<string, ProductType> products = new Dictionary<string, ProductType>();
            //
            using (SqlConnection connection = this.getProductConnection())
            {
                DynamicParameters DynamicParameters = new DynamicParameters();
                DynamicParameters.Add("@StoreCode",  "521DA104-2AC8-DF11-ABB0-00155D046704");
                DynamicParameters.Add("@Date",  "7/30/2013");
                DynamicParameters.Add("@minPriceLevelId", "A56AB05B-017B-4474-8DD6-11BBA48BD2DA");
                DynamicParameters.Add("@maxPriceLevelId",  "CE954516-DFE0-4959-9493-FA22944FC2AA");
                ProductType productType = null;
                connection.Query<PirchProduct, PirchImage, PirchSeries, PirchBrand, int>(
                    "Artefact_Products_CollectAllWithPrice",
                    map: (product, image, series, brand) =>
                    {
                        if (product != null && product.MinimumPrice > -.000001) {
                            if (product.ProductId != null) {
                                product.ImagesIndexOrAdd(image);
                                product.series = series;
                                product.brand = brand;
                                try
                                {
                                    productType = products[product.ProductId.ToString()];
                                    this.AddPirchProductImagesToProductType(productType, product);
                                }
                                catch (Exception e)
                                {
                                    productType = this.ProductTypeForPirchProduct(product);
                                    products.Add(productType.ProductTypeGUID, productType);
                                    this.AddPirchProductImagesToProductType(productType, product);
                                }
                            }
                        }
                        return 1;
                    },param: DynamicParameters, commandType: CommandType.StoredProcedure,splitOn: "ImageId, BrandSeriesId, BrandSplit"
                );
                //
                return products;
            }
        }

        public Dictionary<string, ProductType> ProductGetAllMock()
        {
            Dictionary<string, ProductType> products = new Dictionary<string, ProductType>();
            ProductType product1 = new ProductType();
            product1.Id = 1;
            product1.ProductTypeGUID = "CABINETGUID";
            product1.Name = "Cabinet";
            product1.SKU = "AAABBBCCC111";
            product1.Description = "This cabinet is made of the finest foo";


            // First mock product
            ProductTypeOptionGroup ProductTypeOptionGroup1 = new ProductTypeOptionGroup();
            ProductTypeOptionGroup1.Id = 1;
            ProductTypeOptionGroup1.Name = "Wood";
            ProductTypeOptionGroup1.Description = "Choose what kind of wood you'd like for this cabinet";
            ProductTypeOptionGroup1.AllowMultiple = false;
            ProductTypeOptionGroup1.GUID = "WOODOPTIONGROUPGUID";


            // Product Options
            ProductTypeOptions WoodOption1 = new ProductTypeOptions();
            WoodOption1.Id = 1;
            WoodOption1.Name = "Oak";
            WoodOption1.Description = "Oak is great!";
            WoodOption1.GUID = "OAKOPTIONGUID";

            ProductTypeOptions WoodOption2 = new ProductTypeOptions();
            WoodOption2.Id = 2;
            WoodOption2.Name = "Walnut";
            WoodOption2.Description = "Walnut is great!";
            WoodOption2.GUID = "WALNUTOPTIONGUID";

            ProductTypeOptionGroup1.OptionIndexOrAdd(WoodOption1);
            ProductTypeOptionGroup1.OptionIndexOrAdd(WoodOption2);

            ProductTypeOptionGroup ProductTypeOptionGroup2 = new ProductTypeOptionGroup();
            ProductTypeOptionGroup2.Id = 2;
            ProductTypeOptionGroup2.Name = "Chrome";
            ProductTypeOptionGroup2.Description = "Choose what kind of chrome you'd like for this cabinet";
            ProductTypeOptionGroup2.AllowMultiple = false;
            ProductTypeOptionGroup2.GUID = "CHROMEOPTIONGROUPGUID";

            ProductTypeOptions ChromeOption1 = new ProductTypeOptions();
            ChromeOption1.Id = 3;
            ChromeOption1.Name = "Silver";
            ChromeOption1.Description = "Silver is great!";
            ChromeOption1.GUID = "SILVEROPTIONGUID";

            ProductTypeOptions ChromeOption2 = new ProductTypeOptions();
            ChromeOption2.Id = 4;
            ChromeOption2.Name = "Gold";
            ChromeOption2.Description = "Gold is great!";
            ChromeOption2.GUID = "GoldOPTIONGUID";

            ProductTypeOptionGroup2.OptionIndexOrAdd(ChromeOption1);
            ProductTypeOptionGroup2.OptionIndexOrAdd(ChromeOption2);

            product1.OptionGroupIndexOrAdd(ProductTypeOptionGroup1);
            product1.OptionGroupIndexOrAdd(ProductTypeOptionGroup2);
            


            //  ProductAddOns
            ProductTypeAddOn AddOn1 = new ProductTypeAddOn();
            AddOn1.Id = 1;
            AddOn1.Name = "Extra Drawer";
            AddOn1.GUID = "EXTRADRAWERGUID";
            AddOn1.Description = "This is an extra drawer if you'd like it";
            product1.AddOnIndexOrAdd(AddOn1);

            // Finally add the product
            products.Add(product1.ProductTypeGUID, product1);

            // Second mock product
            ProductType product2 = new ProductType();
            product2.Id = 2;
            product2.ProductTypeGUID = "TABLENOOPTIONS";
            product2.Name = "Stone Table";
            product2.SKU = "DDDEEEFFF222";
            product2.Description = "This table is a stone table with no options or add ons";
            products.Add(product2.ProductTypeGUID, product2);
            return products;
        }


        public int ProductInstanceCreateOrUpdate(ProductInstance productInstance)
        {
            using (var connection = this.getConnection())
            {

                DynamicParameters ParamsForProc = new DynamicParameters();
                ParamsForProc.Add("@FKRoom", productInstance.FKRoom);
                ParamsForProc.Add("@Rating", productInstance.Rating);
                ParamsForProc.Add("@Price", productInstance.Price);
                if (productInstance.Id != 0)
                {
                    ParamsForProc.Add("@Id", productInstance.Id);
                    connection.Execute("ProductInstance_Update", ParamsForProc, commandType: CommandType.StoredProcedure);
                    return productInstance.Id;
                }
                //  These param should not be updated.  If any of these are updated, then its inherantly a different Product Instance.
                ParamsForProc.Add("@FKParentInstance", productInstance.FKParentInstance);
                ParamsForProc.Add("@FKProject", productInstance.FKProject);
                ParamsForProc.Add("@ProductTypeGUID", productInstance.ProductTypeGUID);
                ParamsForProc.Add("@newid", DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute("ProductInstance_Create", ParamsForProc, commandType: CommandType.StoredProcedure);
                return ParamsForProc.Get<int>("@newid"); 
            }
            
        }

        public bool ProductInstanceDelete(int id)
        {            
            using (var connection = this.getConnection())
            {
                DynamicParameters ParamsForProc = new DynamicParameters();
                ParamsForProc.Add("@Id", id);
                connection.Execute("ProductInstance_Delete", ParamsForProc, commandType: CommandType.StoredProcedure);
                return true; 
            }
        }

        public List<SiteArea> SiteAreasForStoreCode(StoreInfo store, int areaType = 1)
        {
            using (var connection = this.getProductConnection()) {
                List<SiteArea> result = new List<SiteArea>();
                //
                String displayInventoryCode = this.GetDisplayInventoryCodeForStorecode(store.StoreCode);
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@storecode", displayInventoryCode);
                dp.Add("@areatype", areaType);
                connection.Query<SiteArea, PirchImage, int>(
                    @"select Inventory.SiteArea.*, Inventory.SiteAreaImage.SiteAreaImageId as ImageId, Inventory.SiteAreaImage.ImageUrl, Inventory.SiteAreaImage.Sequence 
                    from (Inventory.SiteArea left join Inventory.Site on Inventory.SiteArea.SiteId = Inventory.Site.SiteId) 
                    left join Inventory.SiteAreaImage on Inventory.SiteArea.DefaultSiteAreaImageId = Inventory.SiteAreaImage.SiteAreaImageId 
                    where Inventory.Site.Code = @storecode and Inventory.SiteArea.AreaType = @areatype",
                    param: dp,
                    map: (sitearea, image) => {
                        //
                        int siteIndex = result.FindIndex(p => p.SiteAreaId == sitearea.SiteAreaId);
                        if (siteIndex == -1) {
                            result.Add(sitearea);
                        }
                        if (image != null) {
                            sitearea.ImageIndexOrAdd(image.ImageUrl);
                        }
                        //
                        return 1;
                    },
                    splitOn: "ImageId"
                );
                //
                return result;
            }
        }

        public SiteArea GetSiteArea(String siteAreaId)
        {
            using (SqlConnection connection = this.getProductConnection())
            {
                SiteArea area = null;
                //
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@siteAreaId", siteAreaId);
                connection.Query<SiteArea, PirchImage, int>(
                    @"select Inventory.SiteArea.*, Inventory.SiteAreaImage.SiteAreaImageId as ImageId, Inventory.SiteAreaImage.ImageUrl, Inventory.SiteAreaImage.Sequence 
                    from Inventory.SiteArea left join Inventory.SiteAreaImage on Inventory.SiteArea.DefaultSiteAreaImageId = Inventory.SiteAreaImage.SiteAreaImageId
                    where Inventory.SiteArea.SiteAreaId = @siteAreaId",
                     param: dp,
                     map: (sitearea, image) =>
                     {
                         //
                         sitearea.ImageIndexOrAdd(image.ImageUrl);
                         area = sitearea;
                         //
                         return 1;
                     },
                    splitOn: "ImageId"
                );
                //
                return area;
            }
        }

        public SiteArea ProductsForSiteArea(String siteAreaId)
        {
            SiteArea area = this.GetSiteArea(siteAreaId);
            return this.ProductsForSiteArea(area);
        }

        // TODO - Add pricing
        public SiteArea ProductsForSiteArea(SiteArea siteArea)
        {
            using (SqlConnection connection = this.getProductConnection()) {
                // populate the site area's products list
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@siteareaid", siteArea.SiteAreaId);
                connection.Query<PirchProduct, PirchImage, PirchSeries, PirchBrand, int>(
                    // This still needs to account for sequence
                    @"select 
                        ProductCatalog.Product.*, ProductCatalog.ProductImage.ProductImageId as ImageId, ProductCatalog.ProductImage.ImageUrl, ProductCatalog.ProductImage.Sequence,
                        ProductCatalog.BrandSeries.BrandSeriesId, ProductCatalog.BrandSeries.BrandId, ProductCatalog.BrandSeries.Name, ProductCatalog.BrandSeries.Description,
                        ProductCatalog.Brand.BrandId as BrandSplit, ProductCatalog.Brand.ManufacturerId, ProductCatalog.Brand.Name, ProductCatalog.Brand.Description
                    from
                        Inventory.ProductBin join ProductCatalog.Product on Inventory.ProductBin.ProductId = ProductCatalog.Product.ProductId 
                        full outer join ProductCatalog.ProductImage on ProductCatalog.Product.ProductId = ProductCatalog.ProductImage.ProductId
                        left join ProductCatalog.BrandSeries on ProductCatalog.Product.BrandSeriesId = ProductCatalog.BrandSeries.BrandSeriesId
                        left join ProductCatalog.Brand on ProductCatalog.BrandSeries.BrandId = ProductCatalog.Brand.BrandId
                        left join ProductCatalog.Manufacturer on ProductCatalog.Brand.ManufacturerId = ProductCatalog.Manufacturer.ManufacturerId
                    where
                        Inventory.ProductBin.SiteAreaId = @siteareaid",
                    param: dp,
                    map: (product, image, series, brand) => {
                        if (product != null)
                        {
                            if (product.ProductId != null)
                            {
                                product.ImagesIndexOrAdd(image);
                                product.series = series;
                                product.brand = brand;
                                int i = siteArea.ProductTypeIndexOrAddByGuid(this.ProductTypeForPirchProduct(product));
                                if (i != -1) {
                                    this.AddPirchProductImagesToProductType(siteArea.ProductTypes[i], product);
                                }
                            }
                        }
                        
                        return 1;
                    },
                    splitOn: "ImageId, BrandSeriesId, BrandSplit"
                );
                //
                return siteArea;
            }
        }

        public List<RoomsCategory> GetAllRoomsCategories()
        {
            using (SqlConnection connection = this.getConnection()) {
                List<RoomsCategory> result = null;
                result = connection.Query<RoomsCategory>("RoomsCategory_ReadAll", commandType:CommandType.StoredProcedure).ToList();
                return result;
            }
        }

        public RoomsCategory CategoriesForRoomsCategory(String name)
        {
            RoomsCategory cat = this.RoomCategoryForName(name);
            return this.CategoriesForRoomsCategory(cat);
        }

        public RoomsCategory CategoriesForRoomsCategory(RoomsCategory roomcat)
        {
            if (roomcat == null) {
                return roomcat;
            }
            List<ProductCategory> cats = null;
            using (SqlConnection connection = this.getProductConnection()) {
                //
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@spacetype", roomcat.SpaceType);
                cats = connection.Query<ProductCategory>(@"select 
                    *
                from 
                    ProductCatalog.Category
                where
                    ProductCatalog.Category.SpaceType = @spacetype", dp).ToList();
            }
            // Hack the product images! Get this in a better way when we have them
            using (SqlConnection connection = this.getConnection())
            {
                DynamicParameters dp = null;
                List<TempImage> imgs = null;
                foreach (ProductCategory cat in cats) {
                    dp = new DynamicParameters();
                    dp.Add("@catguid", cat.CategoryGuid);
                    imgs = connection.Query<TempImage>("select * from ProductCategoryImages where CategoryGuid = @catguid", dp).ToList();
                    if (imgs.Count > 0) {
                        cat.Image = imgs.First().ImageUrl;
                    }
                }
            }
            roomcat.ProductCategories = cats;
            //
            return roomcat;
        }

        private RoomsCategory RoomCategoryForName(String name)
        {
            using (SqlConnection connection = this.getConnection()) {
                RoomsCategory cat = null;
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@name", name);
                List<RoomsCategory> list = connection.Query<RoomsCategory>("RoomsCategory_ReadByName", dp, commandType: CommandType.StoredProcedure).ToList();
                if (list.Count > 0) {
                    cat = list.First();
                }
                return cat;
            }
        }

        public ProductCategory ProductsForCategory (String guid) {
            ProductCategory cat = this.ProductCategoryForGuid(guid);
            return this.ProductsForCategory(cat);
        }

        // TODO - needs pricing
        public ProductCategory ProductsForCategory(ProductCategory cat)
        {
            using (SqlConnection connection = this.getProductConnection()) {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@guid", cat.CategoryGuid);
                connection.Query<PirchProduct, PirchImage, PirchSeries, PirchBrand, int>(@"select 
                        ProductCatalog.Product.*, ProductCatalog.ProductImage.ProductImageId as ImageId, ProductCatalog.ProductImage.ImageUrl, ProductCatalog.ProductImage.Sequence,
                        ProductCatalog.BrandSeries.BrandSeriesId, ProductCatalog.BrandSeries.BrandId, ProductCatalog.BrandSeries.Name, ProductCatalog.BrandSeries.Description,
                        ProductCatalog.Brand.BrandId as BrandSplit, ProductCatalog.Brand.ManufacturerId, ProductCatalog.Brand.Name, ProductCatalog.Brand.Description
                    from
                        ProductCatalog.Category left join ProductCatalog.BrandCategory on ProductCatalog.Category.CategoryId = ProductCatalog.BrandCategory.CategoryId
                        right join ProductCatalog.Product on ProductCatalog.Product.BrandCategoryId = ProductCatalog.BrandCategory.BrandCategoryId
                        left join ProductCatalog.ProductImage on ProductCatalog.Product.ProductId = ProductCatalog.ProductImage.ProductId
                        left join ProductCatalog.BrandSeries on ProductCatalog.Product.BrandSeriesId = ProductCatalog.BrandSeries.BrandSeriesId
                        left join ProductCatalog.Brand on ProductCatalog.BrandSeries.BrandId = ProductCatalog.Brand.BrandId
                        left join ProductCatalog.Manufacturer on ProductCatalog.Brand.ManufacturerId = ProductCatalog.Manufacturer.ManufacturerId
                    where
                        ProductCatalog.Category.CategoryId = @guid",
                    param:dp,
                    map: (product, image, series, brand) =>
                    {
                        if (product != null)
                        {
                            if (product.ProductId != null)
                            {
                                product.ImagesIndexOrAdd(image);
                                product.series = series;
                                product.brand = brand;
                                int i = cat.ProductTypeIndexOrAddByGuid(this.ProductTypeForPirchProduct(product));
                                if (i != -1)
                                {
                                    this.AddPirchProductImagesToProductType(cat.ProductTypes[i], product);
                                }
                            }
                        }
                        return 1;
                    },
                    splitOn: "ImageId, BrandSeriesId, BrandSplit"
                );
            }
            return cat;
        }

        public ProductCategory ProductCategoryForGuid(String guid)
        {
            ProductCategory result = null;
            using(SqlConnection connection = this.getProductConnection()) {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@guid", guid);
                List<ProductCategory> cats = connection.Query<ProductCategory>(@"select 
                    * 
                from 
                    ProductCatalog.Category
                where
                    ProductCatalog.Category.CategoryId = @guid", dp).ToList();
                if (cats.Count == 0) {
                    return null;
                }
                result = cats.First();
            }
            // HACKEY HACK for images
            using (SqlConnection connection = this.getConnection()) {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("@catguid", result.CategoryGuid);
                List<TempImage> imgs = connection.Query<TempImage>("select * from ProductCategoryImages where CategoryGuid = @catguid", dp).ToList();
                if (imgs.Count > 0)
                {
                    result.Image = imgs.First().ImageUrl;
                }
            }
            return result;
        }

        // This doesn't add images to the ProductType
        private ProductType ProductTypeForPirchProduct(PirchProduct pirchProduct)
        {
            ProductType product = new ProductType();
            //
            product.SKU = pirchProduct.SKU;
            product.Description = pirchProduct.Description;
            product.Name = pirchProduct.Name;
            product.ProductTypeGUID = pirchProduct.ProductId.ToString();
            product.MinimumPrice = Math.Round(pirchProduct.MinimumPrice, 2);
            product.FKRoomsCategory = pirchProduct.SpaceType;
            product.ListPrice = pirchProduct.ListPrice;
            if (pirchProduct.brand != null) {
                product.Manufacturer = this.ManufacturerForPirchManufacturer(pirchProduct.brand);
                if (pirchProduct.series != null) {
                    product.Manufacturer.LineIndexOrAdd(this.LineForPirchSeries(pirchProduct.series));
                }
            }
            //
            return product;
        }

        private void AddPirchProductImagesToProductType(ProductType product, PirchProduct pirchProduct)
        {
            if (pirchProduct.Images != null)
            {
                foreach (PirchImage image in pirchProduct.Images)
                {
                    String finalSrc = "";
                    if (image.ImageUrl != null)
                    {
                        if (String.Compare(image.ImageUrl.Substring(0, 7), "http://") == 0)
                        {
                            finalSrc = image.ImageUrl;
                        }
                        else
                        {
                            finalSrc = "productimages/" + image.ImageUrl;
                        }
                    }
                    product.ImageAdd(finalSrc);
                }
            }
        }

        private Manufacturer ManufacturerForPirchManufacturer(PirchBrand pirchManufacturer) {
            Manufacturer manufacturer = new Manufacturer();
            manufacturer.ManufacturerGuid = pirchManufacturer.BrandId.ToString();
            manufacturer.Name = pirchManufacturer.Name;
            //
            return manufacturer;
        }

        private Line LineForPirchSeries(PirchSeries series)
        {
            Line line = new Line();
            //
            line.LineGuid = series.BrandSeriesId.ToString();
            line.Name = series.Name;
            //
            return line;
        }

        /// <summary>
        /// This will be replaced by something better when Jim creates the relation
        /// </summary>
        /// <param name="storecode">The storecode</param>
        /// <returns></returns>
        private String GetDisplayInventoryCodeForStorecode(String storecode)
        {
            // We need to swap the first "1" in the storecode for a "3" to get the display inventory for a store.
            char[] sb = storecode.ToCharArray();
            sb[0] = "3".ToCharArray()[0];
            return new String(sb);
        }

        //  TODO - Can we change the StoredProc to use AS <OurModelName> and eliminate a bunch of models?   
        protected class PirchProduct
        {
            public Guid ProductId;
            public String ProductNumber;
            public String ProductKey;
            public String SKU;
            public float ListPrice;
            public float MinimumPrice;
            public String StrippedSKU;
            public String Name;
            public String Description;
            public List<PirchImage> Images;
            public PirchManufacturer manufacturer;
            public PirchBrand brand;
            public PirchSeries series;
            public int SpaceType;
            public int ImagesIndexOrAdd(PirchImage image)
            {
                if (image == null) {
                    return -1;
                }
                if (this.Images == null) {
                    this.Images = new List<PirchImage>();
                }
                int index = this.Images.FindIndex(p => p.ImageId == image.ImageId);
                if (index == -1) {
                    this.Images.Add(image);
                    index = this.Images.Count - 1;
                }
                //
                return index;
            }
        }

        protected class PirchImage
        {
            // Images are split across several tables, use this alias for the ID in your query.
            public Guid ImageId;
            public String ImageUrl;
            public int Sequence;
        }


        // We never really want to show manufacturer, only brand. We need to change the models.
        protected class PirchManufacturer
        {
            public Guid ManufacturerId;
            public String Name;
            public String Description;
        }

        protected class PirchBrand
        {
            public Guid BrandId;
            public Guid ManufacturerId;
            public String Name;
            public String Description;
        }

        // BrandSeries in the DB. Maps to Line
        protected class PirchSeries
        {
            public Guid BrandSeriesId;
            public Guid BrandId;
            public String Name;
            public String Description;
        }

        /* TEMP CLASS! This will go away when we have real category images! */
        protected class TempImage
        {
            public int id;
            public Guid CategoryGuid;
            public String ImageUrl;
        }
    }
}
