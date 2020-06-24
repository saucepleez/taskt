using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("If Commands")]
    [Description("This command evaluates a group of combined logical statements to determine if the combined result of the statements is 'true' or 'false' and subsequently performs action(s) based on the result.")]
    public class BeginMultiIfCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyDescription("Multiple If Conditions")]
        [InputSpecification("Add new If condition(s).")]
        [SampleUsage("")]
        [Remarks("All of the conditions must be true to execute the If block.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowIfBuilder)]
        public DataTable v_IfConditionsTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _ifConditionHelper;

        [XmlIgnore]
        private List<ScriptVariable> _scriptVariables { get; set; }

        public BeginMultiIfCommand()
        {
            CommandName = "BeginMultiIfCommand";
            SelectionName = "Begin Multi If";
            CommandEnabled = true;
            CustomRendering = true;

            v_IfConditionsTable = new DataTable();
            v_IfConditionsTable.TableName = DateTime.Now.ToString("MultiIfConditionTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_IfConditionsTable.Columns.Add("Statement");
            v_IfConditionsTable.Columns.Add("CommandData");
        }
       
        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            var engine = (AutomationEngineInstance)sender;

            bool isTrueStatement = true;
            foreach (DataRow rw in v_IfConditionsTable.Rows)
            {
                var commandData = rw["CommandData"].ToString();
                var ifCommand = JsonConvert.DeserializeObject<BeginIfCommand>(commandData);
                var statementResult = ifCommand.DetermineStatementTruth(sender);

                if (!statementResult)
                {
                    isTrueStatement = false;
                    break;
                }
            }

            //report evaluation
            if (isTrueStatement)
            {
                engine.ReportProgress("If Conditions Evaluated True");
            }
            else
            {
                engine.ReportProgress("If Conditions Evaluated False");
            }
            
            int startIndex, endIndex, elseIndex;
            if (parentCommand.AdditionalScriptCommands.Any(item => item.ScriptCommand is ElseCommand))
            {
                elseIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is ElseCommand);

                if (isTrueStatement)
                {
                    startIndex = 0;
                    endIndex = elseIndex;
                }
                else
                {
                    startIndex = elseIndex + 1;
                    endIndex = parentCommand.AdditionalScriptCommands.Count;
                }
            }
            else if (isTrueStatement)
            {
                startIndex = 0;
                endIndex = parentCommand.AdditionalScriptCommands.Count;
            }
            else
            {
                return;
            }

            for (int i = startIndex; i < endIndex; i++)
            {
                if ((engine.IsCancellationPending) || (engine.CurrentLoopCancelled))
                    return;

                engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[i]);
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            
            //get script variables for feeding into if builder form
            _scriptVariables = editor.ScriptVariables;

            //create controls
            var controls = CommandControls.CreateDataGridViewGroupFor("v_IfConditionsTable", this, editor);
            _ifConditionHelper = controls[2] as DataGridView;

            //handle helper click
            var helper = controls[1] as CommandItemControl;
            helper.Click += (sender, e) => CreateIfCondition(sender, e);

            //add for rendering
            RenderedControls.AddRange(controls);

            //define if condition helper
            _ifConditionHelper.Width = 450;
            _ifConditionHelper.Height = 200;
            _ifConditionHelper.AutoGenerateColumns = false;
            _ifConditionHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            _ifConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Condition", DataPropertyName = "Statement", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _ifConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "CommandData", DataPropertyName = "CommandData", ReadOnly = true, Visible = false });
            _ifConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Edit", UseColumnTextForButtonValue = true,  Text = "Edit", Width = 45 });
            _ifConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Delete", UseColumnTextForButtonValue = true, Text = "Delete", Width = 45 });
            _ifConditionHelper.AllowUserToAddRows = false;
            _ifConditionHelper.AllowUserToDeleteRows = true;
            _ifConditionHelper.CellContentClick += IfConditionHelper_CellContentClick;

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            if (v_IfConditionsTable.Rows.Count == 0)
            {
                return "If <Not Configured>";
            }
            else
            {
                var statements = v_IfConditionsTable.AsEnumerable().Select(f => f.Field<string>("Statement")).ToList();
                return string.Join(" && ", statements);
            }
        }

        private void IfConditionHelper_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var buttonSelected = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
                var selectedRow = v_IfConditionsTable.Rows[e.RowIndex];

                if (buttonSelected.Value.ToString() == "Edit")
                {
                    //launch editor
                    var statement = selectedRow["Statement"];
                    var commandData = selectedRow["CommandData"].ToString();

                    var ifCommand = JsonConvert.DeserializeObject<BeginIfCommand>(commandData);

                    var automationCommands = CommandControls.GenerateCommandsandControls().Where(f => f.Command is BeginIfCommand).ToList();
                    frmCommandEditor editor = new frmCommandEditor(automationCommands, null);
                    editor.SelectedCommand = ifCommand;
                    editor.EditingCommand = ifCommand;
                    editor.OriginalCommand = ifCommand;
                    editor.CreationModeInstance = frmCommandEditor.CreationMode.Edit;
                    editor.ScriptVariables = _scriptVariables;

                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        var editedCommand = editor.EditingCommand as BeginIfCommand;
                        var displayText = editedCommand.GetDisplayValue();
                        var serializedData = JsonConvert.SerializeObject(editedCommand);

                        selectedRow["Statement"] = displayText;
                        selectedRow["CommandData"] = serializedData;
                    }
                }
                else if (buttonSelected.Value.ToString() == "Delete")
                {
                    //delete
                    v_IfConditionsTable.Rows.Remove(selectedRow);
                }
                else
                {
                    throw new NotImplementedException("Requested Action is not implemented.");
                }
            }
        }

        private void CreateIfCondition(object sender, EventArgs e)
        {
            var automationCommands = CommandControls.GenerateCommandsandControls().Where(f => f.Command is BeginIfCommand).ToList();

            frmCommandEditor editor = new frmCommandEditor(automationCommands, null);
            editor.SelectedCommand = new BeginIfCommand();
            var res = editor.ShowDialog();

            if (res == DialogResult.OK)
            {
                //get data
                var configuredCommand = editor.SelectedCommand as BeginIfCommand;
                var displayText = configuredCommand.GetDisplayValue();
                var serializedData = JsonConvert.SerializeObject(configuredCommand);

                //add to list
                v_IfConditionsTable.Rows.Add(displayText, serializedData);
            }
        }

       
    }
}