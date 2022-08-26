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
        private const string NumberFormatURL1 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings";
        private const string NumberFormatURL2 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-numeric-format-strings";

        private const string DateTimeFormatURL1 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-date-and-time-format-strings";
        private const string DateTimeFormatURL2 = "https://docs.microsoft.com/en-us/dotnet/standard/base-types/custom-date-and-time-format-strings";

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
                    txtResult.Text = Core.FilePathControls.formatFileFolderPath(value, fmt);
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
                    System.Diagnostics.Process.Start(NumberFormatURL1);
                    System.Diagnostics.Process.Start(NumberFormatURL2);
                    break;
                case "DateTime":
                    System.Diagnostics.Process.Start(DateTimeFormatURL1);
                    System.Diagnostics.Process.Start(DateTimeFormatURL2);
                    break;
                case "File Folder":
                    using (var fm = new Supplemental.frmDialog(Core.FilePathControls.getFormatHelp(), "File Folder Formats", Supplemental.frmDialog.DialogType.OkOnly, 0))
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
                    break;
                case "DateTime":
                    return 1;
                    break;
                case "File Folder":
                    return 2;
                    break;
                default:
                    return 0;
                    break;
            }
        }
        #endregion

       
    }
}
