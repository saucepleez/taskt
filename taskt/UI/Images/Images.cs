using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using taskt.Core.Automation.Commands;

namespace taskt.UI
{
    public static class Images
    {
        //private static Dictionary<string, Image> imageList = new Dictionary<string, Image>();
        private static ImageList uiImages = new ImageList();
        private static Dictionary<string, string> commandImageTable = new Dictionary<string, string>();

        //private static Dictionary<string, string> imageCommandTable = new Dictionary<string, string>()
        //{
        //    // API
        //    {nameof(ExecuteDLLCommand), "taskt.Properties.Resources.command_run_code"},
        //    {nameof(HTTPExecuteRESTAPICommand), "taskt.Properties.Resources.command_run_code"},
        //    {nameof(HTTPSendHTTPRequestCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(HTMLGetHTMLTextByXPath), "taskt.Properties.Resources.command_search"},

        //    // Application
        //    {nameof(RunCSharpCodeCommand), "taskt.Properties.Resources.command_script"},
        //    {nameof(RunBatchScriptFileCommand), "taskt.Properties.Resources.command_script"},
        //    {nameof(StartApplicationCommand), "taskt.Properties.Resources.command_start_process"},
        //    {nameof(StopApplicationCommand), "taskt.Properties.Resources.command_stop_process"},
        //    {nameof(LoadScriptFileCommand), "taskt.Properties.Resources.command_start_process"},
        //    {nameof(UnloadScriptFileCommand), "taskt.Properties.Resources.command_stop_process"},
        //    {nameof(RunScriptFileCommand), "taskt.Properties.Resources.command_start_process"},
        //    {nameof(StopCurrentScriptFileCommand), "taskt.Properties.Resources.command_stop_process"},

        //    // Color
        //    {nameof(CreateColorCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(CreateColorFromExcelColorCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(CreateColorFromHexCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ConvertColorCommand), "taskt.Properties.Resources.command_function"},

        //    // Data
        //    {nameof(DateCalculationCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(FormatDataCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(LogDataCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(PDFTextExtractionCommand), "taskt.Properties.Resources.command_function"},

        //    // Database
        //    {nameof(DatabaseDefineConnectionCommand), "taskt.Properties.Resources.command_database"},
        //    {nameof(DatabaseExecuteQueryCommand), "taskt.Properties.Resources.command_database"},

        //    // DataTable
        //    {nameof(AddDataRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(AddDataTableColumnCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    //{nameof(AddDataTableColumnAndFillValuesByListCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    //{nameof(AddDataTableColumnsAndFillValuesByDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(AddDataTableRowByDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(AddDataTableRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(AddDataTableRowsByDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(CheckDataTableColumnExistsCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ConvertDataTableColumnToDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ConvertDataTableColumnToDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ConvertDataTableColumnToJSONCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ConvertDataTableColumnToListCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ConvertDataTableRowToDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ConvertDataTableRowToDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ConvertDataTableRowToJSONCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ConvertDataTableRowToListCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ConvertDataTableToJSONCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(CopyDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(CreateDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(DeleteDataTableColumnCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(DeleteDataTableRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(FilterDataTableColumnByRowValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(FilterDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(FilterDataTableRowByColumnValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(GetDataRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(GetDataRowCountCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(GetDataRowValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(GetDataTableColumnCountCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(GetDataTableColumnListCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(GetDataTableRowCountCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(GetDataTableValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(LoadDataTableCommand), "taskt.Properties.Resources.command_spreadsheet" },
        //    {nameof(ReplaceDataTableColumnValueCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ReplaceDataTableRowValueCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ReplaceDataTableValueCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ParseDatasetRowCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(RemoveDataRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(SetDataTableColumnValuesByDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(SetDataTableColumnValuesByListCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(SetDataTableRowValuesByDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(SetDataTableRowValuesByDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(SetDataTableValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(WriteDataRowValueCommand), "taskt.Properties.Resources.command_spreadsheet"},

        //    // DateTime
        //    {nameof(CalculateDateTimeCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ConvertDateTimeToExcelSerialCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(CreateDateTimeCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(CreateDateTimeFromExcelSerialCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(CreateDateTimeFromTextCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetDateTimeDifferencesCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(FormatDateTimeCommand), "taskt.Properties.Resources.command_function"},

        //    // Dictionary
        //    {nameof(AddDictionaryItemCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(CheckDictionaryKeyExistsCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(ConcatenateDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(ConvertDictionaryToDataTableCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(ConvertDictionaryToJSONCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(ConvertDictionaryToListCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(CopyDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(CreateDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(FilterDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(GetDictionaryKeyFromValueCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(GetDictionaryKeysListCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(GetDictionaryValueCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(LoadDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(RemoveDictionaryItemCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(ReplaceDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
        //    {nameof(SetDictionaryValueCommand), "taskt.Properties.Resources.command_dictionary"},

        //    // EMail
        //    {nameof(MailKitGetAddressesAsDataTableCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitGetAddressesAsDictionaryCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitGetAddressesAsListCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitGetEMailAttachmentsNameCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitGetEMailFromEMailListCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitGetEMailTextCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitLoadEMailCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitRecieveEMailListUsingIMAPCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitRecieveEMailListUsingPOPCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitSaveEMailCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitSaveEMailAttachmentsCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(MailKitSendEMailCommand), "taskt.Properties.Resources.command_comment"},

