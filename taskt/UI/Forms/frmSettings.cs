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
            txtServerURL.DataBindings.Add("Text", serverSettings, "ServerURL");

            var engineSettings = newAppSettings.EngineSettings;
            chkShowDebug.DataBindings.Add("Checked", engineSettings, "ShowDebugWindow", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAutoCloseWindow.DataBindings.Add("Checked", engineSettings, "AutoCloseDebugWindow", false, DataSourceUpdateMode.OnPropertyChanged);
            chkEnableLogging.DataBindings.Add("Checked", engineSettings, "EnableDiagnosticLogging", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!chkServerEnabled.Checked)
            {
                return;
            }
            if (scriptBuilderForm.CreateSocketConnection(txtServerURL.Text, txtPublicKey.Text))
            {
                lblStatus.Text = "Current Status: Connected";
            }
            else
            {
                lblStatus.Text = "Current Status: Not Connected";
            }
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
            this.Close();
        }
    }
}