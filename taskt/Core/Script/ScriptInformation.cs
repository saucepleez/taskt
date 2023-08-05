using System;

namespace taskt.Core.Script
{
    [Serializable]
    public class ScriptInformation
    {
        public string TasktVersion { get; set; }
        public string Author { get; set; }
        public DateTime LastRunTime { get; set; }
        public int RunTimes { get; set; }
        public string ScriptVersion { get; set; }
        public string Description { get; set; }
        public ScriptInformation()
        {
            this.TasktVersion = "";
            this.Author = "";
            this.LastRunTime = DateTime.Parse("1990-01-01T00:00:00");
            this.RunTimes = 0;
            this.ScriptVersion = "0.0.0";
            this.Description = "";
        }
    }
}
