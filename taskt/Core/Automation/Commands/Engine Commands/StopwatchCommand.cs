using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to stop a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to close an application by its name such as 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    public class StopwatchCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the instance name of the Stopwatch")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Provide a unique instance or way to refer to the stopwatch")]
        [Attributes.PropertyAttributes.SampleUsage("**myStopwatch**, **{vStopWatch}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_StopwatchName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the Stopwatch Action")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Provide a unique instance or way to refer to the stopwatch")]
        [Attributes.PropertyAttributes.SampleUsage("**myStopwatch**, **{vStopWatch}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_StopwatchAction { get; set; }


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply Result To Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Specify String Format")]
        [Attributes.PropertyAttributes.InputSpecification("Specify if a specific string format is required.")]
        [Attributes.PropertyAttributes.SampleUsage("MM/dd/yy, hh:mm, etc.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ToStringFormat { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox StopWatchComboBox;

        [XmlIgnore]
        [NonSerialized]
        public List<Control> MeasureControls;

        public StopwatchCommand()
        {
            this.CommandName = "StopwatchCommand";
            this.SelectionName = "Stopwatch";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_StopwatchName = "default";
            this.v_StopwatchAction = "Start Stopwatch";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;   
            var instanceName = v_StopwatchName.ConvertToUserVariable(sender);
            System.Diagnostics.Stopwatch stopwatch;

            var action = v_StopwatchAction.ConvertToUserVariable(sender);

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

                    elapsedTime.StoreInUserVariable(sender, v_userVariableName);

                    break;
                default:
                    throw new NotImplementedException("Stopwatch Action '" + action + "' not implemented");
            }



        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_StopwatchName", this, editor));

            var StopWatchComboBoxLabel = CommandControls.CreateDefaultLabelFor("v_StopwatchAction", this);
            StopWatchComboBox = (ComboBox)CommandControls.CreateDropdownFor("v_StopwatchAction", this);
            StopWatchComboBox.DataSource = new List<string> { "Start Stopwatch", "Stop Stopwatch", "Restart Stopwatch", "Reset Stopwatch", "Measure Stopwatch" };
            StopWatchComboBox.SelectedIndexChanged += StopWatchComboBox_SelectedValueChanged;
            RenderedControls.Add(StopWatchComboBoxLabel);
            RenderedControls.Add(StopWatchComboBox);

            //create controls for user variable
            MeasureControls = CommandControls.CreateDefaultDropdownGroupFor("v_userVariableName", this, editor);

            //load variables for selection
            var comboBox = (ComboBox)MeasureControls[1];
            comboBox.AddVariableNames(editor);

            MeasureControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor));

            foreach (var ctrl in MeasureControls)
            {
                ctrl.Visible = false;
            }
            RenderedControls.AddRange(MeasureControls);

            return RenderedControls;
        }

        private void StopWatchComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
           
            if (StopWatchComboBox.SelectedValue.ToString() == "Measure Stopwatch")
            {
                foreach (var ctrl in MeasureControls)
                                 ctrl.Visible = true;
               
            }
            else {
                foreach (var ctrl in MeasureControls)
                    ctrl.Visible = false;
            }
            
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: " + v_StopwatchAction + ", Name: " + v_StopwatchName + "]";
        }
    }
}