        //    // Engine
        //    {nameof(ErrorHandlingCommand), "taskt.Properties.Resources.command_error"},
        //    {nameof(GetBotStoreDataCommand), "taskt.Properties.Resources.command_server"},  // get bot data
        //    {nameof(PauseScriptCommand), "taskt.Properties.Resources.command_pause"},
        //    {nameof(SetEngineDelayCommand), "taskt.Properties.Resources.command_pause"},
        //    {nameof(ShowEngineContextCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(SetEnginePreferenceCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(StopwatchCommand), "taskt.Properties.Resources.command_stopwatch"},
        //    {nameof(UploadBotStoreDataCommand), "taskt.Properties.Resources.command_server"},   // upload bot store

        //    // Error
        //    {nameof(CatchExceptionCommand), "taskt.Properties.Resources.command_try"},
        //    {nameof(EndTryCommand), "taskt.Properties.Resources.command_try"},
        //    {nameof(FinallyCommand), "taskt.Properties.Resources.command_try"},
        //    {nameof(ThrowExceptionCommand), "taskt.Properties.Resources.command_try"},
        //    {nameof(TryCommand), "taskt.Properties.Resources.command_try"},

        //    // Excel
        //    {nameof(ExcelActivateSheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelAddWorkbookCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelAddWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelAppendCellCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelAppendRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelCheckCellValueExistsCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelCheckCellValueExistsRCCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelCheckExcelInstanceExistsCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelCheckWorksheetExistsCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelCloseExcelInstanceCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelCopyWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelCreateExcelInstanceCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelDeleteCellCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelDeleteRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelDeleteWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetCellCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetCellRCCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetColumnValuesAsDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetColumnValuesAsDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetColumnValuesAsListCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetCurrentWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetExcelInfoCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetLastRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetRangeCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetRangeCommandAsDT), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetRangeValuesAsDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetRowValuesAsDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetRowValuesAsDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetRowValuesAsListCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetWorksheetsCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelGetWorksheetInfoCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelOpenWorkbookCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelRenameWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelRunMacroCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSaveCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSaveAsCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSetCellCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSetCellRCCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSetColumnValuesFromDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSetColumnValuesFromDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSetColumnValuesFromListCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSetRowValuesFromDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSetRowValuesFromDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSetRowValuesFromListCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelSplitRangeByColumnCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelWriteRangeCommand), "taskt.Properties.Resources.command_spreadsheet"},
        //    {nameof(ExcelWriteRowCommand), "taskt.Properties.Resources.command_spreadsheet"},

        //    // File
        //    {nameof(CheckFileExistsCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(DeleteFileCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(ExtractZipFileCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(ExtractionFilePathCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(GetFileInfoCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(GetFilesCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(MoveFileCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(RenameFileCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WaitForFileToExistCommand), "taskt.Properties.Resources.command_files"},

        //    // Folder
        //    {nameof(CheckFolderExistsCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(CreateFolderCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(DeleteFolderCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(ExtractionFolderPathCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(GetFoldersCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(MoveFolderCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(RenameFolderCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WaitForFolderToExistCommand), "taskt.Properties.Resources.command_files"},

        //    // IE
        //    {nameof(IEBrowserCloseCommand), "taskt.Properties.Resources.command_window_close"},
        //    {nameof(IEBrowserCreateCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(IEBrowserElementActionCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(IEBrowserFindBrowserCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(IEBrowserNavigateURLCommand), "taskt.Properties.Resources.command_web"},

        //    // If
        //    {nameof(BeginIfCommand), "taskt.Properties.Resources.command_begin_if"},
        //    {nameof(BeginMultiIfCommand), "taskt.Properties.Resources.command_begin_multi_if"},
        //    {nameof(ElseCommand), "taskt.Properties.Resources.command_else"},
        //    {nameof(EndIfCommand), "taskt.Properties.Resources.command_end_if"},

        //    // Image
        //    {nameof(ImageRecognitionCommand), "taskt.Properties.Resources.command_camera"},
        //    {nameof(ExecuteOCRCommand), "taskt.Properties.Resources.command_camera"},
        //    {nameof(TakeScreenshotCommand), "taskt.Properties.Resources.command_camera"},

        //    // Input
        //    {nameof(ShowFileDialogCommand), "taskt.Properties.Resources.command_input"},
        //    {nameof(ShowFolderDialogCommand), "taskt.Properties.Resources.command_input"},
        //    {nameof(ShowHTMLInputDialogCommand), "taskt.Properties.Resources.command_input"},
        //    {nameof(ShowUserInputDialogCommand), "taskt.Properties.Resources.command_input"}, // prompt
        //    {nameof(SendAdvancedKeyStrokesCommand), "taskt.Properties.Resources.command_input"},
        //    {nameof(EnterShortcutKeyCommand), "taskt.Properties.Resources.command_input"},
        //    {nameof(EnterKeysCommand), "taskt.Properties.Resources.command_input"},
        //    {nameof(MoveMouseCommand), "taskt.Properties.Resources.command_input"},
        //    {nameof(ClickMouseCommand), "taskt.Properties.Resources.command_input"},
        //    {nameof(UIAutomationUIElementActionCommand), "taskt.Properties.Resources.command_input"},

