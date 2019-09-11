using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace taskt.UI.Forms.Supplemental
{
    public partial class frmRemoteDesktopViewer : Form
    {
        public event EventHandler<LoginResultArgs> LoginUpdateEvent;
        public frmRemoteDesktopViewer(string machineName, string userName, string password, int totalWidth, int totalHeight, bool hideDisplay, bool minimizeOnStart)
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
            this.Text = "Remote Desktop - Machine: " + machineName + " | User: " + userName; 
            this.Width = totalWidth;
            this.Height = totalHeight;

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
                this.WindowState = FormWindowState.Minimized;
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
                LoginUpdateEvent?.Invoke(this, new LoginResultArgs(axRDP.Server, LoginResultArgs.LoginResultCode.Failed, e.discReason.ToString()));
            }

            this.Close();
        }

        private void axRDP_OnLoginComplete(object sender, EventArgs e)
        {
           tmrLoginFailure.Enabled = false;
           LoginUpdateEvent?.Invoke(this, new LoginResultArgs(axRDP.Server, LoginResultArgs.LoginResultCode.Success, ""));
        }
        private void tmrLoginFailure_Tick(object sender, EventArgs e)
        {
            tmrLoginFailure.Enabled = false;
            LoginUpdateEvent?.Invoke(this, new LoginResultArgs(axRDP.Server, LoginResultArgs.LoginResultCode.Failed, "Timeout"));
        }

        private void pnlCover_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            pnlCover.Hide();
        }


    }

    public class LoginResultArgs
    {
       
        public LoginResultArgs(string userName, LoginResultCode result, string additionalDetail)
        {
            this.MachineName = userName;
            this.Result = result;
            this.AdditionalDetail = additionalDetail;
        }
        public LoginResultCode Result;
        public string MachineName { get; set; }
        public string AdditionalDetail { get; set; }
        public enum LoginResultCode
        {
            Success,
            Failed
        }
    }
}
