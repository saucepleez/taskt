namespace taskt.Core
{
    /// <summary>
    /// ClientSettings
    /// </summary>
    public class SafeClientSettings : IClientSettings
    {
        /// <summary>
        /// Client Settings for protect
        /// </summary>
        protected readonly ClientSettings clientSettings;

        public SafeClientSettings(ClientSettings clientSettings)
        {
            this.clientSettings = clientSettings;
        }

        public bool AntiIdleWhileOpen
        {
            get
            {
                return clientSettings.AntiIdleWhileOpen;
            }
        }
        public string RootFolder
        {
            get
            {
                return clientSettings.RootFolder;
            }
        }
        public bool MinimizeToTray
        {
            get
            {
                return clientSettings.MinimizeToTray;
            }
        }
        public string AttendedTasksFolder
        {
            get
            {
                return clientSettings.AttendedTasksFolder;
            }
        }
        public string StartupMode
        {
            get
            {
                return clientSettings.StartupMode;
            }
        }
        public bool PreloadBuilderCommands
        {
            get
            {
                return clientSettings.PreloadBuilderCommands;
            }
        }
        public bool UseSlimActionBar
        {
            get
            {
                return clientSettings.UseSlimActionBar;
            }
        }
        public bool InsertCommandsInline
        {
            get
            {
                return clientSettings.InsertCommandsInline;
            }
        }
        public bool EnableSequenceDragDrop
        {
            get
            {
                return clientSettings.EnableSequenceDragDrop;
            }
        }
        public bool InsertVariableAtCursor
        {
            get
            {
                return clientSettings.InsertVariableAtCursor;
            }
        }
        public bool InsertElseAutomatically
        {
            get
            {
                return clientSettings.InsertElseAutomatically;
            }
        }
        public bool InsertCommentIfLoopAbove
        {
            get
            {
                return clientSettings.InsertCommentIfLoopAbove;
            }
        }
        public bool GroupingBySubgroup
        {
            get
            {
                return clientSettings.GroupingBySubgroup;
            }
        }
        public bool DontShowValidationMessage
        {
            get
            {
                return clientSettings.DontShowValidationMessage;
            }
        }
        public bool ShowPoliteTextInDescription
        {
            get
            {
                return clientSettings.ShowPoliteTextInDescription;
            }
        }
        public bool ShowSampleUsageInDescription
        {
            get
            {
                return clientSettings.ShowSampleUsageInDescription;
            }
        }
        public bool ShowDefaultValueInDescription
        {
            get
            {
                return clientSettings.ShowDefaultValueInDescription;
            }
        }
        public bool ShowIndentLine
        {
            get
            {
                return clientSettings.ShowIndentLine;
            }
        }
        public bool ShowScriptMiniMap
        {
            get
            {
                return clientSettings.ShowScriptMiniMap;
            }
        }
        public int IndentWidth
        {
            get
            {
                return clientSettings.IndentWidth;
            }
        }
        public string DefaultBrowserInstanceName
        {
            get
            {
                return clientSettings.DefaultBrowserInstanceName;
            }
        }
        public string DefaultStopWatchInstanceName
        {
            get
            {
                return clientSettings.DefaultStopWatchInstanceName;
            }
        }
        public string DefaultExcelInstanceName
        {
            get
            {
                return clientSettings.DefaultExcelInstanceName;
            }
        }
        public string DefaultWordInstanceName
        {
            get
            {
                return clientSettings.DefaultWordInstanceName;
            }
        }
        public string DefaultDBInstanceName
        {
            get
            {
                return clientSettings.DefaultDBInstanceName;
            }
        }
        public string DefaultNLGInstanceName
        {
            get
            {
                return clientSettings.DefaultNLGInstanceName;
            }
        }
        public string InstanceNameOrder
        {
            get
            {
                return clientSettings.InstanceNameOrder;
            }
        }
        public bool DontShowDefaultInstanceWhenMultipleItemsExists
        {
            get
            {
                return clientSettings.DontShowDefaultInstanceWhenMultipleItemsExists;
            }
        }
        public bool SearchTargetGroupName
        {
            get
            {
                return clientSettings.SearchTargetGroupName;
            }
        }
        public bool SearchTargetSubGroupName
        {
            get
            {
                return clientSettings.SearchTargetSubGroupName;
            }
        }
        public bool SearchGreedlyGroupName
        {
            get
            {
                return clientSettings.SearchGreedlyGroupName;
            }
        }
        public bool SearchGreedlySubGroupName
        {
            get
            {
                return clientSettings.SearchGreedlySubGroupName;
            }
        }
        public bool ShowCommandSearchBar
        {
            get
            {
                return clientSettings.ShowCommandSearchBar;
            }
        }
        public bool HideNotifyAutomatically
        {
            get
            {
                return clientSettings.HideNotifyAutomatically;
            }
        }
        public bool RememberCommandEditorSizeAndPosition
        {
            get
            {
                return clientSettings.RememberCommandEditorSizeAndPosition;
            }
        }
        public bool RememberSupplementFormsForCommandEditorPosition
        {
            get
            {
                return clientSettings.RememberSupplementFormsForCommandEditorPosition;
            }
        }
        public bool CheckForUpdateAtStartup
        {
            get
            {
                return clientSettings.CheckForUpdateAtStartup;
            }
        }
        public bool SkipBetaVersionUpdate
        {
            get
            {
                return clientSettings.SkipBetaVersionUpdate;
            }
        }
        public bool EnabledAutoSave
        {
            get
            {
                return clientSettings.EnabledAutoSave;
            }
        }
        public int AutoSaveInterval
        {
            get
            {
                return clientSettings.AutoSaveInterval;
            }
        }
        public int RemoveAutoSaveFileDays
        {
            get
            {
                return clientSettings.RemoveAutoSaveFileDays;
            }
        }
        public int RemoveRunWithtoutSavingFileDays
        {
            get
            {
                return clientSettings.RemoveRunWithtoutSavingFileDays;
            }
        }
        public int RemoveBeforeConvertedFileDays
        {
            get
            {
                return clientSettings.RemoveBeforeConvertedFileDays;
            }
        }
        public bool SupportIECommand
        {
            get
            {
                return clientSettings.SupportIECommand;
            }
        }
        public bool ChangeItemsWithWheelWhenNotForcused
        {
            get
            {
                return clientSettings.ChangeItemsWithWheelWhenNotForcused;
            }
        }
        public bool DisplayNumberBeforeParameterDescription
        {
            get
            {
                return clientSettings.DisplayNumberBeforeParameterDescription;
            }
        }

        public bool DisplayParameterOrderInDescription
        {
            get
            {
                return clientSettings.DisplayParameterOrderInDescription;
            }
        }

        public int GUIInspectMaxSiblings
        {
            get
            {
                return clientSettings.GUIInspectMaxSiblings;
            }
        }

        public int GUIInspectMaxDepth
        {
            get
            {
                return clientSettings.GUIInspectMaxDepth;
            }
        }

        public int GUIInspectSearchTime
        {
            get
            {
                return clientSettings.GUIInspectSearchTime;
            }
        }

        public int GUIInspectMouseInterval
        {
            get
            {
                return clientSettings.GUIInspectMouseInterval;
            }
        }
    }
}
