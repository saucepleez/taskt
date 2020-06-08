using System;

namespace taskt.Core.Server.Models
{
    public class PublishedScript
    {
        public Guid WorkerID { get; set; }
        public PublishType ScriptType { get; set; }
        public string ScriptData { get; set; }
        public string FriendlyName { get; set; }
        public bool OverwriteExisting { get; set; }
        public enum PublishType
        {
            ClientReference,
            ServerReference,
        }
    }
}
