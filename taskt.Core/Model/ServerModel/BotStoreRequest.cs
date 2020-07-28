using System;
using taskt.Core.Enums;

namespace taskt.Core.Model.ServerModel
{
    public class BotStoreRequest
    {
        public Guid WorkerID { get; set; }
        public string BotStoreName { get; set; }
        public BotStoreRequestType RequestType { get; set; }
    }
}
