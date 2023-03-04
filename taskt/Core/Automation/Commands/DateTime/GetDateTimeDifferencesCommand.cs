using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Get DateTime Differences")]
    [Attributes.ClassAttributes.Description("This command allows you to Get 2 DateTime Differences.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get 2 DateTime Differences.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetDateTimeDifferencesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_InputDateTime))]
        [PropertyDescription("DateTime1 Variable Name")]
        [PropertyValidationRule("DateTime1", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DateTime1")]
        public string v_DateTime1 { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_InputDateTime))]
        [PropertyDescription("DateTime2 Variable Name")]
        [PropertyValidationRule("DateTime2", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DateTime2")]
        public string v_DateTime2 { get; set; }

        [XmlAttribute]
        [PropertyDescription("Format")]
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
        [PropertyDisplayText(true, "Format")]
        public string v_Format { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("Result is DateTime2 - DateTime1")]
        public string v_Result { get; set; }

        public GetDateTimeDifferencesCommand()
        {
            //this.CommandName = "GetDateTimeDifferencesCommand";
            //this.SelectionName = "Get DateTime Differences";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var myDT1 = v_DateTime1.GetDateTimeVariable(engine);
            var myDT2 = v_DateTime2.GetDateTimeVariable(engine);

            string format = this.GetUISelectionValue(nameof(v_Format), engine);

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

        public override void AddInstance(InstanceCounter counter)
        {
            string format = (string.IsNullOrEmpty(v_Format) ? "" : v_Format.ToLower());

            var prop = new Attributes.PropertyAttributes.PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true);

            var dt1 = string.IsNullOrEmpty(v_DateTime1) ? "" : v_DateTime1;
            var dt2 = string.IsNullOrEmpty(v_DateTime2) ? "" : v_DateTime2;
            counter.addInstance(dt1, prop, true);
            counter.addInstance(dt2, prop, true);

            switch (format)
            {
                case "datetime":
                    string ins = (string.IsNullOrEmpty(v_Result) ? "" : v_Result);
                    counter.addInstance(ins, prop, false);
                    counter.addInstance(ins, prop, true);
                    break;
            }
        }

        public override void RemoveInstance(InstanceCounter counter)
        {
            string format = (string.IsNullOrEmpty(v_Format) ? "" : v_Format.ToLower());

            var prop = new Attributes.PropertyAttributes.PropertyInstanceType(PropertyInstanceType.InstanceType.DateTime, true);
            var dt1 = string.IsNullOrEmpty(v_DateTime1) ? "" : v_DateTime1;
            var dt2 = string.IsNullOrEmpty(v_DateTime2) ? "" : v_DateTime2;
            counter.removeInstance(dt1, prop, true);
            counter.removeInstance(dt2, prop, true);

            switch (format)
            {
                case "datetime":
                    string ins = (string.IsNullOrEmpty(v_Result) ? "" : v_Result);
                    counter.removeInstance(ins, prop, false);
                    counter.removeInstance(ins, prop, true);
                    break;
            }
        }
    }
}