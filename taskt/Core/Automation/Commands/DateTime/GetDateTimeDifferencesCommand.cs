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
    [Attributes.ClassAttributes.Description("This command allows you to Get 2 DateTime Differences.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get 2 DateTime Differences.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetDateTimeDifferencesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a DateTime1 Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**{{{vDateTime}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("DateTime1", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DateTime1 { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a DateTime2 Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**{{{vDateTime}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("DateTime2", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_DateTime2 { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Result Format")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUISelectionOption("Total Days")]
        [PropertyUISelectionOption("Total Hours")]
        [PropertyUISelectionOption("Total Minutes")]
        [PropertyUISelectionOption("Total Seconds")]
        [PropertyUISelectionOption("Days")]
        [PropertyUISelectionOption("Hours")]
        [PropertyUISelectionOption("Minutes")]
        [PropertyUISelectionOption("Seconds")]
        [PropertyUISelectionOption("Ticks")]
        [PropertyUISelectionOption("DateTime")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Format { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("Result is DateTime2 - DateTime1")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Result { get; set; }

        public GetDateTimeDifferencesCommand()
        {
            this.CommandName = "GetDateTimeDifferencesCommand";
            this.SelectionName = "Get DateTime Differences";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var myDT1 = v_DateTime1.GetDateTimeVariable(engine);
            var myDT2 = v_DateTime2.GetDateTimeVariable(engine);
            string format = v_Format.GetUISelectionValue("v_Format", this, engine);

            TimeSpan diff = myDT2 - myDT1;

            string result = "";
            switch (format)
            {
                case "days":
                    result = diff.Days.ToString();
                    break;
                case "hours":
                    result = diff.Hours.ToString();
                    break;
                case "minutes":
                    result = diff.Minutes.ToString();
                    break;
                case "seconds":
                    result = diff.Seconds.ToString();
                    break;
                case "total days":
                    result = diff.TotalDays.ToString();
                    break;
                case "total hours":
                    result = diff.TotalHours.ToString();
                    break;
                case "total minutes":
                    result = diff.TotalMinutes.ToString();
                    break;
                case "total seconds":
                    result = diff.TotalSeconds.ToString();
                    break;
                case "ticks":
                    result = diff.Ticks.ToString();
                    break;
                case "datetime":
                    // return here
                    if (diff.Ticks >= 0)
                    {
                        new DateTime(0).Add(diff).StoreInUserVariable(engine, v_Result);
                    }
                    else
                    {
                        new DateTime(0).Subtract(diff).StoreInUserVariable(engine, v_Result);
                    }
                    return;
            }
            result.StoreInUserVariable(engine, v_Result);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [DateTime1: '" + v_DateTime1 + "', DateTime2: '" + v_DateTime2 + "', Format: '" + v_Format + "', Store: '" + v_Result + "']";
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