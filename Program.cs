using System;
using System.ServiceProcess;
using MessageQueue.Business;
using MessageQueue.Entities;
using MessageQueueService.Business;

namespace MessageQueue
{
    class Program
    {
        public static void Main(string[] args)
        {
            if (Environment.UserInteractive)
            {
                Start(args);
            }
            else
            {
                //running as service
                using (var service = new Service())
                {
                    ServiceBase.Run(service);
                }
            }
        }

        public static void Start(string[] args)
        {
            Console.WriteLine("Service started");
        }

        public static void Stop()
        {
            Console.WriteLine("Service stopped");
        }

        /// <summary>
        /// Method for processing the requests
        /// </summary>
        public static void ProcessRequest()
        {
            BLQueue objQueue = new BLQueue();
            string receiverQueue = Constants.QueuePath.ReceivingQueue;

            try
            {
                while (!string.IsNullOrEmpty(objQueue.PeekQueue(receiverQueue)))
                {
                    string msg = objQueue.ReadQueue(receiverQueue);
                    string senderQueue = Constants.QueuePath.SendingQueue;
                    objQueue.SendMessage(msg, senderQueue,"message");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }
}
