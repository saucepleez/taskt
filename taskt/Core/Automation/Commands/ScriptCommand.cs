using System;
using System.Xml.Serialization;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.ClassAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    //// API
    //[XmlInclude(typeof(ExecuteDLLCommand))]
    //[XmlInclude(typeof(RESTCommand))]
    //[XmlInclude(typeof(HTTPRequestCommand))]
    //[XmlInclude(typeof(HTTPQueryResultCommand))]

    //// Color
    //[XmlInclude(typeof(CreateColorCommand))]
    //[XmlInclude(typeof(CreateColorFromExcelColorCommand))]
    //[XmlInclude(typeof(CreateColorFromHexCommand))]
    //[XmlInclude(typeof(FormatColorCommand))]

    //// Data
    //[XmlInclude(typeof(DateCalculationCommand))]
    //[XmlInclude(typeof(FormatDataCommand))]
    //[XmlInclude(typeof(LogDataCommand))]
    //[XmlInclude(typeof(PDFTextExtractionCommand))]

    //// Database
    //[XmlInclude(typeof(DatabaseDefineConnectionCommand))]
    //[XmlInclude(typeof(DatabaseExecuteQueryCommand))]

    //// DataTable
    //[XmlInclude(typeof(AddDataRowCommand))]
    //[XmlInclude(typeof(AddDataTableColumnCommand))]
    //[XmlInclude(typeof(AddDataTableRowByDictionaryCommand))]
    //[XmlInclude(typeof(AddDataTableRowCommand))]
    //[XmlInclude(typeof(AddDataTableRowsByDataTableCommand))]
    //[XmlInclude(typeof(CheckDataTableColumnExistsCommand))]
    //[XmlInclude(typeof(ConvertDataTableColumnToDataTableCommand))]
    //[XmlInclude(typeof(ConvertDataTableColumnToDictionaryCommand))]
    //[XmlInclude(typeof(ConvertDataTableColumnToJSONCommand))]
    //[XmlInclude(typeof(ConvertDataTableColumnToListCommand))]
    //[XmlInclude(typeof(ConvertDataTableRowToDataTableCommand))]
    //[XmlInclude(typeof(ConvertDataTableRowToDictionaryCommand))]
    //[XmlInclude(typeof(ConvertDataTableRowToJSONCommand))]
    //[XmlInclude(typeof(ConvertDataTableRowToListCommand))]
    //[XmlInclude(typeof(ConvertDataTableToJSONCommand))]
    //[XmlInclude(typeof(CopyDataTableCommand))]
    //[XmlInclude(typeof(CreateDataTableCommand))]
    //[XmlInclude(typeof(DeleteDataTableColumnCommand))]
    //[XmlInclude(typeof(DeleteDataTableRowCommand))]
    //[XmlInclude(typeof(FilterDataTableColumnByRowValueCommand))]
    //[XmlInclude(typeof(FilterDataTableCommand))]
    //[XmlInclude(typeof(FilterDataTableRowByColumnValueCommand))]
    //[XmlInclude(typeof(GetDataRowCommand))]
    //[XmlInclude(typeof(GetDataRowCountCommand))]
    //[XmlInclude(typeof(GetDataRowValueCommand))]
    //[XmlInclude(typeof(GetDataTableColumnCountCommand))]
    //[XmlInclude(typeof(GetDataTableColumnListCommand))]
    //[XmlInclude(typeof(GetDataTableRowCountCommand))]
    //[XmlInclude(typeof(GetDataTableValueCommand))]
    //[XmlInclude(typeof(ReplaceDataTableColumnValueCommand))]
    //[XmlInclude(typeof(ReplaceDataTableRowValueCommand))]
    //[XmlInclude(typeof(ReplaceDataTableValueCommand))]
    //[XmlInclude(typeof(ParseDatasetRowCommand))]
    //[XmlInclude(typeof(RemoveDataRowCommand))]
    //[XmlInclude(typeof(SetDataTableColumnValuesByDataTableCommand))]
    //[XmlInclude(typeof(SetDataTableColumnValuesByListCommand))]
    //[XmlInclude(typeof(SetDataTableRowValuesByDataTableCommand))]
    //[XmlInclude(typeof(SetDataTableRowValuesByDictionaryCommand))]
    //[XmlInclude(typeof(SetDataTableValueCommand))]
    //[XmlInclude(typeof(WriteDataRowValueCommand))]
    //[XmlInclude(typeof(LoadDataTableCommand))]

    //// DateTime
    //[XmlInclude(typeof(CalculateDateTimeCommand))]
    //[XmlInclude(typeof(ConvertDateTimeToExcelSerialCommand))]
    //[XmlInclude(typeof(CreateDateTimeCommand))]
    //[XmlInclude(typeof(CreateDateTimeFromExcelSerialCommand))]
    //[XmlInclude(typeof(CreateDateTimeFromTextCommand))]
    //[XmlInclude(typeof(GetDateTimeDifferencesCommand))]
    //[XmlInclude(typeof(FormatDateTimeCommand))]

    //// Dictionary
    //[XmlInclude(typeof(AddDictionaryCommand))]
    //[XmlInclude(typeof(CheckDictionaryKeyExistsCommand))]
    //[XmlInclude(typeof(CreateDictionaryCommand))]
    //[XmlInclude(typeof(ConcatenateDictionaryCommand))]
    //[XmlInclude(typeof(ConvertDictionaryToDataTableCommand))]
    //[XmlInclude(typeof(ConvertDictionaryToJSONCommand))]
    //[XmlInclude(typeof(ConvertDictionaryToListCommand))]
    //[XmlInclude(typeof(CopyDictionaryCommand))]
    //[XmlInclude(typeof(FilterDictionaryCommand))]
    //[XmlInclude(typeof(GetDictionaryKeyFromValueCommand))]
    //[XmlInclude(typeof(GetDictionaryKeysListCommand))]
    //[XmlInclude(typeof(GetDictionaryValueCommand))]
    //[XmlInclude(typeof(LoadDictionaryCommand))]
    //[XmlInclude(typeof(RemoveDictionaryItemCommand))]
    //[XmlInclude(typeof(ReplaceDictionaryCommand))]
    //[XmlInclude(typeof(SetDictionaryValueCommand))]

    //// EMail
    //[XmlInclude(typeof(MailKitGetAddressesAsDataTableCommand))]
    //[XmlInclude(typeof(MailKitGetAddressesAsDictionaryCommand))]
    //[XmlInclude(typeof(MailKitGetAddressesAsListCommand))]
    //[XmlInclude(typeof(MailKitGetEmailAttachmentsNameCommand))]
    //[XmlInclude(typeof(MailKitGetEMailFromEMailListCommand))]
    //[XmlInclude(typeof(MailKitGetEMailTextCommand))]
    //[XmlInclude(typeof(MailKitLoadEmailCommand))]
    //[XmlInclude(typeof(MailKitRecieveEmailListUsingIMAPCommand))]
    //[XmlInclude(typeof(MailKitRecieveEmailListUsingPOPCommand))]
    //[XmlInclude(typeof(MailKitSaveEmailCommand))]
    //[XmlInclude(typeof(MailKitSaveEmailAttachmentsCommand))]
    //[XmlInclude(typeof(MailKitSendEmailCommand))]

    //// Engine
    //[XmlInclude(typeof(ErrorHandlingCommand))]
    //[XmlInclude(typeof(GetDataCommand))]
    //[XmlInclude(typeof(PauseCommand))]
    //[XmlInclude(typeof(SetEngineDelayCommand))]
    //[XmlInclude(typeof(SetEnginePreferenceCommand))]
    //[XmlInclude(typeof(ShowEngineContextCommand))]
    //[XmlInclude(typeof(StopwatchCommand))]
    //[XmlInclude(typeof(UploadDataCommand))]

    //// Error
    //[XmlInclude(typeof(CatchExceptionCommand))]
    //[XmlInclude(typeof(EndTryCommand))]
    //[XmlInclude(typeof(FinallyCommand))]
    //[XmlInclude(typeof(ThrowExceptionCommand))]
    //[XmlInclude(typeof(TryCommand))]

    //// Excel
    //[XmlInclude(typeof(ExcelActivateSheetCommand))]
    //[XmlInclude(typeof(ExcelAddWorkbookCommand))]
    //[XmlInclude(typeof(ExcelAddWorksheetCommand))]
    //[XmlInclude(typeof(ExcelAppendCellCommand))]
    //[XmlInclude(typeof(ExcelAppendRowCommand))]
    //[XmlInclude(typeof(ExcelCheckCellValueExistsCommand))]
    //[XmlInclude(typeof(ExcelCheckCellValueExistsRCCommand))]
    //[XmlInclude(typeof(ExcelCheckExcelInstanceExistsCommand))]
    //[XmlInclude(typeof(ExcelCheckWorksheetExistsCommand))]
    //[XmlInclude(typeof(ExcelCloseApplicationCommand))]
    //[XmlInclude(typeof(ExcelCopyWorksheetCommand))]
    //[XmlInclude(typeof(ExcelCreateApplicationCommand))]
    //[XmlInclude(typeof(ExcelDeleteCellCommand))]
    //[XmlInclude(typeof(ExcelDeleteRowCommand))]
    //[XmlInclude(typeof(ExcelDeleteWorksheetCommand))]
    //[XmlInclude(typeof(ExcelGetCellCommand))]
    //[XmlInclude(typeof(ExcelGetCellRCCommand))]
    //[XmlInclude(typeof(ExcelGetColumnValuesAsDataTableCommand))]
    //[XmlInclude(typeof(ExcelGetColumnValuesAsDictionaryCommand))]
    //[XmlInclude(typeof(ExcelGetColumnValuesAsListCommand))]
    //[XmlInclude(typeof(ExcelGetCurrentWorksheetCommand))]
    //[XmlInclude(typeof(ExcelGetExcelInfoCommand))]
    //[XmlInclude(typeof(ExcelGetLastRowCommand))]
    //[XmlInclude(typeof(ExcelGetRangeCommand))]
    //[XmlInclude(typeof(ExcelGetRangeCommandAsDT))]
    //[XmlInclude(typeof(ExcelGetRangeValuesAsDataTableCommand))]
    //[XmlInclude(typeof(ExcelGetRowValuesAsDataTableCommand))]
    //[XmlInclude(typeof(ExcelGetRowValuesAsDictionaryCommand))]
    //[XmlInclude(typeof(ExcelGetRowValuesAsListCommand))]
    //[XmlInclude(typeof(ExcelGetWorksheetsCommand))]
    //[XmlInclude(typeof(ExcelGetWorksheetInfoCommand))]
    //[XmlInclude(typeof(ExcelGoToCellCommand))]
    //[XmlInclude(typeof(ExcelOpenWorkbookCommand))]
    //[XmlInclude(typeof(ExcelRenameWorksheetCommand))]
    //[XmlInclude(typeof(ExcelRunMacroCommand))]
    //[XmlInclude(typeof(ExcelSaveCommand))]
    //[XmlInclude(typeof(ExcelSaveAsCommand))]
    //[XmlInclude(typeof(ExcelSetCellCommand))]
    //[XmlInclude(typeof(ExcelSetCellRCCommand))]
    //[XmlInclude(typeof(ExcelSetColumnValuesFromDataTableCommand))]
    //[XmlInclude(typeof(ExcelSetColumnValuesFromDictionaryCommand))]
    //[XmlInclude(typeof(ExcelSetColumnValuesFromListCommand))]
    //[XmlInclude(typeof(ExcelSetRowValuesFromDataTableCommand))]
    //[XmlInclude(typeof(ExcelSetRowValuesFromDictionaryCommand))]
    //[XmlInclude(typeof(ExcelSetRowValuesFromListCommand))]
    //[XmlInclude(typeof(ExcelSplitRangeByColumnCommand))]
    //[XmlInclude(typeof(ExcelWriteRangeCommand))]
    //[XmlInclude(typeof(ExcelWriteRowCommand))]

    //// File
    //[XmlInclude(typeof(CheckFileExistsCommand))]
    //[XmlInclude(typeof(DeleteFileCommand))]
    //[XmlInclude(typeof(ExtractFileCommand))]
    //[XmlInclude(typeof(FormatFilePathCommnad))]
    //[XmlInclude(typeof(GetFileInfoCommand))]
    //[XmlInclude(typeof(GetFilesCommand))]
    //[XmlInclude(typeof(MoveFileCommand))]
    //[XmlInclude(typeof(RenameFileCommand))]
    //[XmlInclude(typeof(WaitForFileToExistCommand))]

    //// Folder
    //[XmlInclude(typeof(CheckFolderExistsCommand))]
    //[XmlInclude(typeof(CreateFolderCommand))]
    //[XmlInclude(typeof(DeleteFolderCommand))]
    //[XmlInclude(typeof(FormatFolderPathCommnad))]
    //[XmlInclude(typeof(GetFoldersCommand))]
    //[XmlInclude(typeof(MoveFolderCommand))]
    //[XmlInclude(typeof(RenameFolderCommand))]
    //[XmlInclude(typeof(WaitForFolderToExistCommand))]

    //// IE
    //[XmlInclude(typeof(IEBrowserCloseCommand))]
    //[XmlInclude(typeof(IEBrowserCreateCommand))]
    //[XmlInclude(typeof(IEBrowserElementActionCommand))]
    //[XmlInclude(typeof(IEBrowserFindBrowserCommand))]
    //[XmlInclude(typeof(IEBrowserNavigateURLCommand))]

    //// If
    //[XmlInclude(typeof(BeginIfCommand))]
    //[XmlInclude(typeof(BeginMultiIfCommand))]
    //[XmlInclude(typeof(ElseCommand))]
    //[XmlInclude(typeof(EndIfCommand))]

    //// Image
    //[XmlInclude(typeof(OCRCommand))]
    //[XmlInclude(typeof(ImageRecognitionCommand))]
    //[XmlInclude(typeof(ScreenshotCommand))]

    //// Input
    //[XmlInclude(typeof(FileDialogCommand))]
    //[XmlInclude(typeof(FolderDialogCommand))]
    //[XmlInclude(typeof(HTMLInputCommand))]
    //[XmlInclude(typeof(UserInputCommand))]
    //[XmlInclude(typeof(SendAdvancedKeyStrokesCommand))]
    //[XmlInclude(typeof(SendHotkeyCommand))]
    //[XmlInclude(typeof(SendKeysCommand))]
    //[XmlInclude(typeof(SendMouseClickCommand))]
    //[XmlInclude(typeof(SendMouseMoveCommand))]
    //[XmlInclude(typeof(UIAutomationCommand))]

    //// JSON
    //[XmlInclude(typeof(AddJSONArrayItemCommand))]
    //[XmlInclude(typeof(AddJSONObjectPropertyCommand))]
    //[XmlInclude(typeof(ConvertJSONToDataTableCommand))]
    //[XmlInclude(typeof(ConvertJSONToDictionaryCommand))]
    //[XmlInclude(typeof(ConvertJSONToListCommand))]
    //[XmlInclude(typeof(CreateJSONVariableCommand))]
    //[XmlInclude(typeof(GetJSONValueListCommand))]
    //[XmlInclude(typeof(GetMultiJSONValueListCommand))]
    //[XmlInclude(typeof(InsertJSONArrayItemCommand))]
    //[XmlInclude(typeof(InsertJSONObjectPropertyCommand))]
    //[XmlInclude(typeof(ParseJSONArrayCommand))]
    //[XmlInclude(typeof(ReadJSONFileCommand))]
    //[XmlInclude(typeof(RemoveJSONArrayItemCommand))]
    //[XmlInclude(typeof(RemoveJSONPropertyCommand))]
    //[XmlInclude(typeof(SetJSONValueCommand))]

    //// List
    //[XmlInclude(typeof(AddListItemCommand))]
    //[XmlInclude(typeof(CheckListItemExistsCommand))]
    //[XmlInclude(typeof(ConcatenateListsCommand))]
    //[XmlInclude(typeof(ConvertListToDataTableCommand))]
    //[XmlInclude(typeof(ConvertListToDictionaryCommand))]
    //[XmlInclude(typeof(ConvertListToJSONCommand))]
    //[XmlInclude(typeof(CopyListCommand))]
    //[XmlInclude(typeof(CreateListCommand))]
    //[XmlInclude(typeof(FilterListCommand))]
    //[XmlInclude(typeof(GetAverageFromListCommand))]
    //[XmlInclude(typeof(GetListCountCommand))]
    //[XmlInclude(typeof(GetListIndexFromValueCommand))]
    //[XmlInclude(typeof(GetListItemCommand))]
    //[XmlInclude(typeof(GetMaxFromListCommand))]
    //[XmlInclude(typeof(GetMedianFromListCommand))]
    //[XmlInclude(typeof(GetMinFromListCommand))]
    //[XmlInclude(typeof(GetSumFromListCommand))]
    //[XmlInclude(typeof(GetVarianceFromListCommand))]
    //[XmlInclude(typeof(ReplaceListCommand))]
    //[XmlInclude(typeof(ReverseListCommand))]
    //[XmlInclude(typeof(SetListIndexCommand))]
    //[XmlInclude(typeof(SetListItemCommand))]
    //[XmlInclude(typeof(SortListCommand))]

    //// Loop
    //[XmlInclude(typeof(BeginLoopCommand))]
    //[XmlInclude(typeof(BeginMultiLoopCommand))]
    //[XmlInclude(typeof(EndLoopCommand))]
    //[XmlInclude(typeof(ExitLoopCommand))]
    //[XmlInclude(typeof(BeginContinousLoopCommand))]
    //[XmlInclude(typeof(BeginListLoopCommand))]
    //[XmlInclude(typeof(BeginNumberOfTimesLoopCommand))]
    //[XmlInclude(typeof(NextLoopCommand))]

    //// Misc
    //[XmlInclude(typeof(ClipboardClearTextCommand))]
    //[XmlInclude(typeof(ClipboardGetTextCommand))]
    //[XmlInclude(typeof(ClipboardSetTextCommand))]
    //[XmlInclude(typeof(CommentCommand))]
    //[XmlInclude(typeof(CreateShortcutCommand))]
    //[XmlInclude(typeof(EncryptionCommand))]
    //[XmlInclude(typeof(MessageBoxCommand))]
    //[XmlInclude(typeof(PingCommand))]
    //[XmlInclude(typeof(PlaySystemSoundCommand))]
    //[XmlInclude(typeof(SequenceCommand))]
    //[XmlInclude(typeof(SMTPSendEmailCommand))]

    //// NLG
    //[XmlInclude(typeof(NLGCreateInstanceCommand))]
    //[XmlInclude(typeof(NLGGeneratePhraseCommand))]
    //[XmlInclude(typeof(NLGSetParameterCommand))]

    //// Numeric
    //[XmlInclude(typeof(CreateNumberVariableCommand))]
    //[XmlInclude(typeof(DecreaseNumericalVariableCommand))]
    //[XmlInclude(typeof(FormatNumberCommand))]
    //[XmlInclude(typeof(IncreaseNumericalVariableCommand))]
    //[XmlInclude(typeof(MathCalculationCommand))]
    //[XmlInclude(typeof(RandomNumberCommand))]
    //[XmlInclude(typeof(RoundNumberCommand))]

    //// Outlook
    //[XmlInclude(typeof(OutlookDeleteEmailsCommand))]
    //[XmlInclude(typeof(OutlookForwardEmailsCommand))]
    //[XmlInclude(typeof(OutlookGetEmailsCommand))]
    //[XmlInclude(typeof(OutlookMoveEmailsCommand))]
    //[XmlInclude(typeof(OutlookReplyToEmailsCommand))]
    //[XmlInclude(typeof(OutlookEmailCommand))]

    //// Program
    //[XmlInclude(typeof(RunCustomCodeCommand))]
    //[XmlInclude(typeof(RunScriptCommand))]
    //[XmlInclude(typeof(StartProcessCommand))]
    //[XmlInclude(typeof(StopProcessCommand))]
    //[XmlInclude(typeof(RunPowershellCommand))]

    //// Regex
    //[XmlInclude(typeof(GetRegexMatchesCommand))]

    //// Remote
    //[XmlInclude(typeof(RemoteAPICommand))]
    //[XmlInclude(typeof(RemoteTaskCommand))]

    //// System
    //[XmlInclude(typeof(EnvironmentVariableCommand))]
    //[XmlInclude(typeof(RemoteDesktopCommand))]
    //[XmlInclude(typeof(OSVariableCommand))]
    //[XmlInclude(typeof(SystemActionCommand))]

    //// Task
    //[XmlInclude(typeof(LoadTaskCommand))]
    //[XmlInclude(typeof(RunTaskCommand))]
    //[XmlInclude(typeof(StopTaskCommand))]
    //[XmlInclude(typeof(UnloadTaskCommand))]

    //// Text
    //[XmlInclude(typeof(CheckTextCommand))]
    //[XmlInclude(typeof(ConcatenateTextVariableCommand))]
    //[XmlInclude(typeof(CreateTextVariableCommand))]
    //[XmlInclude(typeof(ExtractionTextCommand))]
    //[XmlInclude(typeof(GetWordCountCommand))]
    //[XmlInclude(typeof(GetWordLengthCommand))]
    //[XmlInclude(typeof(ModifyTextCommand))]
    //[XmlInclude(typeof(ReadTextFileCommand))]
    //[XmlInclude(typeof(RegExExtractionTextCommand))]
    //[XmlInclude(typeof(ReplaceTextCommand))]
    //[XmlInclude(typeof(SplitTextCommand))]
    //[XmlInclude(typeof(SubstringTextCommand))]
    //[XmlInclude(typeof(WriteTextFileCommand))]

    //// UIAutomation
    //[XmlInclude(typeof(UIAutomationCheckElementExistByXPathCommand))]
    //[XmlInclude(typeof(UIAutomationCheckElementExistCommand))]
    //[XmlInclude(typeof(UIAutomationClickElementCommand))]
    //[XmlInclude(typeof(UIAutomationExpandCollapseItemsInElementCommand))]
    //[XmlInclude(typeof(UIAutomationGetChildElementCommand))]
    //[XmlInclude(typeof(UIAutomationGetChildrenElementsInformationCommand))]
    //[XmlInclude(typeof(UIAutomationGetElementFromElementCommand))]
    //[XmlInclude(typeof(UIAutomationGetElementFromElementByXPathCommand))]
    //[XmlInclude(typeof(UIAutomationGetElementFromTableElementCommand))]
    //[XmlInclude(typeof(UIAutomationGetElementFromWindowCommand))]
    //[XmlInclude(typeof(UIAutomationGetElementFromWindowByXPathCommand))]
    //[XmlInclude(typeof(UIAutomationGetElementTreeXMLFromElementCommand))]
    //[XmlInclude(typeof(UIAutomationGetParentElementCommand))]
    //[XmlInclude(typeof(UIAutomationGetSelectedStateFromElementCommand))]
    //[XmlInclude(typeof(UIAutomationGetSelectionItemsFromElementCommand))]
    //[XmlInclude(typeof(UIAutomationGetTextFromElementCommand))]
    //[XmlInclude(typeof(UIAutomationGetTextFromTableElementCommand))]
    //[XmlInclude(typeof(UIAutomationSelectElementCommand))]
    //[XmlInclude(typeof(UIAutomationSelectItemInElementCommand))]
    //[XmlInclude(typeof(UIAutomationSetTextToElementCommand))]
    //[XmlInclude(typeof(UIAutomationScrollElementCommand))]
    //[XmlInclude(typeof(UIAutomationWaitForElementExistByXPathCommand))]
    //[XmlInclude(typeof(UIAutomationWaitForElementExistCommand))]

    //// Variable
    //[XmlInclude(typeof(VariableCommand))]
    //[XmlInclude(typeof(AddVariableCommand))]
    //[XmlInclude(typeof(CheckVariableExistsCommand))]
    //[XmlInclude(typeof(GetVariableTypeCommand))]
    //[XmlInclude(typeof(SetVariableIndexCommand))]

    //// Web
    //[XmlInclude(typeof(SeleniumBrowserCheckBrowserInstanceExistsCommand))]
    //[XmlInclude(typeof(SeleniumBrowserCloseCommand))]
    //[XmlInclude(typeof(SeleniumBrowserCreateCommand))]
    //[XmlInclude(typeof(SeleniumBrowserElementActionCommand))]
    //[XmlInclude(typeof(SeleniumBrowserExecuteScriptCommand))]
    //[XmlInclude(typeof(SeleniumBrowserGetAnElementValuesAsDataTableCommand))]
    //[XmlInclude(typeof(SeleniumBrowserGetAnElementValuesAsDictionaryCommand))]
    //[XmlInclude(typeof(SeleniumBrowserGetAnElementValuesAsListCommand))]
    //[XmlInclude(typeof(SeleniumBrowserGetElementsValueAsDataTableCommand))]
    //[XmlInclude(typeof(SeleniumBrowserGetElementsValueAsDictionaryCommand))]
    //[XmlInclude(typeof(SeleniumBrowserGetElementsValueAsListCommand))]
    //[XmlInclude(typeof(SeleniumBrowserGetElementsValuesAsDataTableCommand))]
    //[XmlInclude(typeof(SeleniumBrowserGetTableValueAsDataTableCommand))]
    //[XmlInclude(typeof(SeleniumBrowserInfoCommand))]
    //[XmlInclude(typeof(SeleniumBrowserNavigateBackCommand))]
    //[XmlInclude(typeof(SeleniumBrowserNavigateForwardCommand))]
    //[XmlInclude(typeof(SeleniumBrowserNavigateURLCommand))]
    //[XmlInclude(typeof(SeleniumBrowserRefreshCommand))]
    //[XmlInclude(typeof(SeleniumBrowserResizeBrowser))]
    //[XmlInclude(typeof(SeleniumBrowserSwitchFrameCommand))]
    //[XmlInclude(typeof(SeleniumBrowserSwitchWindowCommand))]
    //[XmlInclude(typeof(SeleniumBrowserTakeScreenshotCommand))]

    //// Window
    //[XmlInclude(typeof(ActivateWindowCommand))]
    //[XmlInclude(typeof(CheckWindowNameExistsCommand))]
    //[XmlInclude(typeof(CloseWindowCommand))]
    //[XmlInclude(typeof(GetWindowNamesCommand))]
    //[XmlInclude(typeof(GetWindowPositionCommand))]
    //[XmlInclude(typeof(GetWindowStateCommand))]
    //[XmlInclude(typeof(MoveWindowCommand))]
    //[XmlInclude(typeof(ResizeWindowCommand))]
    //[XmlInclude(typeof(SetWindowStateCommand))]
    //[XmlInclude(typeof(WaitForWindowCommand))]

    //// Word
    //[XmlInclude(typeof(WordAddDocumentCommand))]
    //[XmlInclude(typeof(WordAppendDataTableCommand))]
    //[XmlInclude(typeof(WordAppendImageCommand))]
    //[XmlInclude(typeof(WordAppendTextCommand))]
    //[XmlInclude(typeof(WordCheckWordInstanceExistsCommand))]
    //[XmlInclude(typeof(WordCloseApplicationCommand))]
    //[XmlInclude(typeof(WordCreateApplicationCommand))]
    //[XmlInclude(typeof(WordExportToPDFCommand))]
    //[XmlInclude(typeof(WordOpenDocumentCommand))]
    //[XmlInclude(typeof(WordReadDocumentCommand))]
    //[XmlInclude(typeof(WordReplaceTextCommand))]
    //[XmlInclude(typeof(WordSaveCommand))]
    //[XmlInclude(typeof(WordSaveAsCommand))]


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
        [Attributes.PropertyAttributes.PropertyDescription("Comment Field")]
        [Attributes.PropertyAttributes.InputSpecification("Optional field to enter a custom comment which could potentially describe this command or the need for this command, if required")]
        [Attributes.PropertyAttributes.SampleUsage("I am using this command to ...")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(3, true)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
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

        [XmlIgnore]
        [NonSerialized]
        protected Dictionary<string, Control> ControlsList;

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

            var tp = this.GetType();
            this.CommandName = tp.Name;
            var commandSettings = tp.GetCustomAttribute<CommandSettings>() ?? new CommandSettings();
            this.SelectionName = commandSettings.selectionName;
            this.CommandEnabled = commandSettings.commandEnable;
            this.CustomRendering = commandSettings.customeRender;
        }

        public void GenerateID()
        {
            var id = Guid.NewGuid();
            this.CommandID = id.ToString();
        }

        #region RunCommand
        //public virtual void RunCommand(object sender)
        //{
        //    //RunCommand((AutomationEngineInstance)sender);
        //    System.Threading.Thread.Sleep(DefaultPause);
        //}
        //public virtual void RunCommand(object sender, Script.ScriptAction command)
        //{
        //    System.Threading.Thread.Sleep(DefaultPause);
        //}
        public virtual void RunCommand(Engine.AutomationEngineInstance engine)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }
        public virtual void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction command)
        {
            System.Threading.Thread.Sleep(DefaultPause);
        }
        #endregion

        #region GetDisplayValue
        public virtual string GetDisplayValue()
        {
            return DisplayTextControls.GetDisplayText(this);
        }
        #endregion

        #region Render, Refresh, etc
        //public virtual List<Control> Render(UI.Forms.frmCommandEditor editor, object sender)
        //{
        //    RenderedControls = new List<Control>();
        //    return RenderedControls;
        //}

        public virtual List<Control> Render(UI.Forms.frmCommandEditor editor)
        {
            RenderedControls = new List<Control>();

            var attrAutoRender = this.GetType().GetCustomAttribute<EnableAutomateRender>();
            if (attrAutoRender?.enableAutomateRender ?? false)
            {
                RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor, attrAutoRender.forceRenderComment));

                ControlsList = new Dictionary<string, Control>();
                foreach (Control control in RenderedControls)
                {
                    ControlsList.Add(control.Name, control);
                    if (control is FlowLayoutPanel flp)
                    {
                        foreach (Control c in flp.Controls)
                        {
                            ControlsList.Add(c.Name, c);
                        }
                    }
                }

                return RenderedControls;
            }
            else
            {
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
        #endregion

        #region Validate
        public virtual void BeforeValidate()
        {
        }

        public virtual bool IsValidate(UI.Forms.frmCommandEditor editor)
        {
            (this.IsValid, this.IsWarning, this.validationResult) = ValidationControls.CheckValidation(this);

            return this.IsValid;
        }
        #endregion

        #region intermediate
        /// <summary>
        /// convert to Intermediate script. this method use default convert method.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="variables"></param>
        public virtual void ConvertToIntermediate(EngineSettings settings, List<Script.ScriptVariable> variables)
        {
            //IntermediateControls.ConvertToIntermediate(this, settings, variables);
            var convertMethods = new Dictionary<string, string>();
            var props = this.GetParameterProperties(true);
            foreach(var prop in props)
            {
                var virtualProp = prop.GetVirtualProperty();
                var methods = PropertyControls.GetCustomAttributeWithVirtual<taskt.Core.Automation.Attributes.PropertyAttributes.PropertyIntermediateConvert>(prop, virtualProp) ?? new Attributes.PropertyAttributes.PropertyIntermediateConvert();

                if (methods.intermediateMethod.Length > 0)
                {
                    convertMethods.Add(prop.Name, methods.intermediateMethod);
                }
            }

            if (convertMethods.Count > 0)
            {
                IntermediateControls.ConvertToIntermediate(this, settings, convertMethods, variables);
            }
            else
            {
                IntermediateControls.ConvertToIntermediate(this, settings, variables);
            }
        }

        /// <summary>
        /// convert to intermediate script. this method enable to specify convert methods.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="convertMethods"></param>
        /// <param name="variables"></param>
        public void ConvertToIntermediate(EngineSettings settings, Dictionary<string, string> convertMethods, List<Script.ScriptVariable> variables)
        {
            IntermediateControls.ConvertToIntermediate(this, settings, convertMethods, variables);
        }

        /// <summary>
        /// convert to raw script. this method use default convert method.
        /// </summary>
        /// <param name="settings"></param>
        public virtual void ConvertToRaw(EngineSettings settings)
        {
            //IntermediateControls.ConvertToRaw(this, settings);
            var convertMethods = new Dictionary<string, string>();
            var props = this.GetParameterProperties(true);
            foreach (var prop in props)
            {
                var virtualProp = prop.GetVirtualProperty();
                var methods = PropertyControls.GetCustomAttributeWithVirtual<taskt.Core.Automation.Attributes.PropertyAttributes.PropertyIntermediateConvert>(prop, virtualProp) ?? new Attributes.PropertyAttributes.PropertyIntermediateConvert();
                
                if (methods.rawMethod.Length > 0)
                {
                    convertMethods.Add(prop.Name, methods.rawMethod);
                }
            }

            if (convertMethods.Count > 0)
            {
                IntermediateControls.ConvertToRaw(this, settings, convertMethods);
            }
            else
            {
                IntermediateControls.ConvertToRaw(this, settings);
            }
        }

        /// <summary>
        /// convert to raw script. this method enable to specify convert methods.
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="convertMethods"></param>
        public void ConvertToRaw(EngineSettings settings, Dictionary<string, string> convertMethods)
        {
            IntermediateControls.ConvertToRaw(this, settings, convertMethods);
        }
        #endregion

        public ScriptCommand Clone()
        {
            return (ScriptCommand)MemberwiseClone();
        }

        #region Math Replace
        /// <summary>
        /// check parameters value matches
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="checkParameters"></param>
        /// <param name="checkCommandName"></param>
        /// <param name="checkComment"></param>
        /// <param name="checkDisplayText"></param>
        /// <param name="checkInstanceType"></param>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public bool CheckMatched(string keyword, bool caseSensitive, bool checkParameters, bool checkCommandName, bool checkComment, bool checkDisplayText, bool checkInstanceType, string instanceType)
        {
            this.IsMatched = SearchReplaceControls.CheckMatched(this, keyword, caseSensitive, checkParameters, checkCommandName, checkComment, checkDisplayText, checkInstanceType, instanceType);
            return this.IsMatched;
        }

        /// <summary>
        /// replace parameters value
        /// </summary>
        /// <param name="trg"></param>
        /// <param name="keyword"></param>
        /// <param name="replacedText"></param>
        /// <param name="caseSensitive"></param>
        /// <param name="instanceType"></param>
        /// <returns></returns>
        public bool Replace(SearchReplaceControls.ReplaceTarget trg, string keyword, string replacedText, bool caseSensitive, string instanceType = "")
        {
            this.IsDontSavedCommand =  SearchReplaceControls.Replace(this, trg, keyword, replacedText, caseSensitive, instanceType);
            return this.IsDontSavedCommand;
        }
        #endregion

        #region instance counter

        /// <summary>
        /// general method to Add Instance
        /// </summary>
        /// <param name="counter"></param>
        public virtual void AddInstance(InstanceCounter counter)
        {
            InstanceCounterControls.AddInstance(this, counter);
        }

        /// <summary>
        /// general method to Remove Instance
        /// </summary>
        /// <param name="counter"></param>
        public virtual void RemoveInstance(InstanceCounter counter)
        {

            InstanceCounterControls.RemoveInstance(this, counter);
        }
        #endregion
    }
}