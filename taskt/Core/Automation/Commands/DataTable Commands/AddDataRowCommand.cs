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
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.UI.Forms.Supplement_Forms;


namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command adds a DataRow to a DataTable.")]

    public class AddDataRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("DataTable")]
        [InputSpecification("Enter an existing DataTable to add a DataRow to.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTable { get; set; }

        [XmlElement]
        [PropertyDescription("Data")]
        [InputSpecification("Enter Column Names and Data for each column in the DataRow.")]
        [SampleUsage("[ First Name | John ] || [ {vColumn} | {vData} ]")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_DataRowDataTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private List<CreateDataTableCommand> _dataTableCreationCommands;

        public AddDataRowCommand()
        {
            CommandName = "AddDataRowCommand";
            SelectionName = "Add DataRow";
            CommandEnabled = true;
            CustomRendering = true;

            //initialize data table
            v_DataRowDataTable = new DataTable
            {
                TableName = "AddDataDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            v_DataRowDataTable.Columns.Add("Column Name");
            v_DataRowDataTable.Columns.Add("Data");
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var dataSetVariable = LookupVariable(engine);

            DataTable Dt = (DataTable)dataSetVariable.VariableValue;
            var newRow = Dt.NewRow();

            foreach (DataRow rw in v_DataRowDataTable.Rows)
            {
                var columnName = rw.Field<string>("Column Name").ConvertToUserVariable(sender);
                var data = rw.Field<string>("Data").ConvertToUserVariable(sender);
                newRow.SetField(columnName, data);
            }
            Dt.Rows.Add(newRow);

            dataSetVariable.VariableValue = Dt;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_DataRowDataTable", this, editor));

            CommandItemControl loadSchemaControl = new CommandItemControl();
            loadSchemaControl.ForeColor = System.Drawing.Color.White;
            loadSchemaControl.CommandDisplay = "Load Column Names From Existing DataTable";
            loadSchemaControl.Click += LoadSchemaControl_Click;
            RenderedControls.Add(loadSchemaControl);

            _dataTableCreationCommands = editor.ConfiguredCommands.Where(f => f is CreateDataTableCommand)
                                                                 .Select(f => (CreateDataTableCommand)f)
                                                                 .ToList();

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Add {v_DataRowDataTable.Rows.Count} Field(s) to '{v_DataTable}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTable).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if (requiredVariable == null && v_DataTable.StartsWith(sendingInstance.EngineSettings.VariableStartMarker) 
                                         && v_DataTable.EndsWith(sendingInstance.EngineSettings.VariableEndMarker))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTable.Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                                                     .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }
            return requiredVariable;
        }

        private void LoadSchemaControl_Click(object sender, EventArgs e)
        {
            frmVariableSelector selectionForm = new frmVariableSelector();
            selectionForm.Text = "Load Schema";
            selectionForm.lblHeader.Text = "Select a DataTable from the list";
            foreach (var item in _dataTableCreationCommands)
            {
                selectionForm.lstVariables.Items.Add(item.v_DataTable);
            }

            var result = selectionForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                var tableName = selectionForm.lstVariables.SelectedItem.ToString();
                var schema = _dataTableCreationCommands.Where(f => f.v_DataTable == tableName).FirstOrDefault();

                v_DataRowDataTable.Rows.Clear();

                foreach (DataRow rw in schema.v_ColumnNameDataTable.Rows)
                {
                    v_DataRowDataTable.Rows.Add(rw.Field<string>("Column Name"), "");
                }
            }
        }
    }
}