using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using RestSharp;
using System.Data;
using System.Drawing;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Remote Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to show a message to the user.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class RemoteTaskCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the IP:Port (ex. 192.168.2.200:19312)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define any API endpoint which contains the full URL.")]
        [Attributes.PropertyAttributes.SampleUsage("**https://example.com** or **{vMyUrl}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_BaseURL { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select Parameter Type")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Run Raw Script Data")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Run Local File")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Run Remote File")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary method type.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ParameterType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Wait for Script to Finish?")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Continue Execution")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Await For Result")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary method type.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ExecuteAwait { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Script Parameter Data")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify a list of advanced parameters.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_Parameter { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select the variable to receive the response")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        public RemoteTaskCommand()
        {
            this.CommandName = "RemoteTaskCommand";
            this.SelectionName = "Remote Task";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_ExecuteAwait = "Continue Execution";
        }

        public override void RunCommand(object sender)
        {

            try
            {
                var server = v_BaseURL.ConvertToUserVariable(sender);
                var paramType = v_ParameterType.ConvertToUserVariable(sender);
                var parameter = v_Parameter.ConvertToUserVariable(sender);
                var awaitPreference = v_ExecuteAwait.ConvertToUserVariable(sender);

                var response = Server.LocalTCPListener.SendAutomationTask(server, paramType, parameter, awaitPreference);

                response.StoreInUserVariable(sender, v_userVariableName);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_BaseURL", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ParameterType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Parameter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ExecuteAwait", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_ParameterType} on {v_BaseURL}]";
        }

    }

}

