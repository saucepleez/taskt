using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to create Color.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Color.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CreateColorCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a Color Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**vColor** or **{{{vColor}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Color, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Red Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or *255** or **{{{vRed}}}**")]
        [Remarks("Values range from 0 to 255")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Red", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_Red { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Green Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or *255** or **{{{vGreen}}}**")]
        [Remarks("Values range from 0 to 255")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Green", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_Green { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Blue Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or *255** or **{{{vBlue}}}**")]
        [Remarks("Values range from 0 to 255")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Blue", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_Blue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Alpha Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or *255** or **{{{Alpha}}}**")]
        [Remarks("Values range from 0 to 255")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "255")]
        public string v_Alpha { get; set; }

        public CreateColorCommand()
        {
            this.CommandName = "CreateColorCommand";
            this.SelectionName = "Create Color";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            int r = v_Red.ConvertToUserVariableAsInteger("Red", engine);
            int g = v_Green.ConvertToUserVariableAsInteger("Green", engine);
            int b = v_Blue.ConvertToUserVariableAsInteger("Blue", engine);

            if (string.IsNullOrEmpty(v_Alpha))
            {
                v_Alpha = "255";
            }
            int a = v_Alpha.ConvertToUserVariableAsInteger("Alpha", engine);

            Color co = Color.FromArgb(a, r, g, b);
            co.StoreInUserVariable(engine, v_Color);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Name: '" + v_Color + "']";
        }

        public override List<Control> Render(UI.Forms.frmCommandEditor editor)
        {
            //custom rendering
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }
    }
}