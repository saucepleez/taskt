using System;

namespace taskt.UI.DTOs
{
    public class MachineConfiguration
    {
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime NextConnectionDue { get; set; }
        public string LastKnownStatus { get; set; }
    }
}
