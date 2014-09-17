using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace SalesAdvisorQueueUtils.MessageHandlers
{
    public class HandlerInstantiator
    {
        // This is where we're storing the class name in the message.
        private static readonly String HANDLER_CLASS = "msgPropsHandlerClass";
        // Cache of types to instantiate
        private Dictionary<string, Type> classCache = new Dictionary<string, Type>();

        public MessageHandler GetHandlerForMessage(BrokeredMessage msg)
        {
            String classname = (String)msg.Properties[HANDLER_CLASS];
            Type handlerType = null;
            try
            {
                handlerType = this.classCache[classname];
            }
            catch (KeyNotFoundException e) 
            {
                // This means we haven't instantiated anything of this type yet.
            }
            if (handlerType == null) {
                handlerType = Type.GetType(classname);
                this.classCache[classname] = handlerType;
            }
            return (MessageHandler)Activator.CreateInstance(handlerType);
        }

        public void SetHandlerForMessage(BrokeredMessage msg, MessageHandler handler)
        {
            msg.Properties[HANDLER_CLASS] = handler.GetHandlerClassName();
        }
    }
}
