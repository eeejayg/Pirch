using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorSharedClasses.Communication
{
    public class CustomerServiceInfo
    {
        public static readonly String ENDPOINT_NAME = "net.tcp://{0}/CustomerService";
    }

    [ServiceContract]
    public interface CustomerService
    {

        [OperationContract]
        int AddressCreateOrUpdate(Address address);

        [OperationContract]
        Customers CustomerGetByGUID(String CustomerGUID);
        
        [OperationContract]
        Customers CustomerGetById(int id);        
        
        [OperationContract]
        List<Customers> GetCustomerListBySalesAssociate(int Id);

    
        [OperationContract]
        bool CustomerUpdateDefaultAddress(int AddressId, int CustomerId);

        [OperationContract]
        int CustomerUpdateOrAdd(Customers customer);

    }
}
