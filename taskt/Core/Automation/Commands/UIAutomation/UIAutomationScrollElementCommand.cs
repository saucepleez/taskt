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
        [SampleUsage("**vElement** or **{{{vElement}}}**")]
        [Remarks("")]
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
        [PropertyDescription("Please specify Scroll Direction and Amount")]
        [InputSpecification("")]
        [PropertyUISelectionOption("Small Increment")]
        [PropertyUISelectionOption("Large Increment")]
        [PropertyUISelectionOption("Small Decrement")]
        [PropertyUISelectionOption("Large Decrement")]
        [SampleUsage("**Small Increment** or **Large Increment** or **Small Decrement** or **Large Decrement**")]
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
                case "small increment":
                    amount = ScrollAmount.SmallIncrement;
                    break;
                case "large increment":
                    amount = ScrollAmount.LargeIncrement;
                    break;
                case "small decrement":
                    amount = ScrollAmount.SmallDecrement;
                    break;
                case "large decrement":
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
            return base.GetDisplayValue() + " [Root Element: '" + v_TargetElement + "']";
        }

    }
}