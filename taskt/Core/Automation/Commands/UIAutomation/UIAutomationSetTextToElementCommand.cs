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
    [Attributes.ClassAttributes.Description("This command allows you to set Text Value from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to set Text Value from AutomationElement.")]
    public class UIAutomationSetTextToElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify AutomationElement Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vElement}}}**")]
        [Remarks("Supported Element is TextBox, etc.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.AutomationElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Text to Set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_TextVariable { get; set; }

        public UIAutomationSetTextToElementCommand()
        {
            this.CommandName = "UIAutomationSetTextToElementCommand";
            this.SelectionName = "Set Text To Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);

            string textValue = v_TextVariable.ConvertToUserVariable(sender);

            object textPtn;
            if (targetElement.TryGetCurrentPattern(ValuePattern.Pattern, out textPtn))
            {
                ((ValuePattern)textPtn).SetValue(textValue);
            }
            else
            {
                throw new Exception("AutomationElement '" + v_TargetElement + "' can not set Text");
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
            return base.GetDisplayValue() + " [Target Element: '" + v_TargetElement + "', Text to Set: '" + v_TextVariable + "']";
        }

    }
}