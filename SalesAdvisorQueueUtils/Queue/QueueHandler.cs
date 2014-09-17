using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Practices;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;

using SalesAdvisorQueueUtils.MessageHandlers;

namespace SalesAdvisorQueueUtils.Queue
{
    public class QueueHandler
    {
        public static readonly ArrayList exceptionList = ArrayList.Synchronized(new ArrayList());

        // instance stuff
        private String connectionString = "PrivateQueueConnectionString";
        private String queueName = "PrivateQueueName";
        private QueueClient client;
        // The QueueClient is thread-safe
        public QueueClient queueclient {
            get { return this.client; }
        }
        private HandlerInstantiator instantiator = new HandlerInstantiator();
        public HandlerInstantiator handlerInstantiator
        {
            get { return this.instantiator; }
        }
        private Boolean isStopped = true;
        private Thread thread;

        public QueueHandler(String connectionKey, String queueKey)
        {
            this.connectionString = connectionKey;
            this.queueName = queueKey;
        }

        /// <summary>
        /// Starts the queue. This can throw all kinds of exceptions, to try/catch it.
        /// </summary>
        public void StartQueue()
        {
            // Create the queue if it does not exist already
            /* We'll define the queues in the management portal for now. Also, since this will be used from roles with
             * different permissions, we'll need to make this conditional anyway.
            var namespaceManager = NamespaceManager.CreateFromConnectionString(this.connectionString);
            //
            if (!namespaceManager.QueueExists(this.queueName))
            {
                namespaceManager.CreateQueue(this.queueName);
            }
             * */
            // Initialize the connection to Service Bus Queue
            this.client = QueueClient.CreateFromConnectionString(this.connectionString, this.queueName);
        }

        public void StopQueue()
        {
            this.isStopped = true;
            // Close the connection to Service Bus Queue
            if (this.client != null)
            {
                this.client.Close();
            }
        }

        public void StartListening()
        {
            this.isStopped = false;
            this.thread = new Thread(new ThreadStart(this.Listen));
            this.thread.Start();
        }

        private void Listen()
        {
            BrokeredMessage receivedMessage = null;
            while (!this.isStopped) {
                try
                {
                    receivedMessage = this.client.Receive();
                    this.ReceiveMessage(receivedMessage);
                }
                catch (MessagingException e)
                {
                    // Transient problem, like network busy, or whatever.
                    if (!e.IsTransient)
                    {
                        try
                        {
                            receivedMessage.Abandon();
                        }
                        catch (Exception ex)
                        {
                            // Okay, NOW we have a problem.
                            exceptionList.Add(ex);
                        }
                    }
                    // wait, and try again.
                    Thread.Sleep(10000);
                }
                catch (OperationCanceledException e)
                {
                    if (!this.isStopped)
                    {
                        try
                        {
                            receivedMessage.Abandon();
                        }
                        catch (Exception ex)
                        {
                            exceptionList.Add(ex);
                        }
                        exceptionList.Add(e);
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        receivedMessage.Abandon();
                    }
                    catch (Exception ex)
                    {
                        exceptionList.Add(ex);
                    }
                    exceptionList.Add(e);
                }
            }
        }

        private void ReceiveMessage(BrokeredMessage msg)
        {
            if (msg != null)
            {
                // Process the message
                try
                {
                    MessageHandler handler = null;
                    handler = this.instantiator.GetHandlerForMessage(msg);
                    if (handler != null)
                    {
                        handler.HandleMessage(msg);
                    }
                }
                catch (InvalidCastException e) 
                {
                    exceptionList.Add(e);
                }
                catch (KeyNotFoundException e)
                {
                    exceptionList.Add(e);
                }
                catch (Exception e)
                {
                    exceptionList.Add(e);
                }
            }
        }
    }
}
