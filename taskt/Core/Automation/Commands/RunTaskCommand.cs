using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Task Commands")]
    [Attributes.ClassAttributes.Description("This command runs tasks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run another task.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class RunTaskCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select a Task to run")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the valid path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("c:\\temp\\mytask.xml or [vScriptPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_taskPath { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Assign Variables")]
        [Attributes.PropertyAttributes.InputSpecification("Input required assignments.")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_VariableAssignments { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Assign Variables")]
        [Attributes.PropertyAttributes.InputSpecification("User Preference for assigning variables")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public bool v_AssignVariables { get; set; }


        [XmlIgnore]
        [NonSerialized]
        private DataGridView AssignmentsGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private CheckBox PassParameters;

        public RunTaskCommand()
        {
            this.CommandName = "RunTaskCommand";
            this.SelectionName = "Run Task";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            v_VariableAssignments = new DataTable();
            v_VariableAssignments.Columns.Add("VariableName");
            v_VariableAssignments.Columns.Add("VariableValue");
            v_VariableAssignments.TableName = "RunTaskCommandInputParameters" + DateTime.Now.ToString("MMddyyhhmmss");

            AssignmentsGridViewHelper = new DataGridView();
            AssignmentsGridViewHelper.AllowUserToAddRows = true;
            AssignmentsGridViewHelper.AllowUserToDeleteRows = true;
            AssignmentsGridViewHelper.Size = new Size(400, 250);
            AssignmentsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            AssignmentsGridViewHelper.DataSource = v_VariableAssignments;
            AssignmentsGridViewHelper.Hide();
        }

        public override void RunCommand(object sender)
        {
            var startFile = v_taskPath.ConvertToUserVariable(sender);

            //create variable list
            var variableList = new List<Script.ScriptVariable>();
            foreach (DataRow rw in v_VariableAssignments.Rows)
            {
                var variableName = (string)rw.ItemArray[0];
                var variableValue = ((string)rw.ItemArray[1]).ConvertToUserVariable(sender);
                variableList.Add(new Script.ScriptVariable
                {
                    VariableName = variableName,
                    VariableValue = variableValue
                });
            }

            UI.Forms.frmScriptEngine newEngine = new UI.Forms.frmScriptEngine(startFile, null, variableList, true);
            Core.Automation.Engine.AutomationEngineInstance currentScriptEngine = (Core.Automation.Engine.AutomationEngineInstance) sender;
            currentScriptEngine.tasktEngineUI.Invoke((Action)delegate () { currentScriptEngine.tasktEngineUI.TopMost = false; });
            Application.Run(newEngine);
            //currentScriptEngine.tasktEngineUI.TopMost = false;
            currentScriptEngine.tasktEngineUI.Invoke((Action)delegate () { currentScriptEngine.tasktEngineUI.TopMost = true; });
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create file path and helpers
            RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_taskPath", this));
            var taskPathControl = UI.CustomControls.CommandControls.CreateDefaultInputFor("v_taskPath", this);
            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_taskPath", this, new Control[] { taskPathControl }, editor));
            RenderedControls.Add(taskPathControl);

            taskPathControl.TextChanged += TaskPathControl_TextChanged;

            //
            PassParameters = new CheckBox();
            PassParameters.AutoSize = true;
            PassParameters.Text = "I want to assign variables on startup";
            PassParameters.Font = new Font("Segoe UI Light", 12);
            PassParameters.ForeColor = Color.White;
   
            PassParameters.DataBindings.Add("Checked", this, "v_AssignVariables", false, DataSourceUpdateMode.OnPropertyChanged);
            PassParameters.CheckedChanged += PassParametersCheckbox_CheckedChanged;
            RenderedControls.Add(PassParameters);

            RenderedControls.Add(AssignmentsGridViewHelper);




            return RenderedControls;
        }

        private void TaskPathControl_TextChanged(object sender, EventArgs e)
        {
            PassParameters.Checked = false;
        }

        private void PassParametersCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            var Sender = (CheckBox)sender;
            AssignmentsGridViewHelper.Visible = Sender.Checked;

            //load variables if selected and file exists
            if ((Sender.Checked) && (System.IO.File.Exists(v_taskPath)))
            {
              
                Script.Script deserializedScript = Core.Script.Script.DeserializeFile(v_taskPath);

                foreach (var variable in deserializedScript.Variables)
                {
                    DataRow[] foundVariables  = v_VariableAssignments.Select("VariableName = '" + variable.VariableName + "'");
                    if (foundVariables.Length == 0)
                    {
                        v_VariableAssignments.Rows.Add(variable.VariableName, variable.VariableValue);
                    }                  
                }


                AssignmentsGridViewHelper.DataSource = v_VariableAssignments;
            }


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_taskPath + "]";
        }
    }
}