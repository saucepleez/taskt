using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get")]
    [Attributes.ClassAttributes.CommandSettings("Get Selected State From Element")]
    [Attributes.ClassAttributes.Description("This command allows you to get Selected State from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Selected State from AutomationElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetSelectedStateFromElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When Element is Checked, Result is **True**")]
        public string v_ResultVariable { get; set; }

        public UIAutomationGetSelectedStateFromElementCommand()
        {
            //this.CommandName = "UIAutomationGetSelectedStateFromElementCommand";
            //this.SelectionName = "Get Selected State From Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);

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
                throw new Exception("Thie element does not have Selected State");
            }
            checkState.StoreInUserVariable(engine, v_ResultVariable);
        }
    }
}