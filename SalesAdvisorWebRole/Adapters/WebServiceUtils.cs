using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Web;


namespace SalesAdvisorWebRole.Adapters
{
    public class WebServiceUtils
    {
        private static readonly String SERVICE_ENDPOINT_NAME = "GetDataEndpoint";

        private static RoleInstance GetRandomWorkerInstance()
        {
            RoleInstance selectedInstance = null;
            ICollection<RoleInstance> values = RoleEnvironment.Roles["SalesAdvisorWorkerRole"].Instances;
            if (values.Count() > 0) {
                Random rnd = new Random();
                selectedInstance = values.ElementAt<RoleInstance>(rnd.Next(values.Count()));
            }
            return selectedInstance;
        }

        public static Interface GetEndpointService<Interface>(String serviceUri)
        {
            RoleInstance role = WebServiceUtils.GetRandomWorkerInstance();
            RoleInstanceEndpoint endpoint = role.InstanceEndpoints[SERVICE_ENDPOINT_NAME];
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None, false);
            EndpointAddress address = new EndpointAddress(String.Format(serviceUri, endpoint.IPEndpoint));
            // actually open
            ChannelFactory<Interface> factory = new ChannelFactory<Interface>(binding, address);
            Interface client = factory.CreateChannel();
            return client;
        }
    }
}