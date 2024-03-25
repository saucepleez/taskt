using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Create DateTime From Text")]
    [Attributes.ClassAttributes.Description("This command allows you to create DateTime from Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create DateTime from Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateDateTimeFromTextCommand : ADateTimeCreateCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_OutputDateTime))]
        //public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**2000-01-01**", PropertyDetailSampleUsage.ValueType.Value, "DateTime")]
        [PropertyDetailSampleUsage("**{{{vText}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "DateTime")]
        [Remarks("Recommended to Disable the Auto Calculation")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Text", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Text")]
        [PropertyParameterOrder(6000)]
        public string v_Text { get; set; }

        public CreateDateTimeFromTextCommand()
        {
            //this.CommandName = "CreateDateTimeFromTextCommand";
            //this.SelectionName = "Create DateTime From Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var value = v_Text.ExpandValueOrUserVariable(engine);

            if (DateTime.TryParse(value, out DateTime myDT))
            {
                //tryDT.StoreInUserVariable(engine, v_DateTime);
                this.StoreDateTimeInUserVariable(myDT, nameof(v_DateTime), engine);
            }
            else
            {
                throw new Exception($"Specified Text is not DateTime. Value: '{v_Text}', Expand Value: '{value}'");
            }
        }
    }
}