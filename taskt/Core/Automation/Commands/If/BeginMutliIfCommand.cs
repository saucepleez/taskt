﻿using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("If Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to evaluate a logical statement to determine if the statement is true.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check if a statement is 'true' or 'false' and subsequently take an action based on either condition. Any 'BeginIf' command must have a following 'EndIf' command.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command evaluates supplied arguments and if evaluated to true runs sub elements")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_begin_multi_if))]
    public class BeginMultiIfCommand : ScriptCommand, IHaveDataTableElements
    {
        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Multiple If Conditions - All Must Be True")]
        //[Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowIfBuilder)]
        [Attributes.PropertyAttributes.PropertyCustomUIHelper("Add New If Statement", nameof(CreateIfCondition))]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_IfConditionsTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView IfConditionHelper;

        [XmlIgnore]
        [NonSerialized]
        private ApplicationSettings appSetting;

        [XmlIgnore]
        private List<Script.ScriptVariable> ScriptVariables { get; set; }

        public BeginMultiIfCommand()
        {
            this.CommandName = "BeginMultiIfCommand";
            this.SelectionName = "Begin Multi If";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            v_IfConditionsTable = new DataTable();
            v_IfConditionsTable.TableName = DateTime.Now.ToString("MultiIfConditionTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_IfConditionsTable.Columns.Add("Statement");
            v_IfConditionsTable.Columns.Add("CommandData");
        }

       
        public override void RunCommand(Engine.AutomationEngineInstance engine, Core.Script.ScriptAction parentCommand)
        {
            bool isTrueStatement = true;
            foreach (DataRow rw in v_IfConditionsTable.Rows)
            {
                var commandData = rw["CommandData"].ToString();
                var ifCommand = Newtonsoft.Json.JsonConvert.DeserializeObject<Commands.BeginIfCommand>(commandData);

                //var statementResult = ifCommand.DetermineStatementTruth(sender);
                var statementResult = ConditionControls.DetermineStatementTruth(ifCommand.v_IfActionType, ifCommand.v_IfActionParameterTable, engine);

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
            if (parentCommand.AdditionalScriptCommands.Any(item => item.ScriptCommand is Core.Automation.Commands.ElseCommand))
            {
                elseIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is Core.Automation.Commands.ElseCommand);

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

        public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.Render(editor);
            
            //get script variables for feeding into if builder form
            ScriptVariables = editor.scriptVariables;

            //create controls
            var controls = CommandControls.CreateDataGridViewGroupFor("v_IfConditionsTable", this, editor);
            //IfConditionHelper = controls[2] as DataGridView;
            IfConditionHelper = (DataGridView)(controls.Where(c => (c is DataGridView)).FirstOrDefault());

            //handle helper click
            //var helper = controls[1] as taskt.UI.CustomControls.CommandItemControl;
            //helper.Click += (sender, e) => CreateIfCondition(sender, e);

            //add for rendering
            RenderedControls.AddRange(controls);

            //define if condition helper
            IfConditionHelper.Width = 450;
            IfConditionHelper.Height = 200;
            IfConditionHelper.AutoGenerateColumns = false;
            IfConditionHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            IfConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "Condition", DataPropertyName = "Statement", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            IfConditionHelper.Columns.Add(new DataGridViewTextBoxColumn() { HeaderText = "CommandData", DataPropertyName = "CommandData", ReadOnly = true, Visible = false });
            IfConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Edit", UseColumnTextForButtonValue = true,  Text = "Edit", Width = 45 });
            IfConditionHelper.Columns.Add(new DataGridViewButtonColumn() { HeaderText = "Delete", UseColumnTextForButtonValue = true, Text = "Delete", Width = 45 });
            IfConditionHelper.AllowUserToAddRows = false;
            IfConditionHelper.AllowUserToDeleteRows = true;
            IfConditionHelper.CellContentClick += IfConditionHelper_CellContentClick;

            this.appSetting = editor.appSettings;

            return RenderedControls;
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

                    var ifCommand = Newtonsoft.Json.JsonConvert.DeserializeObject<Commands.BeginIfCommand>(commandData);

                    var automationCommands = taskt.UI.CustomControls.CommandControls.GenerateCommandsAndControls().Where(f => f.Command is BeginIfCommand).ToList();
                    var editor = new UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor(automationCommands, null);
                    editor.selectedCommand = ifCommand;
                    editor.editingCommand = ifCommand;
                    editor.originalCommand = ifCommand;
                    editor.creationMode = UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor.CreationMode.Edit;
                    editor.scriptVariables = ScriptVariables;
                    editor.appSettings = this.appSetting;

                    if (editor.ShowDialog() == DialogResult.OK)
                    {
                        var editedCommand = editor.editingCommand as BeginIfCommand;
                        var displayText = editedCommand.GetDisplayValue();
                        var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(editedCommand);

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
            var automationCommands = taskt.UI.CustomControls.CommandControls.GenerateCommandsAndControls().Where(f => f.Command is BeginIfCommand).ToList();

            var editor = new UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor(automationCommands, null);
            editor.selectedCommand = new BeginIfCommand();
            editor.appSettings = this.appSetting;
            var res = editor.ShowDialog();

            if (res == DialogResult.OK)
            {
                //get data
                var configuredCommand = editor.selectedCommand as BeginIfCommand;
                var displayText = configuredCommand.GetDisplayValue();
                var serializedData = Newtonsoft.Json.JsonConvert.SerializeObject(configuredCommand);

                //add to list
                v_IfConditionsTable.Rows.Add(displayText, serializedData);
            }
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
    }
}