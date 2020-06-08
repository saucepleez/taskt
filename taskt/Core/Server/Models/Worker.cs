using System;

namespace taskt.Core.Server.Models
{
    public class Worker
    {
        public Guid WorkerID { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public DateTime LastCheckIn { get; set; }
        public WorkerStatus Status { get; set; }
    }

    public enum WorkerStatus
    {
        Pending = 0,
        Authorized = 1,
        Revoked = 2
    }
}
