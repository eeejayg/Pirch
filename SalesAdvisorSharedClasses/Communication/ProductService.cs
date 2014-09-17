using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorSharedClasses.Communication
{
    public class ProductServiceInfo
    {
        public static readonly String ENDPOINT_NAME = "net.tcp://{0}/ProductService";
    }

    [ServiceContract]
    public interface ProductService
    {

        [OperationContract]
        Dictionary<string,ProductType> ProductGetAll();

        [OperationContract]
        int ProductInstanceCreateOrUpdate(ProductInstance ProductInstance);

        [OperationContract]
        bool ProductInstanceDelete(int id);

        [OperationContract]
        List<SiteArea> SiteAreasForStoreCode(StoreInfo store, int areaType = 1);

        [OperationContract(Name="ProductsForSiteAreaId")]
        SiteArea ProductsForSiteArea(String siteAreaId);

        [OperationContract(Name = "ProductsForSiteArea")]
        SiteArea ProductsForSiteArea(SiteArea siteArea);

        [OperationContract]
        SiteArea GetSiteArea(String siteAreaId);

        [OperationContract]
        List<RoomsCategory> GetAllRoomsCategories();

        [OperationContract(Name = "CategoriesForRoomCategoryByName")]
        RoomsCategory CategoriesForRoomsCategory(String name);

        [OperationContract(Name = "CategoriesForRoomCategory")]
        RoomsCategory CategoriesForRoomsCategory(RoomsCategory roomcat);

        [OperationContract(Name = "ProductsForCategory")]
        ProductCategory ProductsForCategory(ProductCategory cat);

        [OperationContract(Name = "ProductsForCategoryByGuid")]
        ProductCategory ProductsForCategory(String guid);

        [OperationContract]
        ProductCategory ProductCategoryForGuid(String guid);
    }
}