        //    // JSON
        //    {nameof(AddJSONArrayItemCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(AddJSONObjectPropertyCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(ConvertJSONToDataTableCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(ConvertJSONToDictionaryCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(ConvertJSONToListCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(CreateJSONVariableCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(GetJSONValueListCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(GetMultiJSONValueListCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(InsertJSONArrayItemCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(InsertJSONObjectPropertyCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(ParseJSONArrayCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(ReadJSONFileCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(RemoveJSONArrayItemCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(RemoveJSONPropertyCommand), "taskt.Properties.Resources.command_parse"},
        //    {nameof(SetJSONValueCommand), "taskt.Properties.Resources.command_parse"},

        //    // List
        //    {nameof(AddListItemCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(CheckListItemExistsCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ConcatenateListsCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ConvertListToDataTableCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ConvertListToDictionaryCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ConvertListToJSONCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(CopyListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(CreateListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(FilterListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetAverageFromListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetListCountCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetListIndexCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetListIndexFromValueCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetListItemCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetMaxFromListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetMedianFromListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetMinFromListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetSumFromListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetVarianceFromListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ReplaceListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ReverseListCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(SetListIndexCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(SetListItemCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(SortListCommand), "taskt.Properties.Resources.command_function"},

        //    // Loop
        //    {nameof(BeginLoopCommand), "taskt.Properties.Resources.command_startloop"},
        //    {nameof(BeginMultiLoopCommand), "taskt.Properties.Resources.command_startloop"},
        //    {nameof(EndLoopCommand), "taskt.Properties.Resources.command_endloop"},
        //    {nameof(ExitLoopCommand), "taskt.Properties.Resources.command_exitloop"},
        //    {nameof(BeginContinousLoopCommand), "taskt.Properties.Resources.command_startloop"},
        //    {nameof(BeginListLoopCommand), "taskt.Properties.Resources.command_startloop"},
        //    {nameof(BeginNumberOfTimesLoopCommand), "taskt.Properties.Resources.command_startloop"},
        //    {nameof(NextLoopCommand), "taskt.Properties.Resources.command_nextloop"},

        //    // Misc
        //    {nameof(ClearClipboardTextCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(SetClipboardTextCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(GetClipboardTextCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(CommentCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(CreateShortcutCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(ShowMessageCommand), "taskt.Properties.Resources.command_comment"},
        //    {nameof(PingCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(PlaySystemSoundCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(SMTPSendEmailCommand), "taskt.Properties.Resources.command_smtp"},
        //    {nameof(SequenceCommand), "taskt.Properties.Resources.command_sequence"},

        //    // NLG
        //    {nameof(NLGCreateNLGInstanceCommand), "taskt.Properties.Resources.command_nlg"},
        //    {nameof(NLGGenerateNLGPhraseCommand), "taskt.Properties.Resources.command_nlg"},
        //    {nameof(NLGSetNLGParameterCommand), "taskt.Properties.Resources.command_nlg"},

        //    // Numeric
        //    {nameof(CreateNumericalVariableCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(DecreaseNumericalVariableCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(FormatNumberCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(IncreaseNumericalVariableCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(MathCalculationCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(RandomNumberCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(RoundNumberCommand), "taskt.Properties.Resources.command_function"}, 

        //    // Outlook
        //    {nameof(OutlookDeleteEmailsCommand), "taskt.Properties.Resources.command_smtp"},
        //    {nameof(OutlookForwardEmailsCommand), "taskt.Properties.Resources.command_smtp"},
        //    {nameof(OutlookGetEmailsCommand), "taskt.Properties.Resources.command_smtp"},
        //    {nameof(OutlookMoveEmailsCommand), "taskt.Properties.Resources.command_smtp"},
        //    {nameof(OutlookReplyToEmailsCommand), "taskt.Properties.Resources.command_smtp"},
        //    {nameof(OutlookEmailCommand), "taskt.Properties.Resources.command_smtp"},

        //    // Regex
        //    {nameof(GetRegexMatchesCommand), "taskt.Properties.Resources.command_function"},

        //    // Remote
        //    {nameof(RemoteAPICommand), "taskt.Properties.Resources.command_remote"},
        //    {nameof(RemoteTaskCommand), "taskt.Properties.Resources.command_remote"},

        //    // System
        //    {nameof(GetEnvironmentVariableCommand), "taskt.Properties.Resources.command_system"},
        //    {nameof(LaunchRemoteDesktopCommand), "taskt.Properties.Resources.command_system"},
        //    {nameof(GetOSVariableCommand), "taskt.Properties.Resources.command_system"},
        //    {nameof(SystemActionCommand), "taskt.Properties.Resources.command_script"},

