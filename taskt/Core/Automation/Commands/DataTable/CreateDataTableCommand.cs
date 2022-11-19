using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.Description("This command created a DataTable with the column names provided")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create a new DataTable")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Indicate DataTable Variable Name")]
        [InputSpecification("Indicate a unique reference name for later use")]
        [SampleUsage("**vMyDatatable** or **{{{vMyDatatable}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public string v_DataTableName { get; set; }

        [XmlElement]
        [PropertyDescription("Define Column Names")]
        [InputSpecification("Enter the Column Names required for each column of data")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true)]
        [PropertyDataGridViewColumnSettings("Column Name", "Column Name", false)]
        [PropertyDataGridViewCellEditEvent(nameof(ColumnNamesGridViewHelper_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        //[PropertyControlIntoCommandField("ColumnNamesGridViewHelper")]
        [PropertyDisplayText(true, "Columns")]
        public DataTable v_ColumnNameDataTable { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView ColumnNamesGridViewHelper;

        public CreateDataTableCommand()
        {
            this.CommandName = "CreateDataTableCommand";
            this.SelectionName = "Create DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //initialize data table
            //this.v_ColumnNameDataTable = new System.Data.DataTable
            //{
            //    TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            //};

            //this.v_ColumnNameDataTable.Columns.Add("Column Name");
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            //var dataTableName = v_DataTableName.ConvertToUserVariable(sender);

            DataTable newDT = new DataTable();

            foreach(DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                newDT.Columns.Add(rwColumnName.Field<string>("Column Name"));
            }

            newDT.StoreInUserVariable(engine, v_DataTableName);
        }


        private void ColumnNamesGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var ColumnNamesGridViewHelper = (DataGridView)sender;
            if (e.ColumnIndex >= 0)
            {
                ColumnNamesGridViewHelper.BeginEdit(false);
            }
            else
            {
                ColumnNamesGridViewHelper.EndEdit();
            }
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            var ColumnNamesGridViewHelper = (DataGridView)ControlsList[nameof(v_ColumnNameDataTable)];

            if (ColumnNamesGridViewHelper.IsCurrentCellDirty || ColumnNamesGridViewHelper.IsCurrentRowDirty)
            {
                ColumnNamesGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
                var newRow = v_ColumnNameDataTable.NewRow();
                v_ColumnNameDataTable.Rows.Add(newRow);
                for (var i = v_ColumnNameDataTable.Rows.Count - 1; i >= 0; i--)
                {
                    if (v_ColumnNameDataTable.Rows[i][0].ToString() == "")
                    {
                        v_ColumnNameDataTable.Rows[i].Delete();
                    }
                }
            }
        }
    }
}