using System;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using taskt.UI.Forms;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using System.Data.OleDb;
using System.Drawing;

namespace taskt.Core.Automation.Commands
{



    [Serializable]
    [Attributes.ClassAttributes.Group("Database Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to define a connection to an OLEDB data source")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to create a new connection to a database.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class DatabaseDefineConnectionCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **seleniumInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Define Connection String")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ConnectionString { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Define Connection String Password")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ConnectionStringPassword { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Test Connection Before Proceeding")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Select an option which best fits to the specification you would like to make.")]
        [Attributes.PropertyAttributes.SampleUsage("Select one of the provided options.")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TestConnection { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private TextBox ConnectionString;

        [XmlIgnore]
        [NonSerialized]
        private TextBox ConnectionStringPassword;
        public DatabaseDefineConnectionCommand()
        {
            this.CommandName = "DatabaseDefineConnectionCommand";
            this.SelectionName = "Define Database Connection";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_InstanceName = "sqlDefault";
            this.v_TestConnection = "Yes";
        }

        public override void RunCommand(object sender)
        {
            //get engine and preference
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var instance = v_InstanceName.ConvertToUserVariable(sender);
            var testPreference = v_TestConnection.ConvertToUserVariable(sender);

            //create connection
            var oleDBConnection = CreateConnection(sender);

            //attempt to open and close connection
            if (testPreference == "Yes")
            {
                oleDBConnection.Open();
                oleDBConnection.Close();
            }

            engine.AddAppInstance(instance, oleDBConnection);

        }
        private OleDbConnection CreateConnection(object sender)
        {
            var connection = v_ConnectionString.ConvertToUserVariable(sender);
            var connectionPass = v_ConnectionStringPassword.ConvertToUserVariable(sender);

            if (connectionPass.StartsWith("!"))
            {
                connectionPass = connectionPass.Substring(1);
                connectionPass = EncryptionServices.DecryptString(connectionPass, "taskt-database-automation");
            }

            connection = connection.Replace("#pwd", connectionPass);

            return new OleDbConnection(connection);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            CommandItemControl helperControl = new CommandItemControl();
            helperControl.Padding = new Padding(10, 0, 0, 0);
            helperControl.ForeColor = Color.AliceBlue;
            helperControl.Font = new Font("Segoe UI Semilight", 10);
            helperControl.Name = "connection_helper";
            helperControl.CommandImage = UI.Images.GetUIImage("VariableCommand");
            helperControl.CommandDisplay = "Build Connection String";
            helperControl.Click += (sender, e) => Button_Click(sender, e);


            ConnectionString = (TextBox)CommandControls.CreateDefaultInputFor("v_ConnectionString", this);

            var connectionLabel = CommandControls.CreateDefaultLabelFor("v_ConnectionString", this);
            var connectionHelpers = CommandControls.CreateUIHelpersFor("v_ConnectionString", this, new[] { ConnectionString }, editor);
            CommandItemControl testConnectionControl = new CommandItemControl();
            testConnectionControl.Padding = new Padding(10, 0, 0, 0);
            testConnectionControl.ForeColor = Color.AliceBlue;
            testConnectionControl.Font = new Font("Segoe UI Semilight", 10);
            testConnectionControl.Name = "connection_helper";
            testConnectionControl.CommandImage = UI.Images.GetUIImage("VariableCommand");
            testConnectionControl.CommandDisplay = "Test Connection";
            testConnectionControl.Click += (sender, e) => TestConnection(sender, e);
            RenderedControls.Add(testConnectionControl);

            RenderedControls.Add(connectionLabel);
            RenderedControls.Add(helperControl);
            RenderedControls.AddRange(connectionHelpers);
            RenderedControls.Add(testConnectionControl);
            RenderedControls.Add(ConnectionString);

            ConnectionStringPassword = (TextBox)CommandControls.CreateDefaultInputFor("v_ConnectionStringPassword", this);

            var connectionPassLabel = CommandControls.CreateDefaultLabelFor("v_ConnectionStringPassword", this);
            var connectionPassHelpers = CommandControls.CreateUIHelpersFor("v_ConnectionStringPassword", this, new[] { ConnectionStringPassword }, editor);

            RenderedControls.Add(connectionPassLabel);
            RenderedControls.AddRange(connectionPassHelpers);

            CommandItemControl passwordHelperControl = new CommandItemControl();
            passwordHelperControl.Padding = new Padding(10, 0, 0, 0);
            passwordHelperControl.ForeColor = Color.AliceBlue;
            passwordHelperControl.Font = new Font("Segoe UI Semilight", 10);
            passwordHelperControl.Name = "show_pass_helper";
            passwordHelperControl.CommandImage = UI.Images.GetUIImage("VariableCommand");
            passwordHelperControl.CommandDisplay = "Show Password";
            passwordHelperControl.Click += (sender, e) => TogglePasswordChar(passwordHelperControl, e);

            RenderedControls.Add(passwordHelperControl);


            CommandItemControl encryptHelperControl = new CommandItemControl();
            encryptHelperControl.Padding = new Padding(10, 0, 0, 0);
            encryptHelperControl.ForeColor = Color.AliceBlue;
            encryptHelperControl.Font = new Font("Segoe UI Semilight", 10);
            encryptHelperControl.Name = "show_pass_helper";
            encryptHelperControl.CommandImage = UI.Images.GetUIImage("VariableCommand");
            encryptHelperControl.CommandDisplay = "Encrypt Password";
            encryptHelperControl.Click += (sender, e) => EncryptPassword(passwordHelperControl, e);
            RenderedControls.Add(encryptHelperControl);

            
            var label = new Label();
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            label.ForeColor = Color.White;
            label.Text = "NOTE: If storing the password in the textbox below, please ensure the connection string above contains a database-specific placeholder with #pwd to be replaced at runtime. (;Password=#pwd)";
            RenderedControls.Add(label);


            RenderedControls.Add(ConnectionStringPassword);
            ConnectionStringPassword.PasswordChar = '*';

          

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TestConnection", this, editor));

            return RenderedControls;

        }
        private void TestConnection(object sender, EventArgs e)
        {
            try
            {
                var engine = new Engine.AutomationEngineInstance();
                var oleDBConnection = CreateConnection(engine);
                oleDBConnection.Open();
                oleDBConnection.Close();
                MessageBox.Show("Connection Successful", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection Failed: {ex.ToString()}", "Test Connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
          


        }

        private void Button_Click(object sender, EventArgs e)
        {
            ShowConnectionBuilder();
        }
        private void TogglePasswordChar(CommandItemControl sender, EventArgs e)
        {
            //if password is hidden
            if (ConnectionStringPassword.PasswordChar == '*')
            {
                //show password plain text
                sender.CommandDisplay = "Hide Password";
                ConnectionStringPassword.PasswordChar = '\0';
            }
            else
            {
                //mask password with chars
                sender.CommandDisplay = "Show Password";
                ConnectionStringPassword.PasswordChar = '*';
            }
        }
        private void EncryptPassword(CommandItemControl sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConnectionStringPassword.Text))
            {
                return;
            }

           var acknowledgement =  MessageBox.Show("WARNING! This function will encrypt the password locally but is not extremely secure as the client knows the secret!  Consider using a password management service instead. The encrypted password will be stored with a leading exclamation ('!') whch the automation engine will detect and know to decrypt the value automatically at run-time. Do not encrypt the password multiple times or the decryption will be invalid!  Would you like to proceed?", "Encryption Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (acknowledgement == DialogResult.Yes)
            {
                ConnectionStringPassword.Text = string.Concat($"!{EncryptionServices.EncryptString(ConnectionStringPassword.Text, "taskt-database-automation")}");
            }

        }

        public void ShowConnectionBuilder()
        {

            var MSDASCObj = new MSDASC.DataLinks();
            var connection = new ADODB.Connection();
            MSDASCObj.PromptEdit(connection);

            if (!string.IsNullOrEmpty(connection.ConnectionString))
            {
                ConnectionString.Text = connection.ConnectionString;
            }
        }

        public override string GetDisplayValue()
        {
            return $"{base.GetDisplayValue()} - [Instance Name: '{v_InstanceName}']";
        }
    }
}