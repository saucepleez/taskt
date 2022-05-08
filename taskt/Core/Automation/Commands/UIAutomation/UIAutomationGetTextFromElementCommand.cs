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
    [Attributes.ClassAttributes.Description("This command allows you to get Text Value from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Text Value from AutomationElement.")]
    public class UIAutomationGetTextFromElementCommand : ScriptCommand
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
        [PropertyDescription("Please specify a Variable to store Text Value")]
        [InputSpecification("")]
        [SampleUsage("**vText** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TextVariable { get; set; }

        public UIAutomationGetTextFromElementCommand()
        {
            this.CommandName = "UIAutomationGetTextFromElementCommand";
            this.SelectionName = "Get Text From Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);

            //object patternObj;
            //if (targetElement.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
            //{
            //    // TextBox
            //    ((ValuePattern)patternObj).Current.Value.StoreInUserVariable(sender, v_TextVariable);
            //}
            //else if (targetElement.TryGetCurrentPattern(TextPattern.Pattern, out patternObj))
            //{
            //    // TextBox Multilune
            //    TextPattern tPtn = (TextPattern)patternObj;
            //    tPtn.DocumentRange.GetText(-1).StoreInUserVariable(sender, v_TextVariable);
            //}
            //else if (targetElement.TryGetCurrentPattern(SelectionPattern.Pattern, out patternObj))
            //{
            //    SelectionPattern sPtn = (SelectionPattern)patternObj;
            //    sPtn.Current.GetSelection()[0].GetCurrentPropertyValue(AutomationElement.NameProperty).ToString().StoreInUserVariable(sender, v_TextVariable);
            //}
            //else
            //{
            //    targetElement.Current.Name.StoreInUserVariable(sender, v_TextVariable);
            //}

            string res = AutomationElementControls.GetTextValue(targetElement);
            res.StoreInUserVariable(engine, v_TextVariable);
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
            return base.GetDisplayValue() + " [Root Element: '" + v_TargetElement + "', Store: '" + v_TextVariable + "']";
        }

    }
}