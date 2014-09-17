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
    /// <summary>
    /// This message is only for letting the Pirch CRM know that we've created a new customer
    /// </summary>
    class CustomerCreateHandler : MessageHandler
    {
        public Customers customer;

        public CustomerCreateHandler()
        {
        }

        public override void HandleMessage(BrokeredMessage msg)
        {
            this.customer = msg.GetBody<Customers>();
            if (this.customer.CustomerGuid == null) {
                // we need to figure out if there's a pre-existing customer by this name
                Customers theirCustomer = FLJsonAdapter.getInstance().GetCustomerByName(this.customer.Name());
                if (theirCustomer != null)
                {
                    // they've got one.
                    this.customer.CustomerGuid = theirCustomer.CustomerGuid;
                    // Update customer data using PUT
                    FLJsonAdapter.getInstance().UpdateCustomer(this.customer);
                }
                else
                {
                    // if not, create a guid for this one.
                    this.customer.CustomerGuid = Guid.NewGuid().ToString();
                    // Create customer data using POST
                    FLJsonAdapter.getInstance().CreateCustomer(this.customer);
                }
                // We need to let our DB know what the right GUID is.
                CustomerServiceWorker csw = new CustomerServiceWorker();
                csw.CustomerUpdateOrAddLocal(this.customer);
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
