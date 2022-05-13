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
    [Attributes.ClassAttributes.Description("This command allows you to create DateTime.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create DateTime.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CreateDateTimeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a DateTime Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**vDateTime** or **{{{vDateTime}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Year to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**2000** or **{{{vYear}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "1")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Year", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_Year { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Month to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **{{{vMonth}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "1")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Month", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_Month { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Day to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **{{{vDay}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "1")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Day", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_Day { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Hour to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **{{{vHour}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "0")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Hour", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_Hour { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Minute to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **{{{vMinute}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "0")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Minute", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_Minute { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Second to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **{{{vSecond}}}**")]
        [Remarks("")]
        [PropertyIsOptional(true, "0")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Second", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        public string v_Second { get; set; }

        public CreateDateTimeCommand()
        {
            this.CommandName = "CreateDateTimeCommand";
            this.SelectionName = "Create DateTime";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            if (String.IsNullOrEmpty(v_Year))
            {
                v_Year = "1";
            }
            int year = v_Year.ConvertToUserVariableAsInteger("Year", engine);
            if (year < 1) 
            {
                throw new Exception("Year is less than 1");
            }

            if (String.IsNullOrEmpty(v_Month))
            {
                v_Month = "1";
            }
            int month = v_Month.ConvertToUserVariableAsInteger("Month", engine);
            if ((month < 1) || (month > 12))
            {
                throw new Exception("Month is out of range");
            }

            if (String.IsNullOrEmpty(v_Day))
            {
                v_Day = "1";
            }
            int day = v_Day.ConvertToUserVariableAsInteger("Day", engine);
            if ((day < 1) || (day > 31))
            {
                throw new Exception("Day is out of range");
            }

            if (String.IsNullOrEmpty(v_Hour))
            {
                v_Hour = "0";
            }
            int hour = v_Hour.ConvertToUserVariableAsInteger("Hour", engine);
            if ((hour < 0) || (hour > 24))
            {
                throw new Exception("Hour is out of range");
            }

            if (String.IsNullOrEmpty(v_Minute))
            {
                v_Minute = "0";
            }
            int minute = v_Minute.ConvertToUserVariableAsInteger("Hour", engine);
            if ((minute < 0) || (minute > 24))
            {
                throw new Exception("Minite is out of range");
            }

            if (String.IsNullOrEmpty(v_Second))
            {
                v_Second = "0";
            }
            int second = v_Second.ConvertToUserVariableAsInteger("Second", engine);
            if ((second < 0) || (second > 24))
            {
                throw new Exception("Second is out of range");
            }

            DateTime myDT = new DateTime(year, month, day, hour, minute, second);
            myDT.StoreInUserVariable(engine, v_DateTime);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Create DateTime: '" + v_DateTime + "']";
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