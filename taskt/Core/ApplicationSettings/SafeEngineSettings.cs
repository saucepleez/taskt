namespace taskt.Core
{
    /// <summary>
    /// EngineSettings
    /// </summary>
    public class SafeEngineSettings : IEngineSettings
    {
        /// <summary>
        /// EngineSettings for protect
        /// </summary>
        protected readonly EngineSettings engineSettings;

        public SafeEngineSettings(EngineSettings engineSettings)
        {
            this.engineSettings = engineSettings;
        }

        public bool ShowDebugWindow
        {
            get
            {
                return engineSettings.ShowDebugWindow;
            }
        }

        public bool AutoCloseDebugWindow
        {
            get
            {
                return engineSettings.AutoCloseDebugWindow;
            }
        }

        public bool EnableDiagnosticLogging
        {
            get
            {
                return engineSettings.EnableDiagnosticLogging;
            }
        }

        public bool ShowAdvancedDebugOutput
        {
            get
            {
                return engineSettings.ShowAdvancedDebugOutput;
            }
        }

        public bool CreateMissingVariablesDuringExecution
        {
            get
            {
                return engineSettings.CreateMissingVariablesDuringExecution;
            }
        }

        public bool TrackExecutionMetrics
        {
            get
            {
                return engineSettings.TrackExecutionMetrics;
            }
        }
        public string VariableStartMarker
        {
            get
            {
                return engineSettings.VariableStartMarker;
            }
            set
            {
                engineSettings.VariableStartMarker = value;
            }
        }
        public string VariableEndMarker
        {
            get
            {
                return engineSettings.VariableEndMarker;
            }
            set
            {
                engineSettings.VariableEndMarker = value;
            }
        }
        public System.Windows.Forms.Keys CancellationKey
        {
            get
            {
                return engineSettings.CancellationKey;
            }
        }
        public int DelayBetweenCommands
        {
            get
            {
                return engineSettings.DelayBetweenCommands;
            }
            set
            {
                engineSettings.DelayBetweenCommands = value;
            }
        }
        public bool OverrideExistingAppInstances
        {
            get
            {
                return engineSettings.OverrideExistingAppInstances;
            }
        }
        public bool AutoCloseMessagesOnServerExecution
        {
            get
            {
                return engineSettings.AutoCloseMessagesOnServerExecution;
            }
        }
        public bool AutoCloseDebugWindowOnServerExecution
        {
            get
            {
                return engineSettings.AutoCloseDebugWindowOnServerExecution;
            }
        }
        public bool AutoCalcVariables
        {
            get
            {
                return engineSettings.AutoCalcVariables;
            }
            set
            {
                engineSettings.AutoCalcVariables = value;
            }
        }

        public string CurrentWindowKeyword
        {
            get
            {
                return engineSettings.CurrentWindowKeyword;
            }
        }

        public string DesktopKeyword
        {
            get
            {
                return engineSettings.DesktopKeyword;
            }
        }

        public string AllWindowsKeyword
        {
            get
            {
                return engineSettings.AllWindowsKeyword;
            }
        }

        public string CurrentWindowPositionKeyword
        {
            get
            {
                return engineSettings.CurrentWindowPositionKeyword;
            }
        }

        public string CurrentWindowXPositionKeyword
        {
            get
            {
                return engineSettings.CurrentWindowXPositionKeyword;
            }
        }

        public string CurrentWindowYPositionKeyword
        {
            get
            {
                return engineSettings.CurrentWindowYPositionKeyword;
            }
        }

        public string CurrentWorksheetKeyword
        {
            get
            {
                return engineSettings.CurrentWorksheetKeyword;
            }
        }

        public string NextWorksheetKeyword
        {
            get
            {
                return engineSettings.NextWorksheetKeyword;
            }
        }

        public string PreviousWorksheetKeyword
        {
            get
            {
                return engineSettings.PreviousWorksheetKeyword;
            }
        }

        public bool ExportIntermediateXML
        {
            get
            {
                return engineSettings.ExportIntermediateXML;
            }
        }
        public bool UseNewParser
        {
            get
            {
                return engineSettings.UseNewParser;
            }
        }
        public bool IgnoreFirstVariableMarkerInOutputParameter
        {
            get
            {
                return engineSettings.IgnoreFirstVariableMarkerInOutputParameter;
            }
        }
        public int MaxFileCounter
        {
            get
            {
                return engineSettings.MaxFileCounter;
            }
        }
        //public int MaxUIElementInpectDepth
        //{
        //    get
        //    {
        //        return engineSettings.MaxUIElementInpectDepth;
        //    }
        //}
        //public int MaxUIElementInspectSiblingNodes
        //{
        //    get
        //    {
        //        return engineSettings.MaxUIElementInspectSiblingNodes;
        //    }
        //}
    }
}
