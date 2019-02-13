using System;
using System.Xml.Serialization;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [XmlInclude(typeof(SendKeysCommand))]
    [XmlInclude(typeof(SendAdvancedKeyStrokesCommand))]
    [XmlInclude(typeof(SendMouseMoveCommand))]
    [XmlInclude(typeof(PauseCommand))]
    [XmlInclude(typeof(ActivateWindowCommand))]
    [XmlInclude(typeof(MoveWindowCommand))]
    [XmlInclude(typeof(CommentCommand))]
    [XmlInclude(typeof(ThickAppClickItemCommand))]
    [XmlInclude(typeof(ThickAppGetTextCommand))]
    [XmlInclude(typeof(UIAutomationCommand))]
    [XmlInclude(typeof(ResizeWindowCommand))]
    [XmlInclude(typeof(WaitForWindowCommand))]
    [XmlInclude(typeof(MessageBoxCommand))]
    [XmlInclude(typeof(StopProcessCommand))]
    [XmlInclude(typeof(StartProcessCommand))]
    [XmlInclude(typeof(AddVariableCommand))]
    [XmlInclude(typeof(VariableCommand))]
    [XmlInclude(typeof(RunScriptCommand))]
    [XmlInclude(typeof(CloseWindowCommand))]
    [XmlInclude(typeof(SetWindowStateCommand))]
    [XmlInclude(typeof(BeginExcelDatasetLoopCommand))]
    [XmlInclude(typeof(ExitLoopCommand))]
    [XmlInclude(typeof(EndLoopCommand))]
    [XmlInclude(typeof(ClipboardGetTextCommand))]
    [XmlInclude(typeof(ClipboardSetTextCommand))]
    [XmlInclude(typeof(ScreenshotCommand))]
    [XmlInclude(typeof(ExcelOpenWorkbookCommand))]
    [XmlInclude(typeof(ExcelCreateApplicationCommand))]
    [XmlInclude(typeof(ExcelAddWorkbookCommand))]
    [XmlInclude(typeof(ExcelGoToCellCommand))]
    [XmlInclude(typeof(ExcelSetCellCommand))]
    [XmlInclude(typeof(ExcelCloseApplicationCommand))]
    [XmlInclude(typeof(ExcelGetCellCommand))]
    [XmlInclude(typeof(ExcelRunMacroCommand))]
    [XmlInclude(typeof(ExcelActivateSheetCommand))]
    [XmlInclude(typeof(ExcelDeleteRowCommand))]
    [XmlInclude(typeof(ExcelDeleteCellCommand))]
    [XmlInclude(typeof(ExcelGetLastRowCommand))]
    [XmlInclude(typeof(ExcelSaveAsCommand))]
    [XmlInclude(typeof(ExcelSaveCommand))]
    [XmlInclude(typeof(SeleniumBrowserCreateCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateURLCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateForwardCommand))]
    [XmlInclude(typeof(SeleniumBrowserNavigateBackCommand))]
    [XmlInclude(typeof(SeleniumBrowserRefreshCommand))]
    [XmlInclude(typeof(SeleniumBrowserCloseCommand))]
    [XmlInclude(typeof(SeleniumBrowserElementActionCommand))]
    [XmlInclude(typeof(SeleniumBrowserExecuteScriptCommand))]
    [XmlInclude(typeof(SMTPSendEmailCommand))]
    [XmlInclude(typeof(ErrorHandlingCommand))]
    [XmlInclude(typeof(StringSubstringCommand))]
    [XmlInclude(typeof(StringSplitCommand))]
    [XmlInclude(typeof(BeginIfCommand))]
    [XmlInclude(typeof(EndIfCommand))]
    [XmlInclude(typeof(ElseCommand))]
    [XmlInclude(typeof(OCRCommand))]
    [XmlInclude(typeof(HTTPRequestCommand))]
    [XmlInclude(typeof(HTTPQueryResultCommand))]
    [XmlInclude(typeof(ImageRecognitionCommand))]
    [XmlInclude(typeof(SendMouseClickCommand))]
    [XmlInclude(typeof(ExcelCreateDataSetCommand))]
    [XmlInclude(typeof(DatabaseRunQueryCommand))]
    [XmlInclude(typeof(BeginNumberOfTimesLoopCommand))]
    [XmlInclude(typeof(BeginListLoopCommand))]
    [XmlInclude(typeof(BeginContinousLoopCommand))]
    [XmlInclude(typeof(SequenceCommand))]
    [XmlInclude(typeof(StopTaskCommand))]
    [XmlInclude(typeof(RunTaskCommand))]
    [XmlInclude(typeof(WriteTextFileCommand))]
    [XmlInclude(typeof(ReadTextFileCommand))]
    [XmlInclude(typeof(MoveFileCommand))]
    [XmlInclude(typeof(DeleteFileCommand))]
    [XmlInclude(typeof(RenameFileCommand))]
    [XmlInclude(typeof(WaitForFileToExistCommand))]
    [XmlInclude(typeof(RunCustomCodeCommand))]
    [XmlInclude(typeof(DateCalculationCommand))]
    [XmlInclude(typeof(RegExExtractorCommand))]
    [XmlInclude(typeof(TextExtractorCommand))]
    [XmlInclude(typeof(FormatDataCommand))]
    [XmlInclude(typeof(LogDataCommand))]
    [XmlInclude(typeof(StringReplaceCommand))]
    [XmlInclude(typeof(ExecuteDLLCommand))]
    [XmlInclude(typeof(ParseJsonCommand))]
    [XmlInclude(typeof(SetEngineDelayCommand))]
    [XmlInclude(typeof(PDFTextExtractionCommand))]
    [XmlInclude(typeof(UserInputCommand))]
    [XmlInclude(typeof(GetWordCountCommand))]
    [XmlInclude(typeof(GetWordLengthCommand))]
    [XmlInclude(typeof(HTMLInputCommand))]
    [XmlInclude(typeof(UploadDataCommand))]
    [XmlInclude(typeof(GetDataCommand))]

    public abstract class ScriptCommand
    {
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
        public List<Control> RenderedControls { get; set; }

        public ScriptCommand()
        {
            this.DisplayForeColor = System.Drawing.Color.SteelBlue;
            this.CommandEnabled = false;
            this.DefaultPause = 0;
            this.IsCommented = false;
            this.CustomRendering = false;
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
            return SelectionName;
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
    }
}