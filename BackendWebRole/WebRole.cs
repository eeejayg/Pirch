using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;

using BackendWebRole.Controllers;
using SalesAdvisorQueueUtils.Queue;

namespace BackendWebRole
{
    public class WebRole : RoleEntryPoint
    {
        // So other things can get to the queue
        public static QueueHandler protectedQueue;

        // For the thread dealing with exceptions
        private Boolean IsStopped;
        //
        private static readonly String PROTECTED_QUEUE_CONNECTION_STRING = "ProtectedQueueConnectionString";
        private static readonly String PROTECTED_QUEUE_NAME = "ProtectedQueueName";

        public override void Run()
        {

            while (!IsStopped)
            {
                // Look for exceptions on the QueueHandler exception list
                while (QueueHandler.exceptionList.Count > 0)
                {
                    Exception e = (Exception)QueueHandler.exceptionList[0];
                    QueueHandler.exceptionList.RemoveAt(0);
                    //DebugLog.Log(String.Format("Exception happened on a queue handler! message: {0}", e.Message));
                    // We'll just re-throw right now for debug purposes.
                    throw e;
                }
                Thread.Sleep(5000);
            }
        }

        public override bool OnStart()
        {
            this.IsStopped = false;
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            return base.OnStart();
        }

        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            IsStopped = true;
            StopQueues();
            base.OnStop();
        }

        internal static void StartQueues()
        {
            String protectedQueueConnectionString = null;
            String protectedQueueName = null;
            if (String.Compare(Environment.GetEnvironmentVariable("ISJOSH"), "Ayup") == 0)
            {
                protectedQueueConnectionString = RoleEnvironment.GetConfigurationSettingValue("DevProtectedQueueConnectionString");
                protectedQueueName = RoleEnvironment.GetConfigurationSettingValue("DevProtectedQueueName");
            }
            else
            {
                protectedQueueConnectionString = RoleEnvironment.GetConfigurationSettingValue(PROTECTED_QUEUE_CONNECTION_STRING);
                protectedQueueName = RoleEnvironment.GetConfigurationSettingValue(PROTECTED_QUEUE_NAME);
            }
            BaseApiController.protectedQueue = protectedQueue= new QueueHandler(protectedQueueConnectionString, protectedQueueName);
            // We don't want to listen to the queue, just send messages.
            protectedQueue.StartQueue();
        }

        protected void StopQueues()
        {
            this.IsStopped = true;
            if (protectedQueue != null)
            {
                protectedQueue.StopQueue();
            }
        }
    }
}
