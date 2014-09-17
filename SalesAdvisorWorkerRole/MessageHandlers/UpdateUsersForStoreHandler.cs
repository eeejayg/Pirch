using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

using SalesAdvisorQueueUtils.MessageHandlers;
using SalesAdvisorSharedClasses.Models;
using SalesAdvisorWorkerRole.Adapters;
using SalesAdvisorWorkerRole.Services;

namespace SalesAdvisorWorkerRole.MessageHandlers
{
    class UpdateUsersForStoreHandler : MessageHandler
    {
        private UpdateUsersForStoreMessage payload = new UpdateUsersForStoreMessage();
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

        public UpdateUsersForStoreHandler() 
        {
        }

        public override void HandleMessage(BrokeredMessage msg)
        {
            UserServiceWorker worker = new UserServiceWorker();
            // cast the message's body to our payload
            this.payload = msg.GetBody<UpdateUsersForStoreMessage>();
            // Symmetric difference craziness
            Dictionary<int, User> userDictionary = worker.GetDictionaryOfUsersByStoreCode(this.payload.storeid);
            // Make the JSON call to get the list of users for the store.
            List<User> userlist = FLJsonAdapter.getInstance().GetUsersByStore(this.payload.storeid);
            // update the DB with the list of users
            foreach (User user in userlist) {
                if (user.id == 0)
                {
                    // This means the user wasn't found in the DB, we have to add it.
                    worker.AddUser(user);
                }
                else
                {
                    // Already in the DB, update
                    worker.UpdateUser(user);
                    // remove from dictionary to create difference
                    userDictionary.Remove(user.id);
                }
            }
            // delete all users left in the dictionary
            foreach (KeyValuePair<int, User> kvp in userDictionary) {
                worker.DeleteUser(kvp.Value);
            }
            // complete the message
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
        protected class UpdateUsersForStoreMessage
        {
            [DataMember]
            public String storeid;
        }
    }
}
