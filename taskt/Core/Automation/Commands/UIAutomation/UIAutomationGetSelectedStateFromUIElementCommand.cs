using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Selected State From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Selected State from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Selected State from UIElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetSelectedStateFromUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When UIElement is Selected, Result is **True**")]
        public string v_ResultVariable { get; set; }

        public UIAutomationGetSelectedStateFromUIElementCommand()
        {
            //this.CommandName = "UIAutomationGetSelectedStateFromElementCommand";
            //this.SelectionName = "Get Selected State From Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetUIElementVariable(engine);

            bool checkState;
            if (targetElement.TryGetCurrentPattern(TogglePattern.Pattern, out object patternObj))
            {
                checkState = (((TogglePattern)patternObj).Current.ToggleState == ToggleState.On);
            }
            else if (targetElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out patternObj))
            {
                checkState = ((SelectionItemPattern)patternObj).Current.IsSelected;
            }
            else
            {
                throw new Exception("Thie UIElement does not have Selected State");
            }
            checkState.StoreInUserVariable(engine, v_ResultVariable);
        }
    }
}