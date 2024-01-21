using System;
using System.Collections.Generic;

namespace taskt.Core
{
    /// <summary>
    /// Defines engine settings which can be managed by the user
    /// </summary>
    [Serializable]
    public class EngineSettings
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

        private static string InterStartVariableMaker = "{{{";
        private static string InterEndVariableMaker = "}}}";
        private static string InterCurrentWindowKeyword = "%kwd_current_window%";
        private static string InterDesktopKeyword = "%kwd_desktop%";
        private static string InterAllWindowsKeyword = "%kwd_all_windows%";
        private static string InterCurrentWindowPositionKeyword = "%kwd_current_position%";
        private static string InterCurrentWindowXPositionKeyword = "%kwd_current_xposition%";
        private static string InterCurrentWindowYPositionKeyword = "%kwd_current_yposition%";
        private static string InterCurrentWorksheetKeyword = "%kwd_current_worksheet%";
        private static string InterNextWorksheetKeyword = "%kwd_next_worksheet%";
        private static string InterPreviousWorksheetKeyword = "%kwd_previous_worksheet%";

        private static string[] m_KeyNameList = new string[]
        {
            "BACKSPACE", "BS", "BKSP",
            "BREAK",
            "CAPSLOCK",
            "DELETE", "DEL",
            "UP", "DOWN", "LEFT", "RIGHT",
            "END",
            "ENTER",
            "INSERT", "INS",
            "NUMLOCK",
            "PGDN",
            "PGUP",
            "SCROLLROCK",
            "TAB",
            "F1", "F2", "F3", "F4", "F5", "F6",
            "F7", "F8", "F9", "F10", "F11", "F12",
            "ADD", "SUBTRACT", "MULTIPLY", "DIVIDE",
            "WIN_KEY"
        };
        private static string[] m_DisallowVariableCharList = new string[]
        {
            "+", "-", "*", "%",
            "[", "]", "{", "}",
            ".",
            " ",
            "\u2983", "\u2984",
            "\U0001D542", "\U0001D54E"
        };

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
            AutoCalcVariables = true;
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
        }

        public string[] KeyNameList()
        {
            return m_KeyNameList;
        }

        public string[] DisallowVariableCharList()
        {
            return m_DisallowVariableCharList;
        }

        public string replaceEngineKeyword(string targetString)
        {
            return targetString.Replace(InterStartVariableMaker, this.VariableStartMarker)
                    .Replace(InterEndVariableMaker, this.VariableEndMarker)
                    .Replace(InterCurrentWindowKeyword, this.CurrentWindowKeyword)
                    .Replace(InterCurrentWindowPositionKeyword, this.CurrentWindowPositionKeyword)
                    .Replace(InterCurrentWindowXPositionKeyword, this.CurrentWindowXPositionKeyword)
                    .Replace(InterCurrentWindowYPositionKeyword, this.CurrentWindowYPositionKeyword)
                    .Replace(InterCurrentWorksheetKeyword, this.CurrentWorksheetKeyword)
                    .Replace(InterNextWorksheetKeyword, this.NextWorksheetKeyword)
                    .Replace(InterPreviousWorksheetKeyword, this.PreviousWorksheetKeyword);
        }

        public string convertToIntermediate(string targetString)
        {
            return targetString.Replace(this.VariableStartMarker, "\u2983")
                    .Replace(this.VariableEndMarker, "\u2984");
        }

        public string convertToRaw(string targetString)
        {
            return targetString.Replace("\u2983", this.VariableStartMarker)
                    .Replace("\u2984", this.VariableEndMarker);
        }

        public string convertToIntermediateExcelSheet(string targetString)
        {
            return convertToIntermediate(
                    targetString.Replace(this.CurrentWorksheetKeyword, wrapIntermediateKeyword(InterCurrentWorksheetKeyword))
                        .Replace(this.NextWorksheetKeyword, wrapIntermediateKeyword(InterNextWorksheetKeyword))
                        .Replace(this.PreviousWorksheetKeyword, wrapIntermediateKeyword(InterPreviousWorksheetKeyword))
                    );
        }

        public string convertToRawExcelSheet(string targetString)
        {
            return convertToRaw(
                    targetString.Replace(wrapIntermediateKeyword(InterCurrentWorksheetKeyword), this.CurrentWorksheetKeyword)
                        .Replace(wrapIntermediateKeyword(InterNextWorksheetKeyword), this.NextWorksheetKeyword)
                        .Replace(wrapIntermediateKeyword(InterPreviousWorksheetKeyword), this.PreviousWorksheetKeyword)
                );
        }

        public string convertToIntermediateWindowName(string targetString)
        {
            return convertToIntermediate(
                    targetString.Replace(this.CurrentWindowKeyword, wrapIntermediateKeyword(InterCurrentWindowKeyword))
                        .Replace(this.DesktopKeyword, wrapIntermediateKeyword(InterDesktopKeyword))
                        .Replace(this.AllWindowsKeyword, wrapIntermediateKeyword(InterAllWindowsKeyword))
                );
        }

        public string convertToRawWindowName(string targetString)
        {
            return convertToRaw(
                    targetString.Replace(wrapIntermediateKeyword(InterCurrentWindowKeyword), this.CurrentWindowKeyword)
                        .Replace(wrapIntermediateKeyword(InterDesktopKeyword), this.DesktopKeyword)
                        .Replace(wrapIntermediateKeyword(InterAllWindowsKeyword), this.AllWindowsKeyword)
                );
        }

        public string convertToIntermediateWindowPosition(string targetString)
        {
            return convertToIntermediate(
                    targetString.Replace(this.CurrentWindowPositionKeyword, wrapIntermediateKeyword(InterCurrentWindowPositionKeyword))
                        .Replace(this.CurrentWindowXPositionKeyword, wrapIntermediateKeyword(InterCurrentWindowXPositionKeyword))
                        .Replace(this.CurrentWindowYPositionKeyword, wrapIntermediateKeyword(InterCurrentWindowYPositionKeyword))
                );
        }

        public string convertToRawWindowPosition(string targetString)
        {
            return convertToRaw(
                    targetString.Replace(wrapIntermediateKeyword(InterCurrentWindowPositionKeyword), this.CurrentWindowPositionKeyword)
                        .Replace(wrapIntermediateKeyword(InterCurrentWindowXPositionKeyword), this.CurrentWindowXPositionKeyword)
                        .Replace(wrapIntermediateKeyword(InterCurrentWindowYPositionKeyword), this.CurrentWindowYPositionKeyword)
                );
        }

        public string convertToIntermediateVariableParser(string targetString, List<Core.Script.ScriptVariable> variables)
        {
            Core.Automation.Engine.AutomationEngineInstance engine = new Automation.Engine.AutomationEngineInstance(false);
            engine.engineSettings = this;
            engine.VariableList = variables;
            return ExtensionMethods.ConvertUserVariableToIntermediateNotation(targetString, engine);
        }

        public string wrapVariableMarker(string variableName)
        {
            return this.VariableStartMarker + variableName + this.VariableEndMarker;
        }

        public string unwrapVariableMarker(string variableName)
        {
            if (this.isWrappedVariableMarker(variableName))
            {
                string rmvSt = variableName.Substring(this.VariableStartMarker.Length);
                return rmvSt.Substring(0, rmvSt.Length - this.VariableEndMarker.Length);
            }
            else
            {
                return variableName;
            }
        }

        public bool isWrappedVariableMarker(string variableName)
        {
            return (variableName.StartsWith(this.VariableStartMarker) && variableName.EndsWith(this.VariableEndMarker));
        }

        public string wrapIntermediateVariableMaker(string variableName)
        {
            return "\u2983" + variableName + "\u2984";
        }

        private static string wrapIntermediateKeyword(string kw)
        {
            return "\U0001D542" + kw + "\U0001D54E";
        }

        public bool isValidVariableName(string vName)
        {
            foreach (string s in m_KeyNameList)
            {
                if (vName == s)
                {
                    return false;
                }
            }
            foreach (string s in m_DisallowVariableCharList)
            {
                if (vName.Contains(s))
                {
                    return false;
                }
            }
            if (vName.StartsWith("__INNER_"))
            {
                return false;
            }
            return true;
        }
    }
}
