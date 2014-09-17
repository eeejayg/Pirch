using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorSharedClasses.Communication
{
    public class UserServiceInfo
    {
        public static readonly String ENDPOINT_NAME = "net.tcp://{0}/UserService";
    }

    [ServiceContract]
    public interface UserService
    {
        [OperationContract]
        List<User> GetAllUsers();
        [OperationContract]
        User GetUserById(int id);
        [OperationContract]
        User GetUserByUsername(String username);
        [OperationContract]
        int AddUser(User user);
        [OperationContract]
        List<User> GetUsersByStoreCode(String storeCode);
    }
}
