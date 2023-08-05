using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script Commands")]
    [Attributes.ClassAttributes.SubGruop("taskt Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run Script File")]
    [Attributes.ClassAttributes.Description("This command runs tasks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run another task.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RunScriptFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Path to the Script File")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.xml**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "xml")]
        public string v_taskPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("I want to assign Variables on Startup")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertySelectionChangeEvent(nameof(cmbAssignVariables_SelectedItemChanged))]
        public string v_AssignVariables { get; set; }

        [XmlElement]
        [PropertyDescription("Assign Variables")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true)]
        [PropertyDataGridViewColumnSettings("VariableName", "Variable Name", true)]
        [PropertyDataGridViewColumnSettings("VariableValue", "Variable Value", false)]
        [PropertyDataGridViewColumnSettings("VariableReturn", "Variable Return", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox, "Yes\nNo")]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyCustomUIHelper("Reload Variables", nameof(lnkReloadVariables_Click))]
        public DataTable v_VariableAssignments { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitForFile { get; set; }

        public RunScriptFileCommand()
        {
            //this.CommandName = "RunTaskCommand";
            //this.SelectionName = "Run Task";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //v_VariableAssignments = new DataTable();
            //v_VariableAssignments.Columns.Add("VariableName"); 
            //v_VariableAssignments.Columns.Add("VariableValue");
            //v_VariableAssignments.Columns.Add("VariableReturn");
            //v_VariableAssignments.TableName = "RunTaskCommandInputParameters" + DateTime.Now.ToString("MMddyyhhmmss");

            //AssignmentsGridViewHelper = new DataGridView();
            //AssignmentsGridViewHelper.AllowUserToAddRows = true;
            //AssignmentsGridViewHelper.AllowUserToDeleteRows = true;
            //AssignmentsGridViewHelper.Size = new Size(400, 250);
            //AssignmentsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //AssignmentsGridViewHelper.DataSource = v_VariableAssignments;
            //AssignmentsGridViewHelper.Hide();
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var startFile = v_taskPath.ConvertToUserVariable(sender);
            //string startFile = FilePathControls.FormatFilePath_NoFileCounter(v_taskPath, engine, "xml", true);
            var startFile = FilePathControls.WaitForFile(this, nameof(v_taskPath), nameof(v_WaitForFile), engine);

            //create variable list
            var variableList = new List<Script.ScriptVariable>();
            var variableReturnList = new List<Script.ScriptVariable>();

            foreach (DataRow rw in v_VariableAssignments.Rows)
            {
                var variableName = rw.Field<string>("VariableName");
                object variableValue;
                try
                {
                    variableValue = rw.Field<string>("VariableValue").GetRawVariable(engine).VariableValue;
                }
                catch
                {
                    variableValue = rw.Field<string>("VariableValue").ConvertToUserVariable(engine);
                }

                var variableReturn = "No";
                if (rw.ItemArray[2].GetType().ToString() == "System.String")
                {
                    variableReturn = rw.Field<string>("VariableReturn");
                }
                variableList.Add(new Script.ScriptVariable
                {
                    VariableName = variableName,
                    VariableValue = variableValue
                });
                if (variableReturn == "Yes")
                {
                    variableReturnList.Add(new Script.ScriptVariable
                    {
                        VariableName = variableName,
                        VariableValue = variableValue
                    });
                }
            }

            frmScriptEngine newEngine = new frmScriptEngine(startFile, null, variableList, true, engine.PreloadedTasks);
            
            engine.tasktEngineUI.Invoke((Action)delegate () { engine.tasktEngineUI.TopMost = false; });
            Application.Run(newEngine);

            //get new variable list from the new task engine after it finishes running
            var newVariableList = newEngine.engineInstance.VariableList;
            foreach (var variable in variableReturnList)
            {
                //check if the variables we wish to return are in the new variable list
                if (newVariableList.Exists(x => x.VariableName == variable.VariableName))
                {
                    //if yes, get that variable from the new list
                    Script.ScriptVariable newTemp = newVariableList.Where(x => x.VariableName == variable.VariableName).FirstOrDefault();
                    //check if that variable previously existed in the current engine
                    if (engine.VariableList.Exists(x => x.VariableName == newTemp.VariableName))
                    {
                        //if yes, overwrite it 
                        Script.ScriptVariable currentTemp = engine.VariableList.Where(x => x.VariableName == newTemp.VariableName).FirstOrDefault();
                        engine.VariableList.Remove(currentTemp);
                    }
                    //Add to current engine variable list    
                    engine.VariableList.Add(newTemp);
                }  
            }

            engine.tasktEngineUI.Invoke((Action)delegate () { engine.tasktEngineUI.TopMost = true; });
        }

        private void lnkReloadVariables_Click(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)ControlsList[nameof(v_AssignVariables)];
            cmbAssignVariables_SelectedItemChanged(cmb, null);
        }

        private void cmbAssignVariables_SelectedItemChanged(object sender, EventArgs e)
        {
            var engine = new Engine.AutomationEngineInstance();

            var cmb = (ComboBox)sender;
            var engineSettings = ((frmCommandEditor)cmb.FindForm()).appSettings.EngineSettings;

            bool isVisible = (cmb.SelectedItem?.ToString().Trim().ToLower() == "yes");
            foreach(var item in ControlsList.Where(c => (c.Key.Contains(nameof(v_VariableAssignments)))))
            {
                item.Value.Visible = isVisible;
            }

            //var startFile = v_taskPath.ConvertToUserVariable(engine);
            //var startFile = FilePathControls.FormatFilePath_NoFileCounter(v_taskPath, engine, "xml", true);
            var startFile = this.ConvertToUserVariableAsFilePath(nameof(v_taskPath), engine);

            // check file exists
            if (!System.IO.File.Exists(startFile))
            {
                using(var fm = new taskt.UI.Forms.Supplemental.frmDialog("Script File Not Found. Name: " + startFile, "error", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, 0))
                {
                    fm.ShowDialog();
                }
                return;
            }

            //load variables if selected and file exists
            if (isVisible)
            {
                Script.Script deserializedScript = Script.Script.DeserializeFile(startFile, engineSettings);

                v_VariableAssignments.Rows.Clear();
                foreach (var variable in deserializedScript.Variables)
                {
                    DataRow[] foundVariables  = v_VariableAssignments.Select("VariableName = '" + variable.VariableName + "'");
                    if (foundVariables.Length == 0)
                    {
                        v_VariableAssignments.Rows.Add(variable.VariableName, variable.VariableValue);
                    }                  
                }

                DataGridView AssignmentsGridViewHelper = (DataGridView)ControlsList[nameof(v_VariableAssignments)];

                for (int i = 0; i < AssignmentsGridViewHelper.Rows.Count - 1; i++)
                {
                    DataGridViewComboBoxCell returnComboBox = new DataGridViewComboBoxCell();
                    returnComboBox.Items.Add("Yes");
                    returnComboBox.Items.Add("No");
                    AssignmentsGridViewHelper.Rows[i].Cells[2] = returnComboBox;
                }
            }
        }
    }
}