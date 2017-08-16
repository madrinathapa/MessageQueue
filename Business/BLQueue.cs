using System;
using System.Messaging;
using MessageQueue.Entities;

namespace MessageQueue.Business
{
    public class BLQueue
    {
        /// <summary>
        /// Method for adding message in the queue
        /// </summary>
        /// <param name="message">Message that is to be placed in the queue</param>
        /// <param name="path">Queue's address</param>
        /// <param name="label">Label used</param>
        /// <param name="tranDetails">Details of the transaction</param>
        public void SendMessage(string message, string path, string label)
        {
            System.Messaging.MessageQueue queue = null;
            try
            {
                using (queue = new System.Messaging.MessageQueue(path))
                {
                    using (MessageQueueTransaction tran = new MessageQueueTransaction())
                    {
                        tran.Begin();
                        Message msg = new Message(message)
                        {
                            Label = label,
                            UseDeadLetterQueue = true,
                            TimeToBeReceived = new TimeSpan(7, 0, 0, 0),
                            Priority = MessagePriority.Normal
                        };
                        queue.Send(msg, tran);
                        tran.Commit();
                        queue.Close();
                    }
                }
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }

            finally
            {
                queue?.Dispose();
            }
        }

        /// <summary>
        /// Method for reading the messages from the queue(clears the queue)
        /// </summary>
        /// <param name="path">Address of the queue</param>
        /// <returns>List of all the messages in the queue</returns>
        public string ReadQueue(string path)
        {
            System.Messaging.MessageQueue myQueue = new System.Messaging.MessageQueue(path);
            string msg = null;
            try
            {
                Message message = myQueue.Receive(MessageQueueTransactionType.Single);
                if (message != null)
                {
                    message.Formatter = new XmlMessageFormatter(
                        new String[] { "System.String, mscorlib" });
                    msg = message.Body.ToString();
                }
            }
            catch (MessageQueueException m)
            {
                Console.WriteLine(m.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return msg;
        }

        /// <summary>
        /// Method for peeking a message present in the queue
        /// </summary>
        /// <param name="path">Address of the queue</param>
        /// <returns>Returns the message present in the queue</returns>
        public string PeekQueue(string path)
        {
            string token = null;
            try
            {
                System.Messaging.MessageQueue objMsgQueue = new System.Messaging.MessageQueue(path);
                Message tokenMsg ;
                using (var enumerator = objMsgQueue.GetMessageEnumerator2())
                {
                    tokenMsg = enumerator.MoveNext() ? enumerator.Current : null;
                 }                  
                if (tokenMsg != null)
                {
                    tokenMsg.Formatter = new XmlMessageFormatter(
                                   new String[] { "System.String, mscorlib" });
                    token = tokenMsg.Body.ToString();
                }
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }
            return token;
        }
    }
}