        //    // Text
        //    {nameof(CheckTextCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ConcatenateTextVariableCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(CreateTextVariableCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(EncryptDecryptTextCommand), "taskt.Properties.Resources.command_input"},
        //    {nameof(ExtractionTextCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetWordLengthCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetWordCountCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ModifyTextCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ReadTextFileCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(RegExExtractionTextCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(ReplaceTextCommand), "taskt.Properties.Resources.command_string"},
        //    {nameof(SplitTextCommand), "taskt.Properties.Resources.command_string"},
        //    {nameof(SubstringTextCommand), "taskt.Properties.Resources.command_string"},
        //    {nameof(WriteTextFileCommand), "taskt.Properties.Resources.command_files"},

        //    // UIAutomation
        //    {nameof(UIAutomationCheckUIElementExistCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationCheckUIElementExistByXPathCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationClickUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationExpandCollapseItemsInUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSearchChildUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationGetChildrenUIElementsInformationCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSearchUIElementFromUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSearchUIElementFromUIElementByXPathCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSearchUIElementFromTableUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSearchUIElementFromWindowCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSearchUIElementAndWindowByXPathCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationGetUIElementTreeXMLFromUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSearchParentUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationGetSelectedStateFromUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationGetSelectionItemsFromUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationGetTextFromUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationGetTextFromTableUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSelectUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSelectItemInUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationSetTextToUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationScrollUIElementCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationWaitForUIElementToExistsByXPathCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(UIAutomationWaitForUIElementToExistsCommand), "taskt.Properties.Resources.command_window"},

        //    // Variable
        //    {nameof(CheckVariableExistsCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetVariableIndexCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(GetVariableTypeCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(NewVariableCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(SetVariableValueCommand), "taskt.Properties.Resources.command_function"},
        //    {nameof(SetVariableIndexCommand), "taskt.Properties.Resources.command_function"},

        //    // Web
        //    {nameof(SeleniumBrowserCheckBrowserInstanceExistsCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserCloseWebBrowserInstanceCommand), "taskt.Properties.Resources.command_window_close"},
        //    {nameof(SeleniumBrowserCreateWebBrowserInstanceCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserWebElementActionCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserExecuteScriptCommand), "taskt.Properties.Resources.command_script"},
        //    {nameof(SeleniumBrowserGetAWebElementValuesAsDataTableCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserGetAWebElementValuesAsDictionaryCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserGetAWebElementValuesAsListCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserGetWebElementsValueAsDataTableCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserGetWebElementsValueAsDictionaryCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserGetWebElementsValueAsListCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserGetWebElementsValuesAsDataTableCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserGetTableValueAsDataTableCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserGetWebBrowserInfoCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserNavigateBackCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserNavigateForwardCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserNavigateURLCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserRefreshCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserResizeWebBrowserCommand), "taskt.Properties.Resources.command_web"},
        //    {nameof(SeleniumBrowserSwitchWebBrowserFrameCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(SeleniumBrowserSwitchWebBrowserWindowCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(SeleniumBrowserTakeScreenshotCommand), "taskt.Properties.Resources.command_web"},

        //    // Window
        //    {nameof(ActivateWindowCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(CheckWindowNameExistsCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(CloseWindowCommand), "taskt.Properties.Resources.command_window_close"},
        //    {nameof(GetWindowNamesCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(GetWindowPositionCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(GetWindowStateCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(MoveWindowCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(ResizeWindowCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(SetWindowStateCommand), "taskt.Properties.Resources.command_window"},
        //    {nameof(WaitForWindowToExistsCommand), "taskt.Properties.Resources.command_window"},

        //    // Word
        //    {nameof(WordAddDocumentCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordAppendDataTableCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordAppendImageCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordAppendTextCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordCheckWordInstanceExistsCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordCloseWordInstanceCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordCreateWordInstanceCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordExportToPDFCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordOpenDocumentCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordReadDocumentCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordReplaceTextCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordSaveDocumentCommand), "taskt.Properties.Resources.command_files"},
        //    {nameof(WordSaveDocumentAsCommand), "taskt.Properties.Resources.command_files"},

        //    //// NOTHING ///
        //    {"BeginExcelDatasetLoopCommand", "taskt.Properties.Resources.command_startloop"},
        //    {"ThickAppClickItemCommand", "taskt.Properties.Resources.command_input"},
        //    {"ThickAppGetTextCommand", "taskt.Properties.Resources.command_search"},
        //    {"Setcommand_windowtateCommand", "taskt.Properties.Resources.command_window"},
        //    {"_NotFoundCommand", "taskt.Properties.Resources.command_files"},
        //};

        //public static Dictionary<string, Image> UIImageDictionary()
        //{
        //    //var uiImages = new Dictionary<string, Image>();
        //    //Dictionary<string, Image> uiImages = imageList;
        //    Dictionary<string, Image> uiImages = new Dictionary<string, Image>();

        //    // API
        //    uiImages.Add("ExecuteDLLCommand", taskt.Properties.Resources.command_run_code);
        //    uiImages.Add("RESTCommand", taskt.Properties.Resources.command_run_code);
        //    uiImages.Add("HTTPRequestCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("HTTPQueryResultCommand", taskt.Properties.Resources.command_search);

