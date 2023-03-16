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
using taskt.Core.Automation.Commands;

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
            {nameof(ExecuteDLLCommand), "taskt.Properties.Resources.command_run_code"},
            {nameof(RESTCommand), "taskt.Properties.Resources.command_run_code"},
            {nameof(HTTPRequestCommand), "taskt.Properties.Resources.command_web"},
            {nameof(HTTPQueryResultCommand), "taskt.Properties.Resources.command_search"},

            // Color
            {nameof(CreateColorCommand), "taskt.Properties.Resources.command_function"},
            {nameof(CreateColorFromExcelColorCommand), "taskt.Properties.Resources.command_function"},
            {nameof(CreateColorFromHexCommand), "taskt.Properties.Resources.command_function"},
            {nameof(FormatColorCommand), "taskt.Properties.Resources.command_function"},

            // Data
            {nameof(DateCalculationCommand), "taskt.Properties.Resources.command_function"},
            {nameof(FormatDataCommand), "taskt.Properties.Resources.command_function"},
            {nameof(LogDataCommand), "taskt.Properties.Resources.command_files"},
            {nameof(PDFTextExtractionCommand), "taskt.Properties.Resources.command_function"},

            // Database
            {nameof(DatabaseDefineConnectionCommand), "taskt.Properties.Resources.command_database"},
            {nameof(DatabaseExecuteQueryCommand), "taskt.Properties.Resources.command_database"},

            // DataTable
            {nameof(AddDataRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(AddDataTableColumnCommand), "taskt.Properties.Resources.command_spreadsheet"},
            //{nameof(AddDataTableColumnAndFillValuesByListCommand), "taskt.Properties.Resources.command_spreadsheet"},
            //{nameof(AddDataTableColumnsAndFillValuesByDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(AddDataTableRowByDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(AddDataTableRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(AddDataTableRowsByDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(CheckDataTableColumnExistsCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ConvertDataTableColumnToDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ConvertDataTableColumnToDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ConvertDataTableColumnToJSONCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ConvertDataTableColumnToListCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ConvertDataTableRowToDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ConvertDataTableRowToDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ConvertDataTableRowToJSONCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ConvertDataTableRowToListCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ConvertDataTableToJSONCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(CopyDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(CreateDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(DeleteDataTableColumnCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(DeleteDataTableRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(FilterDataTableColumnByRowValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(FilterDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(FilterDataTableRowByColumnValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(GetDataRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(GetDataRowCountCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(GetDataRowValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(GetDataTableColumnCountCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(GetDataTableColumnListCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(GetDataTableRowCountCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(GetDataTableValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(LoadDataTableCommand), "taskt.Properties.Resources.command_spreadsheet" },
            {nameof(ReplaceDataTableColumnValueCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ReplaceDataTableRowValueCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ReplaceDataTableValueCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ParseDatasetRowCommand), "taskt.Properties.Resources.command_function"},
            {nameof(RemoveDataRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(SetDataTableColumnValuesByDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(SetDataTableColumnValuesByListCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(SetDataTableRowValuesByDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(SetDataTableRowValuesByDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(SetDataTableValueCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(WriteDataRowValueCommand), "taskt.Properties.Resources.command_spreadsheet"},

            // DateTime
            {nameof(CalculateDateTimeCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ConvertDateTimeToExcelSerialCommand), "taskt.Properties.Resources.command_function"},
            {nameof(CreateDateTimeCommand), "taskt.Properties.Resources.command_function"},
            {nameof(CreateDateTimeFromExcelSerialCommand), "taskt.Properties.Resources.command_function"},
            {nameof(CreateDateTimeFromTextCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetDateTimeDifferencesCommand), "taskt.Properties.Resources.command_function"},
            {nameof(FormatDateTimeCommand), "taskt.Properties.Resources.command_function"},

            // Dictionary
            {nameof(AddDictionaryItemCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(CheckDictionaryKeyExistsCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(ConcatenateDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(ConvertDictionaryToDataTableCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(ConvertDictionaryToJSONCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(ConvertDictionaryToListCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(CopyDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(CreateDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(FilterDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(GetDictionaryKeyFromValueCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(GetDictionaryKeysListCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(GetDictionaryValueCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(LoadDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(RemoveDictionaryItemCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(ReplaceDictionaryCommand), "taskt.Properties.Resources.command_dictionary"},
            {nameof(SetDictionaryValueCommand), "taskt.Properties.Resources.command_dictionary"},

            // EMail
            {nameof(MailKitGetAddressesAsDataTableCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitGetAddressesAsDictionaryCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitGetAddressesAsListCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitGetEmailAttachmentsNameCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitGetEMailFromEMailListCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitGetEMailTextCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitLoadEmailCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitRecieveEmailListUsingIMAPCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitRecieveEmailListUsingPOPCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitSaveEmailCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitSaveEmailAttachmentsCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(MailKitSendEmailCommand), "taskt.Properties.Resources.command_comment"},

            // Engine
            {nameof(ErrorHandlingCommand), "taskt.Properties.Resources.command_error"},
            {nameof(GetDataCommand), "taskt.Properties.Resources.command_server"},  // get bot data
            {nameof(PauseCommand), "taskt.Properties.Resources.command_pause"},
            {nameof(SetEngineDelayCommand), "taskt.Properties.Resources.command_pause"},
            {nameof(ShowEngineContextCommand), "taskt.Properties.Resources.command_window"},
            {nameof(SetEnginePreferenceCommand), "taskt.Properties.Resources.command_window"},
            {nameof(StopwatchCommand), "taskt.Properties.Resources.command_stopwatch"},
            {nameof(UploadDataCommand), "taskt.Properties.Resources.command_server"},   // upload bot store

            // Error
            {nameof(CatchExceptionCommand), "taskt.Properties.Resources.command_try"},
            {nameof(EndTryCommand), "taskt.Properties.Resources.command_try"},
            {nameof(FinallyCommand), "taskt.Properties.Resources.command_try"},
            {nameof(ThrowExceptionCommand), "taskt.Properties.Resources.command_try"},
            {nameof(TryCommand), "taskt.Properties.Resources.command_try"},

            // Excel
            {nameof(ExcelActivateSheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelAddWorkbookCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelAddWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelAppendCellCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelAppendRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelCheckCellValueExistsCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelCheckCellValueExistsRCCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelCheckExcelInstanceExistsCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelCheckWorksheetExistsCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelCloseApplicationCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelCopyWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelCreateApplicationCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelDeleteCellCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelDeleteRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelDeleteWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetCellCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetCellRCCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetColumnValuesAsDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetColumnValuesAsDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetColumnValuesAsListCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetCurrentWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetExcelInfoCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetLastRowCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetRangeCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetRangeCommandAsDT), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetRangeValuesAsDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetRowValuesAsDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetRowValuesAsDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetRowValuesAsListCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetWorksheetsCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelGetWorksheetInfoCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelOpenWorkbookCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelRenameWorksheetCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelRunMacroCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSaveCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSaveAsCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSetCellCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSetCellRCCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSetColumnValuesFromDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSetColumnValuesFromDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSetColumnValuesFromListCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSetRowValuesFromDataTableCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSetRowValuesFromDictionaryCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSetRowValuesFromListCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelSplitRangeByColumnCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelWriteRangeCommand), "taskt.Properties.Resources.command_spreadsheet"},
            {nameof(ExcelWriteRowCommand), "taskt.Properties.Resources.command_spreadsheet"},

            // File
            {nameof(CheckFileExistsCommand), "taskt.Properties.Resources.command_files"},
            {nameof(DeleteFileCommand), "taskt.Properties.Resources.command_files"},
            {nameof(ExtractFileCommand), "taskt.Properties.Resources.command_files"},
            {nameof(FormatFilePathCommnad), "taskt.Properties.Resources.command_files"},
            {nameof(GetFileInfoCommand), "taskt.Properties.Resources.command_files"},
            {nameof(GetFilesCommand), "taskt.Properties.Resources.command_files"},
            {nameof(MoveFileCommand), "taskt.Properties.Resources.command_files"},
            {nameof(RenameFileCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WaitForFileToExistCommand), "taskt.Properties.Resources.command_files"},

            // Folder
            {nameof(CheckFolderExistsCommand), "taskt.Properties.Resources.command_files"},
            {nameof(CreateFolderCommand), "taskt.Properties.Resources.command_files"},
            {nameof(DeleteFolderCommand), "taskt.Properties.Resources.command_files"},
            {nameof(FormatFolderPathCommnad), "taskt.Properties.Resources.command_files"},
            {nameof(GetFoldersCommand), "taskt.Properties.Resources.command_files"},
            {nameof(MoveFolderCommand), "taskt.Properties.Resources.command_files"},
            {nameof(RenameFolderCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WaitForFolderToExistCommand), "taskt.Properties.Resources.command_files"},

            // IE
            {nameof(IEBrowserCloseCommand), "taskt.Properties.Resources.command_window_close"},
            {nameof(IEBrowserCreateCommand), "taskt.Properties.Resources.command_web"},
            {nameof(IEBrowserElementActionCommand), "taskt.Properties.Resources.command_web"},
            {nameof(IEBrowserFindBrowserCommand), "taskt.Properties.Resources.command_web"},
            {nameof(IEBrowserNavigateURLCommand), "taskt.Properties.Resources.command_web"},

            // If
            {nameof(BeginIfCommand), "taskt.Properties.Resources.command_begin_if"},
            {nameof(BeginMultiIfCommand), "taskt.Properties.Resources.command_begin_multi_if"},
            {nameof(ElseCommand), "taskt.Properties.Resources.command_else"},
            {nameof(EndIfCommand), "taskt.Properties.Resources.command_end_if"},

            // Image
            {nameof(ImageRecognitionCommand), "taskt.Properties.Resources.command_camera"},
            {nameof(OCRCommand), "taskt.Properties.Resources.command_camera"},
            {nameof(ScreenshotCommand), "taskt.Properties.Resources.command_camera"},

            // Input
            {nameof(FileDialogCommand), "taskt.Properties.Resources.command_input"},
            {nameof(FolderDialogCommand), "taskt.Properties.Resources.command_input"},
            {nameof(HTMLInputCommand), "taskt.Properties.Resources.command_input"},
            {nameof(UserInputCommand), "taskt.Properties.Resources.command_input"}, // prompt
            {nameof(SendAdvancedKeyStrokesCommand), "taskt.Properties.Resources.command_input"},
            {nameof(SendHotkeyCommand), "taskt.Properties.Resources.command_input"},
            {nameof(SendKeysCommand), "taskt.Properties.Resources.command_input"},
            {nameof(SendMouseMoveCommand), "taskt.Properties.Resources.command_input"},
            {nameof(SendMouseClickCommand), "taskt.Properties.Resources.command_input"},
            {nameof(UIAutomationCommand), "taskt.Properties.Resources.command_input"},
            
            // JSON
            {nameof(AddJSONArrayItemCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(AddJSONObjectPropertyCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(ConvertJSONToDataTableCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(ConvertJSONToDictionaryCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(ConvertJSONToListCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(CreateJSONVariableCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(GetJSONValueListCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(GetMultiJSONValueListCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(InsertJSONArrayItemCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(InsertJSONObjectPropertyCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(ParseJSONArrayCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(ReadJSONFileCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(RemoveJSONArrayItemCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(RemoveJSONPropertyCommand), "taskt.Properties.Resources.command_parse"},
            {nameof(SetJSONValueCommand), "taskt.Properties.Resources.command_parse"},

            // List
            {nameof(AddListItemCommand), "taskt.Properties.Resources.command_function"},
            {nameof(CheckListItemExistsCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ConcatenateListsCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ConvertListToDataTableCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ConvertListToDictionaryCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ConvertListToJSONCommand), "taskt.Properties.Resources.command_function"},
            {nameof(CopyListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(CreateListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(FilterListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetAverageFromListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetListCountCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetListIndexCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetListIndexFromValueCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetListItemCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetMaxFromListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetMedianFromListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetMinFromListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetSumFromListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetVarianceFromListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ReplaceListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ReverseListCommand), "taskt.Properties.Resources.command_function"},
            {nameof(SetListIndexCommand), "taskt.Properties.Resources.command_function"},
            {nameof(SetListItemCommand), "taskt.Properties.Resources.command_function"},
            {nameof(SortListCommand), "taskt.Properties.Resources.command_function"},

            // Loop
            {nameof(BeginLoopCommand), "taskt.Properties.Resources.command_startloop"},
            {nameof(BeginMultiLoopCommand), "taskt.Properties.Resources.command_startloop"},
            {nameof(EndLoopCommand), "taskt.Properties.Resources.command_endloop"},
            {nameof(ExitLoopCommand), "taskt.Properties.Resources.command_exitloop"},
            {nameof(BeginContinousLoopCommand), "taskt.Properties.Resources.command_startloop"},
            {nameof(BeginListLoopCommand), "taskt.Properties.Resources.command_startloop"},
            {nameof(BeginNumberOfTimesLoopCommand), "taskt.Properties.Resources.command_startloop"},
            {nameof(NextLoopCommand), "taskt.Properties.Resources.command_nextloop"},

            // Misc
            {nameof(ClipboardClearTextCommand), "taskt.Properties.Resources.command_files"},
            {nameof(ClipboardSetTextCommand), "taskt.Properties.Resources.command_files"},
            {nameof(ClipboardGetTextCommand), "taskt.Properties.Resources.command_files"},
            {nameof(CommentCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(CreateShortcutCommand), "taskt.Properties.Resources.command_files"},
            {nameof(EncryptionCommand), "taskt.Properties.Resources.command_input"},
            {nameof(MessageBoxCommand), "taskt.Properties.Resources.command_comment"},
            {nameof(PingCommand), "taskt.Properties.Resources.command_web"},
            {nameof(PlaySystemSoundCommand), "taskt.Properties.Resources.command_files"},
            {nameof(SMTPSendEmailCommand), "taskt.Properties.Resources.command_smtp"},
            {nameof(SequenceCommand), "taskt.Properties.Resources.command_sequence"},

            // NLG
            {nameof(NLGCreateInstanceCommand), "taskt.Properties.Resources.command_nlg"},
            {nameof(NLGGeneratePhraseCommand), "taskt.Properties.Resources.command_nlg"},
            {nameof(NLGSetParameterCommand), "taskt.Properties.Resources.command_nlg"},

            // Numeric
            {nameof(CreateNumericalVariableCommand), "taskt.Properties.Resources.command_function"},
            {nameof(DecreaseNumericalVariableCommand), "taskt.Properties.Resources.command_function"},
            {nameof(FormatNumberCommand), "taskt.Properties.Resources.command_function"},
            {nameof(IncreaseNumericalVariableCommand), "taskt.Properties.Resources.command_function"},
            {nameof(MathCalculationCommand), "taskt.Properties.Resources.command_function"},
            {nameof(RandomNumberCommand), "taskt.Properties.Resources.command_function"},
            {nameof(RoundNumberCommand), "taskt.Properties.Resources.command_function"}, 

            // Outlook
            {nameof(OutlookDeleteEmailsCommand), "taskt.Properties.Resources.command_smtp"},
            {nameof(OutlookForwardEmailsCommand), "taskt.Properties.Resources.command_smtp"},
            {nameof(OutlookGetEmailsCommand), "taskt.Properties.Resources.command_smtp"},
            {nameof(OutlookMoveEmailsCommand), "taskt.Properties.Resources.command_smtp"},
            {nameof(OutlookReplyToEmailsCommand), "taskt.Properties.Resources.command_smtp"},
            {nameof(OutlookEmailCommand), "taskt.Properties.Resources.command_smtp"},

            // Program
            {nameof(RunCustomCodeCommand), "taskt.Properties.Resources.command_script"},
            {nameof(RunScriptCommand), "taskt.Properties.Resources.command_script"},
            {nameof(StartProcessCommand), "taskt.Properties.Resources.command_start_process"},
            {nameof(StopProcessCommand), "taskt.Properties.Resources.command_stop_process"},

            // Regex
            {nameof(GetRegexMatchesCommand), "taskt.Properties.Resources.command_function"},

            // Remote
            {nameof(RemoteAPICommand), "taskt.Properties.Resources.command_remote"},
            {nameof(RemoteTaskCommand), "taskt.Properties.Resources.command_remote"},

            // System
            {nameof(GetEnvironmentVariableCommand), "taskt.Properties.Resources.command_system"},
            {nameof(RemoteDesktopCommand), "taskt.Properties.Resources.command_system"},
            {nameof(GetOSVariableCommand), "taskt.Properties.Resources.command_system"},
            {nameof(SystemActionCommand), "taskt.Properties.Resources.command_script"},

            // Task
            {nameof(LoadTaskCommand), "taskt.Properties.Resources.command_start_process"},
            {nameof(UnloadTaskCommand), "taskt.Properties.Resources.command_stop_process"},
            {nameof(RunTaskCommand), "taskt.Properties.Resources.command_start_process"},
            {nameof(StopTaskCommand), "taskt.Properties.Resources.command_stop_process"},

            // Text
            {nameof(CheckTextCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ConcatenateTextVariableCommand), "taskt.Properties.Resources.command_function"},
            {nameof(CreateTextVariableCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ExtractionTextCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetWordLengthCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetWordCountCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ModifyTextCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ReadTextFileCommand), "taskt.Properties.Resources.command_files"},
            {nameof(RegExExtractionTextCommand), "taskt.Properties.Resources.command_function"},
            {nameof(ReplaceTextCommand), "taskt.Properties.Resources.command_string"},
            {nameof(SplitTextCommand), "taskt.Properties.Resources.command_string"},
            {nameof(SubstringTextCommand), "taskt.Properties.Resources.command_string"},
            {nameof(WriteTextFileCommand), "taskt.Properties.Resources.command_files"},

            // UIAutomation
            {nameof(UIAutomationCheckElementExistCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationCheckElementExistByXPathCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationClickElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationExpandCollapseItemsInElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetChildElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetChildrenElementsInformationCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetElementFromElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetElementFromElementByXPathCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetElementFromTableElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetElementFromWindowCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetElementFromWindowByXPathCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetElementTreeXMLFromElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetParentElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetSelectedStateFromElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetSelectionItemsFromElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetTextFromElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationGetTextFromTableElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationSelectElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationSelectItemInElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationSetTextToElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationScrollElementCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationWaitForElementExistByXPathCommand), "taskt.Properties.Resources.command_window"},
            {nameof(UIAutomationWaitForElementExistCommand), "taskt.Properties.Resources.command_window"},

            // Variable
            {nameof(CheckVariableExistsCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetVariableIndexCommand), "taskt.Properties.Resources.command_function"},
            {nameof(GetVariableTypeCommand), "taskt.Properties.Resources.command_function"},
            {nameof(NewVariableCommand), "taskt.Properties.Resources.command_function"},
            {nameof(SetVariableValueCommand), "taskt.Properties.Resources.command_function"},
            {nameof(SetVariableIndexCommand), "taskt.Properties.Resources.command_function"},

            // Web
            {nameof(SeleniumBrowserCheckBrowserInstanceExistsCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserCloseCommand), "taskt.Properties.Resources.command_window_close"},
            {nameof(SeleniumBrowserCreateCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserElementActionCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserExecuteScriptCommand), "taskt.Properties.Resources.command_script"},
            {nameof(SeleniumBrowserGetAnElementValuesAsDataTableCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserGetAnElementValuesAsDictionaryCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserGetAnElementValuesAsListCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserGetElementsValueAsDataTableCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserGetElementsValueAsDictionaryCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserGetElementsValueAsListCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserGetElementsValuesAsDataTableCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserGetTableValueAsDataTableCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserInfoCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserNavigateBackCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserNavigateForwardCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserNavigateURLCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserRefreshCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserResizeBrowserCommand), "taskt.Properties.Resources.command_web"},
            {nameof(SeleniumBrowserSwitchFrameCommand), "taskt.Properties.Resources.command_window"},
            {nameof(SeleniumBrowserSwitchWindowCommand), "taskt.Properties.Resources.command_window"},
            {nameof(SeleniumBrowserTakeScreenshotCommand), "taskt.Properties.Resources.command_web"},

            // Window
            {nameof(ActivateWindowCommand), "taskt.Properties.Resources.command_window"},
            {nameof(CheckWindowNameExistsCommand), "taskt.Properties.Resources.command_window"},
            {nameof(CloseWindowCommand), "taskt.Properties.Resources.command_window_close"},
            {nameof(GetWindowNamesCommand), "taskt.Properties.Resources.command_window"},
            {nameof(GetWindowPositionCommand), "taskt.Properties.Resources.command_window"},
            {nameof(GetWindowStateCommand), "taskt.Properties.Resources.command_window"},
            {nameof(MoveWindowCommand), "taskt.Properties.Resources.command_window"},
            {nameof(ResizeWindowCommand), "taskt.Properties.Resources.command_window"},
            {nameof(SetWindowStateCommand), "taskt.Properties.Resources.command_window"},
            {nameof(WaitForWindowCommand), "taskt.Properties.Resources.command_window"},

            // Word
            {nameof(WordAddDocumentCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordAppendDataTableCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordAppendImageCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordAppendTextCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordCheckWordInstanceExistsCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordCloseApplicationCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordCreateApplicationCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordExportToPDFCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordOpenDocumentCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordReadDocumentCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordReplaceTextCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordSaveDocumentCommand), "taskt.Properties.Resources.command_files"},
            {nameof(WordSaveDocumentAsCommand), "taskt.Properties.Resources.command_files"},

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