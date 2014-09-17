using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using SalesAdvisorWorkerRole.Logging;

namespace SalesAdvisorWorkerRole.Services
{
    class WorkerServiceStarter<Type, Interface> where Type: new()
    {
        private String uri;
        private String endpointname;
        private ServiceHost host;

        public WorkerServiceStarter(String uri, String endpointname)
        {
            this.uri = uri;
            this.endpointname = endpointname;
        }

        private void RestartService(Object sender, EventArgs e)
        {
            DebugLog.Log(String.Format("ServiceHost for {0} faulted. Restarting.", typeof(Type).Name));
            host.Abort();
            Thread.Sleep(500);
            this.StartService();
        }

        public void StartService()
        {
            Type serviceBase = new Type();
            this.host = new ServiceHost(serviceBase);
            // Maybe there's a better was to do this?
            host.Faulted += this.RestartService;
            // deal with binding
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            RoleInstanceEndpoint hostEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints[endpointname];
            host.AddServiceEndpoint(
                typeof(Interface),
                binding,
                String.Format(uri, hostEndpoint.IPEndpoint)
                );
            // Start it up!
            try
            {
                host.Open();
                DebugLog.Log(String.Format("Service {0} started.", typeof(Type).Name));
            }
            catch (TimeoutException te)
            {
                DebugLog.Log(String.Format("ServiceHost open failure for {0}, Timeout: {1}", typeof(Type).Name, te.Message));
            }
            catch (CommunicationException ce)
            {
                DebugLog.Log(String.Format("ServiceHost open failure for {0}, Communication Error: {1}", typeof(Type).Name, ce.Message));
            }
        }
    }
}
