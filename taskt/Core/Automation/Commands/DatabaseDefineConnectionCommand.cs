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
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var connection = v_ConnectionString.ConvertToUserVariable(sender);
            var instance = v_InstanceName.ConvertToUserVariable(sender);
            var testPreference = v_TestConnection.ConvertToUserVariable(sender);

            var oleDBConnection = new OleDbConnection(connection);

            //attempt to open and close connection
            if (testPreference == "Yes")
            {
                oleDBConnection.Open();
                oleDBConnection.Close();
            }

            engine.AddAppInstance(instance, oleDBConnection);

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

            RenderedControls.Add(connectionLabel);
            RenderedControls.Add(helperControl);
            RenderedControls.AddRange(connectionHelpers);
            RenderedControls.Add(ConnectionString);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TestConnection", this, editor));

            return RenderedControls;

        }

        private void Button_Click(object sender, EventArgs e)
        {
            ShowConnectionBuilder();
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