using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmProjectBuilder : ThemedForm
    {
        public bool createProject { get; private set; }
        public bool openProject { get; private set; }
        public string newProjectName { get; private set; }
        public string newProjectPath { get; private set; }
        public string existingProjectPath { get; private set; }
        public string existingMainPath { get; private set; }
        public frmProjectBuilder()
        {
            InitializeComponent();
        }

        private void btnCreateProject_Click(object sender, EventArgs e)
        {
            
            string newProjectLocation = txtNewProjectLocation.Text.Trim();
            newProjectName = txtNewProjectName.Text.Trim();

            if (String.IsNullOrEmpty(newProjectName) || String.IsNullOrEmpty(newProjectLocation) || !Directory.Exists(newProjectLocation))
            {
                lblError.Text = "Error: Please enter a valid project name and location";
            }
            else {
                try { 
                    newProjectPath = Path.Combine(newProjectLocation, newProjectName);
                    bool isInvalidProjectName = new[] { @"/", @"\" }.Any(c => newProjectName.Contains(c));
                    if (isInvalidProjectName)
                        throw new Exception("Illegal characters in path");

                    if (!Directory.Exists(newProjectPath))
                    {
                        Directory.CreateDirectory(newProjectPath);
                        createProject = true;
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
            existingMainPath = txtExistingProjectLocation.Text.Trim();
            if (existingMainPath == string.Empty || !File.Exists(existingMainPath))
            {
                lblError.Text = "Error: Please enter a valid project path";
            }
            else
            {
                FileInfo mainFileInfo = new FileInfo(existingMainPath);
                string mainFileName = mainFileInfo.Name;
                if (mainFileName != "Main.xml")
                    lblError.Text = "Error: Please enter a path containing Main.xml";
                else
                {
                    existingProjectPath = Directory.GetParent(existingMainPath).ToString();
                    openProject = true;
                    DialogResult = DialogResult.OK;
                }
                
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
