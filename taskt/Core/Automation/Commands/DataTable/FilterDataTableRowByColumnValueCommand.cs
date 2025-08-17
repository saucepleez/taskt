using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.CommandSettings("Filter DataTable Row By Column Value")]
    [Attributes.ClassAttributes.Description("This command allows you to Filter Rows by reference to Column values.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Filter Rows by reference to Column values.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class FilterDataTableRowByColumnValueCommand : ADataTableCreateFromDataTableCommands, IDataTableColumnPositionProperties, IFilterValueProperties, IHaveDataTableElements
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to Filter")]
        [PropertyValidationRule("DataTable to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to Filter")]
        public override string v_TargetDataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        [PropertyParameterOrder(6000)]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        [PropertyParameterOrder(6001)]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterValueType))]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        [PropertyParameterOrder(6002)]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterAction))]
        [PropertySelectionChangeEvent(nameof(cmbFilterAction_SelectionChangeCommited))]
        [PropertyParameterOrder(6003)]
        public string v_FilterAction { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ActionParameterTable))]
        [PropertyParameterOrder(6004)]
        public DataTable v_FilterActionParameterTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_NewOutputDataTableName))]
        //public string v_NewDataTable { get; set; }

        public FilterDataTableRowByColumnValueCommand()
        {
            //this.CommandName = "FilterDataTableRowByColumnValueCommand";
            //this.SelectionName = "Filter DataTable Row By Column Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //this.v_TargetType = "Text";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var targetDT, var colIndex) = this.ExpandUserVariablesAsDataTableAndColumnIndex(nameof(v_TargetDataTable), nameof(v_ColumnType), nameof(v_ColumnIndex), engine);
            var targetDT = this.ExpandUserVariableAsDataTable(engine);
            (var colIndex, _) = this.ExpandValueOrUserVariableAsDataTableColumn(targetDT, engine);

            var parameters = DataTableControls.GetFieldValues(v_FilterActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_ColumnType), nameof(v_FilterAction), parameters, engine, this);

            //var newDT = DataTableControls.CloneDataTableOnlyColumnName(targetDT);
            var newDT = targetDT.CloneDataTableOnlyColumns();

            int cols = targetDT.Columns.Count;

            int rows = targetDT.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                string value = targetDT.Rows[i][colIndex]?.ToString() ?? "";
                if (checkFunc(value, parameters))
                {
                    int r = newDT.Rows.Count;
                    newDT.Rows.Add();
                    for (int j = 0; j < cols; j++)
                    {
                        newDT.Rows[r][j] = targetDT.Rows[i][j];
                    }
                }
            }

            //res.StoreInUserVariable(engine, v_NewDataTable);
            this.StoreDataTableInUserVariable(newDT, engine);
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_ValueType)];
            var FilterActionComboboxHelper = (ComboBox)ControlsList[nameof(v_FilterAction)];
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
        }

        private void cmbFilterAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            var FilterParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_FilterActionParameterTable)];
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_ValueType)];
            var FilterActionComboboxHelper = (ComboBox)ControlsList[nameof(v_FilterAction)];
            ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
        }
        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var FilterParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_FilterActionParameterTable)];
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_ValueType)];
            var FilterActionComboboxHelper = (ComboBox)ControlsList[nameof(v_FilterAction)];
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
            ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_FilterActionParameterTable));
            DataTableControls.BeforeValidate_NoRowAdding(dgv, v_FilterActionParameterTable);
        }
    }
}