        //    // Data
        //    uiImages.Add("DateCalculationCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("FormatDataCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("GetListCountCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("GetListItemCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("GetWordLengthCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("GetWordCountCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("LogDataCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("MathCalculationCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("ModifyVariableCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("ParseDatasetRowCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("ParseJsonArrayCommand", taskt.Properties.Resources.command_parse);
        //    uiImages.Add("ParseJsonCommand", taskt.Properties.Resources.command_parse);
        //    uiImages.Add("ParseJsonModelCommand", taskt.Properties.Resources.command_parse);
        //    uiImages.Add("PDFTextExtractionCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("RegExExtractorCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("StringReplaceCommand", taskt.Properties.Resources.command_string);
        //    uiImages.Add("StringSplitCommand", taskt.Properties.Resources.command_string);
        //    uiImages.Add("StringSubstringCommand", taskt.Properties.Resources.command_string);
        //    uiImages.Add("TextExtractorCommand", taskt.Properties.Resources.command_function);

        //    // Database
        //    uiImages.Add("DatabaseDefineConnectionCommand", taskt.Properties.Resources.command_database);
        //    uiImages.Add("DatabaseExecuteQueryCommand", taskt.Properties.Resources.command_database);

        //    // DataTable
        //    uiImages.Add("LoadTaskCommand", taskt.Properties.Resources.command_start_process);
        //    uiImages.Add("UnloadTaskCommand", taskt.Properties.Resources.command_stop_process);
        //    uiImages.Add("ExcelAddWorksheetCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("AddDataRowCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("CreateDataTableCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("FilterDataTableCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("GetDataRowCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("GetDataRowCountCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("GetDataRowValueCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("RemoveDataRowCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("WriteDataRowValueCommand", taskt.Properties.Resources.command_spreadsheet);

        //    // Dictionary
        //    uiImages.Add("AddDictionaryCommand", taskt.Properties.Resources.command_dictionary);
        //    uiImages.Add("CreateDictionaryCommand", taskt.Properties.Resources.command_dictionary);
        //    uiImages.Add("GetDictionaryValueCommand", taskt.Properties.Resources.command_dictionary);
        //    uiImages.Add("LoadDictionaryCommand", taskt.Properties.Resources.command_dictionary);

        //    // Engine
        //    uiImages.Add("ErrorHandlingCommand", taskt.Properties.Resources.command_error);
        //    uiImages.Add("GetDataCommand", taskt.Properties.Resources.command_server);  // get bot data
        //    uiImages.Add("PauseCommand", taskt.Properties.Resources.command_pause);
        //    uiImages.Add("SetEngineDelayCommand", taskt.Properties.Resources.command_pause);
        //    uiImages.Add("ShowEngineContextCommand", taskt.Properties.Resources.command_window);
        //    uiImages.Add("SetEnginePreferenceCommand", taskt.Properties.Resources.command_window);
        //    uiImages.Add("StopwatchCommand", taskt.Properties.Resources.command_stopwatch);
        //    uiImages.Add("UploadDataCommand", taskt.Properties.Resources.command_server);   // upload bot store

        //    // Error
        //    uiImages.Add("CatchExceptionCommand", taskt.Properties.Resources.command_try);
        //    uiImages.Add("EndTryCommand", taskt.Properties.Resources.command_try);
        //    uiImages.Add("FinallyCommand", taskt.Properties.Resources.command_try);
        //    uiImages.Add("ThrowExceptionCommand", taskt.Properties.Resources.command_try);
        //    uiImages.Add("TryCommand", taskt.Properties.Resources.command_try);

        //    // Excel
        //    uiImages.Add("ExcelActivateSheetCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelAddWorkbookCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelAppendCellCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelAppendRowCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelCloseApplicationCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelCreateDatasetCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelCreateApplicationCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelDeleteCellCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelDeleteRowCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelGetCellCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelGetLastRowCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelGetRangeCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelGetRangeCommandAsDT", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelGoToCellCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelOpenWorkbookCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelRunMacroCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelSaveCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelSaveAsCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelSetCellCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelSplitRangeByColumnCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelWriteRangeCommand", taskt.Properties.Resources.command_spreadsheet);
        //    uiImages.Add("ExcelWriteRowCommand", taskt.Properties.Resources.command_spreadsheet);

        //    // File
        //    uiImages.Add("DeleteFileCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("ExtractFileCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("GetFilesCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("MoveFileCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("RenameFileCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WaitForFileToExistCommand", taskt.Properties.Resources.command_files);

        //    // Folder
        //    uiImages.Add("CreateFolderCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("DeleteFolderCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("GetFoldersCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("MoveFolderCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("RenameFolderCommand", taskt.Properties.Resources.command_files);

        //    // IE
        //    uiImages.Add("IEBrowserCloseCommand", taskt.Properties.Resources.command_window_close);
        //    uiImages.Add("IEBrowserCreateCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("IEBrowserElementCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("WebBrowserFindBrowserCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("IEBrowserNavigateCommand", taskt.Properties.Resources.command_web);

        //    // If
        //    uiImages.Add("BeginIfCommand", taskt.Properties.Resources.command_begin_if);
        //    uiImages.Add("BeginMultiIfCommand", taskt.Properties.Resources.command_begin_multi_if);
        //    uiImages.Add("ElseCommand", taskt.Properties.Resources.command_else);
        //    uiImages.Add("EndIfCommand", taskt.Properties.Resources.command_end_if);

