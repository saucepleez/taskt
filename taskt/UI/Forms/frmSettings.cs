//Copyright (c) 2018 Jason Bayldon
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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocket4Net;

namespace taskt.UI.Forms
{
    public partial class frmSettings : UI.Forms.UIForm
    {
        Core.ApplicationSettings newAppSettings;
        public frmScriptBuilder scriptBuilderForm;
        public frmSettings(frmScriptBuilder sender)
        {
            scriptBuilderForm = sender;
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            newAppSettings = new Core.ApplicationSettings();
            newAppSettings = newAppSettings.GetOrCreateApplicationSettings();

            var serverSettings = newAppSettings.ServerSettings;
            chkServerEnabled.DataBindings.Add("Checked", serverSettings, "ServerConnectionEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAutomaticallyConnect.DataBindings.Add("Checked", serverSettings, "ConnectToServerOnStartup", false, DataSourceUpdateMode.OnPropertyChanged);
            chkRetryOnDisconnect.DataBindings.Add("Checked", serverSettings, "RetryServerConnectionOnFail", false, DataSourceUpdateMode.OnPropertyChanged);
            chkBypassValidation.DataBindings.Add("Checked", serverSettings, "BypassCertificateValidation", false, DataSourceUpdateMode.OnPropertyChanged);
            txtPublicKey.DataBindings.Add("Text", serverSettings, "ServerPublicKey", false, DataSourceUpdateMode.OnPropertyChanged);
            txtServerURL.DataBindings.Add("Text", serverSettings, "ServerURL", false, DataSourceUpdateMode.OnPropertyChanged);
           

            var engineSettings = newAppSettings.EngineSettings;
            chkShowDebug.DataBindings.Add("Checked", engineSettings, "ShowDebugWindow", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAutoCloseWindow.DataBindings.Add("Checked", engineSettings, "AutoCloseDebugWindow", false, DataSourceUpdateMode.OnPropertyChanged);
            chkEnableLogging.DataBindings.Add("Checked", engineSettings, "EnableDiagnosticLogging", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAdvancedDebug.DataBindings.Add("Checked", engineSettings, "ShowAdvancedDebugOutput", false, DataSourceUpdateMode.OnPropertyChanged);
            chkCreateMissingVariables.DataBindings.Add("Checked", engineSettings, "CreateMissingVariablesDuringExecution", false, DataSourceUpdateMode.OnPropertyChanged);

            var clientSettings = newAppSettings.ClientSettings;
            chkAntiIdle.DataBindings.Add("Checked", clientSettings, "AntiIdleWhileOpen", false, DataSourceUpdateMode.OnPropertyChanged);
            txtAppFolderPath.DataBindings.Add("Text", clientSettings, "RootFolder", false, DataSourceUpdateMode.OnPropertyChanged);


        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Core.Sockets.SocketClient.Connect(txtServerURL.Text);
        }


        private void btnAddAssembly_Click(object sender, EventArgs e)
        {
            //allow importing custom command assemblies
            OpenFileDialog newOpenDialog = new OpenFileDialog();

            if (newOpenDialog.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            newAppSettings.Save(newAppSettings);
            taskt.Core.Sockets.SocketClient.LoadSettings();
            this.Close();
        }

        private void btnUpdateCheck_Click(object sender, EventArgs e)
        {
            taskt.Core.ApplicationUpdate updater = new Core.ApplicationUpdate();
            Core.UpdateManifest manifest = new Core.UpdateManifest();
            try
            {
                 manifest = updater.GetManifest();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting manifest: " + ex.ToString());
                return;
            }
         

            if (manifest.RemoteVersionNewer)
            {
                Supplement_Forms.frmUpdate frmUpdate = new Supplement_Forms.frmUpdate(manifest);
                if (frmUpdate.ShowDialog() == DialogResult.OK)
                {

                    //move update exe to root folder for execution
                    var updaterExecutionResources = Application.StartupPath + "\\resources\\taskt-updater.exe";                
                    var updaterExecutableDestination = Application.StartupPath + "\\taskt-updater.exe";


                    if (!System.IO.File.Exists(updaterExecutionResources))
                    {
                        MessageBox.Show("taskt-updater.exe not found in resources directory!");
                        return;
                    }
                    else
                    {
                        System.IO.File.Copy(updaterExecutionResources, updaterExecutableDestination);
                    }

                    var updateProcess = new System.Diagnostics.Process();
                    updateProcess.StartInfo.FileName = updaterExecutableDestination;
                    updateProcess.StartInfo.Arguments = manifest.PackageURL;

                    updateProcess.Start();
                    Application.Exit();

                }

            }
            else {
                MessageBox.Show("The application is currently up-to-date!", "No Updates Available", MessageBoxButtons.OK);         
            }
            
        }

        private void tmrGetSocketStatus_Tick(object sender, EventArgs e)
        {
            lblStatus.Text = "Socket Status: " + Core.Sockets.SocketClient.GetSocketState();
            if (Core.Sockets.SocketClient.connectionException != string.Empty)
            {
                lblSocketException.Show();
                lblSocketException.Text = Core.Sockets.SocketClient.connectionException;
            }
            else
            {
                lblSocketException.Hide();
            }
        
        }

        private void btnCloseConnection_Click(object sender, EventArgs e)
        {
            Core.Sockets.SocketClient.Disconnect();
        }

        private void chkBypassValidation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBypassValidation.Checked)
            {
                MessageBox.Show("Bypassing SSL Certificate Validation procedures is inherently insecure as the client will trust any server certificate.  Please consider issuing proper SSL Certificates.", "Warning - Insecure", MessageBoxButtons.OK);
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
         
            //prompt user to confirm they want to select a new folder
            var updateFolderRequest = MessageBox.Show("Would you like to change the default root folder that taskt uses to store tasks and information? " + Environment.NewLine + Environment.NewLine +
                "Current Root Folder: " + txtAppFolderPath.Text, "Change Default Root Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if user does not want to update folder then exit
            if (updateFolderRequest == DialogResult.No)
                return;

            //user folder browser to let user select top level folder
            using (var fbd = new FolderBrowserDialog())
            {
         
                //check if user selected a folder
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    //create references to old and new root folders
                    var oldRootFolder = txtAppFolderPath.Text;
                    var newRootFolder = System.IO.Path.Combine(fbd.SelectedPath, "taskt");

                    //ask user to confirm
                    var confirmNewFolderSelection = MessageBox.Show("Please confirm the changes below:" + Environment.NewLine + Environment.NewLine +
                    "Old Root Folder: " + oldRootFolder + Environment.NewLine + Environment.NewLine +
                    "New Root Folder: " + newRootFolder, "Change Default Root Folder", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    //handle if user decides to cancel
                    if (confirmNewFolderSelection == DialogResult.Cancel)
                        return;

                    //ask if we should migrate the data
                    var migrateCopyData = MessageBox.Show("Would you like to attempt to move the data from the old folder to the new folder?  Please note, depending on how many files you have, this could take a few minutes.", "Migrate Data?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    //check if user wants to migrate data
                    if (migrateCopyData == DialogResult.Yes)
                    {

                        try
                        {
                            //find and copy files
                            foreach (string dirPath in System.IO.Directory.GetDirectories(oldRootFolder, "*", SearchOption.AllDirectories))
                            {
                                System.IO.Directory.CreateDirectory(dirPath.Replace(oldRootFolder, newRootFolder));
                            }
                            foreach (string newPath in Directory.GetFiles(oldRootFolder, "*.*", SearchOption.AllDirectories))
                            {
                                System.IO.File.Copy(newPath, newPath.Replace(oldRootFolder, newRootFolder), true);
                            }

                            MessageBox.Show("Data Migration Complete", "Data Migration Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        catch (Exception ex)
                        {
                            //handle any unexpected errors
                            MessageBox.Show("An Error Occured during Data Migration Copy: " + ex.ToString());
                        }
                      
                    }

                    //update textbox which will be updated once user selects "Ok"
                    txtAppFolderPath.Text = newRootFolder;


                }
            }
        }
    }
}