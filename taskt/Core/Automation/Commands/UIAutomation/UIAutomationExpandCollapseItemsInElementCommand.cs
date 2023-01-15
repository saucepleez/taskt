using System;
using System.Xml.Serialization;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to Expand or Collapse Items in AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Expand or Collapse Items in AutomationElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationExpandCollapseItemsInElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
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

        public UIAutomationExpandCollapseItemsInElementCommand()
        {
            this.CommandName = "UIAutomationExpandCollapseItemsInElementCommand";
            this.SelectionName = "Expand Collapse Items In Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);
            var state = v_ItemsState.GetUISelectionValue("v_ItemsState", this, engine);

            object exColPtn;
            if (targetElement.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out exColPtn))
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
                throw new Exception("AutomationElement '" + v_TargetElement + "' does not support Expand/Collapse");
            }
        }
    }
}