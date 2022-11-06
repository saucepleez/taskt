﻿//Copyright (c) 2019 Jason Bayldon
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
using System.Reflection;
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
            this.Image = robot_worker.Properties.Resources.logo;
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
            get { return Color.FromArgb(59, 59, 59); }
        }
        public override Color ToolStripDropDownBackground
        {
            get { return Color.FromArgb(30, 30, 30); }
        }

        public override Color MenuBorder
        {
            get { return Color.FromArgb(30, 30, 30); }
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
        private static Dictionary<string, Image> imageList = new Dictionary<string, Image>();
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
            {"SetListItemCommand", "taskt.Properties.Resources.command_function"},
            {"RemoveListItemCommand", "taskt.Properties.Resources.command_function"},
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
            {"StringCheckStringCommand", "taskt.Properties.Resources.command_string"},
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
            {"SetDictionaryValueCommand", "taskt.Properties.Resources.command_dictionary"},
            {"RemoveDictionaryValueCommand", "taskt.Properties.Resources.command_dictionary"},
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
            {"ExcelCheckExcelInstanceExistsCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelCloseApplicationCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelCreateDatasetCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelCreateApplicationCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelDeleteCellCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelDeleteRowCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelDeleteWorksheetCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGetCellCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGetCellRCCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGetLastRowCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGetRangeCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGetRangeCommandAsDT", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelGoToCellCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelOpenWorkbookCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelRenameWorksheetCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelRunMacroCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelSaveCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelSaveAsCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelSetCellCommand", "taskt.Properties.Resources.command_spreadsheet"},
            {"ExcelSetCellRCCommand", "taskt.Properties.Resources.command_spreadsheet"},
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
            {"SeleniumBrowserCheckBrowserInstanceExistsCommand", "taskt.Properties.Resources.command_web"},
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
            {"WordCheckWordInstanceExistsCommand", "taskt.Properties.Resources.command_files"},
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
            uiImages.Add("ExecuteDLLCommand", robot_worker.Properties.Resources.command_run_code);
            uiImages.Add("RESTCommand", robot_worker.Properties.Resources.command_run_code);
            uiImages.Add("HTTPRequestCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("HTTPQueryResultCommand", robot_worker.Properties.Resources.command_search);

            // Data
            uiImages.Add("DateCalculationCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("FormatDataCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("GetListCountCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("GetListItemCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("SetListItemCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("RemoveListItemCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("GetWordLengthCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("GetWordCountCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("LogDataCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("MathCalculationCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("ModifyVariableCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("ParseDatasetRowCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("ParseJsonArrayCommand", robot_worker.Properties.Resources.command_parse);
            uiImages.Add("ParseJsonCommand", robot_worker.Properties.Resources.command_parse);
            uiImages.Add("ParseJsonModelCommand", robot_worker.Properties.Resources.command_parse);
            uiImages.Add("PDFTextExtractionCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("RegExExtractorCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("StringReplaceCommand", robot_worker.Properties.Resources.command_string);
            uiImages.Add("StringSplitCommand", robot_worker.Properties.Resources.command_string);
            uiImages.Add("StringSubstringCommand", robot_worker.Properties.Resources.command_string);
            uiImages.Add("TextExtractorCommand", robot_worker.Properties.Resources.command_function);

            // Database
            uiImages.Add("DatabaseDefineConnectionCommand", robot_worker.Properties.Resources.command_database);
            uiImages.Add("DatabaseExecuteQueryCommand", robot_worker.Properties.Resources.command_database);

            // DataTable
            uiImages.Add("LoadTaskCommand", robot_worker.Properties.Resources.command_start_process);
            uiImages.Add("UnloadTaskCommand", robot_worker.Properties.Resources.command_stop_process);
            uiImages.Add("ExcelAddWorksheetCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("AddDataRowCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("CreateDataTableCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("FilterDataTableCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCountCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowValueCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("RemoveDataRowCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("WriteDataRowValueCommand", robot_worker.Properties.Resources.command_spreadsheet);

            // Dictionary
            uiImages.Add("AddDictionaryCommand", robot_worker.Properties.Resources.command_dictionary);
            uiImages.Add("CreateDictionaryCommand", robot_worker.Properties.Resources.command_dictionary);
            uiImages.Add("GetDictionaryValueCommand", robot_worker.Properties.Resources.command_dictionary);
            uiImages.Add("SetDictionaryValueCommand", robot_worker.Properties.Resources.command_dictionary);
            uiImages.Add("RemoveDictionaryValueCommand", robot_worker.Properties.Resources.command_dictionary);
            uiImages.Add("LoadDictionaryCommand", robot_worker.Properties.Resources.command_dictionary);

            // Engine
            uiImages.Add("ErrorHandlingCommand", robot_worker.Properties.Resources.command_error);
            uiImages.Add("GetDataCommand", robot_worker.Properties.Resources.command_server);  // get bot data
            uiImages.Add("PauseCommand", robot_worker.Properties.Resources.command_pause);
            uiImages.Add("SetEngineDelayCommand", robot_worker.Properties.Resources.command_pause);
            uiImages.Add("ShowEngineContextCommand", robot_worker.Properties.Resources.command_window);
            uiImages.Add("SetEnginePreferenceCommand", robot_worker.Properties.Resources.command_window);
            uiImages.Add("StopwatchCommand", robot_worker.Properties.Resources.command_stopwatch);
            uiImages.Add("UploadDataCommand", robot_worker.Properties.Resources.command_server);   // upload bot store

            // Error
            uiImages.Add("CatchExceptionCommand", robot_worker.Properties.Resources.command_try);
            uiImages.Add("EndTryCommand", robot_worker.Properties.Resources.command_try);
            uiImages.Add("FinallyCommand", robot_worker.Properties.Resources.command_try);
            uiImages.Add("ThrowExceptionCommand", robot_worker.Properties.Resources.command_try);
            uiImages.Add("TryCommand", robot_worker.Properties.Resources.command_try);

            // Excel
            uiImages.Add("ExcelActivateSheetCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAddWorkbookCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAppendCellCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAppendRowCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCloseApplicationCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCreateDatasetCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCreateApplicationCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteCellCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteRowCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetCellCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetLastRowCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetRangeCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetRangeCommandAsDT", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGoToCellCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelOpenWorkbookCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelRunMacroCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveAsCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSetCellCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSplitRangeByColumnCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelWriteRangeCommand", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelWriteRowCommand", robot_worker.Properties.Resources.command_spreadsheet);

            // File
            uiImages.Add("DeleteFileCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("ExtractFileCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("GetFilesCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("MoveFileCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("RenameFileCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WaitForFileToExistCommand", robot_worker.Properties.Resources.command_files);

            // Folder
            uiImages.Add("CreateFolderCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("DeleteFolderCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("GetFoldersCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("MoveFolderCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("RenameFolderCommand", robot_worker.Properties.Resources.command_files);

            // IE
            uiImages.Add("IEBrowserCloseCommand", robot_worker.Properties.Resources.command_window_close);
            uiImages.Add("IEBrowserCreateCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("IEBrowserElementCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("WebBrowserFindBrowserCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("IEBrowserNavigateCommand", robot_worker.Properties.Resources.command_web);

            // If
            uiImages.Add("BeginIfCommand", robot_worker.Properties.Resources.command_begin_if);
            uiImages.Add("BeginMultiIfCommand", robot_worker.Properties.Resources.command_begin_multi_if);
            uiImages.Add("ElseCommand", robot_worker.Properties.Resources.command_else);
            uiImages.Add("EndIfCommand", robot_worker.Properties.Resources.command_end_if);

            // Image
            uiImages.Add("ImageRecognitionCommand", robot_worker.Properties.Resources.command_camera);
            uiImages.Add("OCRCommand", robot_worker.Properties.Resources.command_camera);
            uiImages.Add("ScreenshotCommand", robot_worker.Properties.Resources.command_camera);

            // Input
            uiImages.Add("HTMLInputCommand", robot_worker.Properties.Resources.command_input);
            uiImages.Add("UserInputCommand", robot_worker.Properties.Resources.command_input); // prompt
            uiImages.Add("SendAdvancedKeyStrokesCommand", robot_worker.Properties.Resources.command_input);
            uiImages.Add("SendHotkeyCommand", robot_worker.Properties.Resources.command_input);
            uiImages.Add("SendKeysCommand", robot_worker.Properties.Resources.command_input);
            uiImages.Add("SendMouseMoveCommand", robot_worker.Properties.Resources.command_input);
            uiImages.Add("SendMouseClickCommand", robot_worker.Properties.Resources.command_input);
            uiImages.Add("UIAutomationCommand", robot_worker.Properties.Resources.command_input);

            // Loop
            uiImages.Add("BeginLoopCommand", robot_worker.Properties.Resources.command_startloop);
            uiImages.Add("BeginMultiLoopCommand", robot_worker.Properties.Resources.command_startloop);
            uiImages.Add("EndLoopCommand", robot_worker.Properties.Resources.command_endloop);
            uiImages.Add("ExitLoopCommand", robot_worker.Properties.Resources.command_exitloop);
            uiImages.Add("BeginContinousLoopCommand", robot_worker.Properties.Resources.command_startloop);
            uiImages.Add("BeginListLoopCommand", robot_worker.Properties.Resources.command_startloop);
            uiImages.Add("BeginNumberOfTimesLoopCommand", robot_worker.Properties.Resources.command_startloop);
            uiImages.Add("NextLoopCommand", robot_worker.Properties.Resources.command_nextloop);

            // Misc
            uiImages.Add("CommentCommand", robot_worker.Properties.Resources.command_comment);
            uiImages.Add("EncryptionCommand", robot_worker.Properties.Resources.command_input);
            uiImages.Add("ClipboardGetTextCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("PingCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("SMTPSendEmailCommand", robot_worker.Properties.Resources.command_smtp);
            uiImages.Add("SequenceCommand", robot_worker.Properties.Resources.command_sequence);
            uiImages.Add("ClipboardSetTextCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("MessageBoxCommand", robot_worker.Properties.Resources.command_comment);

            // NLG
            uiImages.Add("NLGCreateInstanceCommand", robot_worker.Properties.Resources.command_nlg);
            uiImages.Add("NLGGeneratePhraseCommand", robot_worker.Properties.Resources.command_nlg);
            uiImages.Add("NLGSetParameterCommand", robot_worker.Properties.Resources.command_nlg);

            // Outlook
            uiImages.Add("OutlookDeleteEmailsCommand", robot_worker.Properties.Resources.command_smtp);
            uiImages.Add("OutlookForwardEmailsCommand", robot_worker.Properties.Resources.command_smtp);
            uiImages.Add("OutlookGetEmailsCommand", robot_worker.Properties.Resources.command_smtp);
            uiImages.Add("OutlookMoveEmailsCommand", robot_worker.Properties.Resources.command_smtp);
            uiImages.Add("OutlookReplyToEmailsCommand", robot_worker.Properties.Resources.command_smtp);
            uiImages.Add("OutlookEmailCommand", robot_worker.Properties.Resources.command_smtp);

            // Program
            uiImages.Add("RunCustomCodeCommand", robot_worker.Properties.Resources.command_script);
            uiImages.Add("RunScriptCommand", robot_worker.Properties.Resources.command_script);
            uiImages.Add("StartProcessCommand", robot_worker.Properties.Resources.command_start_process);
            uiImages.Add("StopProcessCommand", robot_worker.Properties.Resources.command_stop_process);

            // Regex
            uiImages.Add("GetRegexMatchesCommand", robot_worker.Properties.Resources.command_function);

            // Remote
            uiImages.Add("RemoteAPICommand", robot_worker.Properties.Resources.command_remote);
            uiImages.Add("RemoteTaskCommand", robot_worker.Properties.Resources.command_remote);

            // System
            uiImages.Add("EnvironmentVariableCommand", robot_worker.Properties.Resources.command_system);
            uiImages.Add("RemoteDesktopCommand", robot_worker.Properties.Resources.command_system);
            uiImages.Add("OSVariableCommand", robot_worker.Properties.Resources.command_system);
            uiImages.Add("SystemActionCommand", robot_worker.Properties.Resources.command_script);

            // Task
            uiImages.Add("RunTaskCommand", robot_worker.Properties.Resources.command_start_process);
            uiImages.Add("StopTaskCommand", robot_worker.Properties.Resources.command_stop_process);

            // Text
            uiImages.Add("ReadTextFileCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WriteTextFileCommand", robot_worker.Properties.Resources.command_files);

            // Variable
            uiImages.Add("AddToVariableCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("AddVariableCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("VariableCommand", robot_worker.Properties.Resources.command_function);
            uiImages.Add("SetVariableIndexCommand", robot_worker.Properties.Resources.command_function);

            // Web
            uiImages.Add("SeleniumBrowserCloseCommand", robot_worker.Properties.Resources.command_window_close);
            uiImages.Add("SeleniumBrowserCreateCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserElementActionCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserExecuteScriptCommand", robot_worker.Properties.Resources.command_script);
            uiImages.Add("SeleniumBrowserInfoCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateBackCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateForwardCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateURLCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserRefreshCommand", robot_worker.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserSwitchFrameCommand", robot_worker.Properties.Resources.command_window);
            uiImages.Add("SeleniumBrowserSwitchWindowCommand", robot_worker.Properties.Resources.command_window);
            uiImages.Add("SeleniumBrowserTakeScreenshotCommand", robot_worker.Properties.Resources.command_web);

            // Window
            uiImages.Add("ActivateWindowCommand", robot_worker.Properties.Resources.command_window);
            uiImages.Add("CloseWindowCommand", robot_worker.Properties.Resources.command_window_close);
            uiImages.Add("MoveWindowCommand", robot_worker.Properties.Resources.command_window);
            uiImages.Add("ResizeWindowCommand", robot_worker.Properties.Resources.command_window);
            uiImages.Add("SetWindowStateCommand", robot_worker.Properties.Resources.command_window);
            uiImages.Add("WaitForWindowCommand", robot_worker.Properties.Resources.command_window);

            // Word
            uiImages.Add("WordAddDocumentCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordAppendDataTableCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordAppendImageCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordAppendTextCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordCloseApplicationCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordCreateApplicationCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordExportToPDFCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordOpenDocumentCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordReadDocumentCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordReplaceTextCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordSaveCommand", robot_worker.Properties.Resources.command_files);
            uiImages.Add("WordSaveAsCommand", robot_worker.Properties.Resources.command_files);

            //// NOTHING ///
            uiImages.Add("BeginExcelDatasetLoopCommand", robot_worker.Properties.Resources.command_startloop);
            uiImages.Add("ThickAppClickItemCommand", robot_worker.Properties.Resources.command_input);
            uiImages.Add("ThickAppGetTextCommand", robot_worker.Properties.Resources.command_search);
            uiImages.Add("Setcommand_windowtateCommand", robot_worker.Properties.Resources.command_window);
            uiImages.Add("_NotFoundCommand", robot_worker.Properties.Resources.command_files);

            // release
            //GC.Collect();

            return uiImages;
        }
        public static ImageList UIImageList()
        {
            Dictionary<string, Image> imageIcons = UIImageDictionary();
            if (imageList.Count == 0)
            {
                UIImageDictionary();
            }

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
            uiImages.Images.Add("taskt.Properties.Resources.command_begin_if", robot_worker.Properties.Resources.command_begin_if);
            uiImages.Images.Add("taskt.Properties.Resources.command_begin_multi_if", robot_worker.Properties.Resources.command_begin_multi_if);
            uiImages.Images.Add("taskt.Properties.Resources.command_camera", robot_worker.Properties.Resources.command_camera);
            uiImages.Images.Add("taskt.Properties.Resources.command_comment", robot_worker.Properties.Resources.command_comment);
            uiImages.Images.Add("taskt.Properties.Resources.command_database", robot_worker.Properties.Resources.command_database);
            uiImages.Images.Add("taskt.Properties.Resources.command_dictionary", robot_worker.Properties.Resources.command_dictionary);
            uiImages.Images.Add("taskt.Properties.Resources.command_else", robot_worker.Properties.Resources.command_else);
            uiImages.Images.Add("taskt.Properties.Resources.command_endloop", robot_worker.Properties.Resources.command_endloop);
            uiImages.Images.Add("taskt.Properties.Resources.command_end_if", robot_worker.Properties.Resources.command_end_if);
            uiImages.Images.Add("taskt.Properties.Resources.command_error", robot_worker.Properties.Resources.command_error);
            uiImages.Images.Add("taskt.Properties.Resources.command_exitloop", robot_worker.Properties.Resources.command_exitloop);
            uiImages.Images.Add("taskt.Properties.Resources.command_files", robot_worker.Properties.Resources.command_files);
            uiImages.Images.Add("taskt.Properties.Resources.command_function", robot_worker.Properties.Resources.command_function);
            uiImages.Images.Add("taskt.Properties.Resources.command_input", robot_worker.Properties.Resources.command_input);
            uiImages.Images.Add("taskt.Properties.Resources.command_nextloop", robot_worker.Properties.Resources.command_nextloop);
            uiImages.Images.Add("taskt.Properties.Resources.command_nlg", robot_worker.Properties.Resources.command_nlg);
            uiImages.Images.Add("taskt.Properties.Resources.command_parse", robot_worker.Properties.Resources.command_parse);
            uiImages.Images.Add("taskt.Properties.Resources.command_pause", robot_worker.Properties.Resources.command_pause);
            uiImages.Images.Add("taskt.Properties.Resources.command_remote", robot_worker.Properties.Resources.command_remote);
            uiImages.Images.Add("taskt.Properties.Resources.command_run_code", robot_worker.Properties.Resources.command_run_code);
            uiImages.Images.Add("taskt.Properties.Resources.command_script", robot_worker.Properties.Resources.command_script);
            uiImages.Images.Add("taskt.Properties.Resources.command_search", robot_worker.Properties.Resources.command_search);
            uiImages.Images.Add("taskt.Properties.Resources.command_sequence", robot_worker.Properties.Resources.command_sequence);
            uiImages.Images.Add("taskt.Properties.Resources.command_server", robot_worker.Properties.Resources.command_server);
            uiImages.Images.Add("taskt.Properties.Resources.command_smtp", robot_worker.Properties.Resources.command_smtp);
            uiImages.Images.Add("taskt.Properties.Resources.command_spreadsheet", robot_worker.Properties.Resources.command_spreadsheet);
            uiImages.Images.Add("taskt.Properties.Resources.command_startloop", robot_worker.Properties.Resources.command_startloop);
            uiImages.Images.Add("taskt.Properties.Resources.command_start_process", robot_worker.Properties.Resources.command_start_process);
            uiImages.Images.Add("taskt.Properties.Resources.command_stopwatch", robot_worker.Properties.Resources.command_stopwatch);
            uiImages.Images.Add("taskt.Properties.Resources.command_stop_process", robot_worker.Properties.Resources.command_stop_process);
            uiImages.Images.Add("taskt.Properties.Resources.command_string", robot_worker.Properties.Resources.command_string);
            uiImages.Images.Add("taskt.Properties.Resources.command_system", robot_worker.Properties.Resources.command_system);
            uiImages.Images.Add("taskt.Properties.Resources.command_try", robot_worker.Properties.Resources.command_try);
            uiImages.Images.Add("taskt.Properties.Resources.command_web", robot_worker.Properties.Resources.command_web);
            uiImages.Images.Add("taskt.Properties.Resources.command_window", robot_worker.Properties.Resources.command_window);
            uiImages.Images.Add("taskt.Properties.Resources.command_window_close", robot_worker.Properties.Resources.command_window_close);

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
                        canvas.DrawImage(oldImage, new Rectangle(new Point(0, 0), new Size(16, 16)));
                        return newImage;
                    }
                }
            }
        }

        public static int GetUIImageList(string commandName)
        {
            try
            {
                if (imageCommandTable.ContainsKey(commandName))
                    return uiImages.Images.IndexOfKey(imageCommandTable[commandName]);
                else return -1;
            }
            catch (Exception)
            {
                return uiImages.Images.IndexOfKey("taskt.Properties.Resources.command_files");
            }
        }

        public static Image GetUIImage(string commandName)
        {
            if (imageList.Count == 0)
            {
                UIImageDictionary();
            }
            if (uiImages.Images.Count == 0)
            {
                UIImageList();
            }
            Image retImage = null;
            try
            {
                if (imageCommandTable.ContainsKey(commandName))
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