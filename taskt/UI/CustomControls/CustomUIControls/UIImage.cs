using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using taskt.Properties;

namespace taskt.UI.CustomControls.CustomUIControls
{
    public static class UIImage
    {
        public static Dictionary<string, Image> UIImageDictionary()
        {
            var uiImages = new Dictionary<string, Image>();
            uiImages.Add("PauseScriptCommand", Resources.command_pause);
            uiImages.Add("SetEngineDelayCommand", Resources.command_pause);
            uiImages.Add("AddCodeCommentCommand", Resources.command_comment);
            uiImages.Add("ActivateWindowCommand", Resources.command_window);
            uiImages.Add("MoveWindowCommand", Resources.command_window);
            uiImages.Add("SetWindowStateCommand", Resources.command_window);
            uiImages.Add("WaitForWindowToExistCommand", Resources.command_window);
            uiImages.Add("HTMLInputCommand", Resources.command_input);
            uiImages.Add("UIAutomationCommand", Resources.command_input);
            uiImages.Add("ResizeWindowCommand", Resources.command_window);
            uiImages.Add("ShowMessageCommand", Resources.command_comment);
            uiImages.Add("StopProcessCommand", Resources.command_stop_process);
            uiImages.Add("StartProcessCommand", Resources.command_start_process);
            uiImages.Add("SetVariableCommand", Resources.command_function);
            uiImages.Add("AddToVariableCommand", Resources.command_function);
            uiImages.Add("SetVariableIndexCommand", Resources.command_function);
            uiImages.Add("NewVariableCommand", Resources.command_function);
            uiImages.Add("FormatDataCommand", Resources.command_function);
            uiImages.Add("ModifyStringCommand", Resources.command_function);
            uiImages.Add("DateCalculationCommand", Resources.command_function);
            uiImages.Add("MathCalculationCommand", Resources.command_function);
            uiImages.Add("TextExtractionCommand", Resources.command_function);
            uiImages.Add("GetPDFTextCommand", Resources.command_function);
            uiImages.Add("GetTextLengthCommand", Resources.command_function);
            uiImages.Add("GetWordCountCommand", Resources.command_function);
            uiImages.Add("GetListCountCommand", Resources.command_function);
            uiImages.Add("GetListItemCommand", Resources.command_function);
            uiImages.Add("RunScriptCommand", Resources.command_script);
            uiImages.Add("RunCustomCodeCommand", Resources.command_script);
            uiImages.Add("RunTaskCommand", Resources.command_start_process);
            uiImages.Add("StopCurrentTaskCommand", Resources.command_stop_process);
            uiImages.Add("InputCommand", Resources.command_input);
            uiImages.Add("CloseWindowCommand", Resources.command_window_close);
            uiImages.Add("IECreateBrowserCommand", Resources.command_web);
            uiImages.Add("IENavigateToURLCommand", Resources.command_web);
            uiImages.Add("IEFindBrowserCommand", Resources.command_web);
            uiImages.Add("IECloseBrowserCommand", Resources.command_window_close);
            uiImages.Add("IEElementActionCommand", Resources.command_web);
            uiImages.Add("SendKeystrokesCommand", Resources.command_input);
            uiImages.Add("SendAdvancedKeystrokesCommand", Resources.command_input);
            uiImages.Add("EncryptionCommand", Resources.command_input);
            uiImages.Add("SendMouseMoveCommand", Resources.command_input);
            uiImages.Add("SendMouseClickCommand", Resources.command_input);
            uiImages.Add("Setcommand_windowtateCommand", Resources.command_window);
            uiImages.Add("WebBrowserFindBrowserCommand", Resources.command_web);
            uiImages.Add("EndLoopCommand", Resources.command_endloop);
            uiImages.Add("GetClipboardTextCommand", Resources.command_files);
            uiImages.Add("SetClipboardTextCommand", Resources.command_files);
            uiImages.Add("ExcelCreateApplicationCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelOpenWorkbookCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelAddWorkbookCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelAppendCellCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelAppendRowCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelActivateCellCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelWriteRangeCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelWriteRowCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelCloseApplicationCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelWriteCellCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelGetCellCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelRunMacroCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveWorkbookAsCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteRowCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteRangeCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelGetLastRowIndexCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveWorkbookCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelActivateSheetCommand", Resources.command_spreadsheet);
            uiImages.Add("WordCreateApplicationCommand", Resources.command_files);
            uiImages.Add("WordCloseApplicationCommand", Resources.command_files);
            uiImages.Add("WordOpenDocumentCommand", Resources.command_files);
            uiImages.Add("WordSaveDocumentCommand", Resources.command_files);
            uiImages.Add("WordSaveDocumentAsCommand", Resources.command_files);
            uiImages.Add("WordExportToPDFCommand", Resources.command_files);
            uiImages.Add("WordAddDocumentCommand", Resources.command_files);
            uiImages.Add("WordReadDocumentCommand", Resources.command_files);
            uiImages.Add("WordReplaceTextCommand", Resources.command_files);
            uiImages.Add("WordAppendTextCommand", Resources.command_files);
            uiImages.Add("WordAppendImageCommand", Resources.command_files);
            uiImages.Add("WordAppendDataTableCommand", Resources.command_files);
            uiImages.Add("ExcelSplitRangeByColumnCommand", Resources.command_spreadsheet);
            uiImages.Add("AddDataRowCommand", Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCommand", Resources.command_spreadsheet);
            uiImages.Add("ExcelGetRangeCommand", Resources.command_spreadsheet);
            uiImages.Add("GetDataRowValueCommand", Resources.command_spreadsheet);
            uiImages.Add("WriteDataRowValueCommand", Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCountCommand", Resources.command_spreadsheet);
            uiImages.Add("CreateDataTableCommand", Resources.command_spreadsheet);
            uiImages.Add("FilterDataTableCommand", Resources.command_spreadsheet);
            uiImages.Add("RemoveDataRowCommand", Resources.command_spreadsheet);
            uiImages.Add("MergeDataTableCommand", Resources.command_spreadsheet);
            uiImages.Add("SeleniumCreateBrowserCommand", Resources.command_web);
            uiImages.Add("SeleniumNavigateToURLCommand", Resources.command_web);
            uiImages.Add("SeleniumNavigateForwardCommand", Resources.command_web);
            uiImages.Add("SeleniumNavigateBackCommand", Resources.command_web);
            uiImages.Add("SeleniumRefreshCommand", Resources.command_web);
            uiImages.Add("SeleniumCloseBrowserCommand", Resources.command_window_close);
            uiImages.Add("SeleniumElementActionCommand", Resources.command_web);
            uiImages.Add("SeleniumExecuteScriptCommand", Resources.command_script);
            uiImages.Add("SeleniumSwitchBrowserWindowCommand", Resources.command_window);
            uiImages.Add("SeleniumSwitchBrowserFrameCommand", Resources.command_window);
            uiImages.Add("SeleniumGetBrowserInfoCommand", Resources.command_web);
            uiImages.Add("SendSMTPEmailCommand", Resources.command_smtp);
            uiImages.Add("SendOutlookEmailCommand", Resources.command_smtp);
            uiImages.Add("GetOutlookEmailsCommand", Resources.command_smtp);
            uiImages.Add("MoveCopyOutlookEmailsCommand", Resources.command_smtp);
            uiImages.Add("DeleteOutlookEmailsCommand", Resources.command_smtp);
            uiImages.Add("ForwardOutlookEmailsCommand", Resources.command_smtp);
            uiImages.Add("ReplyToOutlookEmailsCommand", Resources.command_smtp);
            uiImages.Add("ErrorHandlingCommand", Resources.command_error);
            uiImages.Add("TryCommand", Resources.command_try);
            uiImages.Add("CatchCommand", Resources.command_try);
            uiImages.Add("FinallyCommand", Resources.command_try);
            uiImages.Add("EndTryCommand", Resources.command_try);
            uiImages.Add("ThrowCommand", Resources.command_try);
            uiImages.Add("RethrowCommand", Resources.command_try);
            uiImages.Add("BeginRetryCommand", Resources.command_try);
            uiImages.Add("EndRetryCommand", Resources.command_try);
            uiImages.Add("GetExceptionMessageCommand", Resources.command_try);
            uiImages.Add("SubstringCommand", Resources.command_string);
            uiImages.Add("SplitTextCommand", Resources.command_string);
            uiImages.Add("ReplaceTextCommand", Resources.command_string);
            uiImages.Add("BeginIfCommand", Resources.command_begin_if);
            uiImages.Add("NextLoopCommand", Resources.command_nextloop);
            uiImages.Add("BeginMultiIfCommand", Resources.command_begin_multi_if);
            uiImages.Add("EndIfCommand", Resources.command_end_if);
            uiImages.Add("ElseCommand", Resources.command_else);
            uiImages.Add("TakeScreenshotCommand", Resources.command_camera);
            uiImages.Add("PerformOCRCommand", Resources.command_camera);
            uiImages.Add("ImageRecognitionCommand", Resources.command_camera);
            uiImages.Add("GetHTMLSourceCommand", Resources.command_web);
            uiImages.Add("QueryHTMLSourceCommand", Resources.command_search);
            uiImages.Add("LoopCollectionCommand", Resources.command_startloop);
            uiImages.Add("LoopContinuouslyCommand", Resources.command_startloop);
            uiImages.Add("LoopNumberOfTimesCommand", Resources.command_startloop);
            uiImages.Add("BeginLoopCommand", Resources.command_startloop);
            uiImages.Add("BeginMultiLoopCommand", Resources.command_startloop);
            uiImages.Add("ExitLoopCommand", Resources.command_exitloop);
            uiImages.Add("SequenceCommand", Resources.command_sequence);
            uiImages.Add("ReadTextFileCommand", Resources.command_files);
            uiImages.Add("WriteTextFileCommand", Resources.command_files);
            uiImages.Add("MoveCopyFileCommand", Resources.command_files);
            uiImages.Add("ExtractFilesCommand", Resources.command_files);
            uiImages.Add("DeleteFileCommand", Resources.command_files);
            uiImages.Add("RenameFileCommand", Resources.command_files);
            uiImages.Add("WaitForFileCommand", Resources.command_files);
            uiImages.Add("GetFilesCommand", Resources.command_files);
            uiImages.Add("GetFoldersCommand", Resources.command_files);
            uiImages.Add("CreateFolderCommand", Resources.command_files);
            uiImages.Add("DeleteFolderCommand", Resources.command_files);
            uiImages.Add("MoveCopyFolderCommand", Resources.command_files);
            uiImages.Add("RenameFolderCommand", Resources.command_files);
            uiImages.Add("LogDataCommand", Resources.command_files);
            uiImages.Add("ExecuteDLLCommand", Resources.command_run_code);
            uiImages.Add("ExecuteRESTAPICommand", Resources.command_run_code);
            uiImages.Add("ParseJSONModelCommand", Resources.command_parse);
            uiImages.Add("ParseJSONArrayCommand", Resources.command_parse);
            uiImages.Add("UploadBotStoreDataCommand", Resources.command_server);
            uiImages.Add("GetBotStoreDataCommand", Resources.command_server);
            uiImages.Add("StopwatchCommand", Resources.command_stopwatch);
            uiImages.Add("SystemActionCommand", Resources.command_script);
            uiImages.Add("LaunchRemoteDesktopCommand", Resources.command_system);
            uiImages.Add("OSVariableCommand", Resources.command_system);
            uiImages.Add("GenerateNLGPhraseCommand", Resources.command_nlg);
            uiImages.Add("SetNLGParameterCommand", Resources.command_nlg);
            uiImages.Add("CreateNLGInstanceCommand", Resources.command_nlg);
            uiImages.Add("ShowEngineContextCommand", Resources.command_window);
            uiImages.Add("SetEnginePreferenceCommand", Resources.command_window);
            uiImages.Add("DefineDatabaseConnectionCommand", Resources.command_database);
            uiImages.Add("ExecuteDatabaseQueryCommand", Resources.command_database);
            uiImages.Add("RemoteTaskCommand", Resources.command_remote);
            uiImages.Add("RemoteAPICommand", Resources.command_remote);
            uiImages.Add("AddDictionaryItemCommand", Resources.command_dictionary);
            uiImages.Add("CreateDictionaryCommand", Resources.command_dictionary);
            uiImages.Add("GetDictionaryValueCommand", Resources.command_dictionary);
            uiImages.Add("LoadDictionaryCommand", Resources.command_spreadsheet);

            return uiImages;
        }

        public static ImageList UIImageList()
        {
            Dictionary<string, Image> imageIcons = UIImageDictionary();
            ImageList uiImages = new ImageList();
            uiImages.ImageSize = new Size(16, 16);

            foreach (var icon in imageIcons)
            {
                //var someImage = icon.Value;

                //using (Image src = icon.Value)
                //using (Bitmap dst = new Bitmap(16, 16))
                //using (Graphics g = Graphics.FromImage(dst))
                //{
                //    g.SmoothingMode = SmoothingMode.AntiAlias;
                //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //    g.DrawImage(src, 0, 0, dst.Width, dst.Height);
                //    uiImages.Images.Add(icon.Key, dst);
                //}
                uiImages.Images.Add(icon.Key, icon.Value);
            }
            return uiImages;
        }

        public static Image ResizeImageFile(Image image)
        {
            using (Image oldImage = image)
            {
                using (Bitmap newImage = new Bitmap(16, 16, PixelFormat.Format32bppRgb))
                {
                    using (Graphics canvas = Graphics.FromImage(newImage))
                    {
                        canvas.SmoothingMode = SmoothingMode.AntiAlias;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), new Size(16, 16)));
                        return newImage;
                    }
                }
            }
        }

        public static Image GetUIImage(string commandName)
        {
            var uiImageDictionary = UIImageDictionary();
            Image uiImage;
            try
            {
                uiImage = uiImageDictionary[commandName];
            }
            catch (Exception)
            {
                uiImage = Resources.command_files;
            }
            return uiImage;
        }
    }
}
