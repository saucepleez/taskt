using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Windows.Forms;

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

            var targetElement = v_TargetElement.ExpandUserVariableAsUIElement(engine);

            var ct = targetElement.GetCurrentPropertyValue(AutomationElement.ControlTypeProperty) as ControlType;
            if (ct == ControlType.Spinner)
            {
                //targetElement = UIElementControls.SearchGUIElementByXPath(targetElement, "/Edit[1]", 10, engine);
                targetElement = targetElement.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            }

            string textValue = v_TextVariable.ExpandValueOrUserVariable(sender);

            if (targetElement.TryGetCurrentPattern(ValuePattern.Pattern, out object valPtn))
            {
                ((ValuePattern)valPtn).SetValue(textValue);
            }
            else if (targetElement.TryGetCurrentPattern(TextPattern.Pattern, out _))
            {
                targetElement.SetFocus();
                System.Threading.Thread.Sleep(100);
                SendKeys.SendWait("^{HOME}");
                SendKeys.SendWait("^+{END}");
                SendKeys.SendWait("{DEL}");
                SendKeys.SendWait(textValue);
            }
            else
            {
                throw new Exception("UIElement '" + v_TargetElement + "' can not set Text");
            }
        }
    }
}