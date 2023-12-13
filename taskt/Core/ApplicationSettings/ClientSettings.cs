using System;
using System.IO;

namespace taskt.Core
{
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

        public bool RememberSupplementFormsForCommandEditorPosition { get; set; }

        public bool CheckForUpdateAtStartup { get; set; }
        public bool SkipBetaVersionUpdate { get; set; }

        public bool EnabledAutoSave { get; set; }

        private int _AutoSaveInterval;
        public int AutoSaveInterval
        {
            get
            {
                return _AutoSaveInterval;
            }
            set
            {
                if (value >= 1 && value <= 120)
                {
                    _AutoSaveInterval = value;
                }
            }
        }

        private int _RemoveAutoSaveFileDays;
        public int RemoveAutoSaveFileDays
        {
            get
            {
                return _RemoveAutoSaveFileDays;
            }
            set
            {
                if (value > 1)
                {
                    _RemoveAutoSaveFileDays = value;
                }
            }
        }

        private int _RemoveRunWithoutSavingFileDays;
        public int RemoveRunWithtoutSavingFileDays
        {
            get
            {
                return _RemoveRunWithoutSavingFileDays;
            }
            set
            {
                if (value > 1)
                {
                    _RemoveRunWithoutSavingFileDays = value;
                }
            }
        }

        public bool SupportIECommand { get; set; }

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
            RootFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "taskt");
            StartupMode = "Builder Mode";
            AttendedTasksFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "taskt", "My Scripts");
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
            RememberSupplementFormsForCommandEditorPosition = true;

            CheckForUpdateAtStartup = true;
            SkipBetaVersionUpdate = true;

            EnabledAutoSave = true;
            AutoSaveInterval = 5;
            RemoveAutoSaveFileDays = 7;

            RemoveRunWithtoutSavingFileDays = 30;

            SupportIECommand = false;
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
}
