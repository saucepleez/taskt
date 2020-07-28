using System;
using taskt.Core.Enums;

namespace taskt.Core.Model.ServerModel
{
    public class Worker
    {
        public Guid WorkerID { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public DateTime LastCheckIn { get; set; }
        public WorkerStatus Status { get; set; }
    }
}
