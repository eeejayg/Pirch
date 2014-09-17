using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

using SalesAdvisorQueueUtils.Queue;

namespace SalesAdvisorQueueUtils.MessageHandlers
{
    public abstract class MessageHandler
    {

        // You'll only be able to send the message for a given handler once.
        private Boolean hasBeenSent = false;

        /// <summary>
        /// You should handle the BrokeredMessage here. Cast it's body to whatever type you need. The MessageHandler's responsible for
        /// calling Complete() or Abandon() on the message.
        /// </summary>
        /// <param name="msg">The BrokeredMessage you're supposed to handle.</param>
        public abstract void HandleMessage(BrokeredMessage msg);

        /// <summary>
        /// The intent here is that subclasses will want to construct the BrokeredMessage in different ways, so this is where you have
        /// to do that.
        /// </summary>
        /// <returns>A BrokeredMessage to send to the private queue</returns>
        protected abstract BrokeredMessage GetBrokeredMessage();

        /// <summary>
        /// This should return the fully qualified class name, use this.GetType().AssemblyQualifiedName in your subclass.
        /// </summary>
        /// <returns></returns>
        public abstract String GetHandlerClassName();

        /// <summary>
        /// 
        /// </summary>
        public void EnqueueSelfAsMessage(QueueHandler queue)
        {
            if (this.hasBeenSent == false)
            {
                BrokeredMessage msg = this.GetBrokeredMessage();
                queue.handlerInstantiator.SetHandlerForMessage(msg, this);
                queue.queueclient.Send(msg);
                this.hasBeenSent = true;
            }
            else
            {
                throw new Exception("A MessageHandler's message can only be sent once!");
            }
        }
    }
}
