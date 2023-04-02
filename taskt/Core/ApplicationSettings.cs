using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using taskt.Core.IO;

namespace taskt.Core
{
    /// <summary>
    /// Defines settings for the entire application
    /// </summary>
    [Serializable]
    public class ApplicationSettings
    {
        public ServerSettings ServerSettings { get; set; } = new ServerSettings();
        public EngineSettings EngineSettings { get; set; } = new EngineSettings();
        public ClientSettings ClientSettings { get; set; } = new ClientSettings();
        public LocalListenerSettings ListenerSettings { get; set; } = new LocalListenerSettings();
        public ApplicationSettings()
        {

        }

        public void Save(ApplicationSettings appSettings)
        {
            //create settings directory
           
            var settingsDir = Core.IO.Folders.GetFolder(Folders.FolderType.SettingsFolder);

            //if directory does not exist then create directory
            if (!System.IO.Directory.Exists(settingsDir))
            {
                System.IO.Directory.CreateDirectory(settingsDir);
            }

            //create file path
            var filePath = System.IO.Path.Combine(settingsDir, "AppSettings.xml");

            ////create filestream
            //var fileStream = System.IO.File.Create(filePath);

            ////output to xml file
            //XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
            //serializer.Serialize(fileStream, appSettings);
            //fileStream.Close();
            SaveAs(appSettings, filePath);
        }

        public static void SaveAs(ApplicationSettings appSettings, string filePath)
        {
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                serializer.Serialize(fileStream, appSettings);
                fileStream.Close();
            }       
        }

        public ApplicationSettings GetOrCreateApplicationSettings()
        {
            //create settings directory
            var settingsDir = Core.IO.Folders.GetFolder(Folders.FolderType.SettingsFolder);

            //create file path
            var filePath = System.IO.Path.Combine(settingsDir, "AppSettings.xml");

            ApplicationSettings appSettings;
            if (System.IO.File.Exists(filePath))
            {
                ////open file and return it or return new settings on error
                //var fileStream = System.IO.File.Open(filePath, FileMode.Open);

                //try
                //{
                //    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                //    appSettings = (ApplicationSettings)serializer.Deserialize(fileStream);
                //}
                //catch (Exception)
                //{
                //    appSettings = new ApplicationSettings();
                //}

                //fileStream.Close();
                try
                {
                    appSettings = Open(filePath);
                }
                catch
                {
                    appSettings = new ApplicationSettings();
                }
            }
            else
            {
                appSettings = new ApplicationSettings();
            }

            return appSettings;
        }

