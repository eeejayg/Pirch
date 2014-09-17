using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using SalesAdvisorQueueUtils.MessageHandlers;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWorkerRole.Adapters;
using SalesAdvisorWorkerRole.Logging;
using SalesAdvisorWorkerRole.Services;

namespace SalesAdvisorWorkerRole.MessageHandlers
{
    class CustomerUpdateHandler : MessageHandler
    {
        public Customers customer;

        public CustomerUpdateHandler()
        {
        }

        public override void HandleMessage(BrokeredMessage msg)
        {
            this.customer = msg.GetBody<Customers>();
            if (this.customer.CustomerGuid != null)
            {
                // Update customer data using PUT
                FLJsonAdapter.getInstance().UpdateCustomer(this.customer);
            }
            //
            msg.Complete();
        }

        protected override BrokeredMessage GetBrokeredMessage()
        {
            return new BrokeredMessage(this.customer);
        }

        public override String GetHandlerClassName()
        {
            return this.GetType().AssemblyQualifiedName;
        }
    }
}
