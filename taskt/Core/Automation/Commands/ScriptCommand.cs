using System;
using System.Xml.Serialization;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [XmlInclude(typeof(SendKeystrokesCommand))]
    [XmlInclude(typeof(SendAdvancedKeystrokesCommand))]
    [XmlInclude(typeof(CreateDictionaryCommand))]
    [XmlInclude(typeof(SendMouseMoveCommand))]
    [XmlInclude(typeof(PauseScriptCommand))]
    [XmlInclude(typeof(AddDictionaryItemCommand))]
    [XmlInclude(typeof(SendOutlookEmailCommand))]
    [XmlInclude(typeof(GetOutlookEmailsCommand))]
    [XmlInclude(typeof(MoveOutlookEmailsCommand))]
    [XmlInclude(typeof(DeleteOutlookEmailsCommand))]
    [XmlInclude(typeof(ForwardOutlookEmailsCommand))]
    [XmlInclude(typeof(ReplyToOutlookEmailsCommand))]
    [XmlInclude(typeof(ActivateWindowCommand))]
    [XmlInclude(typeof(GetRegexMatchesCommand))]
    [XmlInclude(typeof(MoveWindowCommand))]
    [XmlInclude(typeof(AddCodeCommentCommand))]
    [XmlInclude(typeof(FilterDataTableCommand))]
    [XmlInclude(typeof(CreateDataTableCommand))]
    [XmlInclude(typeof(MergeDataTableCommand))]
    [XmlInclude(typeof(GetDictionaryValueCommand))]
    [XmlInclude(typeof(AddDataRowCommand))]
    [XmlInclude(typeof(GetDataRowCommand))]
    [XmlInclude(typeof(GetDataRowValueCommand))]
    [XmlInclude(typeof(WriteDataRowValueCommand))]
    [XmlInclude(typeof(GetDataRowCountCommand))]
    [XmlInclude(typeof(RemoveDataRowCommand))]
    [XmlInclude(typeof(ExcelAppendRowCommand))]
    [XmlInclude(typeof(ExcelAppendCellCommand))]
    [XmlInclude(typeof(ExcelWriteRangeCommand))]
    [XmlInclude(typeof(ExcelWriteRowCommand))]
    [XmlInclude(typeof(ExcelSplitRangeByColumnCommand))]
    [XmlInclude(typeof(UIAutomationCommand))]
    [XmlInclude(typeof(ResizeWindowCommand))]
    [XmlInclude(typeof(WaitForWindowToExistCommand))]
    [XmlInclude(typeof(ShowMessageCommand))]
    [XmlInclude(typeof(StopProcessCommand))]
    [XmlInclude(typeof(StartProcessCommand))]
    [XmlInclude(typeof(NewVariableCommand))]
    [XmlInclude(typeof(SetVariableCommand))]
    [XmlInclude(typeof(RunScriptCommand))]
    [XmlInclude(typeof(CloseWindowCommand))]
    [XmlInclude(typeof(SetWindowStateCommand))]
    [XmlInclude(typeof(ExitLoopCommand))]
    [XmlInclude(typeof(EndLoopCommand))]
    [XmlInclude(typeof(GetClipboardTextCommand))]
    [XmlInclude(typeof(SetClipboardTextCommand))]
    [XmlInclude(typeof(TakeScreenshotCommand))]
    [XmlInclude(typeof(ExcelOpenWorkbookCommand))]
    [XmlInclude(typeof(ExcelCreateApplicationCommand))]
    [XmlInclude(typeof(ExcelAddWorkbookCommand))]
    [XmlInclude(typeof(ExcelActivateCellCommand))]
    [XmlInclude(typeof(ExcelWriteCellCommand))]
    [XmlInclude(typeof(ExcelCloseApplicationCommand))]
    [XmlInclude(typeof(ExcelGetCellCommand))]
    [XmlInclude(typeof(ExcelGetRangeCommand))]
    [XmlInclude(typeof(ExcelRunMacroCommand))]
    [XmlInclude(typeof(ExcelActivateSheetCommand))]
    [XmlInclude(typeof(ExcelDeleteRowCommand))]
    [XmlInclude(typeof(ExcelDeleteRangeCommand))]
    [XmlInclude(typeof(ExcelGetLastRowIndexCommand))]
    [XmlInclude(typeof(ExcelSaveWorkbookAsCommand))]
    [XmlInclude(typeof(ExcelSaveWorkbookCommand))]
    [XmlInclude(typeof(WordCreateApplicationCommand))]
    [XmlInclude(typeof(WordCloseApplicationCommand))]
    [XmlInclude(typeof(WordOpenDocumentCommand))]
    [XmlInclude(typeof(WordSaveDocumentCommand))]
    [XmlInclude(typeof(WordSaveDocumentAsCommand))]
    [XmlInclude(typeof(WordExportToPDFCommand))]
    [XmlInclude(typeof(WordAddDocumentCommand))]
    [XmlInclude(typeof(WordReadDocumentCommand))]
    [XmlInclude(typeof(WordReplaceTextCommand))]
    [XmlInclude(typeof(WordAppendTextCommand))]
    [XmlInclude(typeof(WordAppendImageCommand))]
    [XmlInclude(typeof(WordAppendDataTableCommand))]
    [XmlInclude(typeof(SeleniumCreateBrowserCommand))]
    [XmlInclude(typeof(SeleniumNavigateToURLCommand))]
    [XmlInclude(typeof(SeleniumNavigateForwardCommand))]
    [XmlInclude(typeof(SeleniumNavigateBackCommand))]
    [XmlInclude(typeof(SeleniumRefreshCommand))]
    [XmlInclude(typeof(SeleniumCloseBrowserCommand))]
    [XmlInclude(typeof(SeleniumElementActionCommand))]
    [XmlInclude(typeof(SeleniumExecuteScriptCommand))]
    [XmlInclude(typeof(SeleniumSwitchBrowserWindowCommand))]
    [XmlInclude(typeof(SeleniumGetBrowserInfoCommand))]
    [XmlInclude(typeof(SendSMTPEmailCommand))]
    [XmlInclude(typeof(ErrorHandlingCommand))]
    [XmlInclude(typeof(SubstringCommand))]
    [XmlInclude(typeof(SplitTextCommand))]
    [XmlInclude(typeof(BeginIfCommand))]
    [XmlInclude(typeof(BeginMultiIfCommand))]
    [XmlInclude(typeof(EndIfCommand))]
    [XmlInclude(typeof(ElseCommand))]
    [XmlInclude(typeof(PerformOCRCommand))]
    [XmlInclude(typeof(GetHTMLSourceCommand))]
    [XmlInclude(typeof(QueryHTMLSourceCommand))]
    [XmlInclude(typeof(ImageRecognitionCommand))]
    [XmlInclude(typeof(SendMouseClickCommand))]
    [XmlInclude(typeof(LoopNumberOfTimesCommand))]
    [XmlInclude(typeof(LoopListCommand))]
    [XmlInclude(typeof(NextLoopCommand))]
    [XmlInclude(typeof(LoopContinuouslyCommand))]
    [XmlInclude(typeof(BeginLoopCommand))]
    [XmlInclude(typeof(BeginMultiLoopCommand))]
    [XmlInclude(typeof(SequenceCommand))]
    [XmlInclude(typeof(StopCurrentTaskCommand))]
    [XmlInclude(typeof(RunTaskCommand))]
    [XmlInclude(typeof(WriteTextFileCommand))]
    [XmlInclude(typeof(ReadTextFileCommand))]
    [XmlInclude(typeof(MoveCopyFileCommand))]
    [XmlInclude(typeof(DeleteFileCommand))]
    [XmlInclude(typeof(RenameFileCommand))]
    [XmlInclude(typeof(WaitForFileCommand))]
    [XmlInclude(typeof(GetFilesCommand))]
    [XmlInclude(typeof(GetFoldersCommand))]
    [XmlInclude(typeof(CreateFolderCommand))]
    [XmlInclude(typeof(DeleteFolderCommand))]
    [XmlInclude(typeof(MoveCopyFolderCommand))]
    [XmlInclude(typeof(RenameFolderCommand))]
    [XmlInclude(typeof(RunCustomCodeCommand))]
    [XmlInclude(typeof(DateCalculationCommand))]
    [XmlInclude(typeof(RegexIsMatchCommand))]
    [XmlInclude(typeof(RegexReplaceCommand))]
    [XmlInclude(typeof(RegexSplitCommand))]
    [XmlInclude(typeof(TextExtractionCommand))]
    [XmlInclude(typeof(FormatDataCommand))]
    [XmlInclude(typeof(LogDataCommand))]
    [XmlInclude(typeof(ReplaceTextCommand))]
    [XmlInclude(typeof(ExecuteDLLCommand))]
    [XmlInclude(typeof(SetEngineDelayCommand))]
    [XmlInclude(typeof(GetPDFTextCommand))]
    [XmlInclude(typeof(InputCommand))]
    [XmlInclude(typeof(GetWordCountCommand))]
    [XmlInclude(typeof(GetListCountCommand))]
    [XmlInclude(typeof(GetListItemCommand))]
    [XmlInclude(typeof(GetTextLengthCommand))]
    [XmlInclude(typeof(HTMLInputCommand))]
    [XmlInclude(typeof(UploadBotStoreDataCommand))]
    [XmlInclude(typeof(GetBotStoreDataCommand))]
    [XmlInclude(typeof(ExecuteRESTAPICommand))]
    [XmlInclude(typeof(ParseJSONArrayCommand))]
    [XmlInclude(typeof(StopwatchCommand))]
    [XmlInclude(typeof(SystemActionCommand))]
    [XmlInclude(typeof(LaunchRemoteDesktopCommand))]
    [XmlInclude(typeof(EnvironmentVariableCommand))]
    [XmlInclude(typeof(OSVariableCommand))]
    [XmlInclude(typeof(ModifyStringCommand))]
    [XmlInclude(typeof(CreateNLGInstanceCommand))]
    [XmlInclude(typeof(SetNLGParameterCommand))]
    [XmlInclude(typeof(GenerateNLGPhraseCommand))]
    [XmlInclude(typeof(IECreateBrowserCommand))]
    [XmlInclude(typeof(IECloseBrowserCommand))]
    [XmlInclude(typeof(IEFindBrowserCommand))]
    [XmlInclude(typeof(IEElementActionCommand))]
    [XmlInclude(typeof(IENavigateToURLCommand))]
    [XmlInclude(typeof(AddToVariableCommand))]
    [XmlInclude(typeof(SetVariableIndexCommand))]
    [XmlInclude(typeof(ShowEngineContextCommand))]
    [XmlInclude(typeof(DefineDatabaseConnectionCommand))]
    [XmlInclude(typeof(ExecuteDatabaseQueryCommand))]
    [XmlInclude(typeof(TryCommand))]
    [XmlInclude(typeof(CatchExceptionCommand))]
    [XmlInclude(typeof(FinallyCommand))]
    [XmlInclude(typeof(EndTryCommand))]
    [XmlInclude(typeof(ThrowExceptionCommand))]
    [XmlInclude(typeof(GetExceptionMessageCommand))]
    [XmlInclude(typeof(RemoteTaskCommand))]
    [XmlInclude(typeof(RemoteAPICommand))]
    [XmlInclude(typeof(SeleniumSwitchBrowserFrameCommand))]
    [XmlInclude(typeof(ParseJSONModelCommand))]
    [XmlInclude(typeof(EncryptionCommand))]
    [XmlInclude(typeof(MathCalculationCommand))]
    [XmlInclude(typeof(SetEnginePreferenceCommand))]
    [XmlInclude(typeof(ExtractFilesCommand))]
    public abstract class ScriptCommand
    {
        [XmlAttribute]
        public string CommandID { get; set; }
        [XmlAttribute]
        public string CommandName { get; set; }
        [XmlAttribute]
        public bool IsCommented { get; set; }
        [XmlAttribute]
        public string SelectionName { get; set; }
        [XmlAttribute]
        public int DefaultPause { get; set; }
        [XmlAttribute]
        public int LineNumber { get; set; }
        [XmlAttribute]
        public bool PauseBeforeExeucution { get; set; }
        [XmlIgnore]
        public Color DisplayForeColor { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Comment Field (Optional)")]
        [Attributes.PropertyAttributes.InputSpecification("Optional field to enter a custom comment which could potentially describe this command or the need for this command, if required")]
        [Attributes.PropertyAttributes.SampleUsage("I am using this command to ...")]
        [Attributes.PropertyAttributes.Remarks("Optional")]
        public string v_Comment { get; set; }
        [XmlAttribute]
        public bool CommandEnabled { get; set; }

        [XmlIgnore]
        public bool CustomRendering { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public List<Control> RenderedControls;

        public ScriptCommand()
        {
            this.DisplayForeColor = System.Drawing.Color.SteelBlue;
            this.CommandEnabled = false;
            this.DefaultPause = 0;
            this.IsCommented = false;
            this.CustomRendering = false;
            this.GenerateID();
        }

        public void GenerateID()
        {
            var id = Guid.NewGuid();
            this.CommandID = id.ToString();
        }

        public virtual void RunCommand(object sender)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }
        public virtual void RunCommand(object sender, Core.Script.ScriptAction command)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }

        public virtual string GetDisplayValue()
        {
            if (String.IsNullOrEmpty(v_Comment))
            {
                return SelectionName;
            }
            else
            {
                return SelectionName + " [" + v_Comment + "]";
            }
        }

        public virtual List<Control> Render(UI.Forms.frmCommandEditor editor)
        {
            RenderedControls = new List<Control>();
            return RenderedControls;
        }

        public virtual List<Control> Render()
        {
            RenderedControls = new List<Control>();
            return RenderedControls;
        }

        public virtual void Refresh(UI.Forms.frmCommandEditor editor = null)
        {

        }

    }
}