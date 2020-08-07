using Serilog.Events;
using System;
using System.Windows.Forms;
using taskt.Core.Enums;

namespace taskt.Core.Settings
{
    /// <summary>
    /// Defines engine settings which can be managed by the user
    /// </summary>
    [Serializable]
    public class EngineSettings
    {
        public bool ShowDebugWindow { get; set; }
        public bool AutoCloseDebugWindow { get; set; }
        public bool ShowAdvancedDebugOutput { get; set; }
        public bool CreateMissingVariablesDuringExecution { get; set; }
        public bool TrackExecutionMetrics { get; set; }
        public string VariableStartMarker { get; set; }
        public string VariableEndMarker { get; set; }
        public Keys CancellationKey { get; set; }
        public int DelayBetweenCommands { get; set; }
        public bool OverrideExistingAppInstances { get; set; }
        public bool EnableDiagnosticLogging { get; set; }
        public bool AutoCloseMessagesOnServerExecution { get; set; }
        public bool AutoCloseDebugWindowOnServerExecution { get; set; }
        public bool AutoCalcVariables { get; set; }
        public SinkType LoggingSinkType { get; set; }
        public string LoggingValue1 { get; set; }
        public string LoggingValue2 { get; set; }
        public string LoggingValue3 { get; set; }
        public string LoggingValue4 { get; set; }
        public LogEventLevel MinLogLevel { get; set; }

        public EngineSettings()
        {
            ShowDebugWindow = true;
            AutoCloseDebugWindow = false;
            ShowAdvancedDebugOutput = true;
            CreateMissingVariablesDuringExecution = true;
            TrackExecutionMetrics = true;
            VariableStartMarker = "{";
            VariableEndMarker = "}";
            CancellationKey = Keys.Pause;
            DelayBetweenCommands = 250;
            OverrideExistingAppInstances = false;
            EnableDiagnosticLogging = true;
            AutoCloseMessagesOnServerExecution = true;
            AutoCloseDebugWindowOnServerExecution = true;
            AutoCalcVariables = true;
            LoggingSinkType = SinkType.File;
            LoggingValue1 = "";
            LoggingValue2 = "";
            LoggingValue3 = "";
            LoggingValue4 = "";
            MinLogLevel = LogEventLevel.Verbose;
        }
    }
}
