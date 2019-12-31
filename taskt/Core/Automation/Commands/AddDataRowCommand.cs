using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to add a datarow to a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a datarow to a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class AddDataRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing DataTable to add rows to.")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataTableName { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Define Data")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the Column Names required for each column of data")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_AddDataDataTable { get; set; }


        [XmlIgnore]
        [NonSerialized]
        private List<CreateDataTableCommand> DataTableCreationCommands;

        public AddDataRowCommand()
        {
            this.CommandName = "AddDataRowCommand";
            this.SelectionName = "Add DataRow";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //initialize data table
            this.v_AddDataDataTable = new System.Data.DataTable
            {
                TableName = "AddDataDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            this.v_AddDataDataTable.Columns.Add("Column Name");
            this.v_AddDataDataTable.Columns.Add("Data");

        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var dataSetVariable = LookupVariable(engine);

            DataTable Dt = (DataTable)dataSetVariable.VariableValue;
            var newRow = Dt.NewRow();

            foreach (DataRow rw in v_AddDataDataTable.Rows)
            {
                var columnName = rw.Field<string>("Column Name").ConvertToUserVariable(sender);
                var data = rw.Field<string>("Data").ConvertToUserVariable(sender);
                newRow.SetField(columnName, data); 
            }

            Dt.Rows.Add(newRow);
        
            dataSetVariable.VariableValue = Dt;
        }
        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTableName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_DataTableName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_DataTableName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTableName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTableName", this, editor));


            RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_AddDataDataTable", this, editor));

            taskt.UI.CustomControls.CommandItemControl loadSchemaControl = new taskt.UI.CustomControls.CommandItemControl();
            loadSchemaControl.ForeColor = System.Drawing.Color.White;
            loadSchemaControl.CommandDisplay = "Load Column Names From Existing Table";
            loadSchemaControl.Click += LoadSchemaControl_Click;
            RenderedControls.Add(loadSchemaControl);

            DataTableCreationCommands = editor.configuredCommands.Where(f => f is CreateDataTableCommand).Select(f => (CreateDataTableCommand)f).ToList();




            return RenderedControls;
        }

        private void LoadSchemaControl_Click(object sender, EventArgs e)
        {
            UI.Forms.Supplemental.frmItemSelector selectionForm = new UI.Forms.Supplemental.frmItemSelector();
            selectionForm.Text = "Load Schema";
            selectionForm.lblHeader.Text = "Select a table from the list";
            foreach (var item in DataTableCreationCommands)
            {
                selectionForm.lstVariables.Items.Add(item.v_DataTableName);
            }

           var result = selectionForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                var tableName = selectionForm.lstVariables.SelectedItem.ToString();
                var schema = DataTableCreationCommands.Where(f => f.v_DataTableName == tableName).FirstOrDefault();

                v_AddDataDataTable.Rows.Clear();

                foreach (DataRow rw in schema.v_ColumnNameDataTable.Rows)
                {
                    v_AddDataDataTable.Rows.Add(rw.Field<string>("Column Name"), "");
                }

            }   
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_AddDataDataTable.Rows.Count} Fields, apply to '{v_DataTableName}']";
        }
    }
}