using System;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.UI.Forms.ScriptBuilder.CommandEditor.Supplemental
{
    public partial class frmUIElementSearchParameter : DialogLikeThemedForm
    {
        private DataTable searchParameters = new DataTable();

        private string xpath = string.Empty;

        public frmUIElementSearchParameter()
        {
            InitializeComponent();
        }

        private void frmUIElementSearchParameter_Load(object sender, EventArgs e)
        {
            searchParameters.Columns.Add("Enabled");
            searchParameters.Columns.Add("ParameterName");
            searchParameters.Columns.Add("ParameterValue");

            dgvSearchParameters.DataSource = searchParameters;
            VP_UIElementControls.CreateEmptySearchParamters(searchParameters);
            EM_UIElementChildrenSearchParametersPropertiesExtensionMethods.RenderUIElementSearchParameter(dgvSearchParameters);

            var enabledColumn = new DataGridViewCheckBoxColumn
            {
                HeaderText = "Enabled"
            };
            dgvSearchParameters.Columns.RemoveAt(0);
            dgvSearchParameters.Columns.Insert(0, enabledColumn);
            for (int row = dgvSearchParameters.Rows.Count - 1; row >= 0; row--)
            {
                dgvSearchParameters.Rows[row].Cells[0].Value = false;
            }
        }


        private void uiBtnOK_Click(object sender, EventArgs e)
        {
            this.xpath = CreateXPath();
            this.DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// create UIElement search XPath from DGV
        /// </summary>
        /// <returns></returns>
        private string CreateXPath()
        {
            dgvSearchParameters.EndEdit();

            string searchParams = string.Empty;
            string searchTag = "*";

            for (int row = 0; row < searchParameters.Rows.Count; row++)
            {
                var r = dgvSearchParameters.Rows[row];
                if ((bool)r.Cells[0].Value)
                {
                    if (r.Cells[1].Value.ToString() == "ControlType")
                    {
                        var tag = r.Cells[2].Value.ToString();
                        if (tag != string.Empty)
                        {
                            searchTag = tag;
                        }
                    }
                    else
                    {
                        searchParams += $"[@{r.Cells[1].Value}=\"{r.Cells[2].Value}\"]";
                    }
                }
            }

            return $"//{searchTag}{searchParams}";
        }

        /// <summary>
        /// get xpath
        /// </summary>
        public string XPath
        {
            get
            {
                return this.xpath;
            }
        }
    }
}