        //    // Image
        //    uiImages.Add("ImageRecognitionCommand", taskt.Properties.Resources.command_camera);
        //    uiImages.Add("OCRCommand", taskt.Properties.Resources.command_camera);
        //    uiImages.Add("ScreenshotCommand", taskt.Properties.Resources.command_camera);

        //    // Input
        //    uiImages.Add("HTMLInputCommand", taskt.Properties.Resources.command_input);
        //    uiImages.Add("UserInputCommand", taskt.Properties.Resources.command_input); // prompt
        //    uiImages.Add("SendAdvancedKeyStrokesCommand", taskt.Properties.Resources.command_input);
        //    uiImages.Add("SendHotkeyCommand", taskt.Properties.Resources.command_input);
        //    uiImages.Add("SendKeysCommand", taskt.Properties.Resources.command_input);
        //    uiImages.Add("SendMouseMoveCommand", taskt.Properties.Resources.command_input);
        //    uiImages.Add("SendMouseClickCommand", taskt.Properties.Resources.command_input);
        //    uiImages.Add("UIAutomationCommand", taskt.Properties.Resources.command_input);

        //    // Loop
        //    uiImages.Add("BeginLoopCommand", taskt.Properties.Resources.command_startloop);
        //    uiImages.Add("BeginMultiLoopCommand", taskt.Properties.Resources.command_startloop);
        //    uiImages.Add("EndLoopCommand", taskt.Properties.Resources.command_endloop);
        //    uiImages.Add("ExitLoopCommand", taskt.Properties.Resources.command_exitloop);
        //    uiImages.Add("BeginContinousLoopCommand", taskt.Properties.Resources.command_startloop);
        //    uiImages.Add("BeginListLoopCommand", taskt.Properties.Resources.command_startloop);
        //    uiImages.Add("BeginNumberOfTimesLoopCommand", taskt.Properties.Resources.command_startloop);
        //    uiImages.Add("NextLoopCommand", taskt.Properties.Resources.command_nextloop);

        //    // Misc
        //    uiImages.Add("CommentCommand", taskt.Properties.Resources.command_comment);
        //    uiImages.Add("EncryptionCommand", taskt.Properties.Resources.command_input);
        //    uiImages.Add("ClipboardGetTextCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("PingCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("SMTPSendEmailCommand", taskt.Properties.Resources.command_smtp);
        //    uiImages.Add("SequenceCommand", taskt.Properties.Resources.command_sequence);
        //    uiImages.Add("ClipboardSetTextCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("MessageBoxCommand", taskt.Properties.Resources.command_comment);

        //    // NLG
        //    uiImages.Add("NLGCreateInstanceCommand", taskt.Properties.Resources.command_nlg);
        //    uiImages.Add("NLGGeneratePhraseCommand", taskt.Properties.Resources.command_nlg);
        //    uiImages.Add("NLGSetParameterCommand", taskt.Properties.Resources.command_nlg);

        //    // Outlook
        //    uiImages.Add("OutlookDeleteEmailsCommand", taskt.Properties.Resources.command_smtp);
        //    uiImages.Add("OutlookForwardEmailsCommand", taskt.Properties.Resources.command_smtp);
        //    uiImages.Add("OutlookGetEmailsCommand", taskt.Properties.Resources.command_smtp);
        //    uiImages.Add("OutlookMoveEmailsCommand", taskt.Properties.Resources.command_smtp);
        //    uiImages.Add("OutlookReplyToEmailsCommand", taskt.Properties.Resources.command_smtp);
        //    uiImages.Add("OutlookEmailCommand", taskt.Properties.Resources.command_smtp);

        //    // Program
        //    uiImages.Add("RunCustomCodeCommand", taskt.Properties.Resources.command_script);
        //    uiImages.Add("RunScriptCommand", taskt.Properties.Resources.command_script);
        //    uiImages.Add("StartProcessCommand", taskt.Properties.Resources.command_start_process);
        //    uiImages.Add("StopProcessCommand", taskt.Properties.Resources.command_stop_process);

        //    // Regex
        //    uiImages.Add("GetRegexMatchesCommand", taskt.Properties.Resources.command_function);

        //    // Remote
        //    uiImages.Add("RemoteAPICommand", taskt.Properties.Resources.command_remote);
        //    uiImages.Add("RemoteTaskCommand", taskt.Properties.Resources.command_remote);

        //    // System
        //    uiImages.Add("EnvironmentVariableCommand", taskt.Properties.Resources.command_system);
        //    uiImages.Add("RemoteDesktopCommand", taskt.Properties.Resources.command_system);
        //    uiImages.Add("OSVariableCommand", taskt.Properties.Resources.command_system);
        //    uiImages.Add("SystemActionCommand", taskt.Properties.Resources.command_script);

        //    // Task
        //    uiImages.Add("RunTaskCommand", taskt.Properties.Resources.command_start_process);
        //    uiImages.Add("StopTaskCommand", taskt.Properties.Resources.command_stop_process);

        //    // Text
        //    uiImages.Add("ReadTextFileCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WriteTextFileCommand", taskt.Properties.Resources.command_files);

