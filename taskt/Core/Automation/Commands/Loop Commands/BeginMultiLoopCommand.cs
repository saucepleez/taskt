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
    [Group("Loop Commands")]
    [Description("This command evaluates a group of specified logical statements and executes the contained commands repeatedly (in loop) " +
        "until the result of the logical statements becomes false.")]
    public class BeginMultiLoopCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyDescription("Multiple Loop Conditions")]
        [InputSpecification("Add new Loop condition(s).")]
        [SampleUsage("")]
        [Remarks("All of the conditions must be true to execute the loop block.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowLoopBuilder)]
        public DataTable v_LoopConditionsTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _loopConditionHelper;

        [XmlIgnore]
        private List<ScriptVariable> _scriptVariables { get; set; }

        public BeginMultiLoopCommand()
        {
            CommandName = "BeginMultiLoopCommand";
            SelectionName = "Begin Multi Loop";
            CommandEnabled = true;
            CustomRendering = true;

            v_LoopConditionsTable = new DataTable();
            v_LoopConditionsTable.TableName = DateTime.Now.ToString("MultiLoopConditionTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_LoopConditionsTable.Columns.Add("Statement");
            v_LoopConditionsTable.Columns.Add("CommandData");
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            var engine = (AutomationEngineInstance)sender;
            bool isTrueStatement = DetermineMultiStatementTruth(sender);
            engine.ReportProgress("Starting Loop");

            while (isTrueStatement)
            {
                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop");
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress("Continuing Next Loop");
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }
                isTrueStatement = DetermineMultiStatementTruth(sender);
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //get script variables for feeding into loop builder form
            _scriptVariables = editor.ScriptVariables;

            //create controls
            var controls = CommandControls.CreateDataGridViewGroupFor("v_LoopConditionsTable", this, editor);
            _loopConditionHelper = controls[2] as DataGridView;

            //handle helper click
            var helper = controls[1] as CommandItemControl;
            helper.Click += (sender, e) => CreateLoopCondition(sender, e);

            //add for rendering
            RenderedControls.AddRange(controls);

            //define if condition helper
            _loopConditionHelper.Width = 450;
            _loopConditionHelper.Height = 200;
            _loopConditionHelper.AutoGenerateColumns = false;
            _loopConditionHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            _loopConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Condition", DataPropertyName = "Statement", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _loopConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "CommandData", DataPropertyName = "CommandData", ReadOnly = true, Visible = false });
            _loopConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Edit", UseColumnTextForButtonValue = true, Text = "Edit", Width = 45 });
            _loopConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Delete", UseColumnTextForButtonValue = true, Text = "Delete", Width = 45 });
            _loopConditionHelper.AllowUserToAddRows = false;
            _loopConditionHelper.AllowUserToDeleteRows = true;
            _loopConditionHelper.CellContentClick += LoopConditionHelper_CellContentClick;

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            if (v_LoopConditionsTable.Rows.Count == 0)
            {
                return "Loop <Not Configured>";
            }
            else
            {
                var statements = v_LoopConditionsTable.AsEnumerable().Select(f => f.Field<string>("Statement")).ToList();
                return string.Join(" && ", statements);
            }
        }

        private bool DetermineMultiStatementTruth(object sender)
        {
            bool isTrueStatement = true;
            foreach (DataRow rw in v_LoopConditionsTable.Rows)
            {
                var commandData = rw["CommandData"].ToString();
                var loopCommand = JsonConvert.DeserializeObject<BeginLoopCommand>(commandData);
                var statementResult = loopCommand.DetermineStatementTruth(sender);

                if (!statementResult)
                {
                    isTrueStatement = false;
                    break;
                }
            }
            return isTrueStatement;
        }

        private void LoopConditionHelper_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var buttonSelected = senderGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewButtonCell;
                var selectedRow = v_LoopConditionsTable.Rows[e.RowIndex];

                if (buttonSelected.Value.ToString() == "Edit")
                {
                    //launch editor
                    var statement = selectedRow["Statement"];
                    var commandData = selectedRow["CommandData"].ToString();
                    var loopCommand = JsonConvert.DeserializeObject<BeginLoopCommand>(commandData);

                    var automationCommands = CommandControls.GenerateCommandsandControls().Where(f => f.Command is BeginLoopCommand).ToList();
                    frmCommandEditor editor = new frmCommandEditor(automationCommands, null);
                    editor.SelectedCommand = loopCommand;
                    editor.EditingCommand = loopCommand;
                    editor.OriginalCommand = loopCommand;
                    editor.CreationModeInstance = frmCommandEditor.CreationMode.Edit;
                    editor.ScriptVariables = _scriptVariables;

                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        var editedCommand = editor.EditingCommand as BeginLoopCommand;
                        var displayText = editedCommand.GetDisplayValue();
                        var serializedData = JsonConvert.SerializeObject(editedCommand);

                        selectedRow["Statement"] = displayText;
                        selectedRow["CommandData"] = serializedData;
                    }
                }
                else if (buttonSelected.Value.ToString() == "Delete")
                {
                    //delete
                    v_LoopConditionsTable.Rows.Remove(selectedRow);
                }
                else
                {
                    throw new NotImplementedException("Requested Action is not implemented.");
                }
            }
        }

        private void CreateLoopCondition(object sender, EventArgs e)
        {
            var automationCommands = CommandControls.GenerateCommandsandControls().Where(f => f.Command is BeginLoopCommand).ToList();

            frmCommandEditor editor = new frmCommandEditor(automationCommands, null);
            editor.SelectedCommand = new BeginLoopCommand();
            var res = editor.ShowDialog();

            if (res == DialogResult.OK)
            {
                //get data
                var configuredCommand = editor.SelectedCommand as BeginLoopCommand;
                var displayText = configuredCommand.GetDisplayValue();
                var serializedData = JsonConvert.SerializeObject(configuredCommand);

                //add to list
                v_LoopConditionsTable.Rows.Add(displayText, serializedData);
            }
        }
    }
}