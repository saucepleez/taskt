//Copyright (c) 2019 Jason Bayldon
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.CustomControls
{
    #region Custom UI Components

    public class UITabControl : TabControl
    {

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    DrawControl(e.Graphics);
        //}

        //internal void DrawControl(Graphics g)
        //{
        //    if (!Visible)
        //        return;

        //    Brush br = new SolidBrush(Color.Black);
        //    Brush brTab = new SolidBrush(Color.Orange);

        //    Rectangle TabControlArea = ClientRectangle;
        //    Rectangle TabArea = DisplayRectangle;

        //    g.FillRectangle(br, TabControlArea);
        //    g.FillRectangle(brTab, TabArea);

        //    br.Dispose();
        //    brTab.Dispose();

        //    //for (int i = 0; i < TabCount; i++)
        //    //    DrawTab(g, TabPages[i], i, false);

        //    //if (_mouseTabIndex != null && _mouseTabIndex != _mouseTabIndexSave && _mouseTabIndex != SelectedIndex)
        //    //    DrawTab(g, TabPages[(int)_mouseTabIndex], (int)_mouseTabIndex, true);

        //    //_mouseTabIndexSave = _mouseTabIndex;
        //}

        //internal void DrawTab(Graphics g, TabPage tabPage, int nIndex, bool mouseOverTab)
        //{
        //    //var recBounds = GetTabRect(nIndex);

        //    //SetBounds(ref recBounds);
        //    //var pt = SetPointsForTabFill(recBounds);

        //    //DrawTabBounds(g, recBounds);

        //    //FillTabl(g, recBounds, pt, false);

        //    //DrawTabSeparators(g, recBounds, nIndex, 0 /*y-bottom*/);

        //    //if (SelectedIndex == nIndex)
        //    //{
        //    //    DrawTabGradient(g, recBounds, pt, nIndex, 0/*width*/, 1/*height*/);
        //    //    DrawTabSeparators(g, recBounds, nIndex, 1 /*y-bottom*/);
        //    //}

        //    //if (mouseOverTab)
        //    //    DrawTabGradient(g, recBounds, pt, nIndex, -2/*width*/, 0/*height*/);

        //    //DrawText(g, recBounds, tabPage.Text);
        //}

        //private void DrawText(Graphics g, Rectangle recBounds, string text)
        //{
        //    var strFormat = new StringFormat();
        //    strFormat.Alignment = strFormat.LineAlignment = StringAlignment.Center;

        //    g.TextRenderingHint =
        //        System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

        //    //var fnt = new Font(MsFonts.familyPTSans, 8F, FontStyle.Regular,  GraphicsUnit.Point, (byte)204);
        //    var fnt = new Font("Arial", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));

        //    RectangleF tabTextArea = recBounds;
        //    var br = new SolidBrush(Color.Black);
        //    g.DrawString(text, fnt, br, tabTextArea);

        //    br.Dispose();
        //    strFormat.Dispose();
        //    fnt.Dispose();
        //}
    }

    public partial class UIPanel : Panel
    {
        private taskt.Core.Theme _Theme = new taskt.Core.Theme();
        public taskt.Core.Theme Theme
        {
            get { return _Theme; }
            set { _Theme = value; }
        }

        protected override void OnPaint(PaintEventArgs e)
        {


            var brush = this.Theme.CreateGradient(this.ClientRectangle);
            e.Graphics.FillRectangle(brush, this.ClientRectangle);


            base.OnPaint(e);
        }
    }

    public partial class UIPictureButton : PictureBox
    {
        private bool isMouseOver;
        public bool IsMouseOver
        {
            get
            {
                return isMouseOver;
            }
            set
            {
                this.isMouseOver = value;
                this.Invalidate();
            }
        }
        private string displayText;
        public string DisplayText
        {
            get
            {
                return displayText;
            }
            set
            {
                this.displayText = value;
                this.Invalidate();
            }
        }
        public override string Text
        {
            get
            {
                return displayText;
            }
            set
            {
                this.displayText = value;
                this.Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            if (IsMouseOver)
            {
                this.Cursor = Cursors.Hand;
                this.BackColor = Color.Transparent;
            }
            else
            {
                this.Cursor = Cursors.Arrow;
                this.BackColor = Color.Transparent;
            }

            if (this.Image != null)
                e.Graphics.DrawImage(this.Image, (this.Width / 2) - 16, 3, 32, 32);

            if (DisplayText != null)
            {
                var stringSize = e.Graphics.MeasureString(DisplayText, new Font("Segoe UI Bold", 8, FontStyle.Bold), 200);
                e.Graphics.DrawString(DisplayText, new Font("Segoe UI", 8, FontStyle.Bold), new SolidBrush(DisplayTextBrush), ((this.Width / 2) - (stringSize.Width / 2)), this.Height - 14);
            }
        }
        private Color displayTextBrush;
        public Color DisplayTextBrush
        {
            get
            {
                return displayTextBrush;
            }
            set
            {
                displayTextBrush = value;
                this.Invalidate();
            }
        }
        public UIPictureButton()
        {
            this.Image = Properties.Resources.logo;
            this.DisplayTextBrush = Color.White;
            this.Size = new Size(48, 48);
            this.DisplayText = "Text";
            this.Font = new Font("Segoe UI", 8, FontStyle.Bold);
            this.MouseEnter += UIPictureButton_MouseEnter;
            this.MouseLeave += UIPictureButton1_MouseLeave;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var parms = base.CreateParams;
                parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }


        private void UIPictureButton_MouseEnter(object sender, System.EventArgs e)
        {
            this.IsMouseOver = true;
        }
        private void UIPictureButton1_MouseLeave(object sender, System.EventArgs e)
        {
            this.IsMouseOver = false;
        }
    }

    public class UIElement
    {
        public string AutomationID { get; set; }
        public string ControlName { get; set; }
        public string ControlType { get; set; }
    }

    public class UIListView : ListView
    {
        public UIListView()
        {
            this.DoubleBuffered = true;
        }
        public event ScrollEventHandler Scroll;
        protected virtual void OnScroll(ScrollEventArgs e)
        {
            ScrollEventHandler handler = this.Scroll;
            if (handler != null) handler(this, e);
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == 0x115)
            { // Trap WM_VSCROLL
                OnScroll(new ScrollEventArgs((ScrollEventType)(m.WParam.ToInt32() & 0xffff), 0));
            }
        }


    }

    public class UISplitContainer : SplitContainer
    {
        public UISplitContainer()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            MethodInfo mi = typeof(Control).GetMethod("SetStyle",
              BindingFlags.NonPublic | BindingFlags.Instance);
            object[] args = new object[] { ControlStyles.OptimizedDoubleBuffer, true };
            mi.Invoke(this.Panel1, args);
            mi.Invoke(this.Panel2, args);
        }
    }

    public class UITreeView : TreeView
    {
        [System.Runtime.InteropServices.DllImport("uxtheme.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hwnd, string pszSubAppName, string pszSubIdList);
        public UITreeView()
        {
            this.DoubleBuffered = true;
            SetWindowTheme(this.Handle, "explorer", null);
        }
    }

    public class UIGroupBox : GroupBox
    {
        public UIGroupBox()
        {
            this.DoubleBuffered = true;
            this.TitleBackColor = Color.SteelBlue;
            this.TitleForeColor = Color.White;
            this.TitleFont = new Font(this.Font.FontFamily, Font.Size, FontStyle.Bold);
            this.BackColor = Color.Transparent;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            GroupBoxRenderer.DrawParentBackground(e.Graphics, this.ClientRectangle, this);
            var rect = ClientRectangle;

            SolidBrush backColorBrush = new SolidBrush(TitleBackColor);
            e.Graphics.FillRectangle(backColorBrush, 0, 0, this.Width, 18);
            backColorBrush.Dispose();

            TextRenderer.DrawText(e.Graphics, Text, TitleFont, new Point(2, 2), TitleForeColor);
            //ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.SteelBlue, ButtonBorderStyle.None);
        }
        public Color TitleBackColor { get; set; }
        public HatchStyle TitleHatchStyle { get; set; }
        public Font TitleFont { get; set; }
        public Color TitleForeColor { get; set; }
    }

    public class UIFlowLayoutPanel : FlowLayoutPanel
    {
        public UIFlowLayoutPanel()
        {
            this.DoubleBuffered = true;
        }
    }

    public class UIPictureBox : PictureBox
    {
        private string encodedimage;

        public string EncodedImage
        {
            get
            {
                return encodedimage;
            }
            set
            {
                encodedimage = value;
            }
        }
    }

    public class UIMenuStrip : MenuStrip
    {
        public UIMenuStrip()
        {
            //this.Renderer = new UIMenuStripRenderer();

            var renderer = new ToolStripProfessionalRenderer(new UIMenuStripColorTable());
            renderer.RenderMenuItemBackground += Renderer_RenderMenuItemBackground;
            this.Renderer = renderer;

        }

        private void Renderer_RenderMenuItemBackground(object sender, ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            Color c = e.Item.Selected ? Color.FromArgb(59, 59, 59) : Color.FromArgb(30, 30, 30);
            using (SolidBrush brush = new SolidBrush(c))
                e.Graphics.FillRectangle(brush, rc);
        }
    }

    public class UIMenuStripColorTable : ProfessionalColorTable
    {
      
        public override Color MenuItemPressedGradientBegin
        {
            get { return Color.FromArgb(59, 59, 59); }
        }
        public override Color MenuItemPressedGradientMiddle
        {
            get { return Color.FromArgb(59, 59, 59); }
        }
        public override Color MenuItemPressedGradientEnd
        {
            get { return Color.FromArgb(59, 59, 59); }
        }
        
        public override Color MenuItemSelected
        {
            get { return Color.FromArgb(59,59,59); }
        }
        public override Color ToolStripDropDownBackground
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color MenuBorder
        {
            get { return Color.FromArgb(30,30,30); }
        }

        public override Color ImageMarginGradientBegin
        {
            get { return Color.FromArgb(30, 30, 30); }
        }
        public override Color ImageMarginGradientMiddle
        {
            get { return Color.FromArgb(30, 30, 30); }
        }
        public override Color ImageMarginGradientEnd
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color ButtonSelectedHighlight
        {
            get { return Color.FromArgb(59, 59, 59); }
        }

    }

    #endregion Custom UI Components
}
namespace taskt.UI
{
    public static class Images
    {
        //private static Dictionary<string, Image> imageList = new Dictionary<string, Image>();
        private static ImageList uiImages = new ImageList();
        private static Dictionary<string, string> imageCommandTable = new Dictionary<string, string>()
        {
            // API
            {"ExecuteDLLCommand", "taskt.Properties.Resources.command_run_code"},
            {"RESTCommand", "taskt.Properties.Resources.command_run_code"},
            {"HTTPRequestCommand", "taskt.Properties.Resources.command_web"},
            {"HTTPQueryResultCommand", "taskt.Properties.Resources.command_search"},

            // Data
            {"ConvertListToJSONCommand", "taskt.Properties.Resources.command_function"},
            {"DateCalculationCommand", "taskt.Properties.Resources.command_function"},
            {"FormatDataCommand", "taskt.Properties.Resources.command_function"},
            {"GetListCountCommand", "taskt.Properties.Resources.command_function"},
            {"GetListItemCommand", "taskt.Properties.Resources.command_function"},
            {"GetWordLengthCommand", "taskt.Properties.Resources.command_function"},
            {"GetWordCountCommand", "taskt.Properties.Resources.command_function"},
            {"LogDataCommand", "taskt.Properties.Resources.command_files"},
            {"MathCalculationCommand", "taskt.Properties.Resources.command_function"},
            {"ModifyVariableCommand", "taskt.Properties.Resources.command_function"},
            {"ParseDatasetRowCommand", "taskt.Properties.Resources.command_function"},
            {"ParseJSONArrayCommand", "taskt.Properties.Resources.command_parse"},
            {"ParseJsonCommand", "taskt.Properties.Resources.command_parse"},
            {"ParseJsonModelCommand", "taskt.Properties.Resources.command_parse"},
            {"PDFTextExtractionCommand", "taskt.Properties.Resources.command_function"},
            {"RegExExtractorCommand", "taskt.Properties.Resources.command_function"},
            {"StringReplaceCommand", "taskt.Properties.Resources.command_string"},
            {"StringSplitCommand", "taskt.Properties.Resources.command_string"},
            {"StringSubstringCommand", "taskt.Properties.Resources.command_string"},
            {"TextExtractorCommand", "taskt.Properties.Resources.command_function"},

            // Database
            {"DatabaseDefineConnectionCommand", "taskt.Properties.Resources.command_database"},
            {"DatabaseExecuteQueryCommand", "taskt.Properties.Resources.command_database"},

            // DataTable
            {"LoadTaskCommand", "taskt.Properties.Resources.command_start_process"},
            {"UnloadTaskCommand", "taskt.Properties.Resources.command_stop_process"},
            {"ExcelAddWorksheetCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"AddDataRowCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"CreateDataTableCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"FilterDataTableCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"GetDataRowCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"GetDataRowCountCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"GetDataRowValueCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"RemoveDataRowCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"WriteDataRowValueCommand", "taskt.Properties.Resources.command_spreadsheet"},

            // Dictionary
            {"AddDictionaryCommand", "taskt.Properties.Resources.command_dictionary"},
            {"CreateDictionaryCommand", "taskt.Properties.Resources.command_dictionary"},
            {"GetDictionaryValueCommand", "taskt.Properties.Resources.command_dictionary"},
            {"LoadDictionaryCommand", "taskt.Properties.Resources.command_dictionary"},

            // Engine
            {"ErrorHandlingCommand", "taskt.Properties.Resources.command_error"},
            {"GetDataCommand", "taskt.Properties.Resources.command_server"},  // get bot data
            {"PauseCommand", "taskt.Properties.Resources.command_pause"},
            {"SetEngineDelayCommand", "taskt.Properties.Resources.command_pause"},
            {"ShowEngineContextCommand", "taskt.Properties.Resources.command_window"},
            {"SetEnginePreferenceCommand", "taskt.Properties.Resources.command_window"},
            {"StopwatchCommand", "taskt.Properties.Resources.command_stopwatch"},
            {"UploadDataCommand", "taskt.Properties.Resources.command_server"},   // upload bot store

            // Error
            {"CatchExceptionCommand", "taskt.Properties.Resources.command_try"},
            {"EndTryCommand", "taskt.Properties.Resources.command_try"},
            {"FinallyCommand", "taskt.Properties.Resources.command_try"},
            {"ThrowExceptionCommand", "taskt.Properties.Resources.command_try"},
            {"TryCommand", "taskt.Properties.Resources.command_try"},

            // Excel
            {"ExcelActivateSheetCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelAddWorkbookCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelAppendCellCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelAppendRowCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelCloseApplicationCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelCreateDatasetCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelCreateApplicationCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelDeleteCellCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelDeleteRowCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGetCellCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGetLastRowCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGetRangeCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGetRangeCommandAsDT", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGoToCellCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelOpenWorkbookCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelRunMacroCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelSaveCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelSaveAsCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelSetCellCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelSplitRangeByColumnCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelWriteRangeCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelWriteRowCommand", "taskt.Properties.Resources.command_spreadsheet"},

            // File
            {"CheckFileExistsCommand", "taskt.Properties.Resources.command_files"},
            {"DeleteFileCommand", "taskt.Properties.Resources.command_files"},
            {"ExtractFileCommand", "taskt.Properties.Resources.command_files"},
            {"GetFilesCommand", "taskt.Properties.Resources.command_files"},
            {"MoveFileCommand", "taskt.Properties.Resources.command_files"},
            {"RenameFileCommand", "taskt.Properties.Resources.command_files"},
            {"WaitForFileToExistCommand", "taskt.Properties.Resources.command_files"},

            // Folder
            {"CheckFolderExistsCommand", "taskt.Properties.Resources.command_files"},
            {"CreateFolderCommand", "taskt.Properties.Resources.command_files"},
            {"DeleteFolderCommand", "taskt.Properties.Resources.command_files"},
            {"GetFoldersCommand", "taskt.Properties.Resources.command_files"},
            {"MoveFolderCommand", "taskt.Properties.Resources.command_files"},
            {"RenameFolderCommand", "taskt.Properties.Resources.command_files"},

            // IE
            {"IEBrowserCloseCommand", "taskt.Properties.Resources.command_window_close"},
            {"IEBrowserCreateCommand", "taskt.Properties.Resources.command_web"},
            {"IEBrowserElementCommand", "taskt.Properties.Resources.command_web"},
            {"WebBrowserFindBrowserCommand", "taskt.Properties.Resources.command_web"},
            {"IEBrowserNavigateCommand", "taskt.Properties.Resources.command_web"},

            // If
            {"BeginIfCommand", "taskt.Properties.Resources.command_begin_if"},
            {"BeginMultiIfCommand", "taskt.Properties.Resources.command_begin_multi_if"},
            {"ElseCommand", "taskt.Properties.Resources.command_else"},
            {"EndIfCommand", "taskt.Properties.Resources.command_end_if"},

            // Image
            {"ImageRecognitionCommand", "taskt.Properties.Resources.command_camera"},
            {"OCRCommand", "taskt.Properties.Resources.command_camera"},
            {"ScreenshotCommand", "taskt.Properties.Resources.command_camera"},

            // Input
            {"FileDialogCommand", "taskt.Properties.Resources.command_input"},
            {"FolderDialogCommand", "taskt.Properties.Resources.command_input"},
            {"HTMLInputCommand", "taskt.Properties.Resources.command_input"},
            {"UserInputCommand", "taskt.Properties.Resources.command_input"}, // prompt
            {"SendAdvancedKeyStrokesCommand", "taskt.Properties.Resources.command_input"},
            {"SendHotkeyCommand", "taskt.Properties.Resources.command_input"},
            {"SendKeysCommand", "taskt.Properties.Resources.command_input"},
            {"SendMouseMoveCommand", "taskt.Properties.Resources.command_input"},
            {"SendMouseClickCommand", "taskt.Properties.Resources.command_input"},
            {"UIAutomationCommand", "taskt.Properties.Resources.command_input"},

            // Loop
            {"BeginLoopCommand", "taskt.Properties.Resources.command_startloop"},
            {"BeginMultiLoopCommand", "taskt.Properties.Resources.command_startloop"},
            {"EndLoopCommand", "taskt.Properties.Resources.command_endloop"},
            {"ExitLoopCommand", "taskt.Properties.Resources.command_exitloop"},
            {"BeginContinousLoopCommand", "taskt.Properties.Resources.command_startloop"},
            {"BeginListLoopCommand", "taskt.Properties.Resources.command_startloop"},
            {"BeginNumberOfTimesLoopCommand", "taskt.Properties.Resources.command_startloop"},
            {"NextLoopCommand", "taskt.Properties.Resources.command_nextloop"},

            // Misc
            {"CommentCommand", "taskt.Properties.Resources.command_comment"},
            {"EncryptionCommand", "taskt.Properties.Resources.command_input"},
            {"ClipboardGetTextCommand", "taskt.Properties.Resources.command_files"},
            {"PingCommand", "taskt.Properties.Resources.command_web"},
            {"SMTPSendEmailCommand", "taskt.Properties.Resources.command_smtp"},
            {"SequenceCommand", "taskt.Properties.Resources.command_sequence"},
            {"ClipboardSetTextCommand", "taskt.Properties.Resources.command_files"},
            {"MessageBoxCommand", "taskt.Properties.Resources.command_comment"},

            // NLG
            {"NLGCreateInstanceCommand", "taskt.Properties.Resources.command_nlg"},
            {"NLGGeneratePhraseCommand", "taskt.Properties.Resources.command_nlg"},
            {"NLGSetParameterCommand", "taskt.Properties.Resources.command_nlg"},

            // Outlook
            {"OutlookDeleteEmailsCommand", "taskt.Properties.Resources.command_smtp"},
            {"OutlookForwardEmailsCommand", "taskt.Properties.Resources.command_smtp"},
            {"OutlookGetEmailsCommand", "taskt.Properties.Resources.command_smtp"},
            {"OutlookMoveEmailsCommand", "taskt.Properties.Resources.command_smtp"},
            {"OutlookReplyToEmailsCommand", "taskt.Properties.Resources.command_smtp"},
            {"OutlookEmailCommand", "taskt.Properties.Resources.command_smtp"},

            // Program
            {"RunCustomCodeCommand", "taskt.Properties.Resources.command_script"},
            {"RunScriptCommand", "taskt.Properties.Resources.command_script"},
            {"StartProcessCommand", "taskt.Properties.Resources.command_start_process"},
            {"StopProcessCommand", "taskt.Properties.Resources.command_stop_process"},

            // Regex
            {"GetRegexMatchesCommand", "taskt.Properties.Resources.command_function"},

            // Remote
            {"RemoteAPICommand", "taskt.Properties.Resources.command_remote"},
            {"RemoteTaskCommand", "taskt.Properties.Resources.command_remote"},

            // System
            {"EnvironmentVariableCommand", "taskt.Properties.Resources.command_system"},
            {"RemoteDesktopCommand", "taskt.Properties.Resources.command_system"},
            {"OSVariableCommand", "taskt.Properties.Resources.command_system"},
            {"SystemActionCommand", "taskt.Properties.Resources.command_script"},

            // Task
            {"RunTaskCommand", "taskt.Properties.Resources.command_start_process"},
            {"StopTaskCommand", "taskt.Properties.Resources.command_stop_process"},

            // Text
            {"ReadTextFileCommand", "taskt.Properties.Resources.command_files"},
            {"WriteTextFileCommand", "taskt.Properties.Resources.command_files"},

            // Variable
            {"AddToVariableCommand", "taskt.Properties.Resources.command_function"},
            {"AddVariableCommand", "taskt.Properties.Resources.command_function"},
            {"VariableCommand", "taskt.Properties.Resources.command_function"},
            {"SetVariableIndexCommand", "taskt.Properties.Resources.command_function"},

            // Web
            {"SeleniumBrowserCloseCommand", "taskt.Properties.Resources.command_window_close"},
            {"SeleniumBrowserCreateCommand", "taskt.Properties.Resources.command_web"},
            {"SeleniumBrowserElementActionCommand", "taskt.Properties.Resources.command_web"},
            {"SeleniumBrowserExecuteScriptCommand", "taskt.Properties.Resources.command_script"},
            {"SeleniumBrowserInfoCommand", "taskt.Properties.Resources.command_web"},
            {"SeleniumBrowserNavigateBackCommand", "taskt.Properties.Resources.command_web"},
            {"SeleniumBrowserNavigateForwardCommand", "taskt.Properties.Resources.command_web"},
            {"SeleniumBrowserNavigateURLCommand", "taskt.Properties.Resources.command_web"},
            {"SeleniumBrowserRefreshCommand", "taskt.Properties.Resources.command_web"},
            {"SeleniumBrowserSwitchFrameCommand", "taskt.Properties.Resources.command_window"},
            {"SeleniumBrowserSwitchWindowCommand", "taskt.Properties.Resources.command_window"},
            {"SeleniumBrowserTakeScreenshotCommand", "taskt.Properties.Resources.command_web"},

            // Window
            {"ActivateWindowCommand", "taskt.Properties.Resources.command_window"},
            {"CheckWindowNameExistsCommand", "taskt.Properties.Resources.command_window"},
            {"CloseWindowCommand", "taskt.Properties.Resources.command_window_close"},
            {"GetWindowNamesCommand", "taskt.Properties.Resources.command_window"},
            {"MoveWindowCommand", "taskt.Properties.Resources.command_window"},
            {"ResizeWindowCommand", "taskt.Properties.Resources.command_window"},
            {"SetWindowStateCommand", "taskt.Properties.Resources.command_window"},
            {"WaitForWindowCommand", "taskt.Properties.Resources.command_window"},

            // Word
            {"WordAddDocumentCommand", "taskt.Properties.Resources.command_files"},
            {"WordAppendDataTableCommand", "taskt.Properties.Resources.command_files"},
            {"WordAppendImageCommand", "taskt.Properties.Resources.command_files"},
            {"WordAppendTextCommand", "taskt.Properties.Resources.command_files"},
            {"WordCloseApplicationCommand", "taskt.Properties.Resources.command_files"},
            {"WordCreateApplicationCommand", "taskt.Properties.Resources.command_files"},
            {"WordExportToPDFCommand", "taskt.Properties.Resources.command_files"},
            {"WordOpenDocumentCommand", "taskt.Properties.Resources.command_files"},
            {"WordReadDocumentCommand", "taskt.Properties.Resources.command_files"},
            {"WordReplaceTextCommand", "taskt.Properties.Resources.command_files"},
            {"WordSaveCommand", "taskt.Properties.Resources.command_files"},
            {"WordSaveAsCommand", "taskt.Properties.Resources.command_files"},

            //// NOTHING ///
            {"BeginExcelDatasetLoopCommand", "taskt.Properties.Resources.command_startloop"},
            {"ThickAppClickItemCommand", "taskt.Properties.Resources.command_input"},
            {"ThickAppGetTextCommand", "taskt.Properties.Resources.command_search"},
            {"Setcommand_windowtateCommand", "taskt.Properties.Resources.command_window"},
            {"_NotFoundCommand", "taskt.Properties.Resources.command_files"},
        };

        public static Dictionary<string, Image> UIImageDictionary()
        {
            //var uiImages = new Dictionary<string, Image>();
            //Dictionary<string, Image> uiImages = imageList;
            Dictionary<string, Image> uiImages = new Dictionary<string, Image>();

            // API
            uiImages.Add("ExecuteDLLCommand", taskt.Properties.Resources.command_run_code);
            uiImages.Add("RESTCommand", taskt.Properties.Resources.command_run_code);
            uiImages.Add("HTTPRequestCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("HTTPQueryResultCommand", taskt.Properties.Resources.command_search);

            // Data
            uiImages.Add("DateCalculationCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("FormatDataCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("GetListCountCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("GetListItemCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("GetWordLengthCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("GetWordCountCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("LogDataCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("MathCalculationCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("ModifyVariableCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("ParseDatasetRowCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("ParseJsonArrayCommand", taskt.Properties.Resources.command_parse);
            uiImages.Add("ParseJsonCommand", taskt.Properties.Resources.command_parse);
            uiImages.Add("ParseJsonModelCommand", taskt.Properties.Resources.command_parse);
            uiImages.Add("PDFTextExtractionCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("RegExExtractorCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("StringReplaceCommand", taskt.Properties.Resources.command_string);
            uiImages.Add("StringSplitCommand", taskt.Properties.Resources.command_string);
            uiImages.Add("StringSubstringCommand", taskt.Properties.Resources.command_string);
            uiImages.Add("TextExtractorCommand", taskt.Properties.Resources.command_function);

            // Database
            uiImages.Add("DatabaseDefineConnectionCommand", taskt.Properties.Resources.command_database);
            uiImages.Add("DatabaseExecuteQueryCommand", taskt.Properties.Resources.command_database);

            // DataTable
            uiImages.Add("LoadTaskCommand", taskt.Properties.Resources.command_start_process);
            uiImages.Add("UnloadTaskCommand", taskt.Properties.Resources.command_stop_process);
            uiImages.Add("ExcelAddWorksheetCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("AddDataRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("CreateDataTableCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("FilterDataTableCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCountCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowValueCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("RemoveDataRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("WriteDataRowValueCommand", taskt.Properties.Resources.command_spreadsheet);

            // Dictionary
            uiImages.Add("AddDictionaryCommand", taskt.Properties.Resources.command_dictionary);
            uiImages.Add("CreateDictionaryCommand", taskt.Properties.Resources.command_dictionary);
            uiImages.Add("GetDictionaryValueCommand", taskt.Properties.Resources.command_dictionary);
            uiImages.Add("LoadDictionaryCommand", taskt.Properties.Resources.command_dictionary);

            // Engine
            uiImages.Add("ErrorHandlingCommand", taskt.Properties.Resources.command_error);
            uiImages.Add("GetDataCommand", taskt.Properties.Resources.command_server);  // get bot data
            uiImages.Add("PauseCommand", taskt.Properties.Resources.command_pause);
            uiImages.Add("SetEngineDelayCommand", taskt.Properties.Resources.command_pause);
            uiImages.Add("ShowEngineContextCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("SetEnginePreferenceCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("StopwatchCommand", taskt.Properties.Resources.command_stopwatch);
            uiImages.Add("UploadDataCommand", taskt.Properties.Resources.command_server);   // upload bot store

            // Error
            uiImages.Add("CatchExceptionCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("EndTryCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("FinallyCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("ThrowExceptionCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("TryCommand", taskt.Properties.Resources.command_try);

            // Excel
            uiImages.Add("ExcelActivateSheetCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAddWorkbookCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAppendCellCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAppendRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCloseApplicationCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCreateDatasetCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCreateApplicationCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteCellCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetCellCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetLastRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetRangeCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetRangeCommandAsDT", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGoToCellCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelOpenWorkbookCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelRunMacroCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveAsCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSetCellCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSplitRangeByColumnCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelWriteRangeCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelWriteRowCommand", taskt.Properties.Resources.command_spreadsheet);

            // File
            uiImages.Add("DeleteFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("ExtractFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("GetFilesCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("MoveFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("RenameFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WaitForFileToExistCommand", taskt.Properties.Resources.command_files);

            // Folder
            uiImages.Add("CreateFolderCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("DeleteFolderCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("GetFoldersCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("MoveFolderCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("RenameFolderCommand", taskt.Properties.Resources.command_files);

            // IE
            uiImages.Add("IEBrowserCloseCommand", taskt.Properties.Resources.command_window_close);
            uiImages.Add("IEBrowserCreateCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("IEBrowserElementCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("WebBrowserFindBrowserCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("IEBrowserNavigateCommand", taskt.Properties.Resources.command_web);

            // If
            uiImages.Add("BeginIfCommand", taskt.Properties.Resources.command_begin_if);
            uiImages.Add("BeginMultiIfCommand", taskt.Properties.Resources.command_begin_multi_if);
            uiImages.Add("ElseCommand", taskt.Properties.Resources.command_else);
            uiImages.Add("EndIfCommand", taskt.Properties.Resources.command_end_if);

            // Image
            uiImages.Add("ImageRecognitionCommand", taskt.Properties.Resources.command_camera);
            uiImages.Add("OCRCommand", taskt.Properties.Resources.command_camera);
            uiImages.Add("ScreenshotCommand", taskt.Properties.Resources.command_camera);

            // Input
            uiImages.Add("HTMLInputCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("UserInputCommand", taskt.Properties.Resources.command_input); // prompt
            uiImages.Add("SendAdvancedKeyStrokesCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("SendHotkeyCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("SendKeysCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("SendMouseMoveCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("SendMouseClickCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("UIAutomationCommand", taskt.Properties.Resources.command_input);

            // Loop
            uiImages.Add("BeginLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("BeginMultiLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("EndLoopCommand", taskt.Properties.Resources.command_endloop);
            uiImages.Add("ExitLoopCommand", taskt.Properties.Resources.command_exitloop);
            uiImages.Add("BeginContinousLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("BeginListLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("BeginNumberOfTimesLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("NextLoopCommand", taskt.Properties.Resources.command_nextloop);

            // Misc
            uiImages.Add("CommentCommand", taskt.Properties.Resources.command_comment);
            uiImages.Add("EncryptionCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("ClipboardGetTextCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("PingCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SMTPSendEmailCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("SequenceCommand", taskt.Properties.Resources.command_sequence);
            uiImages.Add("ClipboardSetTextCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("MessageBoxCommand", taskt.Properties.Resources.command_comment);

            // NLG
            uiImages.Add("NLGCreateInstanceCommand", taskt.Properties.Resources.command_nlg);
            uiImages.Add("NLGGeneratePhraseCommand", taskt.Properties.Resources.command_nlg);
            uiImages.Add("NLGSetParameterCommand", taskt.Properties.Resources.command_nlg);

            // Outlook
            uiImages.Add("OutlookDeleteEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookForwardEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookGetEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookMoveEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookReplyToEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookEmailCommand", taskt.Properties.Resources.command_smtp);

            // Program
            uiImages.Add("RunCustomCodeCommand", taskt.Properties.Resources.command_script);
            uiImages.Add("RunScriptCommand", taskt.Properties.Resources.command_script);
            uiImages.Add("StartProcessCommand", taskt.Properties.Resources.command_start_process);
            uiImages.Add("StopProcessCommand", taskt.Properties.Resources.command_stop_process);

            // Regex
            uiImages.Add("GetRegexMatchesCommand", taskt.Properties.Resources.command_function);

            // Remote
            uiImages.Add("RemoteAPICommand", taskt.Properties.Resources.command_remote);
            uiImages.Add("RemoteTaskCommand", taskt.Properties.Resources.command_remote);

            // System
            uiImages.Add("EnvironmentVariableCommand", taskt.Properties.Resources.command_system);
            uiImages.Add("RemoteDesktopCommand", taskt.Properties.Resources.command_system);
            uiImages.Add("OSVariableCommand", taskt.Properties.Resources.command_system);
            uiImages.Add("SystemActionCommand", taskt.Properties.Resources.command_script);

            // Task
            uiImages.Add("RunTaskCommand", taskt.Properties.Resources.command_start_process);
            uiImages.Add("StopTaskCommand", taskt.Properties.Resources.command_stop_process);

            // Text
            uiImages.Add("ReadTextFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WriteTextFileCommand", taskt.Properties.Resources.command_files);

            // Variable
            uiImages.Add("AddToVariableCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("AddVariableCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("VariableCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("SetVariableIndexCommand", taskt.Properties.Resources.command_function);

            // Web
            uiImages.Add("SeleniumBrowserCloseCommand", taskt.Properties.Resources.command_window_close);
            uiImages.Add("SeleniumBrowserCreateCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserElementActionCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserExecuteScriptCommand", taskt.Properties.Resources.command_script);
            uiImages.Add("SeleniumBrowserInfoCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateBackCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateForwardCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateURLCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserRefreshCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserSwitchFrameCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("SeleniumBrowserSwitchWindowCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("SeleniumBrowserTakeScreenshotCommand", taskt.Properties.Resources.command_web);

            // Window
            uiImages.Add("ActivateWindowCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("CloseWindowCommand", taskt.Properties.Resources.command_window_close);
            uiImages.Add("MoveWindowCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("ResizeWindowCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("SetWindowStateCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("WaitForWindowCommand", taskt.Properties.Resources.command_window);

            // Word
            uiImages.Add("WordAddDocumentCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordAppendDataTableCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordAppendImageCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordAppendTextCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordCloseApplicationCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordCreateApplicationCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordExportToPDFCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordOpenDocumentCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordReadDocumentCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordReplaceTextCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordSaveCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordSaveAsCommand", taskt.Properties.Resources.command_files);

            //// NOTHING ///
            uiImages.Add("BeginExcelDatasetLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("ThickAppClickItemCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("ThickAppGetTextCommand", taskt.Properties.Resources.command_search);
            uiImages.Add("Setcommand_windowtateCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("_NotFoundCommand", taskt.Properties.Resources.command_files);

            // release
            //GC.Collect();

            return uiImages;
        }
        public static ImageList UIImageList()
        {
            //Dictionary<string, Image> imageIcons = UIImageDictionary();
            //if (imageList.Count == 0)
            //{
            //    UIImageDictionary();
            //}

            //ImageList uiImages = new ImageList();

            if (uiImages.Images.Count > 0)
            {
                return uiImages;
            }

            uiImages.ImageSize = new Size(16, 16);
            //foreach (var icon in imageIcons)
            //foreach (var icon in imageIcons)
            //{

            //    //var someImage = icon.Value;

            //    //using (Image src = icon.Value)
            //    //using (Bitmap dst = new Bitmap(16, 16))
            //    //using (Graphics g = Graphics.FromImage(dst))
            //    //{
            //    //    g.SmoothingMode = SmoothingMode.AntiAlias;
            //    //    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //    //    g.DrawImage(src, 0, 0, dst.Width, dst.Height);
            //    //    uiImages.Images.Add(icon.Key, dst);
            //    //}

            //    uiImages.Images.Add(icon.Key, icon.Value);
            //}
            uiImages.Images.Add("taskt.Properties.Resources.command_begin_if", taskt.Properties.Resources.command_begin_if);
            uiImages.Images.Add("taskt.Properties.Resources.command_begin_multi_if", taskt.Properties.Resources.command_begin_multi_if);
            uiImages.Images.Add("taskt.Properties.Resources.command_camera", taskt.Properties.Resources.command_camera);
            uiImages.Images.Add("taskt.Properties.Resources.command_comment", taskt.Properties.Resources.command_comment);
            uiImages.Images.Add("taskt.Properties.Resources.command_database", taskt.Properties.Resources.command_database);
            uiImages.Images.Add("taskt.Properties.Resources.command_dictionary", taskt.Properties.Resources.command_dictionary);
            uiImages.Images.Add("taskt.Properties.Resources.command_else", taskt.Properties.Resources.command_else);
            uiImages.Images.Add("taskt.Properties.Resources.command_endloop", taskt.Properties.Resources.command_endloop);
            uiImages.Images.Add("taskt.Properties.Resources.command_end_if", taskt.Properties.Resources.command_end_if);
            uiImages.Images.Add("taskt.Properties.Resources.command_error", taskt.Properties.Resources.command_error);
            uiImages.Images.Add("taskt.Properties.Resources.command_exitloop", taskt.Properties.Resources.command_exitloop);
            uiImages.Images.Add("taskt.Properties.Resources.command_files", taskt.Properties.Resources.command_files);
            uiImages.Images.Add("taskt.Properties.Resources.command_function", taskt.Properties.Resources.command_function);
            uiImages.Images.Add("taskt.Properties.Resources.command_input", taskt.Properties.Resources.command_input);
            uiImages.Images.Add("taskt.Properties.Resources.command_nextloop", taskt.Properties.Resources.command_nextloop);
            uiImages.Images.Add("taskt.Properties.Resources.command_nlg", taskt.Properties.Resources.command_nlg);
            uiImages.Images.Add("taskt.Properties.Resources.command_parse", taskt.Properties.Resources.command_parse);
            uiImages.Images.Add("taskt.Properties.Resources.command_pause", taskt.Properties.Resources.command_pause);
            uiImages.Images.Add("taskt.Properties.Resources.command_remote", taskt.Properties.Resources.command_remote);
            uiImages.Images.Add("taskt.Properties.Resources.command_run_code", taskt.Properties.Resources.command_run_code);
            uiImages.Images.Add("taskt.Properties.Resources.command_script", taskt.Properties.Resources.command_script);
            uiImages.Images.Add("taskt.Properties.Resources.command_search", taskt.Properties.Resources.command_search);
            uiImages.Images.Add("taskt.Properties.Resources.command_sequence", taskt.Properties.Resources.command_sequence);
            uiImages.Images.Add("taskt.Properties.Resources.command_server", taskt.Properties.Resources.command_server);
            uiImages.Images.Add("taskt.Properties.Resources.command_smtp", taskt.Properties.Resources.command_smtp);
            uiImages.Images.Add("taskt.Properties.Resources.command_spreadsheet", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Images.Add("taskt.Properties.Resources.command_startloop", taskt.Properties.Resources.command_startloop);
            uiImages.Images.Add("taskt.Properties.Resources.command_start_process", taskt.Properties.Resources.command_start_process);
            uiImages.Images.Add("taskt.Properties.Resources.command_stopwatch", taskt.Properties.Resources.command_stopwatch);
            uiImages.Images.Add("taskt.Properties.Resources.command_stop_process", taskt.Properties.Resources.command_stop_process);
            uiImages.Images.Add("taskt.Properties.Resources.command_string", taskt.Properties.Resources.command_string);
            uiImages.Images.Add("taskt.Properties.Resources.command_system", taskt.Properties.Resources.command_system);
            uiImages.Images.Add("taskt.Properties.Resources.command_try", taskt.Properties.Resources.command_try);
            uiImages.Images.Add("taskt.Properties.Resources.command_web", taskt.Properties.Resources.command_web);
            uiImages.Images.Add("taskt.Properties.Resources.command_window", taskt.Properties.Resources.command_window);
            uiImages.Images.Add("taskt.Properties.Resources.command_window_close", taskt.Properties.Resources.command_window_close);

            // release
            //imageIcons.Clear();

            return uiImages;
        }
        public static Image ResizeImageFile(Image image)
        {
            using (System.Drawing.Image oldImage = image)
            {
                using (Bitmap newImage = new Bitmap(16, 16, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
                {
                    using (Graphics canvas = Graphics.FromImage(newImage))
                    {
                        canvas.SmoothingMode = SmoothingMode.AntiAlias;
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), new Size(16,16)));
                        return newImage;
                    }
                }
            }
        }

        public static int GetUIImageList(string commandName)
        {
            //return uiImages.Images.IndexOfKey(imageCommandTable[commandName]);
            try
            {
                return uiImages.Images.IndexOfKey(imageCommandTable[commandName]);
            }
            catch (Exception)
            {
                return uiImages.Images.IndexOfKey("taskt.Properties.Resources.command_files");
            }
        }

        public static Image GetUIImage(string commandName)
        {
            ////var uiImageDictionary = UIImageDictionary();
            //if (imageList.Count == 0)
            //{
            //    UIImageDictionary();
            //}

            //Image uiImage;
            //try
            //{
            //    //uiImage = uiImageDictionary[commandName];
            //    uiImage = imageList[commandName];
            //}
            //catch (Exception)
            //{
            //    uiImage = Properties.Resources.command_files;
            //}

            if (uiImages.Images.Count == 0)
            {
                UIImageList();
            }

            Image retImage;
            try
            {
                retImage = uiImages.Images[uiImages.Images.IndexOfKey(imageCommandTable[commandName])];
            }
            catch (Exception)
            {
                retImage = uiImages.Images[uiImages.Images.IndexOfKey("taskt.Properties.Resources.command_files")];
            }

            return retImage;
        }
    }
}