using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

using SalesAdvisorQueueUtils.MessageHandlers;
using SalesAdvisorWorkerRole.Logging;

namespace SalesAdvisorWorkerRole.MessageHandlers
{
    class UpdateUserHandler : MessageHandler
    {
        private UpdateUserMessage payload = new UpdateUserMessage();
        public String guid {
            set { this.payload.guid = value; }
            get { return this.payload.guid; }
        }

        public UpdateUserHandler()
        {
        }

        public override void HandleMessage(BrokeredMessage msg)
        {
            this.payload = msg.GetBody<UpdateUserMessage>();
            DebugLog.Log(String.Format("Got an UpdateUser message, guid: {0}", this.payload.guid));
            msg.Complete();
        }

        protected override BrokeredMessage GetBrokeredMessage()
        {
            return new BrokeredMessage(this.payload);
        }

        public override String GetHandlerClassName()
        {
            return this.GetType().AssemblyQualifiedName;
        }

        /// <summary>
        /// Inner class for holding and serializing data
        /// </summary>
        [DataContract]
        protected class UpdateUserMessage
        {
            [DataMember]
            public String guid;
        }
    }
}