        //    // Variable
        //    uiImages.Add("AddToVariableCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("AddVariableCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("VariableCommand", taskt.Properties.Resources.command_function);
        //    uiImages.Add("SetVariableIndexCommand", taskt.Properties.Resources.command_function);

        //    // Web
        //    uiImages.Add("SeleniumBrowserCloseCommand", taskt.Properties.Resources.command_window_close);
        //    uiImages.Add("SeleniumBrowserCreateCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("SeleniumBrowserElementActionCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("SeleniumBrowserExecuteScriptCommand", taskt.Properties.Resources.command_script);
        //    uiImages.Add("SeleniumBrowserInfoCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("SeleniumBrowserNavigateBackCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("SeleniumBrowserNavigateForwardCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("SeleniumBrowserNavigateURLCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("SeleniumBrowserRefreshCommand", taskt.Properties.Resources.command_web);
        //    uiImages.Add("SeleniumBrowserSwitchFrameCommand", taskt.Properties.Resources.command_window);
        //    uiImages.Add("SeleniumBrowserSwitchWindowCommand", taskt.Properties.Resources.command_window);
        //    uiImages.Add("SeleniumBrowserTakeScreenshotCommand", taskt.Properties.Resources.command_web);

        //    // Window
        //    uiImages.Add("ActivateWindowCommand", taskt.Properties.Resources.command_window);
        //    uiImages.Add("CloseWindowCommand", taskt.Properties.Resources.command_window_close);
        //    uiImages.Add("MoveWindowCommand", taskt.Properties.Resources.command_window);
        //    uiImages.Add("ResizeWindowCommand", taskt.Properties.Resources.command_window);
        //    uiImages.Add("SetWindowStateCommand", taskt.Properties.Resources.command_window);
        //    uiImages.Add("WaitForWindowCommand", taskt.Properties.Resources.command_window);

        //    // Word
        //    uiImages.Add("WordAddDocumentCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordAppendDataTableCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordAppendImageCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordAppendTextCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordCloseApplicationCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordCreateApplicationCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordExportToPDFCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordOpenDocumentCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordReadDocumentCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordReplaceTextCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordSaveCommand", taskt.Properties.Resources.command_files);
        //    uiImages.Add("WordSaveAsCommand", taskt.Properties.Resources.command_files);

        //    //// NOTHING ///
        //    uiImages.Add("BeginExcelDatasetLoopCommand", taskt.Properties.Resources.command_startloop);
        //    uiImages.Add("ThickAppClickItemCommand", taskt.Properties.Resources.command_input);
        //    uiImages.Add("ThickAppGetTextCommand", taskt.Properties.Resources.command_search);
        //    uiImages.Add("Setcommand_windowtateCommand", taskt.Properties.Resources.command_window);
        //    uiImages.Add("_NotFoundCommand", taskt.Properties.Resources.command_files);

        //    // release
        //    //GC.Collect();

        //    return uiImages;
        //}

        private static void CreateCommandImageTable()
        {
            commandImageTable.Clear();
            var classes = Assembly.GetAssembly(typeof(ScriptCommand)).GetTypes()
                            .Where(t =>
                            {
                                return t.IsSubclassOf(typeof(ScriptCommand)) && !t.IsAbstract;
                            }).ToList();

            foreach (var c in classes)
            {
                var attr = (Core.Automation.Attributes.ClassAttributes.CommandIcon)c.GetCustomAttribute(typeof(Core.Automation.Attributes.ClassAttributes.CommandIcon)) ?? new Core.Automation.Attributes.ClassAttributes.CommandIcon();
                commandImageTable.Add(c.Name, attr.iconName);
            }
        }


