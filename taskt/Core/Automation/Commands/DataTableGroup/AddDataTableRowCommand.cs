using System;
using System.Data;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Add DataTable Row")]
    [Attributes.ClassAttributes.Description("This command allows you to add a row to a DataTable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add a row to a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class AddDataTableRowCommand : ADataTableBothDataTableCommands, IHaveDataTableElements
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        //public string v_DataTable { get; set; }

        [XmlElement]
        [PropertyDescription("Column Names and Values")]
        [InputSpecification("Enter the Column Names required for each column of data")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true)]
        [PropertyDataGridViewColumnSettings("Column Name", "Column Name", false)]
        [PropertyDataGridViewColumnSettings("Data", "Data", false)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        // todo: enable
        //[PropertyCustomUIHelper("Load Column Names From Existing Table", nameof(LoadSchemaControl_Click), "load_column")]
        [PropertyDisplayText(true, "Rows", "items")]
        [PropertyParameterOrder(6000)]
        public DataTable v_AddDataDataTable { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private List<CreateDataTableCommand> DataTableCreationCommands;

        public AddDataTableRowCommand()
        {
            //this.CommandName = "AddDataTableRowCommand";
            //this.SelectionName = "Add DataTable Row";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //DataTable dataTable = v_DataTable.ExpandUserVariableAsDataTable(engine);
            var myDT = this.ExpandUserVariableAsDataTable(engine);

            var newRow = myDT.NewRow();

            foreach (DataRow rw in v_AddDataDataTable.Rows)
            {
                var columnName = (rw.Field<string>("Column Name") ?? "").ExpandValueOrUserVariable(engine);
                var data = (rw.Field<string>("Data") ?? "").ExpandValueOrUserVariable(engine);
                newRow.SetField(columnName, data); 
            }

            myDT.Rows.Add(newRow);
        }

        //public override List<Control> Render(UI.Forms.frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    DataTableCreationCommands = editor.configuredCommands.Where(f => f is CreateDataTableCommand).Select(f => (CreateDataTableCommand)f).ToList();

        //    return RenderedControls;
        //}

        //private void LoadSchemaControl_Click(object sender, EventArgs e)
        //{
        //    var items = new List<string>();
        //    foreach (var item in DataTableCreationCommands)
        //    {
        //        items.Add(item.v_DataTableName);
        //    }

        //    using (var selectionForm = new UI.Forms.Supplemental.frmItemSelector(items, "Load Schema", "Select a table from the list"))
        //    {
        //        if (selectionForm.ShowDialog() == DialogResult.OK)
        //        {
        //            var tableName = selectionForm.selectedItem.ToString();
        //            var schema = DataTableCreationCommands.Where(f => f.v_DataTableName == tableName).FirstOrDefault();

        //            v_AddDataDataTable.Rows.Clear();

        //            foreach (DataRow rw in schema.v_ColumnNameDataTable.Rows)
        //            {
        //                v_AddDataDataTable.Rows.Add(rw.Field<string>("Column Name"), "");
        //            }
        //        }
        //    }
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + $" [{v_AddDataDataTable.Rows.Count} Fields, apply to '{v_DataTableName}']";
        //}

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_AddDataDataTable)], v_AddDataDataTable);
        }
    }
}