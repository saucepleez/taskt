using System;
using System.Xml.Serialization;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using taskt.UI.CustomControls;
using OpenQA.Selenium.DevTools.V102.Inspector;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    // API
    [XmlInclude(typeof(ExecuteDLLCommand))]
    [XmlInclude(typeof(RESTCommand))]
    [XmlInclude(typeof(HTTPRequestCommand))]
    [XmlInclude(typeof(HTTPQueryResultCommand))]

    // Color
    [XmlInclude(typeof(CreateColorCommand))]
    [XmlInclude(typeof(CreateColorFromExcelColorCommand))]
    [XmlInclude(typeof(CreateColorFromHexCommand))]
    [XmlInclude(typeof(FormatColorCommand))]

    // Data
    [XmlInclude(typeof(DateCalculationCommand))]
    [XmlInclude(typeof(FormatDataCommand))]
    [XmlInclude(typeof(LogDataCommand))]
    [XmlInclude(typeof(PDFTextExtractionCommand))]

    // Database
    [XmlInclude(typeof(DatabaseDefineConnectionCommand))]
    [XmlInclude(typeof(DatabaseExecuteQueryCommand))]

    // DataTable
    [XmlInclude(typeof(AddDataRowCommand))]
    [XmlInclude(typeof(AddDataTableColumnCommand))]
    [XmlInclude(typeof(AddDataTableRowByDictionaryCommand))]
    [XmlInclude(typeof(AddDataTableRowCommand))]
    [XmlInclude(typeof(AddDataTableRowsByDataTableCommand))]
    [XmlInclude(typeof(CheckDataTableColumnExistsCommand))]
    [XmlInclude(typeof(ConvertDataTableColumnToDataTableCommand))]
    [XmlInclude(typeof(ConvertDataTableColumnToDictionaryCommand))]
    [XmlInclude(typeof(ConvertDataTableColumnToJSONCommand))]
    [XmlInclude(typeof(ConvertDataTableColumnToListCommand))]
    [XmlInclude(typeof(ConvertDataTableRowToDataTableCommand))]
    [XmlInclude(typeof(ConvertDataTableRowToDictionaryCommand))]
    [XmlInclude(typeof(ConvertDataTableRowToJSONCommand))]
    [XmlInclude(typeof(ConvertDataTableRowToListCommand))]
    [XmlInclude(typeof(ConvertDataTableToJSONCommand))]
    [XmlInclude(typeof(CopyDataTableCommand))]
    [XmlInclude(typeof(CreateDataTableCommand))]
    [XmlInclude(typeof(DeleteDataTableColumnCommand))]
    [XmlInclude(typeof(DeleteDataTableRowCommand))]
    [XmlInclude(typeof(FilterDataTableColumnByRowValueCommand))]
    [XmlInclude(typeof(FilterDataTableCommand))]
    [XmlInclude(typeof(FilterDataTableRowByColumnValueCommand))]
    [XmlInclude(typeof(GetDataRowCommand))]
    [XmlInclude(typeof(GetDataRowCountCommand))]
    [XmlInclude(typeof(GetDataRowValueCommand))]
    [XmlInclude(typeof(GetDataTableColumnCountCommand))]
    [XmlInclude(typeof(GetDataTableColumnListCommand))]
    [XmlInclude(typeof(GetDataTableRowCountCommand))]
    [XmlInclude(typeof(GetDataTableValueCommand))]
    [XmlInclude(typeof(ReplaceDataTableColumnValueCommand))]
    [XmlInclude(typeof(ReplaceDataTableRowValueCommand))]
    [XmlInclude(typeof(ReplaceDataTableValueCommand))]
    [XmlInclude(typeof(ParseDatasetRowCommand))]
    [XmlInclude(typeof(RemoveDataRowCommand))]
    [XmlInclude(typeof(SetDataTableColumnValuesByDataTableCommand))]
    [XmlInclude(typeof(SetDataTableColumnValuesByListCommand))]
    [XmlInclude(typeof(SetDataTableRowValuesByDataTableCommand))]
    [XmlInclude(typeof(SetDataTableRowValuesByDictionaryCommand))]
    [XmlInclude(typeof(SetDataTableValueCommand))]
    [XmlInclude(typeof(WriteDataRowValueCommand))]
    [XmlInclude(typeof(LoadDataTableCommand))]

    // DateTime
    [XmlInclude(typeof(CalculateDateTimeCommand))]
    [XmlInclude(typeof(ConvertDateTimeToExcelSerialCommand))]
    [XmlInclude(typeof(CreateDateTimeCommand))]
    [XmlInclude(typeof(CreateDateTimeFromExcelSerialCommand))]
    [XmlInclude(typeof(CreateDateTimeFromTextCommand))]
    [XmlInclude(typeof(GetDateTimeDifferencesCommand))]
    [XmlInclude(typeof(FormatDateTimeCommand))]

    // Dictionary
    [XmlInclude(typeof(AddDictionaryCommand))]
    [XmlInclude(typeof(CheckDictionaryKeyExistsCommand))]
    [XmlInclude(typeof(CreateDictionaryCommand))]
    [XmlInclude(typeof(ConcatenateDictionaryCommand))]
    [XmlInclude(typeof(ConvertDictionaryToDataTableCommand))]
    [XmlInclude(typeof(ConvertDictionaryToJSONCommand))]
    [XmlInclude(typeof(ConvertDictionaryToListCommand))]
    [XmlInclude(typeof(CopyDictionaryCommand))]
    [XmlInclude(typeof(FilterDictionaryCommand))]
    [XmlInclude(typeof(GetDictionaryKeyFromValueCommand))]
    [XmlInclude(typeof(GetDictionaryKeysListCommand))]
    [XmlInclude(typeof(GetDictionaryValueCommand))]
    [XmlInclude(typeof(LoadDictionaryCommand))]
    [XmlInclude(typeof(RemoveDictionaryItemCommand))]
    [XmlInclude(typeof(ReplaceDictionaryCommand))]
    [XmlInclude(typeof(SetDictionaryValueCommand))]

    // EMail
    [XmlInclude(typeof(MailKitGetAddressesAsDataTableCommand))]
    [XmlInclude(typeof(MailKitGetAddressesAsDictionaryCommand))]
    [XmlInclude(typeof(MailKitGetAddressesAsListCommand))]
    [XmlInclude(typeof(MailKitGetEmailAttachmentsNameCommand))]
    [XmlInclude(typeof(MailKitGetEMailFromEMailListCommand))]
    [XmlInclude(typeof(MailKitGetEMailTextCommand))]
    [XmlInclude(typeof(MailKitLoadEmailCommand))]
    [XmlInclude(typeof(MailKitRecieveEmailListUsingIMAPCommand))]
    [XmlInclude(typeof(MailKitRecieveEmailListUsingPOPCommand))]
    [XmlInclude(typeof(MailKitSaveEmailCommand))]
    [XmlInclude(typeof(MailKitSaveEmailAttachmentsCommand))]
    [XmlInclude(typeof(MailKitSendEmailCommand))]

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
    [XmlInclude(typeof(ExcelAddWorksheetCommand))]
    [XmlInclude(typeof(ExcelAppendCellCommand))]
    [XmlInclude(typeof(ExcelAppendRowCommand))]
    [XmlInclude(typeof(ExcelCheckCellValueExistsCommand))]
    [XmlInclude(typeof(ExcelCheckCellValueExistsRCCommand))]
    [XmlInclude(typeof(ExcelCheckExcelInstanceExistsCommand))]
    [XmlInclude(typeof(ExcelCheckWorksheetExistsCommand))]
    [XmlInclude(typeof(ExcelCloseApplicationCommand))]
    [XmlInclude(typeof(ExcelCopyWorksheetCommand))]
    [XmlInclude(typeof(ExcelCreateApplicationCommand))]
    [XmlInclude(typeof(ExcelDeleteCellCommand))]
    [XmlInclude(typeof(ExcelDeleteRowCommand))]
    [XmlInclude(typeof(ExcelDeleteWorksheetCommand))]
    [XmlInclude(typeof(ExcelGetCellCommand))]
    [XmlInclude(typeof(ExcelGetCellRCCommand))]
    [XmlInclude(typeof(ExcelGetColumnValuesAsDataTableCommand))]
    [XmlInclude(typeof(ExcelGetColumnValuesAsDictionaryCommand))]
    [XmlInclude(typeof(ExcelGetColumnValuesAsListCommand))]
    [XmlInclude(typeof(ExcelGetCurrentWorksheetCommand))]
    [XmlInclude(typeof(ExcelGetExcelInfoCommand))]
    [XmlInclude(typeof(ExcelGetLastRowCommand))]
    [XmlInclude(typeof(ExcelGetRangeCommand))]
    [XmlInclude(typeof(ExcelGetRangeCommandAsDT))]
    [XmlInclude(typeof(ExcelGetRangeValuesAsDataTableCommand))]
    [XmlInclude(typeof(ExcelGetRowValuesAsDataTableCommand))]
    [XmlInclude(typeof(ExcelGetRowValuesAsDictionaryCommand))]
    [XmlInclude(typeof(ExcelGetRowValuesAsListCommand))]
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
    [XmlInclude(typeof(ExcelSetColumnValuesFromDataTableCommand))]
    [XmlInclude(typeof(ExcelSetColumnValuesFromDictionaryCommand))]
    [XmlInclude(typeof(ExcelSetColumnValuesFromListCommand))]
    [XmlInclude(typeof(ExcelSetRowValuesFromDataTableCommand))]
    [XmlInclude(typeof(ExcelSetRowValuesFromDictionaryCommand))]
    [XmlInclude(typeof(ExcelSetRowValuesFromListCommand))]
    [XmlInclude(typeof(ExcelSplitRangeByColumnCommand))]
    [XmlInclude(typeof(ExcelWriteRangeCommand))]
    [XmlInclude(typeof(ExcelWriteRowCommand))]

    // File
    [XmlInclude(typeof(CheckFileExistsCommand))]
    [XmlInclude(typeof(DeleteFileCommand))]
    [XmlInclude(typeof(ExtractFileCommand))]
    [XmlInclude(typeof(FormatFilePathCommnad))]
    [XmlInclude(typeof(GetFileInfoCommand))]
    [XmlInclude(typeof(GetFilesCommand))]
    [XmlInclude(typeof(MoveFileCommand))]
    [XmlInclude(typeof(RenameFileCommand))]
    [XmlInclude(typeof(WaitForFileToExistCommand))]

    // Folder
    [XmlInclude(typeof(CheckFolderExistsCommand))]
    [XmlInclude(typeof(CreateFolderCommand))]
    [XmlInclude(typeof(DeleteFolderCommand))]
    [XmlInclude(typeof(FormatFolderPathCommnad))]
    [XmlInclude(typeof(GetFoldersCommand))]
    [XmlInclude(typeof(MoveFolderCommand))]
    [XmlInclude(typeof(RenameFolderCommand))]
    [XmlInclude(typeof(WaitForFolderToExistCommand))]

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

    // JSON
    [XmlInclude(typeof(ConvertJSONToDataTableCommand))]
    [XmlInclude(typeof(ConvertJSONToDictionaryCommand))]
    [XmlInclude(typeof(ConvertJSONToListCommand))]
    [XmlInclude(typeof(ParseJSONArrayCommand))]
    [XmlInclude(typeof(ParseJsonCommand))]
    [XmlInclude(typeof(ParseJsonModelCommand))]
    [XmlInclude(typeof(ReadJSONFileCommand))]

    // List
    [XmlInclude(typeof(AddListItemCommand))]
    [XmlInclude(typeof(CheckListItemExistsCommand))]
    [XmlInclude(typeof(ConcatenateListsCommand))]
    [XmlInclude(typeof(ConvertListToDataTableCommand))]
    [XmlInclude(typeof(ConvertListToDictionaryCommand))]
    [XmlInclude(typeof(ConvertListToJSONCommand))]
    [XmlInclude(typeof(CopyListCommand))]
    [XmlInclude(typeof(CreateListCommand))]
    [XmlInclude(typeof(FilterListCommand))]
    [XmlInclude(typeof(GetAverageFromListCommand))]
    [XmlInclude(typeof(GetListCountCommand))]
    [XmlInclude(typeof(GetListIndexFromValueCommand))]
    [XmlInclude(typeof(GetListItemCommand))]
    [XmlInclude(typeof(GetMaxFromListCommand))]
    [XmlInclude(typeof(GetMedianFromListCommand))]
    [XmlInclude(typeof(GetMinFromListCommand))]
    [XmlInclude(typeof(GetSumFromListCommand))]
    [XmlInclude(typeof(GetVarianceFromListCommand))]
    [XmlInclude(typeof(ReplaceListCommand))]
    [XmlInclude(typeof(ReverseListCommand))]
    [XmlInclude(typeof(SetListIndexCommand))]
    [XmlInclude(typeof(SetListItemCommand))]
    [XmlInclude(typeof(SortListCommand))]

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
    [XmlInclude(typeof(CreateShortcutCommand))]

    // NLG
    [XmlInclude(typeof(NLGCreateInstanceCommand))]
    [XmlInclude(typeof(NLGGeneratePhraseCommand))]
    [XmlInclude(typeof(NLGSetParameterCommand))]

    // Numeric
    [XmlInclude(typeof(CreateNumberVariableCommand))]
    [XmlInclude(typeof(DecreaseNumericalVariableCommand))]
    [XmlInclude(typeof(FormatNumberCommand))]
    [XmlInclude(typeof(IncreaseNumericalVariableCommand))]
    [XmlInclude(typeof(MathCalculationCommand))]
    [XmlInclude(typeof(RandomNumberCommand))]
    [XmlInclude(typeof(RoundNumberCommand))]

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
    [XmlInclude(typeof(LoadTaskCommand))]
    [XmlInclude(typeof(RunTaskCommand))]
    [XmlInclude(typeof(StopTaskCommand))]
    [XmlInclude(typeof(UnloadTaskCommand))]

    // Text
    [XmlInclude(typeof(CheckTextCommand))]
    [XmlInclude(typeof(ConcatenateTextVariableCommand))]
    [XmlInclude(typeof(ExtractionTextCommand))]
    [XmlInclude(typeof(GetWordCountCommand))]
    [XmlInclude(typeof(GetWordLengthCommand))]
    [XmlInclude(typeof(ModifyTextCommand))]
    [XmlInclude(typeof(ReadTextFileCommand))]
    [XmlInclude(typeof(RegExExtractionTextCommand))]
    [XmlInclude(typeof(ReplaceTextCommand))]
    [XmlInclude(typeof(SplitTextCommand))]
    [XmlInclude(typeof(SubstringTextCommand))]
    [XmlInclude(typeof(WriteTextFileCommand))]

    // UIAutomation
    [XmlInclude(typeof(UIAutomationCheckElementExistByXPathCommand))]
    [XmlInclude(typeof(UIAutomationCheckElementExistCommand))]
    [XmlInclude(typeof(UIAutomationClickElementCommand))]
    [XmlInclude(typeof(UIAutomationExpandCollapseItemsInElementCommand))]
    [XmlInclude(typeof(UIAutomationGetChildElementCommand))]
    [XmlInclude(typeof(UIAutomationGetChildrenElementsInformationCommand))]
    [XmlInclude(typeof(UIAutomationGetElementFromElementCommand))]
    [XmlInclude(typeof(UIAutomationGetElementFromElementByXPathCommand))]
    [XmlInclude(typeof(UIAutomationGetElementFromTableElementCommand))]
    [XmlInclude(typeof(UIAutomationGetElementFromWindowCommand))]
    [XmlInclude(typeof(UIAutomationGetElementFromWindowByXPathCommand))]
    [XmlInclude(typeof(UIAutomationGetElementTreeXMLFromElementCommand))]
    [XmlInclude(typeof(UIAutomationGetParentElementCommand))]
    [XmlInclude(typeof(UIAutomationGetSelectedStateFromElementCommand))]
    [XmlInclude(typeof(UIAutomationGetSelectionItemsFromElementCommand))]
    [XmlInclude(typeof(UIAutomationGetTextFromElementCommand))]
    [XmlInclude(typeof(UIAutomationGetTextFromTableElementCommand))]
    [XmlInclude(typeof(UIAutomationSelectElementCommand))]
    [XmlInclude(typeof(UIAutomationSelectItemInElementCommand))]
    [XmlInclude(typeof(UIAutomationSetTextToElementCommand))]
    [XmlInclude(typeof(UIAutomationScrollElementCommand))]
    [XmlInclude(typeof(UIAutomationWaitForElementExistByXPathCommand))]
    [XmlInclude(typeof(UIAutomationWaitForElementExistCommand))]

    // Variable
    [XmlInclude(typeof(VariableCommand))]
    [XmlInclude(typeof(AddVariableCommand))]
    [XmlInclude(typeof(CheckVariableExistsCommand))]
    [XmlInclude(typeof(GetVariableTypeCommand))]
    [XmlInclude(typeof(SetVariableIndexCommand))]

    // Web
    [XmlInclude(typeof(SeleniumBrowserCheckBrowserInstanceExistsCommand))]
    [XmlInclude(typeof(SeleniumBrowserCloseCommand))]
    [XmlInclude(typeof(SeleniumBrowserCreateCommand))]
    [XmlInclude(typeof(SeleniumBrowserElementActionCommand))]
    [XmlInclude(typeof(SeleniumBrowserExecuteScriptCommand))]
    [XmlInclude(typeof(SeleniumBrowserGetAnElementValuesAsDataTableCommand))]
    [XmlInclude(typeof(SeleniumBrowserGetAnElementValuesAsDictionaryCommand))]
    [XmlInclude(typeof(SeleniumBrowserGetAnElementValuesAsListCommand))]
    [XmlInclude(typeof(SeleniumBrowserGetElementsValueAsDataTableCommand))]
    [XmlInclude(typeof(SeleniumBrowserGetElementsValueAsDictionaryCommand))]
    [XmlInclude(typeof(SeleniumBrowserGetElementsValueAsListCommand))]
    [XmlInclude(typeof(SeleniumBrowserGetElementsValuesAsDataTableCommand))]
    [XmlInclude(typeof(SeleniumBrowserGetTableValueAsDataTableCommand))]
    [XmlInclude(typeof(SeleniumBrowserInfoCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateBackCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateForwardCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateURLCommand))]
    [XmlInclude(typeof(SeleniumBrowserRefreshCommand))]
    [XmlInclude(typeof(SeleniumBrowserResizeBrowser))]
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
    //[XmlInclude(typeof(ThickAppClickItemCommand))]
    //[XmlInclude(typeof(ThickAppGetTextCommand))]
    //[XmlInclude(typeof(BeginExcelDatasetLoopCommand))]
    //[XmlInclude(typeof(DatabaseRunQueryCommand))]
    //[XmlInclude(typeof(AddDataTableColumnAndFillValuesByListCommand))]
    //[XmlInclude(typeof(AddDataTableColumnsAndFillValuesByDataTableCommand))]
   

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
        public bool IsWarning { get; protected set; }

        [XmlIgnore]
        public string validationResult { get; protected set; }

        [XmlIgnore]
        public bool IsMatched { get; protected set; }

        [XmlIgnore]
        public bool IsDontSavedCommand { get; set; }

        [XmlIgnore]
        public bool IsNewInsertedCommand { get; set; }

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
            this.IsWarning = false;
            this.validationResult = "";
            this.IsMatched = false;
            this.IsDontSavedCommand = false;
            this.IsNewInsertedCommand = false;
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
            string displayValue;
            if (String.IsNullOrEmpty(v_Comment))
            {
                displayValue = SelectionName;
            }
            else
            {
                displayValue = SelectionName + " [" + v_Comment + "]";
            }
            Attributes.ClassAttributes.EnableAutomateDisplayText autoDisp = (Attributes.ClassAttributes.EnableAutomateDisplayText)this.GetType().GetCustomAttribute(typeof(Attributes.ClassAttributes.EnableAutomateDisplayText));

            if ((autoDisp == null) || (!autoDisp.enableAutomateDisplayText))
            {
                return displayValue;
            }
            else
            {
                string t = "";
                var props = this.GetType().GetProperties();
                foreach (var prop in props)
                {
                    t += getPropertyDisplayValue(prop, this);
                }

                if (t == "")
                {
                    return displayValue;
                }
                else
                {
                    t = t.Trim();
                    return displayValue + " [ " + t.Substring(0, t.Length - 1) + " ]";
                }
            }
        }

        private static string getPropertyDisplayValue(PropertyInfo prop, ScriptCommand command)
        {
            if (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
            {
                object value = prop.GetValue(command);
                //if (value is System.Data.DataTable)
                //{
                //    return "";
                //}

                Attributes.PropertyAttributes.PropertyDisplayText dispProp = (Attributes.PropertyAttributes.PropertyDisplayText)prop.GetCustomAttribute(typeof(Attributes.PropertyAttributes.PropertyDisplayText));
                if ((dispProp == null) || (!dispProp.parameterDisplay))
                {
                    return "";
                }
                else
                {
                    string dispValue;
                    if (value == null)
                    {
                        dispValue = "''";
                    }
                    else if (value is System.Data.DataTable)
                    {
                        dispValue = ((System.Data.DataTable)value).Rows.Count + " items";
                    }
                    else if (!(value is string))
                    {
                        dispValue = "'" + value.ToString() + "'";
                    }
                    else
                    {
                        dispValue = "'" + value + "'";
                    }

                    if (dispProp.afterText != "")
                    {
                        return dispProp.parameterName + ": " + dispValue + " " + dispProp.afterText + ", ";
                    }
                    else
                    {
                        return dispProp.parameterName + ": " + dispValue + ", ";
                    }
                }
            }
            else
            {
                return "";
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

            Attributes.ClassAttributes.EnableAutomateRender render = (Attributes.ClassAttributes.EnableAutomateRender)this.GetType().GetCustomAttribute(typeof(Attributes.ClassAttributes.EnableAutomateRender));
            if ((render == null) || (!render.enableAutomateRender))
            {
                return RenderedControls;
            }
            else
            {
                RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));
                return RenderedControls;
            }
        }

        public virtual List<Control> Render()
        {
            RenderedControls = new List<Control>();
            return RenderedControls;
        }

        public virtual void AfterShown()
        {

        }

        public virtual void Refresh(UI.Forms.frmCommandEditor editor = null)
        {

        }

        public virtual void BeforeValidate()
        {
        }

        public virtual bool IsValidate(UI.Forms.frmCommandEditor editor)
        {
            //this.IsValid = true;
            //this.IsWarning = false;
            this.validationResult = "";

            var props = this.GetType().GetProperties();

            bool v = true;
            bool w = true;
            foreach(var prop in props)
            {
                if (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
                {
                    Core.Automation.Attributes.PropertyAttributes.PropertyValidationRule vr = (Core.Automation.Attributes.PropertyAttributes.PropertyValidationRule)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyValidationRule));
                    if (vr != null)
                    {
                        object va = prop.GetValue(this);
                        string propertyValue = (va == null) ? "" : va.ToString();
                        v &= checkValidateByFlags(vr.parameterName, propertyValue, vr.errorRule, prop.Name, prop);
                        w &= !checkValidateByFlags(vr.parameterName, propertyValue, vr.warningRule, prop.Name, prop);
                    }
                }
            }

            this.IsValid = v;
            this.IsWarning = w;

            return this.IsValid;
        }

        private bool checkValidateByFlags(string paramShortName, string value, Core.Automation.Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags flags, string propertyName, PropertyInfo prop)
        {
            if (flags == Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.None)
            {
                return true;
            }

            bool result = true;

            if ((flags & Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty) == Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.Empty)
            {
                if (string.IsNullOrEmpty(value))
                {
                    this.validationResult += paramShortName + " is empty.\n";
                    result = false;
                }
            }
            if ((flags & Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.LessThanZero) == Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.LessThanZero)
            {
                double v;
                if (double.TryParse(value, out v))
                {
                    if (v < 0.0)
                    {
                        this.validationResult += paramShortName + " is less than zero.\n";
                        result = false;
                    }
                }
            }
            if ((flags & Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.GreaterThanZero) == Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.GreaterThanZero)
            {
                double v;
                if (double.TryParse(value, out v))
                {
                    if (v > 0.0)
                    {
                        this.validationResult += paramShortName + " is greater than zero.\n";
                        result = false;
                    }
                }
            }
            if ((flags & Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.EqualsZero) == Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.EqualsZero)
            {
                double v;
                if (double.TryParse(value, out v))
                {
                    if (v == 0.0)
                    {
                        this.validationResult += paramShortName + " is equals zero.\n";
                        result = false;
                    }
                }
            }
            if ((flags & Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.NotEqualsZero) == Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.NotEqualsZero)
            {
                double v;
                if (double.TryParse(value, out v))
                {
                    if (v != 0.0)
                    {
                        this.validationResult += paramShortName + " is not equals zero.\n";
                        result = false;
                    }
                }
            }
            if ((flags & Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.NotSelectionOption) == Attributes.PropertyAttributes.PropertyValidationRule.ValidationRuleFlags.NotSelectionOption)
            {
                if (!IsUISelectionValue(propertyName, value, prop))
                {
                    this.validationResult += paramShortName + " is strange value '" + value + "'";
                    result = false;
                }
            }

            return result;
        }

        public bool IsUISelectionValue(string propertyName, string value, PropertyInfo prop)
        {
            var options = (Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption[])prop.GetCustomAttributes(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyUISelectionOption));
            if (options.Length == 0)
            {
                return true;
            }

            var sensitive = (Core.Automation.Attributes.PropertyAttributes.PropertyValueSensitive)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyValueSensitive));

            Func<string, string> cnvFunc;
            if (sensitive.caseSensitive && sensitive.whiteSpaceSensitive)
            {
                cnvFunc = (v) =>
                {
                    return v;
                };
            }
            else if (sensitive.caseSensitive && !sensitive.whiteSpaceSensitive)
            {
                cnvFunc = (v) =>
                {
                    return v.Trim().Replace(" ", "").Replace("\t", "");
                };
            }
            else if (!sensitive.caseSensitive && sensitive.whiteSpaceSensitive)
            {
                cnvFunc = (v) =>
                {
                    return v.ToLower();
                };
            }
            else
            {
                cnvFunc = (v) =>
                {
                    return v.ToLower().Trim().Replace(" ", "").Replace("\t", "");
                };
            }

            value = cnvFunc(value);
            foreach (var option in options)
            {
                if (value == cnvFunc(option.uiOption))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual void convertToIntermediate(Core.EngineSettings settings, List<Core.Script.ScriptVariable> variables)
        {
            var myPropaties = this.GetType().GetProperties();
            foreach (var prop in myPropaties)
            {
                if (prop.Name.StartsWith("v_"))
                {
                    var targetValue = prop.GetValue(this);
                    if (targetValue != null)
                    {
                        if (targetValue is string)
                        {
                            targetValue = settings.convertToIntermediate(targetValue.ToString());
                            prop.SetValue(this, targetValue);
                        }
                        else if (targetValue is System.Data.DataTable)
                        {
                            ((System.Data.DataTable)targetValue).AcceptChanges();
                            var trgDT = ((System.Data.DataTable)targetValue).Copy();
                            var rows = trgDT.Rows.Count;
                            var cols = trgDT.Columns.Count;
                            for (int i = 0; i < cols; i++)
                            {
                                if (trgDT.Columns[i].ReadOnly)
                                {
                                    continue;
                                }
                                for (int j = 0; j < rows; j++) 
                                {
                                    var v = settings.convertToIntermediate(trgDT.Rows[j][i].ToString());
                                    trgDT.Rows[j][i] = v;
                                }
                            }
                            prop.SetValue(this, trgDT);
                        }
                    }
                }
            }
        }

        public void convertToIntermediate(Core.EngineSettings settings, Dictionary<string, string> convertMethods, List<Core.Script.ScriptVariable> variables)
        {
            Type settingsType = settings.GetType();
            var myPropaties = this.GetType().GetProperties();
            foreach (var prop in myPropaties)
            {
                if (prop.Name.StartsWith("v_"))
                {
                    var targetValue = prop.GetValue(this);
                    if (targetValue != null)
                    {
                        System.Reflection.MethodInfo methodOfConverting = null;
                        foreach(var meth in convertMethods)
                        {
                            if (meth.Key == prop.Name)
                            {
                                switch (meth.Value)
                                {
                                    case "convertToIntermediateVariableParser":
                                        methodOfConverting = settingsType.GetMethod(meth.Value, new Type[] { typeof(string), typeof(List<Core.Script.ScriptVariable>) });
                                        break;
                                    default:
                                        methodOfConverting = settingsType.GetMethod(meth.Value, new Type[] { typeof(string) });
                                        break;
                                }
                                break;
                            }
                        }
                        if (methodOfConverting == null)
                        {
                            methodOfConverting = settingsType.GetMethod("convertToIntermediate", new Type[] { typeof(string) });
                        }

                        if (targetValue is string)
                        {
                            switch (methodOfConverting.Name)
                            {
                                case "convertToIntermediateVariableParser":
                                    targetValue = methodOfConverting.Invoke(settings, new object[] { targetValue.ToString(), variables });
                                    break;
                                default:
                                    targetValue = methodOfConverting.Invoke(settings, new object[] { targetValue.ToString()});
                                    break;
                            }
                            prop.SetValue(this, targetValue);
                        }
                        else if (targetValue is System.Data.DataTable)
                        {
                            ((System.Data.DataTable)targetValue).AcceptChanges();
                            var trgDT = ((System.Data.DataTable)targetValue).Copy();
                            var rows = trgDT.Rows.Count;
                            var cols = trgDT.Columns.Count;
                            for (int i = 0; i < cols; i++)
                            {
                                if (trgDT.Columns[i].ReadOnly)
                                {
                                    continue;
                                }
                                for (int j = 0; j < rows; j++)
                                {
                                    //var v = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString(), variables });
                                    //string v = settings.convertToIntermediate(trgDT.Rows[j][i].ToString());
                                    object newCellValue;
                                    switch (methodOfConverting.Name)
                                    {
                                        case "convertToIntermediateVariableParser":
                                            newCellValue = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString(), variables });
                                            break;
                                        default:
                                            newCellValue = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString() });
                                            break;
                                    }
                                    trgDT.Rows[j][i] = newCellValue;
                                }
                            }
                            prop.SetValue(this, trgDT);
                        }
                    }
                }
            }
        }

        public virtual void convertToRaw(Core.EngineSettings settings)
        {
            var myPropaties = this.GetType().GetProperties();
            foreach (var prop in myPropaties)
            {
                if (prop.Name.StartsWith("v_"))
                {
                    var targetValue = prop.GetValue(this);
                    if (targetValue != null)
                    {
                        if (targetValue is string)
                        {
                            targetValue = settings.convertToRaw(targetValue.ToString());
                            prop.SetValue(this, targetValue);
                        }
                        else if (targetValue is System.Data.DataTable)
                        {
                            var trgDT = (System.Data.DataTable)targetValue;
                            var rows = trgDT.Rows.Count;
                            var cols = trgDT.Columns.Count;
                            for (int i = 0; i < cols; i++)
                            {
                                if (trgDT.Columns[i].ReadOnly)
                                {
                                    continue;
                                }
                                for (int j = 0; j < rows; j++)
                                {
                                    var v = settings.convertToRaw(trgDT.Rows[j][i].ToString());
                                    trgDT.Rows[j][i] = v;
                                }
                            }
                            prop.SetValue(this, trgDT);
                        }
                    }
                }
            }
        }

        public void convertToRaw(Core.EngineSettings settings, Dictionary<string, string> convertMethods)
        {
            Type settingsType = settings.GetType();
            var myPropaties = this.GetType().GetProperties();
            foreach (var prop in myPropaties)
            {
                if (prop.Name.StartsWith("v_"))
                {
                    var targetValue = prop.GetValue(this);
                    if (targetValue != null)
                    {
                        System.Reflection.MethodInfo methodOfConverting = null;
                        foreach (var meth in convertMethods)
                        {
                            if (meth.Key == prop.Name)
                            {
                                methodOfConverting = settingsType.GetMethod(meth.Value);
                                break;
                            }
                        }
                        if (methodOfConverting == null)
                        {
                            methodOfConverting = settingsType.GetMethod("convertToRaw");
                        }

                        if (targetValue is string)
                        {
                            targetValue = methodOfConverting.Invoke(settings, new object[] { targetValue.ToString() });
                            prop.SetValue(this, targetValue);
                        }
                        else if (targetValue is System.Data.DataTable)
                        {
                            ((System.Data.DataTable)targetValue).AcceptChanges();
                            var trgDT = ((System.Data.DataTable)targetValue).Copy();
                            var rows = trgDT.Rows.Count;
                            var cols = trgDT.Columns.Count;
                            for (int i = 0; i < cols; i++)
                            {
                                if (trgDT.Columns[i].ReadOnly)
                                {
                                    continue;
                                }
                                for (int j = 0; j < rows; j++)
                                {
                                    var v = methodOfConverting.Invoke(settings, new object[] { trgDT.Rows[j][i].ToString() });
                                    trgDT.Rows[j][i] = v;
                                }
                            }
                            prop.SetValue(this, trgDT);
                        }
                    }
                }
            }
        }

        public ScriptCommand Clone()
        {
            return (ScriptCommand)MemberwiseClone();
        }

        public bool checkMatched(string keyword, bool caseSensitive, bool checkParameters, bool checkCommandName, bool checkComment, bool checkDisplayText, bool checkInstanceType, string instanceType)
        {
            if (!caseSensitive)
            {
                keyword = keyword.ToLower();
            }

            // command name
            if (checkCommandName)
            {
                string name = this.SelectionName;
                if (!caseSensitive)
                {
                    name = name.ToLower();
                }
                if (name.Contains(keyword))
                {
                    this.IsMatched = true;
                    return true;
                }
            }
            
            // display text
            if (checkDisplayText)
            {
                string disp = this.GetDisplayValue();
                if (!caseSensitive)
                {
                    disp = disp.ToLower();
                }
                if (disp.Contains(keyword))
                {
                    this.IsMatched = true;
                    return true;
                }
            }

            // comment
            if (checkComment)
            {
                var cmt = this.v_Comment;
                if (cmt != null)
                {
                    if (!caseSensitive)
                    {
                        cmt = cmt.ToLower();
                    }
                    if (cmt.Contains(keyword))
                    {
                        this.IsMatched = true;
                        return true;
                    }
                }
            }

            // parameters
            if (checkParameters)
            {
                
                // case sensitive function
                Func<string, string> CaseFunc;
                if (caseSensitive)
                {
                    CaseFunc = (str) =>
                    {
                        return str;
                    };
                }
                else
                {
                    CaseFunc = (str) =>
                    {
                        return str.ToLower();
                    };
                }
                var myPropaties = this.GetType().GetProperties();
                foreach (var prop in myPropaties)
                {
                    if (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
                    {
                        var propValue = prop.GetValue(this);
                        if (propValue == null)
                        {
                            continue;   // next
                        }
                        if (propValue is string)
                        {
                            if (CaseFunc((string)propValue).Contains(keyword))
                            {
                                this.IsMatched = true;
                                return true;
                            }
                        }
                        else if (propValue is System.Data.DataTable)
                        {
                            ((System.Data.DataTable)propValue).AcceptChanges();
                            var trgDT = ((System.Data.DataTable)propValue);
                            var rows = trgDT.Rows.Count;
                            var cols = trgDT.Columns.Count;
                            for (var i = 0; i < cols; i++)
                            {
                                if (trgDT.Columns[i].ReadOnly)
                                {
                                    continue;
                                }
                                for (var j = 0; j < rows; j++)
                                {
                                    if (CaseFunc(trgDT.Rows[j][i].ToString()).Contains(keyword))
                                    {
                                        this.IsMatched = true;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (checkInstanceType)
            {
                if (checkInstanceMatched(keyword, instanceType, caseSensitive))
                {
                    this.IsMatched = true;
                    return true;
                }
            }

            this.IsMatched = false;
            return false;
        }

        public bool checkInstanceMatched(string keyword, string instanceType, bool caseSensitive)
        {
            Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType comparedType = InstanceCounter.GetInstanceType(instanceType);

            Func<string, string> convFunc;
            if (caseSensitive)
            {
                convFunc = (trg) =>
                {
                    return trg;
                };
            }
            else
            {
                keyword = keyword.ToLower();
                convFunc = (trg) =>
                {
                    return trg.ToLower();
                };
            }
          
            System.Reflection.PropertyInfo[] myPropaties = this.GetType().GetProperties();
            foreach(System.Reflection.PropertyInfo prop in myPropaties)
            {
                var ins = (Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType));
                if ((ins != null) && (ins.instanceType == comparedType))
                {
                    var targetValue = prop.GetValue(this);
                    if ((targetValue != null) && convFunc(targetValue.ToString()).Contains(keyword))
                    {
                        this.IsMatched = true;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ReplaceAllParameters(string keyword, string replacedText, bool caseSensitive)
        {
            Func<string, string> convFunc;
            if (caseSensitive)
            {
                convFunc = (trg) =>
                {
                    return trg;
                };
            }
            else
            {
                keyword = keyword.ToLower();
                convFunc = (trg) =>
                {
                    return trg.ToLower();
                };
            }

            bool isReplaced = false;
            var targetProperties = this.GetType().GetProperties().Where(f => (f.Name.StartsWith("v_") && (f.Name != "v_Comment")));
            foreach(var prop in targetProperties)
            {
                var targetValue = prop.GetValue(this);
                if (targetValue == null)
                {
                    continue;
                }
                else if (targetValue is string)
                {
                    var currentValue = convFunc(targetValue.ToString());
                    var newValue = currentValue.Replace(keyword, replacedText);
                    if (currentValue != newValue)
                    {
                        prop.SetValue(this, newValue);
                        isReplaced = true;
                    }
                }
                else if (targetValue is System.Data.DataTable)
                {
                    var targetDT = (System.Data.DataTable)targetValue;
                    var rows = targetDT.Rows.Count;
                    var cols = targetDT.Columns.Count;
                    for (int i = 0; i < cols; i++)
                    {
                        if (targetDT.Columns[i].ReadOnly)
                        {
                            continue;
                        }
                        for (int j = 0; j < rows; j++)
                        {
                            var currentValue = convFunc(targetDT.Rows[j][i].ToString());
                            var newValue = currentValue.Replace(keyword, replacedText);
                            if (currentValue != newValue)
                            {
                                //prop.SetValue(this, newValue);
                                targetDT.Rows[j][i] = newValue;
                                isReplaced = true;
                            }
                        }
                    }
                }
            }
            return isReplaced;
        }

        public bool ReplaceInstance(string keyword, string replacedText, string instanceType, bool caseSensitive)
        {
            Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType comparedType = InstanceCounter.GetInstanceType(instanceType);

            Func<string, string> convFunc;
            if (caseSensitive)
            {
                convFunc = (trg) =>
                {
                    return trg;
                };
            }
            else
            {
                keyword = keyword.ToLower();
                convFunc = (trg) =>
                {
                    return trg.ToLower();
                };
            }

            bool isReplaced = false;

            var myPropaties = this.GetType().GetProperties();
            foreach(var prop in myPropaties)
            {
                var attr = (Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType));
                if (attr == null)
                {
                    continue;
                }
                else if (attr.instanceType == comparedType)
                {
                    var currentValue = convFunc(prop.GetValue(this).ToString());
                    var newValue = currentValue.Replace(keyword, replacedText);
                    if (currentValue != newValue)
                    {
                        prop.SetValue(this, newValue);
                        isReplaced = true;
                    }
                }
            }

            return isReplaced;
        }

        public bool ReplaceComment(string keyword, string replacedText, bool caseSensitive)
        {
            Func<string, string> convFunc;
            if (caseSensitive)
            {
                convFunc = (trg) =>
                {
                    return trg;
                };
            }
            else
            {
                keyword = keyword.ToLower();
                convFunc = (trg) =>
                {
                    return trg.ToLower();
                };
            }

            //var commentProp = this.GetProperty("v_Comment");
            var commentProp = this.GetType().GetProperty("v_Comment");
            var commentValue = commentProp.GetValue(this);
            if (commentValue != null)
            {
                var currentComment = convFunc(commentValue.ToString());
                var newComment = currentComment.Replace(keyword, replacedText);
                if (currentComment != newComment)
                {
                    commentProp.SetValue(this, newComment);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public virtual void addInstance(InstanceCounter counter)
        {
            //Type cmdType = command.GetType();
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
                {
                    if (prop.GetValue(this) != null)
                    {
                        string insValue = prop.GetValue(this).ToString();
                        var insType = (Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType));
                        var direction = (Core.Automation.Attributes.PropertyAttributes.PropertyParameterDirection)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyParameterDirection));
                        if ((insType != null) && (direction != null) &&
                                (insType.instanceType != Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none))
                        {
                            if (direction.porpose == Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)
                            {
                                counter.addInstance(insValue, insType, false);
                            }
                            //counter.addInstance(insValue, insType, (direction.porpose != Automation.Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output));
                            counter.addInstance(insValue, insType, true);
                        }
                        else if ((insType != null) && (insType.instanceType != Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none))
                        {
                            counter.addInstance(insValue, insType, true);
                        }
                    }
                }
            }
        }

        public virtual void removeInstance(InstanceCounter counter)
        {
            var props = this.GetType().GetProperties();
            foreach (var prop in props)
            {
                if (prop.Name.StartsWith("v_") && (prop.Name != "v_Comment"))
                {
                    if (prop.GetValue(this) != null)
                    {
                        string insValue = prop.GetValue(this).ToString();
                        var insType = (Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyInstanceType));
                        var direction = (Core.Automation.Attributes.PropertyAttributes.PropertyParameterDirection)prop.GetCustomAttribute(typeof(Core.Automation.Attributes.PropertyAttributes.PropertyParameterDirection));
                        if ((insType != null) && (direction != null) &&
                                (insType.instanceType != Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none))
                        {
                            //counter.removeInstance(insValue, insType, (direction.porpose != Automation.Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output));
                            if (direction.porpose == Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)
                            {
                                counter.removeInstance(insValue, insType, false);
                            }
                            counter.removeInstance(insValue, insType, true);
                        }
                        else if ((insType != null) && (insType.instanceType != Automation.Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.none))
                        {
                            counter.removeInstance(insValue, insType, true);
                        }
                    }
                }
            }
        }

    }
}