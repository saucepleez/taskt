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
    [Attributes.ClassAttributes.Description("This command allows you to Convert DateTime To Excel Serial Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want Convert DateTime To Excel Serial Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ConvertDateTimeToExcelSerialCommand : ScriptCommand
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
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store Excel Serial Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vSerial** or **{{{vSerial}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Serial", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Serial { get; set; }

        public ConvertDateTimeToExcelSerialCommand()
        {
            this.CommandName = "ConvertDateTimeToExcelSerialCommand";
            this.SelectionName = "Convert DateTime To Excel Serial";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            DateTime myDT = v_DateTime.GetDateTimeVariable(engine);

            double serial = myDT.ToOADate();
            serial.ToString().StoreInUserVariable(engine, v_Serial);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Create DateTime: '" + v_DateTime + "' From '" + v_Serial + "']";
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