using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmFormatChecker : ThemedForm
    {
        public frmFormatChecker()
        {
            InitializeComponent();
            cmbType.SelectedIndex = 0;
        }

        public frmFormatChecker(string valueType) : this()
        {
            cmbType.SelectedIndex = getTypeIndex(valueType);
        }
        private void frmFormatChecker_Load(object sender, EventArgs e)
        {

        }

        #region value, value buttons

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            updateResult();
        }
        private void bntFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                cmbType.SelectedIndex = getTypeIndex("File Folder");
                txtValue.Text = openFileDialog1.FileName;
            }
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                cmbType.SelectedIndex = getTypeIndex("File Folder");
                txtValue.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnNow_Click(object sender, EventArgs e)
        {
            cmbType.SelectedIndex = getTypeIndex("DateTime");
            txtValue.Text = DateTime.Now.ToString();
        }
        #endregion

        #region Format
        private void txtFormat_TextChanged(object sender, EventArgs e)
        {
            updateResult();
        }

        private void updateResult()
        {
            string value = txtValue.Text;
            string fmt = txtFormat.Text;
            switch (cmbType.Text)
            {
                case "Number":
                    decimal numValue;
                    if (decimal.TryParse(value, out numValue))
                    {
                        try
                        {
                            txtResult.Text = numValue.ToString(fmt);
                        }
                        catch
                        {
                            txtResult.Text = "Format '" + fmt + "' is strange format";
                        }
                    }
                    else
                    {
                        txtResult.Text = "Error '" + value + "' is not number";
                    }
                    break;
                case "DateTime":
                    DateTime dtValue;
                    if (DateTime.TryParse(value, out dtValue))
                    {
                        try
                        {
                            txtResult.Text = dtValue.ToString(fmt);
                        }
                        catch
                        {
                            txtResult.Text = "Error '" + fmt + "' is strange format";
                        }
                    }
                    else
                    {
                        txtResult.Text = "Error '" + value + "' is not DateTime";

                    }
                    break;
                case "File Folder":
                    txtResult.Text = Core.Automation.Commands.FilePathControls.FormatFileFolderPath(value, fmt);
                    break;
                default:
                    break;
            }
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            switch (cmbType.Text)
            {
                case "Number":
                    System.Diagnostics.Process.Start(Core.MyURLs.NumberFormatURL1);
                    System.Diagnostics.Process.Start(Core.MyURLs.NumberFormatURL2);
                    break;
                case "DateTime":
                    System.Diagnostics.Process.Start(Core.MyURLs.DateTimeFormatURL1);
                    System.Diagnostics.Process.Start(Core.MyURLs.DateTimeFormatURL2);
                    break;
                case "File Folder":
                    using (var fm = new Supplemental.frmDialog(Core.Automation.Commands.FilePathControls.GetFormatHelp(), "File Folder Formats", Supplemental.frmDialog.DialogType.OkOnly, 0))
                    {
                        fm.ShowDialog();
                    }
                    break;
            }
        }
        #endregion

        #region type
        private int getTypeIndex(string typeValue)
        {
            if (!cmbType.Items.Contains(typeValue))
            {
                return 0;
            }

            switch (typeValue)
            {
                case "Number":
                    return 0;
                case "DateTime":
                    return 1;
                case "File Folder":
                    return 2;
                default:
                    return 0;
            }
        }

        #endregion

        #region footer buttons
        private void uiBtnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region properties
        public string Format
        {
            get
            {
                return this.txtFormat.Text;
            }
        }
        public string Value
        {
            get
            {
                return this.txtValue.Text;
            }
        }
        #endregion

        #region self call
        public static void ShowFormatCheckerFormLinkClicked(TextBox formatTextBox, string type = "")
        {
             using (var fm = new frmFormatChecker(type))
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    formatTextBox.Text = fm.Format;
                }
            }
        }
        public static void ShowFormatCheckerFormLinkClicked(ComboBox formatComboBox, string type = "")
        {
            using (var fm = new frmFormatChecker(type))
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    formatComboBox.Text = fm.Format;
                }
            }
        }
        #endregion
    }
}
