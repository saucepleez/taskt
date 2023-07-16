using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("UIElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Set Text To UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to set Text Value from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to set Text Value from UIElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSetTextToUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OneLineTextBox))]
        [PropertyDescription("Text to Set")]
        [InputSpecification("Text", true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDisplayText(true, "Text")]
        public string v_TextVariable { get; set; }

        public UIAutomationSetTextToUIElementCommand()
        {
            //this.CommandName = "UIAutomationSetTextToElementCommand";
            //this.SelectionName = "Set Text To Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetUIElementVariable(engine);

            string textValue = v_TextVariable.ConvertToUserVariable(sender);

            // todo: support range value pattern
            if (targetElement.TryGetCurrentPattern(ValuePattern.Pattern, out object textPtn))
            {
                ((ValuePattern)textPtn).SetValue(textValue);
            }
            else
            {
                throw new Exception("UIElement '" + v_TargetElement + "' can not set Text");
            }
        }
    }
}