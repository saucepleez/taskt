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
    [Attributes.ClassAttributes.Description("This command allows you to Scroll AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Scroll AutomationElement.")]
    public class UIAutomationScrollElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify AutomationElement Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vElement}}}**")]
        [Remarks("Supported Element must have a ScrollBar.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify ScrollBar Type")]
        [InputSpecification("")]
        [PropertyUISelectionOption("Vertical")]
        [PropertyUISelectionOption("Horizonal")]
        [SampleUsage("**Horizonal** or **Vertical**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("ScrollBar", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ScrollBarType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Scroll Method")]
        [InputSpecification("")]
        [PropertyUISelectionOption("Scroll Small Down or Right")]
        [PropertyUISelectionOption("Scroll Large Down or Right")]
        [PropertyUISelectionOption("Scroll Small Up or Left")]
        [PropertyUISelectionOption("Scroll Large Up or Left")]
        [SampleUsage("**Scroll Small Down or Right** or **Scroll Large Down or Right** or **Scroll Small Up or Left** or **Scroll Large Up or Left**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Direction and Amount", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DirectionAndAmount{ get; set; }

        public UIAutomationScrollElementCommand()
        {
            this.CommandName = "UIAutomationScrollElementCommand";
            this.SelectionName = "Scroll Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);
            var scrollbarType = v_ScrollBarType.GetUISelectionValue("v_ScrollBarType", this, engine);

            var dirAndAmo = v_DirectionAndAmount.GetUISelectionValue("v_DirectionAndAmount", this, engine);
            ScrollAmount amount = ScrollAmount.NoAmount;
            switch (dirAndAmo)
            {
                case "scroll small down or right":
                    amount = ScrollAmount.SmallIncrement;
                    break;
                case "scroll large down or right":
                    amount = ScrollAmount.LargeIncrement;
                    break;
                case "scroll small up or left":
                    amount = ScrollAmount.SmallDecrement;
                    break;
                case "scroll large up or left":
                    amount = ScrollAmount.LargeDecrement;
                    break;
            }

            object scrollPtn;
            //if (targetElement.TryGetCurrentPattern(ScrollPattern.Pattern, out scrollPtn))
            //{
            //    ScrollPattern sp = (ScrollPattern)scrollPtn;
            //    switch (scrollbarType)
            //    {
            //        case "horizonal":
            //            sp.ScrollHorizontal(amount);
            //            break;
            //        case "vertical":
            //            sp.ScrollVertical(amount);
            //            break;
            //    }
            //}
            //else
            //{
            //    throw new Exception("AutomationElement '" + v_TargetElement + "' does not have ScrollBar");
            //}
            if (!targetElement.TryGetCurrentPattern(ScrollPattern.Pattern, out scrollPtn))
            {
                if (targetElement.Current.ControlType == ControlType.ScrollBar)
                {
                    var parentElement = AutomationElementControls.GetParentElement(targetElement);
                    if (!parentElement.TryGetCurrentPattern(ScrollPattern.Pattern, out scrollPtn))
                    {
                        throw new Exception("AutomationElement '" + v_TargetElement + "' does not have ScrollBar");
                    }
                }
                else
                {
                    throw new Exception("AutomationElement '" + v_TargetElement + "' is not ScrollBar and does not have ScrollBar");
                }
            }
            ScrollPattern sp = (ScrollPattern)scrollPtn;
            switch (scrollbarType)
            {
                case "horizonal":
                    sp.ScrollHorizontal(amount);
                    break;
                case "vertical":
                    sp.ScrollVertical(amount);
                    break;
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrl = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrl);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target Element: '" + v_TargetElement + "', ScrollBar Type: " + v_ScrollBarType + ", Scroll Method: " + v_DirectionAndAmount + "]";
        }

    }
}