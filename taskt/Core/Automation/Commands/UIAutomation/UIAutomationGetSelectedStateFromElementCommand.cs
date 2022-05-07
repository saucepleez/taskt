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
    [Attributes.ClassAttributes.SubGruop("Get")]
    [Attributes.ClassAttributes.Description("This command allows you to get Selected State from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Selected State from AutomationElement.")]
    public class UIAutomationGetSelectedStateFromElementCommand : ScriptCommand
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
        public string v_RootElement { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify a Variable to store Selected State")]
        [InputSpecification("")]
        [SampleUsage("**vText** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ResultVariable { get; set; }

        public UIAutomationGetSelectedStateFromElementCommand()
        {
            this.CommandName = "UIAutomationGetSelectedStateFromElementCommand";
            this.SelectionName = "Get Selected State From Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_RootElement.GetAutomationElementVariable(engine);

            object patternObj;
            bool checkState;
            if (targetElement.TryGetCurrentPattern(TogglePattern.Pattern, out patternObj))
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

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrl = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrl);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Root Element: '" + v_RootElement + "', Store: '" + v_ResultVariable + "']";
        }

    }
}