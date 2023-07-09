using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Element Action")]
    [Attributes.ClassAttributes.CommandSettings("Select Element")]
    [Attributes.ClassAttributes.Description("This command allows you to Select AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Select AutomationElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSelectElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        public UIAutomationSelectElementCommand()
        {
            //this.CommandName = "UIAutomationSelectElementCommand";
            //this.SelectionName = "Select Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetUIElementVariable(engine);

            if (targetElement.TryGetCurrentPattern(TogglePattern.Pattern, out object checkPtn))
            {
                TogglePattern ptn = (TogglePattern)checkPtn;
                switch (ptn.Current.ToggleState)
                {
                    case ToggleState.Off:
                    case ToggleState.Indeterminate:
                        do
                        {
                            ptn.Toggle();
                        } while (ptn.Current.ToggleState != ToggleState.On);
                        break;
                }
            }
            else if (targetElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out checkPtn))
            {
                ((SelectionItemPattern)checkPtn).Select();
            }
        }
    }
}