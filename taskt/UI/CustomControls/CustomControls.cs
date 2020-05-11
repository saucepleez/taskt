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
        public static Dictionary<string, Image> UIImageDictionary()
        {
            var uiImages = new Dictionary<string, Image>();
            uiImages.Add("PauseCommand", taskt.Properties.Resources.command_pause);
            uiImages.Add("SetEngineDelayCommand", taskt.Properties.Resources.command_pause);
            uiImages.Add("CommentCommand", taskt.Properties.Resources.command_comment);
            uiImages.Add("ActivateWindowCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("MoveWindowCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("SetWindowStateCommand", taskt.Properties.Resources.command_window);            
            uiImages.Add("HTMLInputCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("UIAutomationCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("ThickAppClickItemCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("ThickAppGetTextCommand", taskt.Properties.Resources.command_search);
            uiImages.Add("ResizeWindowCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("MessageBoxCommand", taskt.Properties.Resources.command_comment);
            uiImages.Add("StopProcessCommand", taskt.Properties.Resources.command_stop_process); 
            uiImages.Add("StartProcessCommand", taskt.Properties.Resources.command_start_process);
            uiImages.Add("VariableCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("AddToVariableCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("SetVariableIndexCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("AddVariableCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("FormatDataCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("ParseDatasetRowCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("ModifyVariableCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("DateCalculationCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("MathCalculationCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("RegExExtractorCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("TextExtractorCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("PDFTextExtractionCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("GetWordLengthCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("GetWordCountCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("GetListCountCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("GetListItemCommand", taskt.Properties.Resources.command_function);
            uiImages.Add("RunScriptCommand", taskt.Properties.Resources.command_script);
            uiImages.Add("RunCustomCodeCommand", taskt.Properties.Resources.command_script);
            uiImages.Add("RunTaskCommand", taskt.Properties.Resources.command_start_process);
            uiImages.Add("StopTaskCommand", taskt.Properties.Resources.command_stop_process);
            uiImages.Add("UserInputCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("CloseWindowCommand", taskt.Properties.Resources.command_window_close);
            uiImages.Add("IEBrowserCreateCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("IEBrowserNavigateCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("IEBrowserCloseCommand", taskt.Properties.Resources.command_window_close);
            uiImages.Add("IEBrowserElementCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SendKeysCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("SendAdvancedKeyStrokesCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("EncryptionCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("SendMouseMoveCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("SendMouseClickCommand", taskt.Properties.Resources.command_input);
            uiImages.Add("Setcommand_windowtateCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("WebBrowserFindBrowserCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("EndLoopCommand", taskt.Properties.Resources.command_endloop);
            uiImages.Add("ClipboardGetTextCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("ClipboardSetTextCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("ExcelCreateApplicationCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelOpenWorkbookCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelAddWorkbookCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGoToCellCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelCloseApplicationCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSetCellCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetCellCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelRunMacroCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveAsCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelDeleteCellCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelGetLastRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelSaveCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("ExcelActivateSheetCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("WordCreateApplicationCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordCloseApplicationCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordOpenDocumentCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordSaveCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordSaveAsCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordExportToPDFCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordAddDocumentCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordReadDocumentCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordReplaceTextCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordAppendTextCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordAppendImageCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WordAppendDataTableCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("ExcelSplitRangeByColumnCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("AddDataRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowValueCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("WriteDataRowValueCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("GetDataRowCountCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("CreateDataTableCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("FilterDataTableCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("RemoveDataRowCommand", taskt.Properties.Resources.command_spreadsheet);
            uiImages.Add("SeleniumBrowserCreateCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateURLCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateForwardCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserNavigateBackCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserRefreshCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserCloseCommand", taskt.Properties.Resources.command_window_close);
            uiImages.Add("SeleniumBrowserElementActionCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SeleniumBrowserExecuteScriptCommand", taskt.Properties.Resources.command_script);
            uiImages.Add("SeleniumBrowserSwitchWindowCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("SeleniumBrowserSwitchFrameCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("SeleniumBrowserInfoCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("SMTPSendEmailCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookEmailCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookGetEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookMoveEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookDeleteEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookForwardEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("OutlookReplyToEmailsCommand", taskt.Properties.Resources.command_smtp);
            uiImages.Add("ErrorHandlingCommand", taskt.Properties.Resources.command_error);
            uiImages.Add("TryCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("CatchExceptionCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("FinallyCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("EndTryCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("ThrowExceptionCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("GetExceptionMessageCommand", taskt.Properties.Resources.command_try);
            uiImages.Add("StringSubstringCommand", taskt.Properties.Resources.command_string);
            uiImages.Add("StringSplitCommand", taskt.Properties.Resources.command_string);
            uiImages.Add("StringReplaceCommand", taskt.Properties.Resources.command_string);
            uiImages.Add("BeginIfCommand", taskt.Properties.Resources.command_begin_if);
            uiImages.Add("NextLoopCommand", taskt.Properties.Resources.command_nextloop);
            uiImages.Add("BeginMultiIfCommand", taskt.Properties.Resources.command_begin_multi_if);
            uiImages.Add("EndIfCommand", taskt.Properties.Resources.command_end_if);
            uiImages.Add("ElseCommand", taskt.Properties.Resources.command_else);
            uiImages.Add("ScreenshotCommand", taskt.Properties.Resources.command_camera);
            uiImages.Add("OCRCommand", taskt.Properties.Resources.command_camera);
            uiImages.Add("ImageRecognitionCommand", taskt.Properties.Resources.command_camera);
            uiImages.Add("HTTPRequestCommand", taskt.Properties.Resources.command_web);
            uiImages.Add("HTTPQueryResultCommand", taskt.Properties.Resources.command_search);
            uiImages.Add("BeginListLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("BeginContinousLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("BeginExcelDatasetLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("BeginNumberOfTimesLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("BeginLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("BeginMultiLoopCommand", taskt.Properties.Resources.command_startloop);
            uiImages.Add("ExitLoopCommand", taskt.Properties.Resources.command_exitloop);
            uiImages.Add("SequenceCommand", taskt.Properties.Resources.command_sequence);
            uiImages.Add("ReadTextFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WriteTextFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("MoveFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("DeleteFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("RenameFileCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("WaitForFileToExistCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("GetFilesCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("GetFoldersCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("CreateFolderCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("DeleteFolderCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("MoveFolderCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("RenameFolderCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("LogDataCommand", taskt.Properties.Resources.command_files);
            uiImages.Add("ExecuteDLLCommand", taskt.Properties.Resources.command_run_code);
            uiImages.Add("RESTCommand", taskt.Properties.Resources.command_run_code);
            uiImages.Add("ParseJsonCommand", taskt.Properties.Resources.command_parse);
            uiImages.Add("ParseJsonModelCommand", taskt.Properties.Resources.command_parse);
            uiImages.Add("ParseJsonArrayCommand", taskt.Properties.Resources.command_parse);
            uiImages.Add("UploadDataCommand", taskt.Properties.Resources.command_server);
            uiImages.Add("GetDataCommand", taskt.Properties.Resources.command_server);
            uiImages.Add("StopwatchCommand", taskt.Properties.Resources.command_stopwatch);
            uiImages.Add("SystemActionCommand", taskt.Properties.Resources.command_script);
            uiImages.Add("RemoteDesktopCommand", taskt.Properties.Resources.command_system);
            uiImages.Add("NLGGeneratePhraseCommand", taskt.Properties.Resources.command_nlg);
            uiImages.Add("NLGSetParameterCommand", taskt.Properties.Resources.command_nlg);
            uiImages.Add("NLGCreateInstanceCommand", taskt.Properties.Resources.command_nlg);
            uiImages.Add("ShowEngineContextCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("SetEnginePreferenceCommand", taskt.Properties.Resources.command_window);
            uiImages.Add("DatabaseDefineConnectionCommand", taskt.Properties.Resources.command_database);
            uiImages.Add("DatabaseExecuteQueryCommand", taskt.Properties.Resources.command_database);
            uiImages.Add("RemoteTaskCommand", taskt.Properties.Resources.command_remote);
            uiImages.Add("RemoteAPICommand", taskt.Properties.Resources.command_remote);
            uiImages.Add("AddDictionaryCommand", taskt.Properties.Resources.command_dictionary);
            uiImages.Add("CreateDictionaryCommand", taskt.Properties.Resources.command_dictionary);
            uiImages.Add("GetDictionaryValueCommand", taskt.Properties.Resources.command_dictionary);
            uiImages.Add("LoadDictionaryCommand", taskt.Properties.Resources.command_dictionary);

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
                uiImage = Properties.Resources.command_files;
            }
            return uiImage;
        }
    }
}