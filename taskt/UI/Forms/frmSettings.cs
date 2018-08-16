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
    }
}