using System;
using System.Windows.Forms;

namespace taskt.UI.Forms.ScriptBuilder.Supplemental
{
    public partial class frmScriptInformations : ThemedForm
    {
        //public frmScriptBuilder scriptBuilderForm;
        public Core.Script.ScriptInformation infos { get; set; }
        public frmScriptInformations()
        {
            InitializeComponent();
        }
        private void frmScriptInformations_Load(object sender, EventArgs e)
        {
            txtScriptAuthor.DataBindings.Add("Text", infos, "Author", false, DataSourceUpdateMode.OnPropertyChanged);
            txtScriptVersion.DataBindings.Add("Text", infos, "ScriptVersion", false, DataSourceUpdateMode.OnPropertyChanged);
            txtScriptDescription.DataBindings.Add("Text", infos, "Description", false, DataSourceUpdateMode.OnPropertyChanged);
            txtTasktVersion.Text = infos.TasktVersion;
            txtRunTimes.Text = infos.RunTimes.ToString();
            txtLastRun.Text = infos.LastRunTime.ToString();
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
