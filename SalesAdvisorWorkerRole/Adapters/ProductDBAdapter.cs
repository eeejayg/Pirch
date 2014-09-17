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
    class ProductDBAdapter : IDBAdapter
    {
        private static readonly ProductDBAdapter instance = new ProductDBAdapter();

        public static ProductDBAdapter getInstance()
        {
            return instance;
        }

        private ProductDBAdapter()
        {
        }

        /**
         * Gets the appropriate Sql connection. If you're not using Dapper, you need to open the connection yourself.
         */
        public SqlConnection getConnection()
        {
            SqlConnection connection = new SqlConnection(RoleEnvironment.GetConfigurationSettingValue("ProductDBConnectionString"));
            connection.Open();
            return connection;
        }
    }
}
