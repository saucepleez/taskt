using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add an in-line comment to the script.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add code comments or document code.  Usage of variables (ex. [vVar]) within the comment block will be parsed and displayed when running the script.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is for visual purposes only")]
    public class PingCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter ip address or host name that you want to ping")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Ip address or hostname you want to ping")]
        [Attributes.PropertyAttributes.SampleUsage("** 192.168.0.1 or www.google.com**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_HostName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply Result To Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }
        public PingCommand()
        {
            this.CommandName = "PingCommand";
            this.SelectionName = "Ping Command";
            this.DisplayForeColor = System.Drawing.Color.ForestGreen;
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_HostName", this, editor));


            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }

        public override void RunCommand(object sender)
        {

            Ping ping = new Ping();
            string hstname = v_HostName.ConvertToUserVariable(sender);

            PingReply pingresult = ping.Send(hstname);
            var pingReply = Common.ConvertObjectToJson(pingresult);

            pingReply.StoreInUserVariable(sender, v_userVariableName);

        }

        public override string GetDisplayValue()
        {
            return $"Ping Host '{this.v_HostName}'";
        }
    }
}