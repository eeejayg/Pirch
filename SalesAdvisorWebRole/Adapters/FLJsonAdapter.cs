using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

using Microsoft.WindowsAzure.ServiceRuntime;

namespace SalesAdvisorWebRole.Adapters
{
    /*
     * Responsible for dealing with REST calls to the fixtures living backend
     */
    public class FLJsonAdapter
    {
        private static FLJsonAdapter instance = new FLJsonAdapter();
        // Endpoints
        private static String ENDPOINT_LIST_USERS = "";

        public static FLJsonAdapter getInstance() {
            return instance;
        }

        // instance stuff
        private String serverUrl;
        private String _storeId;
        public String storeId
        {
            set { this._storeId = value; }
            get { return this._storeId; }
        }

        private FLJsonAdapter()
        {
            this.serverUrl = RoleEnvironment.GetConfigurationSettingValue("FLEndpointServer");
        }

        private String constructEndpointUrl(String endpoint)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.serverUrl);
            builder.Append(endpoint);
            return builder.ToString();
        }
    }
}