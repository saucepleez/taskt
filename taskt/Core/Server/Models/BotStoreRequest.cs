using System;

namespace taskt.Core.Server.Models
{
    public class BotStoreRequest
    {
        public Guid WorkerID { get; set; }
        public string BotStoreName { get; set; }
        public BotStoreRequestType RequestType { get; set; }
        public enum BotStoreRequestType
        {
            BotStoreValue,
            BotStoreModel
        }
    }
}
