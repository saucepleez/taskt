using System;
using System.Collections.Generic;
using taskt.Core.Model.EngineModel;

namespace taskt.Core.Metrics
{
    public class ExecutionMetric
    {
        public string FileName { get; set; }
        public TimeSpan AverageExecutionTime { get; set; }
        public List<ScriptFinishedEventArgs> ExecutionData { get; set; }
    }
}
