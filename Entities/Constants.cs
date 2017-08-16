using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessageQueue.Entities
{
    public class Constants
    {
        public struct QueuePath
        {
            public const string ReceivingQueue = @".\reciever";
            public const string SendingQueue = @".\sender";
        }
    }
}
