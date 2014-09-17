using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

using SalesAdvisorQueueUtils.Queue;
using SalesAdvisorQueueUtils.MessageHandlers;
using SalesAdvisorSharedClasses.Communication;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWorkerRole.Logging;
using SalesAdvisorWorkerRole.MessageHandlers;
using SalesAdvisorWorkerRole.Services;

namespace SalesAdvisorWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        public static QueueHandler privateQueue;
        public static QueueHandler protectedQueue;
        public static readonly int MAX_DELIVERY_ATTEMPTS = 5;

        //
        bool IsStopped;

        // Services
        private WorkerServiceStarter<UserServiceWorker, UserService> userWorkerService;
        private WorkerServiceStarter<CustomerServiceWorker, CustomerService> customerWorkerService;
        private WorkerServiceStarter<ProductServiceWorker, ProductService> productWorkerService;
        private WorkerServiceStarter<TestServiceWorker, TestService> testWorkerService;
        private WorkerServiceStarter<ProjectServiceWorker, ProjectService> projectWorkerService;
        // Queues
        private String connectionStringKey = "PrivateQueueConnectionString";
        private String queueNameKey = "PrivateQueueName";
        private String protectedQueueConnectionString = "ProtectedQueueConnectionString";
        private String protectedQueueName = "ProtectedQueueName";

        public override void Run()
        {
            
            while (!IsStopped)
            {
                // Look for exceptions on the QueueHandler exception list
                while (QueueHandler.exceptionList.Count > 0) {
                    Exception e = (Exception)QueueHandler.exceptionList[0];
                    QueueHandler.exceptionList.RemoveAt(0);
                    DebugLog.Log(String.Format("Exception happened on a queue handler! message: {0}", e.Message));
                    // We'll just re-throw right now for debug purposes.
                    throw e;
                }
                Thread.Sleep(5000);
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            // Start services
            this.StartServices();
            // instantiate queues
            this.StartQueues();
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            IsStopped = true;
            this.StopQueues();
            base.OnStop();
        }

        // services

        private void StartServices()
        {
            this.userWorkerService = new WorkerServiceStarter<UserServiceWorker, UserService>(UserServiceInfo.ENDPOINT_NAME, "GetDataEndpoint");
            this.userWorkerService.StartService();

            this.customerWorkerService = new WorkerServiceStarter<CustomerServiceWorker, CustomerService>(CustomerServiceInfo.ENDPOINT_NAME, "GetDataEndpoint");
            this.customerWorkerService.StartService();

            this.productWorkerService = new WorkerServiceStarter<ProductServiceWorker, ProductService>(ProductServiceInfo.ENDPOINT_NAME, "GetDataEndpoint");
            this.productWorkerService.StartService();

            this.testWorkerService = new WorkerServiceStarter<TestServiceWorker, TestService>(TestServiceInfo.ENDPOINT_NAME, "GetDataEndpoint");
            this.testWorkerService.StartService();
            
            this.projectWorkerService = new WorkerServiceStarter<ProjectServiceWorker, ProjectService>(ProjectServiceInfo.ENDPOINT_NAME, "GetDataEndpoint");
            this.projectWorkerService.StartService();
        }

        private void StartQueues()
        {
            DebugLog.Log(String.Format("*********************************\nTrying to start queues at {0}", DateTime.Now.ToString()));
            DebugLog.Log(String.Format("Is josh? {0}", Environment.GetEnvironmentVariable("ISJOSH")));
            String connectionString = null;
            String queueName = null;
            String protectedQueueConnectionString = null;
            String protectedQueueName = null;
            if (String.Compare(Environment.GetEnvironmentVariable("ISJOSH"), "Ayup") == 0)
            {
                connectionString = RoleEnvironment.GetConfigurationSettingValue("DevQueueConnectionString");
                queueName = RoleEnvironment.GetConfigurationSettingValue("DevQueueName");
                protectedQueueConnectionString = RoleEnvironment.GetConfigurationSettingValue("DevProtectedQueueConnectionString");
                protectedQueueName = RoleEnvironment.GetConfigurationSettingValue("DevProtectedQueueName");
            }
            else
            {
                connectionString = RoleEnvironment.GetConfigurationSettingValue(this.connectionStringKey);
                queueName = RoleEnvironment.GetConfigurationSettingValue(this.queueNameKey);
                protectedQueueConnectionString = RoleEnvironment.GetConfigurationSettingValue(this.protectedQueueConnectionString);
                protectedQueueName = RoleEnvironment.GetConfigurationSettingValue(this.protectedQueueName);
            }
            privateQueue = new QueueHandler(connectionString, queueName);
            privateQueue.StartQueue();
            privateQueue.StartListening();
            protectedQueue = new QueueHandler(protectedQueueConnectionString, protectedQueueName);
            protectedQueue.StartQueue();
            protectedQueue.StartListening();
            DebugLog.Log(String.Format("*********************************\nFinished starting queues at {0}", DateTime.Now.ToString()));
            // set us up some delegates
            UpdateUserMessage.Del += (UpdateUserMessage msg) => {
                DebugLog.Log(String.Format("Got update user message from protected queue, guid: {0}", msg.guid));
            };
            UpdateUsersForStoreMessage.Del += (UpdateUsersForStoreMessage msg) => {
                UserServiceWorker worker = new UserServiceWorker();
                if (worker.IsStoreCodeValid(msg.storeid)) {
                    UpdateUsersForStoreHandler handler = new UpdateUsersForStoreHandler();
                    handler.storeid = msg.storeid;
                    handler.EnqueueSelfAsMessage(privateQueue);
                }
            };
        }

        private void StopQueues()
        {
            if (privateQueue != null) {
                privateQueue.StopQueue();
            }
            if (protectedQueue != null) {
                protectedQueue.StopQueue();
            }
        }
    }
}
