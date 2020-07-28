using System;

namespace taskt.Core.Model.ServerModel
{
    public class BotStore
    {
        public Guid StoreID { get; set; }
        public string BotStoreName { get; set; }
        public string BotStoreValue { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public Guid LastUpdatedBy { get; set; }
    }
}
