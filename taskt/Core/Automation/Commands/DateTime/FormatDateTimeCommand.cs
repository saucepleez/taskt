using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to Format DateTime Text.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Format DateTime Text.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class FormatDateTimeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a DateTime Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**{{{vDateTime}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("DateTime Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify DateTime Format")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**MM/dd/yyyy** or **HH:mm:ss** or **{{{vFormat}}}**")]
        [Remarks("Please refer to the Microsoft DateTime.ToString() page for format details")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Format { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Result { get; set; }

        public FormatDateTimeCommand()
        {
            this.CommandName = "FormatDateTimeCommand";
            this.SelectionName = "Format DateTime";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var myDT = v_DateTime.GetDateTimeVariable(engine);
            string format = v_Format.ConvertToUserVariable(engine);

            myDT.ToString(format).StoreInUserVariable(engine, v_Result);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [DateTime: '" + v_DateTime + "', Format: '" + v_Format + "', Store: '" + v_Result + "']";
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