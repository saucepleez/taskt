﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Create DateTime")]
    [Attributes.ClassAttributes.Description("This command allows you to create DateTime.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create DateTime.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateDateTimeCommand : ADateTimeCreateCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_OutputDateTime))]
        //public string v_DateTime { get; set; }

        [XmlAttribute]
        [PropertyDescription("Year to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**2000**", PropertyDetailSampleUsage.ValueType.Value, "Year")]
        [PropertyDetailSampleUsage("**{{{vYear}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Year")]
        [Remarks("")]
        [PropertyIsOptional(true, "2000")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Year", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Year")]
        [PropertyParameterOrder(6000)]
        public string v_Year { get; set; }

        [XmlAttribute]
        [PropertyDescription("Month to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Month")]
        [PropertyDetailSampleUsage("**{{{vMonth}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Month")]
        [Remarks("")]
        [PropertyIsOptional(true, "1")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Month", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(1, 12)]
        [PropertyDisplayText(true, "Month")]
        [PropertyParameterOrder(6001)]
        public string v_Month { get; set; }

        [XmlAttribute]
        [PropertyDescription("Day to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Day")]
        [PropertyDetailSampleUsage("**{{{vDay}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Day")]
        [Remarks("")]
        [PropertyIsOptional(true, "1")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Day", PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(1, 31)]
        [PropertyDisplayText(true, "Day")]
        [PropertyParameterOrder(6002)]
        public string v_Day { get; set; }

        [XmlAttribute]
        [PropertyDescription("Hour to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Hour")]
        [PropertyDetailSampleUsage("**{{{vHour}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Hour")]
        [Remarks("")]
        [PropertyIsOptional(true, "0")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Hour", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(0, 23)]
        [PropertyDisplayText(true, "Hour")]
        [PropertyParameterOrder(6003)]
        public string v_Hour { get; set; }

        [XmlAttribute]
        [PropertyDescription("Minute to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Minute")]
        [PropertyDetailSampleUsage("**{{{vMinute}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Minute")]
        [Remarks("")]
        [PropertyIsOptional(true, "0")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Minute", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(0, 59)]
        [PropertyDisplayText(true, "Minute")]
        [PropertyParameterOrder(6004)]
        public string v_Minute { get; set; }

        [XmlAttribute]
        [PropertyDescription("Second to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Second")]
        [PropertyDetailSampleUsage("**{{{vSecond}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Second")]
        [Remarks("")]
        [PropertyIsOptional(true, "0")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Second", PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.NotBetween)]
        [PropertyValueRange(0, 59)]
        [PropertyDisplayText(true, "Second")]
        [PropertyParameterOrder(6005)]
        public string v_Second { get; set; }

        public CreateDateTimeCommand()
        {
            //this.CommandName = "CreateDateTimeCommand";
            //this.SelectionName = "Create DateTime";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            int year = this.ExpandValueOrUserVariableAsInteger(nameof(v_Year), engine);

            int month = this.ExpandValueOrUserVariableAsInteger(nameof(v_Month), engine);

            int day = this.ExpandValueOrUserVariableAsInteger(nameof(v_Day), engine);

            int hour = this.ExpandValueOrUserVariableAsInteger(nameof(v_Hour), engine);

            int minute = this.ExpandValueOrUserVariableAsInteger(nameof(v_Minute), engine);

            int second = this.ExpandValueOrUserVariableAsInteger(nameof(v_Second), engine);

            try
            {
                DateTime myDT = new DateTime(year, month, day, hour, minute, second);
                //myDT.StoreInUserVariable(engine, v_DateTime);
                this.StoreDateTimeInUserVariable(myDT, nameof(v_DateTime), engine);
            }
            catch
            {
                throw new Exception($"Specified DateTime is not DateTime. Year: '{year}', Month: '{month}', Day: '{day}', Hour: '{hour}', Minute: '{minute}', Second: '{second}'");
            }
        }
    }
}