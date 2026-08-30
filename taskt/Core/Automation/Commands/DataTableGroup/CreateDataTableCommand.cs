using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.CommandSettings("Create DataTable")]
    [Attributes.ClassAttributes.Description("This command created a DataTable with the column names provided")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a new DataTable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CreateDataTableCommand : ADataTableInputDataTableCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        public override string v_DataTable { get; set; }

        [XmlElement]
        [PropertyDescription("Column Names")]
        [InputSpecification("Enter the Column Names required for each column of data")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true)]
        [PropertyDataGridViewColumnSettings("Column Name", "Column Name", false)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls)+"+"+nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyDisplayText(true, "Columns")]
        [PropertyParameterOrder(6000)]
        public DataTable v_ColumnNameDataTable { get; set; }

        public CreateDataTableCommand()
        {
            //this.CommandName = "CreateDataTableCommand";
            //this.SelectionName = "Create DataTable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //initialize data table
            //this.v_ColumnNameDataTable = new System.Data.DataTable
            //{
            //    TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            //};

            //this.v_ColumnNameDataTable.Columns.Add("Column Name");
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var newDT = new DataTable();

            var nameList = new List<string>();

            // check column name is empty
            //for (int i = v_ColumnNameDataTable.Rows.Count - 1; i >= 0 ; i--)
            for (int i = 0; i < v_ColumnNameDataTable.Rows.Count; i++)
            {
                var name = v_ColumnNameDataTable.Rows[i].Field<string>("Column Name") ?? "";
                if (!string.IsNullOrEmpty(name))
                {
                    nameList.Add(name);
                }
                else
                {
                    
                    throw new Exception($"Column Name is Empty. Row: {i}");
                }
            }

            // dup check
            int num = nameList.Count;
            for (int i = 0; i < num - 1; i++)
            {
                for (int j = i + 1; j < num; j++)
                {
                    if (nameList[i] == nameList[j])
                    {
                        throw new Exception($"Duplicate Column Name '{nameList[i]}'. Row1: {i}, Row2: {j}");
                    }
                }
            }

            //foreach(DataRow row in v_ColumnNameDataTable.Rows)
            //{
            //    newDT.Columns.Add(row.Field<string>("Column Name"));
            //}

            foreach(var col in nameList)
            {
                newDT.Columns.Add(col);
            }

            //newDT.StoreInUserVariable(engine, v_DataTable);
            this.StoreDataTableInUserVariable(newDT, nameof(v_DataTable), engine);
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ColumnNameDataTable)], v_ColumnNameDataTable);
        }
    }
}