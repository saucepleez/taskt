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
        //[SampleUsage("**myStopwatch**, **{{{vStopWatch}}}**")]
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
        //[PropertyDescription("Apply Result To Variable")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("DateTime Format")]
        [InputSpecification("DateTime Format", true)]
        //[SampleUsage("MM/dd/yy, hh:mm, etc.")]
        [PropertyDetailSampleUsage("**MM/dd/yy**", "Specify **MM/dd/yy** for DateTime Format. It is **Strongly Recommended** to **Disable Automatic Calculation** when using this format")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("DateTime Format", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_ToStringFormat { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //public ComboBox StopWatchComboBox;

        //[XmlIgnore]
        //[NonSerialized]
        //public List<Control> MeasureControls;

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
            

            //var action = v_StopwatchAction.ConvertToUserVariable(sender);
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

                //default:
                //    throw new NotImplementedException("Stopwatch Action '" + action + "' not implemented");
            }
        }

        private void cmbStopWatchComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            //if (StopWatchComboBox.Text == "Measure Stopwatch")
            //{
            //    foreach (var ctrl in MeasureControls)
            //    {
            //        ctrl.Visible = true;
            //    }
            //}
            //else
            //{
            //    foreach (var ctrl in MeasureControls)
            //    {
            //        ctrl.Visible = false;
            //    }
            //}

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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctlStopwatchName = CommandControls.CreateDefaultDropdownGroupFor("v_StopwatchName", this, editor);
        //    CommandControls.AddInstanceNames((ComboBox)ctlStopwatchName.Where(t => (t.Name == "v_StopwatchName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.StopWatch);
        //    RenderedControls.AddRange(ctlStopwatchName);

        //    //var StopWatchComboBoxLabel = CommandControls.CreateDefaultLabelFor("v_StopwatchAction", this);
        //    //StopWatchComboBox = (ComboBox)CommandControls.CreateDropdownFor("v_StopwatchAction", this);
        //    //StopWatchComboBox.DataSource = new List<string> { "Start Stopwatch", "Stop Stopwatch", "Restart Stopwatch", "Reset Stopwatch", "Measure Stopwatch" };
        //    //StopWatchComboBox.SelectedIndexChanged += StopWatchComboBox_SelectedValueChanged;
        //    //RenderedControls.Add(StopWatchComboBoxLabel);
        //    //RenderedControls.Add(StopWatchComboBox);
        //    var stopwatchActionCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_StopwatchAction", this, editor);
        //    RenderedControls.AddRange(stopwatchActionCtrls);
        //    StopWatchComboBox = (ComboBox)stopwatchActionCtrls.Find(t => t is ComboBox);
        //    StopWatchComboBox.SelectedIndex = 0;
        //    StopWatchComboBox.SelectedIndexChanged += (sender, e) => StopWatchComboBox_SelectedValueChanged(sender, e);

        //    //create controls for user variable
        //    MeasureControls = CommandControls.CreateDefaultDropdownGroupFor("v_userVariableName", this, editor);

        //    //load variables for selection
        //    var comboBox = (ComboBox)MeasureControls[2];
        //    comboBox.AddVariableNames(editor);

        //    MeasureControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor));

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_StopwatchName = editor.appSettings.ClientSettings.DefaultStopWatchInstanceName;
        //    }

        //    foreach (var ctrl in MeasureControls)
        //    {
        //        ctrl.Visible = false;
        //    }
        //    RenderedControls.AddRange(MeasureControls);

        //    return RenderedControls;
        //}


        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Action: " + v_StopwatchAction + ", Name: " + v_StopwatchName + "]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    this.IsValid = true;
        //    this.validationResult = "";

        //    if (String.IsNullOrEmpty(v_StopwatchName))
        //    {
        //        this.validationResult += "Instance name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(v_StopwatchAction))
        //    {
        //        this.validationResult += "Stopwach Action is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if ((v_StopwatchAction == "Measure Stopwatch") && String.IsNullOrEmpty(v_userVariableName))
        //    {
        //        this.validationResult += "Variable is empty.";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}