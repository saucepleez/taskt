using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmDisplayManager : UIForm
    {
        public frmDisplayManager()
        {
            InitializeComponent();
        }
        BindingList<MachineConfiguration> Machines = new BindingList<MachineConfiguration>();
        private void frmDisplayManager_Load(object sender, EventArgs e)
        {
            dgvMachines.DataSource = Machines;
        }

        private void btnAddMachine_Click(object sender, EventArgs e)
        {
            var newMachine = new MachineConfiguration();
            newMachine.MachineName = "HostName";
            newMachine.UserName = "Administrator";
            newMachine.Password = "12345";
            newMachine.NextConnectionDue = DateTime.Now;
            newMachine.LastKnownStatus = "Just Added";
            Machines.Add(newMachine);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Machines.Count == 0)
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

            foreach (var machine in Machines)
            {
                if ((string.IsNullOrEmpty(machine.MachineName)) || (string.IsNullOrEmpty(machine.UserName)) || (string.IsNullOrEmpty(machine.Password)))
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
                    Supplemental.frmRemoteDesktopViewer viewer = new Supplemental.frmRemoteDesktopViewer(machine.MachineName, machine.UserName, machine.Password, windowWidth, windowHeight, chkHideScreen.Checked, chkStartMinimized.Checked);
                    viewer.LoginUpdateEvent += Viewer_LoginUpdateEvent;
                    viewer.Show();
                }
            }

        }

        private void Viewer_LoginUpdateEvent(object sender, Supplemental.LoginResultArgs e)
        {
            //var frmViewer = (Supplement_Forms.frmRemoteDesktopViewer)sender;
            var connResult = e.Result.ToString();
            LogEvent("Machine '" + e.MachineName + "' login attempt was '" + connResult + "' " + e.AdditionalDetail);

            var machine = Machines.Where(f => f.MachineName == e.MachineName).FirstOrDefault();

            var status = "Connection Result: '" + connResult + "'";
            if (!string.IsNullOrEmpty(e.AdditionalDetail))
            {
                status += " (" + e.AdditionalDetail + ")";
            }


            machine.LastKnownStatus = status;


            if (e.Result == Supplemental.LoginResultArgs.LoginResultCode.Failed)
            {
                var frmSender = (Supplemental.frmRemoteDesktopViewer)sender;
                frmSender.Close();
            }


        }

        private void LogEvent(string log)
        {
            lstEventLogs.Items.Add(DateTime.Now.ToString() + " - " + log);
            lstEventLogs.SelectedIndex = lstEventLogs.Items.Count - 1;
        }



        public class MachineConfiguration
        {
            public string MachineName { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public DateTime NextConnectionDue { get; set; }
            public string LastKnownStatus { get; set; }
        }
    }
}

