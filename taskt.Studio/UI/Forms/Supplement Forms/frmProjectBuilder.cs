using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Enums;
using taskt.Core.IO;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmProjectBuilder : ThemedForm
    {
        public bool CreateProject { get; private set; }
        public bool OpenProject { get; private set; }
        public string NewProjectName { get; private set; }
        public string NewProjectPath { get; private set; }
        public string ExistingProjectPath { get; private set; }
        public string ExistingMainPath { get; private set; }

        public frmProjectBuilder()
        {
            InitializeComponent();
            txtNewProjectLocation.Text = Folders.GetFolder(FolderType.ScriptsFolder);
        }

        private void btnCreateProject_Click(object sender, EventArgs e)
        {
            string newProjectLocation = txtNewProjectLocation.Text.Trim();
            NewProjectName = txtNewProjectName.Text.Trim();

            if (string.IsNullOrEmpty(NewProjectName) || string.IsNullOrEmpty(newProjectLocation) || !Directory.Exists(newProjectLocation))
            {
                lblError.Text = "Error: Please enter a valid project name and location";
            }
            else {
                try {
                    NewProjectPath = Path.Combine(newProjectLocation, NewProjectName);
                    bool isInvalidProjectName = new[] { @"/", @"\" }.Any(c => NewProjectName.Contains(c));
                    if (isInvalidProjectName)
                        throw new Exception("Illegal characters in path");

                    if (!Directory.Exists(NewProjectPath))
                    {
                        Directory.CreateDirectory(NewProjectPath);
                        CreateProject = true;
                        DialogResult = DialogResult.OK;
                    }
                    else
                        lblError.Text = "Error: Project already exists";
                }
                catch(Exception ex)
                {
                    lblError.Text = "Error: " + ex.Message;
                }
            }
        }

        private void btnOpenProject_Click(object sender, EventArgs e)
        {
            ExistingMainPath = txtExistingProjectLocation.Text.Trim();
            if (ExistingMainPath == string.Empty || !File.Exists(ExistingMainPath))
            {
                lblError.Text = "Error: Please enter a valid project path";
            }
            else
            {
                ExistingProjectPath = Directory.GetParent(ExistingMainPath).ToString();
                OpenProject = true;
                DialogResult = DialogResult.OK;
            }
        }

        private void btnFolderManager_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtNewProjectLocation.Text = fbd.SelectedPath;
            }
        }

        private void btnFileManager_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtExistingProjectLocation.Text = ofd.FileName;
            }
        }
    }
}
