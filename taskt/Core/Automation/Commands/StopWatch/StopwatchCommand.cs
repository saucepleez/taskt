using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("StopWatch Commands")]
    [Attributes.ClassAttributes.CommandSettings("StopWatch")]
    [Attributes.ClassAttributes.Description("This command allows you to stop a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to close an application by its name such as 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class StopwatchCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Stopwatch Instance Name")]
        [InputSpecification("StopWatch Instance Name", true)]
        [PropertyDetailSampleUsage("**myStopWatch**", PropertyDetailSampleUsage.ValueType.Value, "StopWatch Instance")]
        [PropertyDetailSampleUsage("**{{{vStopWatch}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "StopWatch Instance")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.StopWatch)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public string v_StopwatchName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Stopwatch Action")]
        [PropertyUISelectionOption("Start Stopwatch")]
        [PropertyUISelectionOption("Stop Stopwatch")]
        [PropertyUISelectionOption("Restart Stopwatch")]
        [PropertyUISelectionOption("Reset Stopwatch")]
        [PropertyUISelectionOption("Measure Stopwatch")]
        [PropertySelectionChangeEvent(nameof(cmbStopWatchComboBox_SelectedValueChanged))]
        [PropertyValidationRule("Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Action")]
        public string v_StopwatchAction { get; set; }


        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("DateTime Format")]
        [InputSpecification("DateTime Format", true)]
        [PropertyDetailSampleUsage("**MM/dd/yy**", "Specify **MM/dd/yy** for DateTime Format. It is **Strongly Recommended** to **Disable Automatic Calculation** when using this format")]
        [PropertyDetailSampleUsage("**hh:mm**", PropertyDetailSampleUsage.ValueType.Value, "Format")]
        [PropertyDetailSampleUsage("**{{{vFormat}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Format")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("DateTime Format", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        // TODO: add format checker
        public string v_ToStringFormat { get; set; }

        public StopwatchCommand()
        {
            //this.CommandName = "StopwatchCommand";
            //this.SelectionName = "Stopwatch";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_StopwatchName = "";
            //this.v_StopwatchAction = "Start Stopwatch";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;   
            var instanceName = v_StopwatchName.ConvertToUserVariable(engine);
            
            var action = this.GetUISelectionValue(nameof(v_StopwatchAction), engine);
            System.Diagnostics.Stopwatch stopwatch;
            switch (action)
            {
                case "Start Stopwatch":
                    //start a new stopwatch
                    stopwatch = new System.Diagnostics.Stopwatch();
                    engine.AddAppInstance(instanceName, stopwatch);
                    stopwatch.Start();
                    break;

                case "Stop Stopwatch":
                    //stop existing stopwatch
                    stopwatch = (System.Diagnostics.Stopwatch)engine.AppInstances[instanceName];
                    stopwatch.Stop();
                    break;

                case "Restart Stopwatch":
                    //restart which sets to 0 and automatically starts
                    stopwatch = (System.Diagnostics.Stopwatch)engine.AppInstances[instanceName];
                    stopwatch.Restart();
                    break;

                case "Reset Stopwatch":
                    //reset which sets to 0
                    stopwatch = (System.Diagnostics.Stopwatch)engine.AppInstances[instanceName];
                    stopwatch.Reset();
                    break;

                case "Measure Stopwatch":
                    //check elapsed which gives measure
                    stopwatch = (System.Diagnostics.Stopwatch)engine.AppInstances[instanceName];
                    string elapsedTime;
                    if (string.IsNullOrEmpty(v_ToStringFormat))
                    {
                        elapsedTime = stopwatch.Elapsed.ToString();
                    }
                    else
                    {
                        var format = v_ToStringFormat.ConvertToUserVariable(sender);
                        elapsedTime = stopwatch.Elapsed.ToString(format);
                    }

                    elapsedTime.StoreInUserVariable(engine, v_userVariableName);
                    break;
            }
        }

        private void cmbStopWatchComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var selectedAction = ((ComboBox)sender).SelectedItem?.ToString() ?? "";
            if (selectedAction == "Measure Stopwatch")
            {
                GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_userVariableName), true);
                GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_ToStringFormat), true);
            }
            else
            {
                GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_userVariableName), false);
                GeneralPropertyControls.SetVisibleParameterControlGroup(ControlsList, nameof(v_ToStringFormat), false);
            }
        }
    }
}