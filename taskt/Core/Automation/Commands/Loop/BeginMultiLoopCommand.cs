using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.User32;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Drawing;
using System.Text;

namespace taskt.Core.Automation.Commands
{




    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to evaluate a logical statement to determine if the statement is true. The following actions will repeat continuously until that statement becomes false")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check if a statement is 'true' or 'false' and subsequently loop actions based on either condition. Any 'BeginLoop' command must have a following 'EndLoop' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command evaluates supplied arguments and if evaluated to true runs sub elements")]
    public class BeginMultiLoopCommand : ScriptCommand
    {

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Multiple Loop Conditions - All Must Be True")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowLoopBuilder)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_LoopConditionsTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView LoopConditionHelper;

        [XmlIgnore]
        private List<Script.ScriptVariable> ScriptVariables { get; set; }

        public BeginMultiLoopCommand()
        {
            this.CommandName = "BeginMultiLoopCommand";
            this.SelectionName = "Begin Multi Loop";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            v_LoopConditionsTable = new DataTable();
            v_LoopConditionsTable.TableName = DateTime.Now.ToString("MultiLoopConditionTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_LoopConditionsTable.Columns.Add("Statement");
            v_LoopConditionsTable.Columns.Add("CommandData");

        }


        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

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


        private bool DetermineMultiStatementTruth(object sender)
        {
            bool isTrueStatement = true;
            foreach (DataRow rw in v_LoopConditionsTable.Rows)
            {
                var commandData = rw["CommandData"].ToString();
                var loopCommand = Newtonsoft.Json.JsonConvert.DeserializeObject<Commands.BeginLoopCommand>(commandData);

                var statementResult = loopCommand.DetermineStatementTruth(sender);

                if (!statementResult)
                {
                    isTrueStatement = false;
                    break;
                }
            }
            return isTrueStatement;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //get script variables for feeding into loop builder form
            ScriptVariables = editor.scriptVariables;

            //create controls
            var controls = CommandControls.CreateDataGridViewGroupFor("v_LoopConditionsTable", this, editor);
            LoopConditionHelper = controls[2] as DataGridView;

            //handle helper click
            var helper = controls[1] as taskt.UI.CustomControls.CommandItemControl;
            helper.Click += (sender, e) => CreateLoopCondition(sender, e);

            //add for rendering
            RenderedControls.AddRange(controls);

            //define if condition helper
            LoopConditionHelper.Width = 450;
            LoopConditionHelper.Height = 200;
            LoopConditionHelper.AutoGenerateColumns = false;
            LoopConditionHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            LoopConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Condition", DataPropertyName = "Statement", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            LoopConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "CommandData", DataPropertyName = "CommandData", ReadOnly = true, Visible = false });
            LoopConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Edit", UseColumnTextForButtonValue = true, Text = "Edit", Width = 45 });
            LoopConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Delete", UseColumnTextForButtonValue = true, Text = "Delete", Width = 45 });
            LoopConditionHelper.AllowUserToAddRows = false;
            LoopConditionHelper.AllowUserToDeleteRows = true;
            LoopConditionHelper.CellContentClick += LoopConditionHelper_CellContentClick;


            return RenderedControls;
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

                    var loopCommand = Newtonsoft.Json.JsonConvert.DeserializeObject<Commands.BeginLoopCommand>(commandData);

                    var automationCommands = taskt.UI.CustomControls.CommandControls.GenerateCommandsandControls().Where(f => f.Command is BeginLoopCommand).ToList();
                    frmCommandEditor editor = new frmCommandEditor(automationCommands, null);
                    editor.selectedCommand = loopCommand;
                    editor.editingCommand = loopCommand;
                    editor.originalCommand = loopCommand;
                    editor.creationMode = frmCommandEditor.CreationMode.Edit;
                    editor.scriptVariables = ScriptVariables;

                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        var editedCommand = editor.editingCommand as BeginLoopCommand;
                        var displayText = editedCommand.GetDisplayValue();
                        var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(editedCommand);

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

            var automationCommands = taskt.UI.CustomControls.CommandControls.GenerateCommandsandControls().Where(f => f.Command is BeginLoopCommand).ToList();

            frmCommandEditor editor = new frmCommandEditor(automationCommands, null);
            editor.selectedCommand = new BeginLoopCommand();
            var res = editor.ShowDialog();

            if (res == DialogResult.OK)
            {
                //get data
                var configuredCommand = editor.selectedCommand as BeginLoopCommand;
                var displayText = configuredCommand.GetDisplayValue();
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(configuredCommand);

                //add to list
                v_LoopConditionsTable.Rows.Add(displayText, serializedData);

            }

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
    }
}