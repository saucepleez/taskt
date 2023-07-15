using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("UIElement Action")]
    [Attributes.ClassAttributes.CommandSettings("Expand Collapse Items In UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to Expand or Collapse Items in UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Expand or Collapse Items in UIElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationExpandCollapseItemsInUIElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyDescription("Items State")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("", true)]
        [PropertyUISelectionOption("Expand")]
        [PropertyUISelectionOption("Collapse")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Items State", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "State")]
        public string v_ItemsState { get; set; }

        public UIAutomationExpandCollapseItemsInUIElementCommand()
        {
            //this.CommandName = "UIAutomationExpandCollapseItemsInElementCommand";
            //this.SelectionName = "Expand Collapse Items In Element";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetUIElementVariable(engine);
            var state = v_ItemsState.GetUISelectionValue("v_ItemsState", this, engine);

            if (targetElement.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out object exColPtn))
            {
                switch (state)
                {
                    case "expand":
                        ((ExpandCollapsePattern)exColPtn).Expand();
                        break;
                    case "collapse":
                        ((ExpandCollapsePattern)exColPtn).Collapse();
                        break;
                }
            }
            else
            {
                throw new Exception("UIElement '" + v_TargetElement + "' does not support Expand/Collapse");
            }
        }
    }
}