using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using SalesAdvisorSharedClasses.Communication;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWorkerRole.Adapters;
using SalesAdvisorWorkerRole.Logging;

using Dapper;

namespace SalesAdvisorWorkerRole.Services
{
    [ServiceBehavior(
        InstanceContextMode=InstanceContextMode.Single
        )]
    class UserServiceWorker : UserService
    {
        private SqlConnection getConnection()
        {
            return DBAdapter.getInstance().getConnection();
        }

        public List<User> GetAllUsers()
        {
            using (var connection = this.getConnection())
            {
                return connection.Query<User>("User_ReadAll", commandType: CommandType.StoredProcedure).ToList();
            }
        }

        public User GetUserByUsername(String username)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@uname", username);
                List<User> result = connection.Query<User>("User_ReadByUsername", p, commandType: CommandType.StoredProcedure).ToList();
                return result.Count > 0 ? result[0] : null;
            }
        }

        public User GetUserById(int id)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@id", id);
                List<User> result = connection.Query<User>("User_ReadById", p, commandType: CommandType.StoredProcedure).ToList();
                return result.Count > 0 ? result[0] : null;
            }
        }

        public User GetUserByGuid(String guid)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@guid", guid);
                List<User> result = connection.Query<User>("User_ReadByGuid", p, commandType: CommandType.StoredProcedure).ToList();
                return result.Count > 0 ? result[0] : null;
            }
        }

        public int AddUser(User user)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@firstname", user.firstName);
                p.Add("@lastname", user.lastName);
                p.Add("@username", user.userName);
                p.Add("@role", user.roleid);
                p.Add("@storeCode", user.storeCode);
                p.Add("@imagePath", user.imagePath);
                p.Add("@title", user.title);
                p.Add("@guid", user.userguid);
                p.Add("@primaryphone", user.primaryphone);
                p.Add("@email", user.email);
                p.Add("@newid", direction: ParameterDirection.ReturnValue, dbType: DbType.Int32);
                p.Add("@storeRow", direction: ParameterDirection.ReturnValue, dbType: DbType.Int32);
                connection.Execute("User_Add", p, commandType: CommandType.StoredProcedure);
                return p.Get<int>("@newid");
            }
        }

        public void UpdateUser(User user)
        {
            using (var connection = this.getConnection())
            {
                if (user.id == 0) {
                    // gotta use the guid
                    DynamicParameters p = new DynamicParameters();
                    p.Add("@firstname", user.firstName);
                    p.Add("@lastname", user.lastName);
                    p.Add("@username", user.userName);
                    p.Add("@role", user.roleid);
                    p.Add("@storeCode", user.storeCode);
                    p.Add("@imagePath", user.imagePath);
                    p.Add("@title", user.title);
                    p.Add("@guid", user.userguid);
                    p.Add("@primaryphone", user.primaryphone);
                    p.Add("@email", user.email);
                    connection.Execute("User_UpdateByGuid", p, commandType: CommandType.StoredProcedure);
                } else {
                    DynamicParameters p = new DynamicParameters();
                    p.Add("@firstname", user.firstName);
                    p.Add("@lastname", user.lastName);
                    p.Add("@username", user.userName);
                    p.Add("@role", user.roleid);
                    p.Add("@storeCode", user.storeCode);
                    p.Add("@imagePath", user.imagePath);
                    p.Add("@title", user.title);
                    p.Add("@id", user.id);
                    p.Add("@guid", user.userguid);
                    p.Add("@primaryphone", user.primaryphone);
                    p.Add("@email", user.email);
                    connection.Execute("User_UpdateById", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public List<User> GetUsersByStoreCode(String storeCode)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@storeCode", storeCode);
                IEnumerable<User> result = connection.Query<User>("User_ReadByStoreCode", p, commandType: CommandType.StoredProcedure);
                return result.ToList();
            }
        }

        public Dictionary<int, User> GetDictionaryOfUsersByStoreCode(String storeCode)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@storeCode", storeCode);
                var reader = connection.Query<User>("User_ReadByStoreCode", p, commandType: CommandType.StoredProcedure);
                return reader.ToDictionary(row => row.id);
            }
        }

        /// <summary>
        /// This actually just removes the row from UsersRoles, instead of deleting the user's row
        /// </summary>
        /// <param name="user"></param>
        public void DeleteUser(User user)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@id", user.id);
                var reader = connection.Execute("User_Delete", p, commandType: CommandType.StoredProcedure);
            }
        }

        public Boolean IsStoreCodeValid(String storecode)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@storeCode", storecode);
                List<StoreInfo> stores = connection.Query<StoreInfo>("StoreCode_ReadByCode", p, commandType: CommandType.StoredProcedure).ToList();
                return stores.Count > 0;
            }
        }
    }
}
