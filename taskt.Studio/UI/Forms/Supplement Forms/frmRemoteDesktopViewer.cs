using System;
using System.Windows.Forms;
using taskt.Core.Enums;
using taskt.UI.FormEventArgs;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmRemoteDesktopViewer : Form
    {
        public event EventHandler<LoginResultEventArgs> LoginUpdateEvent;

        public frmRemoteDesktopViewer(string machineName, string userName, string password, int totalWidth,
            int totalHeight, bool hideDisplay, bool minimizeOnStart)
        {
            InitializeComponent();

            //detect if we should attempt to hide the display
            if (hideDisplay)
            {
                pnlCover.Dock = DockStyle.Fill;
            }
            else
            {
                pnlCover.Hide();
            }

            //set text and form properties
            Text = "Remote Desktop - Machine: " + machineName + " | User: " + userName;
            Width = totalWidth;
            Height = totalHeight;

            //declare credentials
            axRDP.Server = machineName;
            axRDP.UserName = userName;
            axRDP.AdvancedSettings7.ClearTextPassword = password;

            //defaults to false
            axRDP.AdvancedSettings7.RedirectDrives = false;
            axRDP.AdvancedSettings7.RedirectPrinters = false;
            axRDP.AdvancedSettings7.RedirectClipboard = false;

            //initiate connection
            axRDP.Connect();

            //declare timeout
            tmrLoginFailure.Enabled = true;

            if (minimizeOnStart)
            {
                WindowState = FormWindowState.Minimized;
            }
        }

        private void frmRemoteDesktopViewer_Load(object sender, EventArgs e)
        {
        }

        private void axRDP_OnDisconnected(object sender, AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEvent e)
        {
            //codes https://social.technet.microsoft.com/wiki/contents/articles/37870.rds-remote-desktop-client-disconnect-codes-and-reasons.aspx
            if (e.discReason != 3)
            {
                LoginUpdateEvent?.Invoke(this, new LoginResultEventArgs(axRDP.Server, LoginResultCode.Failed,
                    e.discReason.ToString()));
            }

            Close();
        }

        private void axRDP_OnLoginComplete(object sender, EventArgs e)
        {
           tmrLoginFailure.Enabled = false;
           LoginUpdateEvent?.Invoke(this, new LoginResultEventArgs(axRDP.Server, LoginResultCode.Success, ""));
        }

        private void tmrLoginFailure_Tick(object sender, EventArgs e)
        {
            tmrLoginFailure.Enabled = false;
            LoginUpdateEvent?.Invoke(this, new LoginResultEventArgs(axRDP.Server, LoginResultCode.Failed, "Timeout"));
        }

        private void pnlCover_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            pnlCover.Hide();
        }
    }
}