        public static ApplicationSettings Open(string filePath)
        {
            ApplicationSettings appSettings = null;
            using (FileStream fileStream = System.IO.File.Open(filePath, FileMode.Open))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(ApplicationSettings));
                    appSettings = (ApplicationSettings)serializer.Deserialize(fileStream);
                }
                catch (Exception ex)
                {
                    //appSettings = new ApplicationSettings();
                    throw ex;
                }
                finally
                {
                    fileStream.Close();
                }
            }
            return appSettings;
        }

        public string replaceApplicationKeyword(string targetString)
        {
            return this.ClientSettings.replaceClientKeyword(this.EngineSettings.replaceEngineKeyword(targetString));
        }
    }
    /// <summary>
    /// Defines Server settings for tasktServer if using the server component to manage the client
    /// </summary>
    [Serializable]
    public class ServerSettings
    {
        public bool ServerConnectionEnabled { get; set; }
        public bool ConnectToServerOnStartup { get; set; }
        public bool RetryServerConnectionOnFail { get; set; }
        public bool BypassCertificateValidation { get; set; }
        public string ServerURL { get; set; }
        public string ServerPublicKey { get; set; }
        public string HTTPServerURL { get; set; }
        public Guid HTTPGuid { get; set; }

        public ServerSettings()
        {
            HTTPServerURL = "https://localhost:44377/";
        }
    }
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
            return Core.ExtensionMethods.ConvertToUserVariable_Intermediate(targetString, engine);
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
            foreach(string s in m_KeyNameList)
            {
                if (vName == s)
                {
                    return false;
                }
            }
            foreach(string s in m_DisallowVariableCharList)
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
    /// <summary>
    /// Defines application/client-level settings which can be managed by the user
    /// </summary>
    [Serializable]
    public class ClientSettings
    {
        public bool AntiIdleWhileOpen { get; set; }
        public string RootFolder { get; set; }
        public bool MinimizeToTray { get; set; }
        public string AttendedTasksFolder { get; set; }
        public string StartupMode { get; set; }
        public bool PreloadBuilderCommands { get; set; }
        public bool UseSlimActionBar { get; set; }
        public bool InsertCommandsInline { get; set; }
        public bool EnableSequenceDragDrop { get; set; }
        public bool InsertVariableAtCursor { get; set; }
        public bool InsertElseAutomatically { get; set; }
        public bool InsertCommentIfLoopAbove { get; set; }
        public bool GroupingBySubgroup { get; set; }
        public bool DontShowValidationMessage { get; set; }
        public bool ShowPoliteTextInDescription { get; set; }
        public bool ShowSampleUsageInDescription { get; set; }
        public bool ShowDefaultValueInDescription { get; set; }
        public bool ShowIndentLine { get; set; }
        public bool ShowScriptMiniMap { get; set; }
        private int _IndentWidth = 16;
        public int IndentWidth 
        {
            get
            {
                return this._IndentWidth;
            }
            set
            {
                if (value >= 1 && value <= 32)
                {
                    this._IndentWidth = value;
                }
            }
        }
        public string DefaultBrowserInstanceName { get; set; }
        public string DefaultStopWatchInstanceName { get; set; }
        public string DefaultExcelInstanceName { get; set; }
        public string DefaultWordInstanceName { get; set; }
        public string DefaultDBInstanceName { get; set; }
        public string DefaultNLGInstanceName { get; set; }
        private string _InstanceNameOrder = "Creation Frequently"; 
        public string InstanceNameOrder 
        {
            get
            {
                return this._InstanceNameOrder;
            }
            set
            {
                switch (value.ToLower())
                {
                    case "creation frequently":
                    case "by name":
                    case "frequency of use":
                    case "no sorting":
                        this._InstanceNameOrder = value;
                        break;
                    default:
                        this._InstanceNameOrder = "Frequency of use";
                        break;
                }
            }
        }

        public bool DontShowDefaultInstanceWhenMultipleItemsExists { get; set; }

        public bool SearchTargetGroupName { get; set; }
        public bool SearchTargetSubGroupName { get; set; }
        public bool SearchGreedlyGroupName { get; set; }
        public bool SearchGreedlySubGroupName { get; set; }

        public bool ShowCommandSearchBar { get; set; }

        public bool HideNotifyAutomatically { get; set; }

        public bool RememberCommandEditorSizeAndPosition { get; set; }

        public bool CheckForUpdateAtStartup { get; set; }
        public bool SkipBetaVersionUpdate { get; set; }

        private static string InterDefaultBrowserInstanceNameKeyword = "%kwd_default_browser_instance%";
        private static string InterDefaultStopWatchInstanceNameKeyword = "%kwd_default_stopwatch_instance%";
        private static string InterDefaultExcelInstanceNameKeyword = "%kwd_default_excel_instance%";
        private static string InterDefaultWordInstanceNameKeyword = "%kwd_default_word_instance%";
        private static string InterDefaultDBInstanceNameKeyword = "%kwd_default_db_instance%";
        private static string InterDefaultNLGInstanceNameKeyword = "%kwd_default_nlg_instance%";

        public ClientSettings()
        {
            MinimizeToTray = false;
            AntiIdleWhileOpen = false;
            RootFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "taskt");
            StartupMode = "Builder Mode";
            AttendedTasksFolder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "taskt", "My Scripts");
            PreloadBuilderCommands = false;
            UseSlimActionBar = true;
            InsertCommandsInline = true;
            EnableSequenceDragDrop = true;
            InsertVariableAtCursor = true;
            InsertElseAutomatically = false;
            InsertCommentIfLoopAbove = false;
            GroupingBySubgroup = true;
            DontShowValidationMessage = false;
            ShowPoliteTextInDescription = true;
            ShowSampleUsageInDescription = true;
            ShowDefaultValueInDescription = true;
            ShowIndentLine = true;
            ShowScriptMiniMap = false;
            DefaultBrowserInstanceName = "RPABrowser";
            DefaultStopWatchInstanceName = "RPAStopwatch";
            DefaultExcelInstanceName = "RPAExcel";
            DefaultWordInstanceName = "RPAWord";
            DefaultDBInstanceName = "RPADB";
            DefaultNLGInstanceName = "nlgDefaultInstance";
            DontShowDefaultInstanceWhenMultipleItemsExists = false;

            SearchTargetGroupName = true;
            SearchTargetSubGroupName = false;
            SearchGreedlyGroupName = true;
            SearchGreedlySubGroupName = false;

            ShowCommandSearchBar = false;
            HideNotifyAutomatically = true;
            RememberCommandEditorSizeAndPosition = true;

            CheckForUpdateAtStartup = true;
            SkipBetaVersionUpdate = true;
        }

        public string replaceClientKeyword(string targetString)
        {
            return targetString.Replace(InterDefaultBrowserInstanceNameKeyword, this.DefaultBrowserInstanceName)
                    .Replace(InterDefaultStopWatchInstanceNameKeyword, this.DefaultStopWatchInstanceName)
                    .Replace(InterDefaultExcelInstanceNameKeyword, this.DefaultExcelInstanceName)
                    .Replace(InterDefaultWordInstanceNameKeyword, this.DefaultWordInstanceName)
                    .Replace(InterDefaultDBInstanceNameKeyword, this.DefaultDBInstanceName)
                    .Replace(InterDefaultNLGInstanceNameKeyword, this.DefaultNLGInstanceName);
        }
    }
    /// <summary>
    /// Defines Server settings for tasktServer if using the server component to manage the client
    /// </summary>
    [Serializable]
    public class LocalListenerSettings
    {
        public bool StartListenerOnStartup { get; set; }
        public bool LocalListeningEnabled { get; set; }
        public bool RequireListenerAuthenticationKey { get; set; }
        public int ListeningPort { get; set; }
        public string AuthKey { get; set; }
        public bool EnableWhitelist { get; set; }
        public string IPWhiteList { get; set; }
        public LocalListenerSettings()
        {
            StartListenerOnStartup = false;
            LocalListeningEnabled = false;
            RequireListenerAuthenticationKey = false;
            EnableWhitelist = false;
            ListeningPort = 19312;
            AuthKey = Guid.NewGuid().ToString();
            IPWhiteList = "";
        }
    }

    [Serializable]
    public class WhiteListIP
    {
        string _value;
        public WhiteListIP(string s)
        {
            _value = s;
        }
        public string Value { get { return _value; } set { _value = value; } }
    }
}
