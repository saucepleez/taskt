using System;
using System.Collections.Generic;
using taskt.Core.Automation.Engine.EngineEventArgs;

namespace taskt.Core.Metrics
{
    public class ExecutionMetric
    {
        public string FileName { get; set; }
        public TimeSpan AverageExecutionTime { get; set; }
        public List<ScriptFinishedEventArgs> ExecutionData { get; set; }
    }
}