        public static ImageList UIImageList()
        {
            if (commandImageTable.Count == 0)
            {
                CreateCommandImageTable();
            }

            if (uiImages.Images.Count > 0)
            {
                return uiImages;
            }

            uiImages.ImageSize = new Size(16, 16);

            // check dup
            var lst = new List<string>();
            foreach (var item in commandImageTable)
            {
                if (!lst.Contains(item.Value))
                {
                    lst.Add(item.Value);
                }
            }
            foreach (var v in lst)
            {
                var img = (Image)Properties.Resources.ResourceManager.GetObject(v);
                uiImages.Images.Add(v, img);
            }

            //uiImages.Images.Add("taskt.Properties.Resources.command_begin_if", taskt.Properties.Resources.command_begin_if);
            //uiImages.Images.Add("taskt.Properties.Resources.command_begin_multi_if", taskt.Properties.Resources.command_begin_multi_if);
            //uiImages.Images.Add("taskt.Properties.Resources.command_camera", taskt.Properties.Resources.command_camera);
            //uiImages.Images.Add("taskt.Properties.Resources.command_comment", taskt.Properties.Resources.command_comment);
            //uiImages.Images.Add("taskt.Properties.Resources.command_database", taskt.Properties.Resources.command_database);
            //uiImages.Images.Add("taskt.Properties.Resources.command_dictionary", taskt.Properties.Resources.command_dictionary);
            //uiImages.Images.Add("taskt.Properties.Resources.command_else", taskt.Properties.Resources.command_else);
            //uiImages.Images.Add("taskt.Properties.Resources.command_endloop", taskt.Properties.Resources.command_endloop);
            //uiImages.Images.Add("taskt.Properties.Resources.command_end_if", taskt.Properties.Resources.command_end_if);
            //uiImages.Images.Add("taskt.Properties.Resources.command_error", taskt.Properties.Resources.command_error);
            //uiImages.Images.Add("taskt.Properties.Resources.command_exitloop", taskt.Properties.Resources.command_exitloop);
            //uiImages.Images.Add("taskt.Properties.Resources.command_files", taskt.Properties.Resources.command_files);
            //uiImages.Images.Add("taskt.Properties.Resources.command_function", taskt.Properties.Resources.command_function);
            //uiImages.Images.Add("taskt.Properties.Resources.command_input", taskt.Properties.Resources.command_input);
            //uiImages.Images.Add("taskt.Properties.Resources.command_nextloop", taskt.Properties.Resources.command_nextloop);
            //uiImages.Images.Add("taskt.Properties.Resources.command_nlg", taskt.Properties.Resources.command_nlg);
            //uiImages.Images.Add("taskt.Properties.Resources.command_parse", taskt.Properties.Resources.command_parse);
            //uiImages.Images.Add("taskt.Properties.Resources.command_pause", taskt.Properties.Resources.command_pause);
            //uiImages.Images.Add("taskt.Properties.Resources.command_remote", taskt.Properties.Resources.command_remote);
            //uiImages.Images.Add("taskt.Properties.Resources.command_run_code", taskt.Properties.Resources.command_run_code);
            //uiImages.Images.Add("taskt.Properties.Resources.command_script", taskt.Properties.Resources.command_script);
            //uiImages.Images.Add("taskt.Properties.Resources.command_search", taskt.Properties.Resources.command_search);
            //uiImages.Images.Add("taskt.Properties.Resources.command_sequence", taskt.Properties.Resources.command_sequence);
            //uiImages.Images.Add("taskt.Properties.Resources.command_server", taskt.Properties.Resources.command_server);
            //uiImages.Images.Add("taskt.Properties.Resources.command_smtp", taskt.Properties.Resources.command_smtp);
            //uiImages.Images.Add("taskt.Properties.Resources.command_spreadsheet", taskt.Properties.Resources.command_spreadsheet);
            //uiImages.Images.Add("taskt.Properties.Resources.command_startloop", taskt.Properties.Resources.command_startloop);
            //uiImages.Images.Add("taskt.Properties.Resources.command_start_process", taskt.Properties.Resources.command_start_process);
            //uiImages.Images.Add("taskt.Properties.Resources.command_stopwatch", taskt.Properties.Resources.command_stopwatch);
            //uiImages.Images.Add("taskt.Properties.Resources.command_stop_process", taskt.Properties.Resources.command_stop_process);
            //uiImages.Images.Add("taskt.Properties.Resources.command_string", taskt.Properties.Resources.command_string);
            //uiImages.Images.Add("taskt.Properties.Resources.command_system", taskt.Properties.Resources.command_system);
            //uiImages.Images.Add("taskt.Properties.Resources.command_try", taskt.Properties.Resources.command_try);
            //uiImages.Images.Add("taskt.Properties.Resources.command_web", taskt.Properties.Resources.command_web);
            //uiImages.Images.Add("taskt.Properties.Resources.command_window", taskt.Properties.Resources.command_window);
            //uiImages.Images.Add("taskt.Properties.Resources.command_window_close", taskt.Properties.Resources.command_window_close);

            return uiImages;
        }

        //public static Image ResizeImageFile(Image image)
        //{
        //    using (System.Drawing.Image oldImage = image)
        //    {
        //        using (Bitmap newImage = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
        //        {
        //            using (Graphics canvas = Graphics.FromImage(newImage))
        //            {
        //                canvas.SmoothingMode = SmoothingMode.AntiAlias;
        //                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //                canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), new Size(16,16)));
        //                return newImage;
        //            }
        //        }
        //    }
        //}

        public static int GetUIImageList(string commandName)
        {
            try
            {
                return uiImages.Images.IndexOfKey(commandImageTable[commandName]);
            }
            catch (Exception)
            {
                //return uiImages.Images.IndexOfKey("taskt.Properties.Resources.command_files");
                //return uiImages.Images.IndexOfKey("command_files");
                return uiImages.Images.IndexOfKey(nameof(Properties.Resources.command_files));
            }
        }

        public static Image GetUIImage(string commandName)
        {
            if (uiImages.Images.Count == 0)
            {
                UIImageList();
            }

            Image retImage;
            try
            {
                retImage = uiImages.Images[uiImages.Images.IndexOfKey(commandImageTable[commandName])];
            }
            catch (Exception)
            {
                //retImage = uiImages.Images[uiImages.Images.IndexOfKey("taskt.Properties.Resources.command_files")];
                //retImage = uiImages.Images[uiImages.Images.IndexOfKey("command_files")];
                retImage = uiImages.Images[uiImages.Images.IndexOfKey(nameof(Properties.Resources.command_files))];
            }

            return retImage;
        }
    }
}
