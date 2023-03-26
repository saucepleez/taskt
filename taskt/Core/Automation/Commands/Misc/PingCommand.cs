using System;
using System.Net.NetworkInformation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Network/Internet")]
    [Attributes.ClassAttributes.CommandSettings("Ping")]
    [Attributes.ClassAttributes.Description("This command allows you to add an in-line comment to the script.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add code comments or document code.  Usage of variables (ex. [vVar]) within the comment block will be parsed and displayed when running the script.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is for visual purposes only")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class PingCommand : ScriptCommand
    {

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("IP Address or Host Name that you want to ping (ex. 192.168.0.1, {{{vHost}}})")]
        [InputSpecification("IP Address or Host Name", true)]
        [PropertyDetailSampleUsage("**192.168.0.1**", PropertyDetailSampleUsage.ValueType.Value, "IP Address")]
        [PropertyDetailSampleUsage("**http://example.com**", PropertyDetailSampleUsage.ValueType.Value, "Host Name")]
        [PropertyDetailSampleUsage("**{{{vHost}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "IP or Host")]
        [PropertyValidationRule("IP or Host", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "IP or Host")]
        public string v_HostName { get; set; }

        // TODO: json only?
        [XmlAttribute]
        //[PropertyDescription("Apply Result To Variable")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("The result is PingRelay class converted to JSON format.")]
        public string v_userVariableName { get; set; }

        public PingCommand()
        {
            //this.CommandName = "PingCommand";
            //this.SelectionName = "Ping Command";
            //this.DisplayForeColor = System.Drawing.Color.ForestGreen;
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            Ping ping = new Ping();
            string hstname = v_HostName.ConvertToUserVariable(engine);

            PingReply pingresult = ping.Send(hstname);
            var pingReply = Common.ConvertObjectToJson(pingresult);

            pingReply.StoreInUserVariable(engine, v_userVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_HostName", this, editor));


        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_userVariableName", this, VariableNameControl, editor));
        //    RenderedControls.Add(VariableNameControl);

        //    return RenderedControls;
        //}


        //public override string GetDisplayValue()
        //{
        //    return $"Ping Host '{this.v_HostName}'";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_HostName))
        //    {
        //        this.validationResult += "IP address or Host is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}