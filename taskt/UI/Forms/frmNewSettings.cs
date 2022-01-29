using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms
{
    public partial class frmNewSettings : ThemedForm
    {
        Core.ApplicationSettings newAppSettings;
        frmScriptBuilder scriptBuilderForm;

        private enum FontSize
        {
            Small,
            Normal,
            Large
        }
        public frmNewSettings(frmScriptBuilder fm)
        {
            InitializeComponent();
            this.scriptBuilderForm = fm;
        }

        private void frmNewSettings_Load(object sender, EventArgs e)
        {
            newAppSettings = new Core.ApplicationSettings();
            newAppSettings = newAppSettings.GetOrCreateApplicationSettings();

            tvSettingsMenu.ExpandAll();
        }

        private void tvSettingsMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode rootNode = tvSettingsMenu.SelectedNode;
            while (rootNode.Parent != null)
            {
                rootNode = rootNode.Parent;
            }

            flowLayoutSettings.SuspendLayout();

            switch (rootNode.Text + " - " + tvSettingsMenu.SelectedNode.Text)
            {
                case "Application - Start Up":
                    showApplicationStartUpSetting();
                    break;
                case "Application - Folder":
                    showApplicationFolderSettings();
                    break;
                default:
                    break;
            }

            flowLayoutSettings.ResumeLayout();
        }

        private void removeSettingControls()
        {
            flowLayoutSettings.Controls.Clear();
        }

        private void showApplicationStartUpSetting()
        {
            removeSettingControls();

            createLabel("lblTitie", "App Settings", FontSize.Large, true);
            createCheckBox("chkAntiIdle", "Anti-Idle (while app is open)", newAppSettings.ClientSettings, "AntiIdleWhileOpen", true);
            CheckBox chkPre = createCheckBox("chkPreLoadCommands", "Load Commands at Startup (Reduces Flicker)", newAppSettings.ClientSettings, "PreloadBuilderCommands", true);
            chkPre.Visible = false;
            createLabel("lblStartMode", "Start Mode:", FontSize.Normal, false);
            createComboBox("cmbStartMode", new string[] { "Builder Mode","Attended Task Mode"}, 200, newAppSettings.ClientSettings, "StartupMode", true);
        }

        private void showApplicationFolderSettings()
        {
            removeSettingControls();

            createLabel("lblTitle", "Folder", FontSize.Large, true);
            createLabel("lblRootFolder", "taskt Root Folder", FontSize.Small, true);
            createTextBox("txtAppFolderPath", 440, newAppSettings.ClientSettings, "RootFolder", false);
            createButton("btnSelectFolder", "...", 42, true);
        }

        private Label createLabel(string name, string text, FontSize fontSize = FontSize.Normal, bool isBreak = false)
        {
            Label lbl = new Label();
            lbl.Name = name;
            lbl.Text = text;

            lbl.AutoSize = true;

            switch (fontSize)
            {
                case FontSize.Small:
                    lbl.Font = new Font("Segoe UI Semilight", (Single)9.75);
                    lbl.ForeColor = Color.SlateGray;
                    lbl.Padding = new Padding(0, 16, 0, 0);
                    break;
                case FontSize.Normal:
                    lbl.Font = new Font("Segoe UI Semibold", 12, FontStyle.Bold);
                    lbl.ForeColor = Color.SteelBlue;
                    lbl.Padding = new Padding(0, 4, 0, 0);
                    break;
                case FontSize.Large:
                    lbl.Font = new Font("Segoe UI Light", (Single)15.75);
                    lbl.ForeColor = Color.SteelBlue;
                    break;
            }

            flowLayoutSettings.Controls.Add(lbl);
            flowLayoutSettings.SetFlowBreak(lbl, isBreak);

            return lbl;
        }

        private TextBox createTextBox(string name, int width, object source, string memberName, bool isBreak = false)
        {
            TextBox txt = new TextBox();
            txt.Name = name;
            txt.Width = width;
            txt.Height = 29;
            txt.Font = new Font("Segoe UI", 12);

            txt.DataBindings.Add("Text", source, memberName, false, DataSourceUpdateMode.OnPropertyChanged);

            flowLayoutSettings.Controls.Add(txt);
            flowLayoutSettings.SetFlowBreak(txt, isBreak);

            return txt;
        }

        private CheckBox createCheckBox(string name, string text, object source, string memberName, bool isBreak = false)
        {
            CheckBox chk = new CheckBox();
            chk.Name = name;
            chk.AutoSize = true;
            chk.Text = text;
            chk.Font = new Font("Segoe UI Semilight", (Single)11.25);
            chk.ForeColor = Color.SteelBlue;

            chk.DataBindings.Add("Checked", source, memberName, false, DataSourceUpdateMode.OnPropertyChanged);

            flowLayoutSettings.Controls.Add(chk);
            flowLayoutSettings.SetFlowBreak(chk, isBreak);

            return chk;
        }

        private ComboBox createComboBox(string name, string[] items, int width, object source, string memberName, bool isBreak = false)
        {
            ComboBox cmb = new ComboBox();
            cmb.Name = name;
            cmb.Font = new Font("Segoe UI", 12);
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;

            cmb.Items.AddRange(items);
            cmb.Width = width;

            cmb.DataBindings.Add("Text", source, memberName, false, DataSourceUpdateMode.OnPropertyChanged);

            flowLayoutSettings.Controls.Add(cmb);
            flowLayoutSettings.SetFlowBreak(cmb, isBreak);

            return cmb;
        }

        private Button createButton(string name, string text, int width, bool isBreak = false)
        {
            Button btn = new Button();
            btn.Name = name;
            btn.Text = text;
            btn.Width = width;
            btn.Height = 25;
            btn.Font = new Font("Segoe UI", (Single)9.75);

            flowLayoutSettings.Controls.Add(btn);
            flowLayoutSettings.SetFlowBreak(btn, isBreak);

            return btn;
        }

        
    }
}
