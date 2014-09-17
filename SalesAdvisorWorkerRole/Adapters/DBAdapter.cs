using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dapper;
using Microsoft.WindowsAzure.ServiceRuntime;
using SalesAdvisorSharedClasses.Models;

namespace SalesAdvisorWorkerRole.Adapters
{
    class DBAdapter : IDBAdapter
    {
        private static DBAdapter instance = new DBAdapter();

        public static DBAdapter getInstance()
        {
            return instance;
        }

        // instance stuff

        private DBAdapter()
        {
        }

        /**
         * Gets the appropriate Sql connection. If you're not using Dapper, you need to open the connection yourself.
         */
        public SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(RoleEnvironment.GetConfigurationSettingValue("SADBConnectionString"));
            connection.Open();
            return connection;
        }

        // Execute a stored procedure that returns an Id
        public int StoredProcReturningId(string proc, Dictionary<string, object> procParams, string returnName)
        {
            using (var connection = this.getConnection())
            {
                DynamicParameters dp = new DynamicParameters();
                foreach (KeyValuePair<string, object> p in procParams)
                {
                    dp.Add(p.Key, p.Value);
                }

                dp.Add("@newid", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
                connection.Execute(proc, dp, commandType: CommandType.StoredProcedure);
                return dp.Get<int>("@newid");
            }
        }
    }
}
