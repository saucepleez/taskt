using System;
using taskt.Core.Enums;

namespace taskt.Core.Model.ServerModel
{
    public class PublishedScript
    {
        public Guid WorkerID { get; set; }
        public PublishType ScriptType { get; set; }
        public string ScriptData { get; set; }
        public string FriendlyName { get; set; }
        public bool OverwriteExisting { get; set; }
    }
}
