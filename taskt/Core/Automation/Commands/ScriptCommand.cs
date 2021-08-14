using System;
using System.Xml.Serialization;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    // API
    [XmlInclude(typeof(ExecuteDLLCommand))]
    [XmlInclude(typeof(RESTCommand))]
    [XmlInclude(typeof(HTTPRequestCommand))]
    [XmlInclude(typeof(HTTPQueryResultCommand))]

    // Data
    [XmlInclude(typeof(ConvertListToJSONCommand))]
    [XmlInclude(typeof(DateCalculationCommand))]
    [XmlInclude(typeof(FormatDataCommand))]
    [XmlInclude(typeof(GetListCountCommand))]
    [XmlInclude(typeof(GetListItemCommand))]
    [XmlInclude(typeof(GetWordCountCommand))]
    [XmlInclude(typeof(GetWordLengthCommand))]
    [XmlInclude(typeof(LogDataCommand))]
    [XmlInclude(typeof(MathCalculationCommand))]
    [XmlInclude(typeof(ModifyVariableCommand))]
    [XmlInclude(typeof(ParseDatasetRowCommand))]
    [XmlInclude(typeof(ParseJSONArrayCommand))]
    [XmlInclude(typeof(ParseJsonCommand))]
    [XmlInclude(typeof(ParseJsonModelCommand))]
    [XmlInclude(typeof(PDFTextExtractionCommand))]
    [XmlInclude(typeof(RegExExtractorCommand))]
    [XmlInclude(typeof(StringCheckTextCommand))]
    [XmlInclude(typeof(StringReplaceCommand))]
    [XmlInclude(typeof(StringSubstringCommand))]
    [XmlInclude(typeof(StringSplitCommand))]
    [XmlInclude(typeof(TextExtractorCommand))]

    // Database
    [XmlInclude(typeof(DatabaseDefineConnectionCommand))]
    [XmlInclude(typeof(DatabaseExecuteQueryCommand))]

    // DataTable
    [XmlInclude(typeof(AddDataRowCommand))]
    [XmlInclude(typeof(FilterDataTableCommand))]
    [XmlInclude(typeof(CreateDataTableCommand))]
    [XmlInclude(typeof(GetDataRowCommand))]
    [XmlInclude(typeof(GetDataRowCountCommand))]
    [XmlInclude(typeof(GetDataRowValueCommand))]
    [XmlInclude(typeof(RemoveDataRowCommand))]
    [XmlInclude(typeof(WriteDataRowValueCommand))]

    // Dictionary
    [XmlInclude(typeof(AddDictionaryCommand))]
    [XmlInclude(typeof(CreateDictionaryCommand))]
    [XmlInclude(typeof(GetDictionaryValueCommand))]
    [XmlInclude(typeof(LoadDictionaryCommand))]

    // Engine
    [XmlInclude(typeof(ErrorHandlingCommand))]
    [XmlInclude(typeof(GetDataCommand))]
    [XmlInclude(typeof(PauseCommand))]
    [XmlInclude(typeof(SetEngineDelayCommand))]
    [XmlInclude(typeof(SetEnginePreferenceCommand))]
    [XmlInclude(typeof(ShowEngineContextCommand))]
    [XmlInclude(typeof(StopwatchCommand))]
    [XmlInclude(typeof(UploadDataCommand))]

    // Error
    [XmlInclude(typeof(CatchExceptionCommand))]
    [XmlInclude(typeof(EndTryCommand))]
    [XmlInclude(typeof(FinallyCommand))]
    [XmlInclude(typeof(ThrowExceptionCommand))]
    [XmlInclude(typeof(TryCommand))]

    // Excel
    [XmlInclude(typeof(ExcelActivateSheetCommand))]
    [XmlInclude(typeof(ExcelAddWorkbookCommand))]
    [XmlInclude(typeof(ExcelAppendCellCommand))]
    [XmlInclude(typeof(ExcelAppendRowCommand))]
    [XmlInclude(typeof(ExcelCheckCellValueExistsCommand))]
    [XmlInclude(typeof(ExcelCheckCellValueExistsRCCommand))]
    [XmlInclude(typeof(ExcelCheckExcelInstanceExistsCommand))]
    [XmlInclude(typeof(ExcelCheckWorksheetExistsCommand))]
    [XmlInclude(typeof(ExcelCloseApplicationCommand))]
    [XmlInclude(typeof(ExcelCopyWorksheetCommand))]
    [XmlInclude(typeof(ExcelCreateDataSetCommand))]
    [XmlInclude(typeof(ExcelCreateApplicationCommand))]
    [XmlInclude(typeof(ExcelDeleteCellCommand))]
    [XmlInclude(typeof(ExcelDeleteRowCommand))]
    [XmlInclude(typeof(ExcelDeleteWorksheetCommand))]
    [XmlInclude(typeof(ExcelGetCellCommand))]
    [XmlInclude(typeof(ExcelGetCellRCCommand))]
    [XmlInclude(typeof(ExcelGetCurrentWorksheetCommand))]
    [XmlInclude(typeof(ExcelGetExcelInfoCommand))]
    [XmlInclude(typeof(ExcelGetLastRowCommand))]
    [XmlInclude(typeof(ExcelGetRangeCommand))]
    [XmlInclude(typeof(ExcelGetRangeCommandAsDT))]
    [XmlInclude(typeof(ExcelGetWorksheetsCommand))]
    [XmlInclude(typeof(ExcelGetWorksheetInfoCommand))]
    [XmlInclude(typeof(ExcelGoToCellCommand))]
    [XmlInclude(typeof(ExcelOpenWorkbookCommand))]
    [XmlInclude(typeof(ExcelRenameWorksheetCommand))]
    [XmlInclude(typeof(ExcelRunMacroCommand))]
    [XmlInclude(typeof(ExcelSaveCommand))]
    [XmlInclude(typeof(ExcelSaveAsCommand))]
    [XmlInclude(typeof(ExcelSetCellCommand))]
    [XmlInclude(typeof(ExcelSetCellRCCommand))]
    [XmlInclude(typeof(ExcelSplitRangeByColumnCommand))]
    [XmlInclude(typeof(ExcelWriteRangeCommand))]
    [XmlInclude(typeof(ExcelWriteRowCommand))]

    // File
    [XmlInclude(typeof(CheckFileExistsCommand))]
    [XmlInclude(typeof(DeleteFileCommand))]
    [XmlInclude(typeof(ExtractFileCommand))]
    [XmlInclude(typeof(GetFilesCommand))]
    [XmlInclude(typeof(MoveFileCommand))]
    [XmlInclude(typeof(RenameFileCommand))]
    [XmlInclude(typeof(WaitForFileToExistCommand))]

    // Folder
    [XmlInclude(typeof(CheckFolderExistsCommand))]
    [XmlInclude(typeof(CreateFolderCommand))]
    [XmlInclude(typeof(DeleteFolderCommand))]
    [XmlInclude(typeof(GetFoldersCommand))]
    [XmlInclude(typeof(MoveFolderCommand))]
    [XmlInclude(typeof(RenameFolderCommand))]

    // IE
    [XmlInclude(typeof(IEBrowserCloseCommand))]
    [XmlInclude(typeof(IEBrowserCreateCommand))]
    [XmlInclude(typeof(IEBrowserElementActionCommand))]
    [XmlInclude(typeof(IEBrowserFindBrowserCommand))]
    [XmlInclude(typeof(IEBrowserNavigateURLCommand))]

    // If
    [XmlInclude(typeof(BeginIfCommand))]
    [XmlInclude(typeof(BeginMultiIfCommand))]
    [XmlInclude(typeof(ElseCommand))]
    [XmlInclude(typeof(EndIfCommand))]

    // Image
    [XmlInclude(typeof(OCRCommand))]
    [XmlInclude(typeof(ImageRecognitionCommand))]
    [XmlInclude(typeof(ScreenshotCommand))]

    // Input
    [XmlInclude(typeof(FileDialogCommand))]
    [XmlInclude(typeof(FolderDialogCommand))]
    [XmlInclude(typeof(HTMLInputCommand))]
    [XmlInclude(typeof(UserInputCommand))]
    [XmlInclude(typeof(SendAdvancedKeyStrokesCommand))]
    [XmlInclude(typeof(SendHotkeyCommand))]
    [XmlInclude(typeof(SendKeysCommand))]
    [XmlInclude(typeof(SendMouseClickCommand))]
    [XmlInclude(typeof(SendMouseMoveCommand))]
    [XmlInclude(typeof(UIAutomationCommand))]

    // Loop
    [XmlInclude(typeof(BeginLoopCommand))]
    [XmlInclude(typeof(BeginMultiLoopCommand))]
    [XmlInclude(typeof(EndLoopCommand))]
    [XmlInclude(typeof(ExitLoopCommand))]
    [XmlInclude(typeof(BeginContinousLoopCommand))]
    [XmlInclude(typeof(BeginListLoopCommand))]
    [XmlInclude(typeof(BeginNumberOfTimesLoopCommand))]
    [XmlInclude(typeof(NextLoopCommand))]

    // Misc
    [XmlInclude(typeof(ClipboardClearTextCommand))]
    [XmlInclude(typeof(CommentCommand))]
    [XmlInclude(typeof(EncryptionCommand))]
    [XmlInclude(typeof(ClipboardGetTextCommand))]
    [XmlInclude(typeof(PingCommand))]
    [XmlInclude(typeof(SMTPSendEmailCommand))]
    [XmlInclude(typeof(SequenceCommand))]
    [XmlInclude(typeof(ClipboardSetTextCommand))]
    [XmlInclude(typeof(MessageBoxCommand))]

    // NLG
    [XmlInclude(typeof(NLGCreateInstanceCommand))]
    [XmlInclude(typeof(NLGGeneratePhraseCommand))]
    [XmlInclude(typeof(NLGSetParameterCommand))]

    // Outlook
    [XmlInclude(typeof(OutlookDeleteEmailsCommand))]
    [XmlInclude(typeof(OutlookForwardEmailsCommand))]
    [XmlInclude(typeof(OutlookGetEmailsCommand))]
    [XmlInclude(typeof(OutlookMoveEmailsCommand))]
    [XmlInclude(typeof(OutlookReplyToEmailsCommand))]
    [XmlInclude(typeof(OutlookEmailCommand))]

    // Program
    [XmlInclude(typeof(RunCustomCodeCommand))]
    [XmlInclude(typeof(RunScriptCommand))]
    [XmlInclude(typeof(StartProcessCommand))]
    [XmlInclude(typeof(StopProcessCommand))]
    [XmlInclude(typeof(RunPowershellCommand))]

    // Regex
    [XmlInclude(typeof(GetRegexMatchesCommand))]

    // Remote
    [XmlInclude(typeof(RemoteAPICommand))]
    [XmlInclude(typeof(RemoteTaskCommand))]

    // System
    [XmlInclude(typeof(EnvironmentVariableCommand))]
    [XmlInclude(typeof(RemoteDesktopCommand))]
    [XmlInclude(typeof(OSVariableCommand))]
    [XmlInclude(typeof(SystemActionCommand))]

    // Task
    [XmlInclude(typeof(RunTaskCommand))]
    [XmlInclude(typeof(StopTaskCommand))]

    // Text
    [XmlInclude(typeof(ReadTextFileCommand))]
    [XmlInclude(typeof(WriteTextFileCommand))]

    // Variable
    [XmlInclude(typeof(AddToVariableCommand))]
    [XmlInclude(typeof(VariableCommand))]
    [XmlInclude(typeof(AddVariableCommand))]
    [XmlInclude(typeof(SetVariableIndexCommand))]

    // Web
    [XmlInclude(typeof(SeleniumBrowserCheckBrowserInstanceExistsCommand))]
    [XmlInclude(typeof(SeleniumBrowserCloseCommand))]
    [XmlInclude(typeof(SeleniumBrowserCreateCommand))]
    [XmlInclude(typeof(SeleniumBrowserElementActionCommand))]
    [XmlInclude(typeof(SeleniumBrowserExecuteScriptCommand))]
    [XmlInclude(typeof(SeleniumBrowserInfoCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateBackCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateForwardCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateURLCommand))]
    [XmlInclude(typeof(SeleniumBrowserRefreshCommand))]
    [XmlInclude(typeof(SeleniumBrowserSwitchFrameCommand))]
    [XmlInclude(typeof(SeleniumBrowserSwitchWindowCommand))]
    [XmlInclude(typeof(SeleniumBrowserTakeScreenshotCommand))]

    // Window
    [XmlInclude(typeof(ActivateWindowCommand))]
    [XmlInclude(typeof(CheckWindowNameExistsCommand))]
    [XmlInclude(typeof(CloseWindowCommand))]
    [XmlInclude(typeof(GetWindowNamesCommand))]
    [XmlInclude(typeof(GetWindowPositionCommand))]
    [XmlInclude(typeof(GetWindowStateCommand))]
    [XmlInclude(typeof(MoveWindowCommand))]
    [XmlInclude(typeof(ResizeWindowCommand))]
    [XmlInclude(typeof(SetWindowStateCommand))]
    [XmlInclude(typeof(WaitForWindowCommand))]

    // Word
    [XmlInclude(typeof(WordAddDocumentCommand))]
    [XmlInclude(typeof(WordAppendDataTableCommand))]
    [XmlInclude(typeof(WordAppendImageCommand))]
    [XmlInclude(typeof(WordAppendTextCommand))]
    [XmlInclude(typeof(WordCheckWordInstanceExistsCommand))]
    [XmlInclude(typeof(WordCloseApplicationCommand))]
    [XmlInclude(typeof(WordCreateApplicationCommand))]
    [XmlInclude(typeof(WordExportToPDFCommand))]
    [XmlInclude(typeof(WordOpenDocumentCommand))]
    [XmlInclude(typeof(WordReadDocumentCommand))]
    [XmlInclude(typeof(WordReplaceTextCommand))]
    [XmlInclude(typeof(WordSaveCommand))]
    [XmlInclude(typeof(WordSaveAsCommand))]


    // ?
    [XmlInclude(typeof(ThickAppClickItemCommand))]
    [XmlInclude(typeof(ThickAppGetTextCommand))]
    [XmlInclude(typeof(DatabaseRunQueryCommand))]
    [XmlInclude(typeof(BeginExcelDatasetLoopCommand))]
    [XmlInclude(typeof(ExcelAddWorksheetCommand))]
    [XmlInclude(typeof(LoadTaskCommand))]
    [XmlInclude(typeof(UnloadTaskCommand))]

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
        public bool IsValid { get; protected set; }

        [XmlIgnore]
        public string validationResult { get; protected set; }

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
            this.IsValid = true;
            this.validationResult = "";
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

        public virtual List<Control> Render(UI.Forms.frmCommandEditor editor, object sender)
        {
            RenderedControls = new List<Control>();
            return RenderedControls;
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

        public virtual void BeforeValidate()
        {
        }

        public virtual bool IsValidate(UI.Forms.frmCommandEditor editor)
        {
            this.IsValid = true;
            this.validationResult = "";
            return true;
        }

        public System.Reflection.PropertyInfo GetProperty(string propertyName)
        {
            return this.GetType().GetProperty(propertyName);
        }

        public static List<Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo> GetAdditionalParameterInfo(System.Reflection.PropertyInfo prop)
        {
            return prop.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo), true).Cast<Core.Automation.Attributes.PropertyAttributes.PropertyAddtionalParameterInfo>().ToList();
        }
    }
}