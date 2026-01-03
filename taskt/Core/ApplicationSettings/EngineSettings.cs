using System;

namespace taskt.Core
{
    /// <summary>
    /// Defines engine settings which can be managed by the user
    /// </summary>
    [Serializable]
    public sealed class EngineSettings : IEngineSettings
    {
        public bool ShowDebugWindow { get; set; }
        public bool AutoCloseDebugWindow { get; set; }
        public bool EnableDiagnosticLogging { get; set; }
        public bool ShowAdvancedDebugOutput { get; set; }
        public bool CreateMissingVariablesDuringExecution { get; set; }
        public bool TrackExecutionMetrics { get; set; }
        public string VariableStartMarker { get; set; }
        public string VariableEndMarker { get; set; }
        public System.Windows.Forms.Keys CancellationKey { get; set; }
        private int _delayBetweenCommands;
        public int DelayBetweenCommands
        {
            get
            {
                return this._delayBetweenCommands;
            }
            set
            {
                if (value > 0)
                {
                    _delayBetweenCommands = value;
                }
            }
        }
        public bool OverrideExistingAppInstances { get; set; }
        public bool AutoCloseMessagesOnServerExecution { get; set; }
        public bool AutoCloseDebugWindowOnServerExecution { get; set; }
        public bool AutoCalcVariables { get; set; }
        public string CurrentWindowKeyword { get; set; }
        public string DesktopKeyword { get; set; }
        public string AllWindowsKeyword { get; set; }
        public string CurrentWindowPositionKeyword { get; set; }
        public string CurrentWindowXPositionKeyword { get; set; }
        public string CurrentWindowYPositionKeyword { get; set; }
        public string CurrentWorksheetKeyword { get; set; }
        public string NextWorksheetKeyword { get; set; }
        public string PreviousWorksheetKeyword { get; set; }
        public bool ExportIntermediateXML { get; set; }
        public bool UseNewParser { get; set; }
        public bool IgnoreFirstVariableMarkerInOutputParameter { get; set; }
        public int MaxFileCounter { get; set; }
        //public int MaxUIElementInpectDepth { get; set; }
        //public int MaxUIElementInspectSiblingNodes { get; set; }

        public EngineSettings()
        {
            ShowDebugWindow = true;
            AutoCloseDebugWindow = true;
            EnableDiagnosticLogging = true;
            ShowAdvancedDebugOutput = false;
            CreateMissingVariablesDuringExecution = true;
            TrackExecutionMetrics = true;
            VariableStartMarker = "{";
            VariableEndMarker = "}";
            CancellationKey = System.Windows.Forms.Keys.Pause;
            DelayBetweenCommands = 250;
            OverrideExistingAppInstances = false;
            AutoCloseMessagesOnServerExecution = true;
            AutoCloseDebugWindowOnServerExecution = true;
            AutoCalcVariables = false;
            CurrentWindowKeyword = "Current Window";
            DesktopKeyword = "Desktop";
            AllWindowsKeyword = "All Windows";
            CurrentWindowPositionKeyword = "Current Position";
            CurrentWindowXPositionKeyword = "Current XPosition";
            CurrentWindowYPositionKeyword = "Current YPosition";
            CurrentWorksheetKeyword = "Current Sheet";
            NextWorksheetKeyword = "Next Sheet";
            PreviousWorksheetKeyword = "Previous Sheet";
            ExportIntermediateXML = true;
            UseNewParser = true;
            IgnoreFirstVariableMarkerInOutputParameter = true;
            MaxFileCounter = 999;
            //MaxUIElementInpectDepth = 256;
            //MaxUIElementInspectSiblingNodes = int.MaxValue;
        }

        /// <summary>
        /// clone instance
        /// </summary>
        /// <returns></returns>
        public EngineSettings Clone()
        {
            return (EngineSettings)MemberwiseClone();
        }

        // todo: move these methods to other file
        //public string convertToIntermediate(string targetString)
        //{
        //    return targetString.Replace(this.VariableStartMarker, "\u2983")
        //            .Replace(this.VariableEndMarker, "\u2984");
        //}

        //public string convertToRaw(string targetString)
        //{
        //    return targetString.Replace("\u2983", this.VariableStartMarker)
        //            .Replace("\u2984", this.VariableEndMarker);
        //}

        //public string convertToIntermediateVariableParser(string targetString, List<Core.Script.ScriptVariable> variables)
        //{
        //    Core.Automation.Engine.AutomationEngineInstance engine = new Automation.Engine.AutomationEngineInstance(false);
        //    engine.engineSettings = this;
        //    engine.VariableList = variables;
        //    return ExtensionMethods.ConvertUserVariableToIntermediateNotation(targetString, engine);
        //}
    }
}
