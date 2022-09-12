using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
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
        [PropertyDescription("Please specify AutomationElement Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vElement}}}**")]
        [Remarks("Supported Element is ComboBox, Tree, etc.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.AutomationElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Element")]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Items State")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyUISelectionOption("Expand")]
        [PropertyUISelectionOption("Collapse")]
        [SampleUsage("**Expand** or **Collapse**")]
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrl = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrl);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Target Element: '" + v_TargetElement + "', State: " + v_ItemsState + "]";
        //}

    }
}