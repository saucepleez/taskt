namespace taskt.Core
{
    public interface IClientSettings
    {
        /// <summary>
        /// Anti Idle while open
        /// </summary>
        bool AntiIdleWhileOpen { get; }

        /// <summary>
        /// taskt root folder
        /// </summary>
        string RootFolder { get; }

        /// <summary>
        /// when minimize to system tray
        /// </summary>
        bool MinimizeToTray { get; }

        /// <summary>
        /// attended taskt folder
        /// </summary>
        string AttendedTasksFolder { get; }

        /// <summary>
        /// taskt start up mode
        /// </summary>
        string StartupMode { get; }

        /// <summary>
        /// preload builder commands
        /// </summary>
        bool PreloadBuilderCommands { get; }

        /// <summary>
        /// use slim menu bar in ScriptBuilder
        /// </summary>
        bool UseSlimActionBar { get; }

        /// <summary>
        /// insert commands inline
        /// </summary>
        bool InsertCommandsInline { get; }

        /// <summary>
        /// enable drag&drop to Sequence command
        /// </summary>
        bool EnableSequenceDragDrop { get; }

        /// <summary>
        /// insert variable at curser (textbox, combobox)
        /// </summary>
        bool InsertVariableAtCursor { get; }

        /// <summary>
        /// insert else commands automatically when insert if commands
        /// </summary>
        bool InsertElseAutomatically { get; }

        /// <summary>
        /// insert comment command above if/loop commands
        /// </summary>
        bool InsertCommentIfLoopAbove { get; }

        /// <summary>
        /// grouping by subgruop
        /// </summary>
        bool GroupingBySubgroup { get; }

        /// <summary>
        /// don't show validation message when validation error
        /// </summary>
        bool DontShowValidationMessage { get; }

        /// <summary>
        /// show polite text in parameters description
        /// </summary>
        bool ShowPoliteTextInDescription { get; }
        
        /// <summary>
        /// show sample usage in parameters description
        /// </summary>
        bool ShowSampleUsageInDescription { get; }

        /// <summary>
        /// show default value in parameters description
        /// </summary>
        bool ShowDefaultValueInDescription { get; }

        /// <summary>
        /// show indent line
        /// </summary>
        bool ShowIndentLine { get; }

        /// <summary>
        /// show script minimap
        /// </summary>
        bool ShowScriptMiniMap { get; }

        /// <summary>
        /// indent width
        /// </summary>
        int IndentWidth{ get; }

        /// <summary>
        /// default WebBrowser instance name
        /// </summary>
        string DefaultBrowserInstanceName { get; }

        /// <summary>
        /// default StopWatch instance name
        /// </summary>
        string DefaultStopWatchInstanceName { get; }

        /// <summary>
        /// default Excel instance name
        /// </summary>
        string DefaultExcelInstanceName { get; }

        /// <summary>
        /// default Word instance name
        /// </summary>
        string DefaultWordInstanceName { get; }

        /// <summary>
        /// default DB instance name
        /// </summary>
        string DefaultDBInstanceName { get; }

        /// <summary>
        /// default NLG instance name
        /// </summary>
        string DefaultNLGInstanceName { get; }

        /// <summary>
        /// instance name order
        /// </summary>
        string InstanceNameOrder { get; }

        /// <summary>
        /// don't show default instance when multiple items exists
        /// </summary>
        bool DontShowDefaultInstanceWhenMultipleItemsExists { get; }

        /// <summary>
        /// search target group name
        /// </summary>
        bool SearchTargetGroupName { get; }

        /// <summary>
        /// search target subgroup name
        /// </summary>
        bool SearchTargetSubGroupName { get; }

        /// <summary>
        /// search greedly group name
        /// </summary>
        bool SearchGreedlyGroupName { get; }

        /// <summary>
        /// search greedly subgroup name
        /// </summary>
        bool SearchGreedlySubGroupName { get; }

        /// <summary>
        /// show command search bar
        /// </summary>
        bool ShowCommandSearchBar { get; }

        /// <summary>
        /// hide Notify automatically
        /// </summary>
        bool HideNotifyAutomatically { get; }

        /// <summary>
        /// remember CommandEditor form size and position
        /// </summary>
        bool RememberCommandEditorSizeAndPosition { get; }

        /// <summary>
        /// remember supplement forms size and position
        /// </summary>
        bool RememberSupplementFormsForCommandEditorPosition { get; }

        /// <summary>
        /// check update at startup
        /// </summary>
        bool CheckForUpdateAtStartup { get; }

        /// <summary>
        /// skip beta version
        /// </summary>
        bool SkipBetaVersionUpdate { get; }

        /// <summary>
        /// enable auto save
        /// </summary>
        bool EnabledAutoSave { get; }

        /// <summary>
        /// auto save interval (min)
        /// </summary>
        int AutoSaveInterval { get; }

        /// <summary>
        /// remove auto save files days
        /// </summary>
        int RemoveAutoSaveFileDays { get; }

        /// <summary>
        /// remove run-without-saving files days
        /// </summary>
        int RemoveRunWithtoutSavingFileDays { get; }

        /// <summary>
        /// remove before converted files days
        /// </summary>
        int RemoveBeforeConvertedFileDays { get; }

        /// <summary>
        /// support/show IE commands
        /// </summary>
        bool SupportIECommand { get; }

        /// <summary>
        /// change items with mouse wheel when not focused
        /// </summary>
        bool ChangeItemsWithWheelWhenNotForcused { get; }

        /// <summary>
        /// display number before parameter description
        /// </summary>
        bool DisplayNumberBeforeParameterDescription { get; }

        /// <summary>
        /// display parameter order in description (instead of number)
        /// </summary>
        bool DisplayParameterOrderInDescription { get; }
    }
}
