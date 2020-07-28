using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using taskt.Core.Enums;
using taskt.UI.DTOs;
using taskt.UI.FormEventArgs;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmDisplayManager : UIForm
    {
        private BindingList<MachineConfiguration> _machines = new BindingList<MachineConfiguration>();

        public frmDisplayManager()
        {
            InitializeComponent();
        }

        private void frmDisplayManager_Load(object sender, EventArgs e)
        {
            dgvMachines.DataSource = _machines;
        }

        private void btnAddMachine_Click(object sender, EventArgs e)
        {
            var newMachine = new MachineConfiguration();
            newMachine.MachineName = "HostName";
            newMachine.UserName = "Administrator";
            newMachine.Password = "12345";
            newMachine.NextConnectionDue = DateTime.Now;
            newMachine.LastKnownStatus = "Just Added";
            _machines.Add(newMachine);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_machines.Count == 0)
            {
                LogEvent("No machines were found!");
                return;
            }

            LogEvent("Enabling Remote Desktop Polling");
            dgvMachines.ReadOnly = true;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            tmrCheck.Enabled = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            tmrCheck.Enabled = false;
            LogEvent("Disabling Remote Desktop Polling");
            dgvMachines.ReadOnly = false;
            btnStart.Enabled = true;
            btnStop.Enabled = false;
        }

        private void tmrCheck_Tick(object sender, EventArgs e)
        {
            dgvMachines.Refresh();

            foreach (var machine in _machines)
            {
                if ((string.IsNullOrEmpty(machine.MachineName)) ||
                    (string.IsNullOrEmpty(machine.UserName)) ||
                    (string.IsNullOrEmpty(machine.Password)))
                {
                    continue;
                }

                if (machine.NextConnectionDue <= DateTime.Now)
                {
                    int windowWidth, windowHeight;

                    if (!int.TryParse(txtWidth.Text, out windowWidth))
                    {
                        windowWidth = 1920;
                    }

                    if (!int.TryParse(txtHeight.Text, out windowHeight))
                    {
                        windowHeight = 1080;
                    }

                    LogEvent("Machine '" + machine.MachineName + "' is due for desktop login");
                    machine.LastKnownStatus = "Attempting to login";
                    machine.NextConnectionDue = DateTime.Now.AddMinutes(2);
                    LogEvent("Next Connection for Machine '" + machine.MachineName + "' due at '" + machine.NextConnectionDue + "'");
                    frmRemoteDesktopViewer viewer = new frmRemoteDesktopViewer(
                        machine.MachineName,
                        machine.UserName,
                        machine.Password,
                        windowWidth,
                        windowHeight,
                        chkHideScreen.Checked,
                        chkStartMinimized.Checked
                        );
                    viewer.LoginUpdateEvent += Viewer_LoginUpdateEvent;
                    viewer.Show();
                }
            }
        }

        private void Viewer_LoginUpdateEvent(object sender, LoginResultEventArgs e)
        {
            //var frmViewer = (Supplement_Forms.frmRemoteDesktopViewer)sender;
            var connResult = e.Result.ToString();
            LogEvent("Machine '" + e.MachineName + "' login attempt was '" + connResult + "' " + e.AdditionalDetail);

            var machine = _machines.Where(f => f.MachineName == e.MachineName).FirstOrDefault();

            var status = "Connection Result: '" + connResult + "'";
            if (!string.IsNullOrEmpty(e.AdditionalDetail))
            {
                status += " (" + e.AdditionalDetail + ")";
            }

            machine.LastKnownStatus = status;

            if (e.Result == LoginResultCode.Failed)
            {
                var frmSender = (frmRemoteDesktopViewer)sender;
                frmSender.Close();
            }
        }

        private void LogEvent(string log)
        {
            lstEventLogs.Items.Add(DateTime.Now.ToString() + " - " + log);
            lstEventLogs.SelectedIndex = lstEventLogs.Items.Count - 1;
        }
    }
}

