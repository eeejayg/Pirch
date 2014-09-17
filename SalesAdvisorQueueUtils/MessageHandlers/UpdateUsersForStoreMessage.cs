using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace SalesAdvisorQueueUtils.MessageHandlers
{
    public class UpdateUsersForStoreMessage : MessageHandler
    {
        public delegate void OnMessageReceived(UpdateUsersForStoreMessage msg);
        public static OnMessageReceived Del;

        private UpdateMessage payload = new UpdateMessage();
        public String storeid
        {
            get
            {
                return this.payload.storeid;
            }
            set
            {
                this.payload.storeid = value;
            }
        }

        public UpdateUsersForStoreMessage() {
        }

        public override void HandleMessage(BrokeredMessage msg)
        {
            this.payload = msg.GetBody<UpdateMessage>();
            Del(this);
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
        /// Inner class for serializing data
        /// </summary>
        [DataContract]
        protected class UpdateMessage
        {
            [DataMember]
            public String storeid;
        }
    }
}
