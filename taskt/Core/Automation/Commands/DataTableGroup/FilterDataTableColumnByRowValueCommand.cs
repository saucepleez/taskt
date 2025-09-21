using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.CommandSettings("Filter DataTable Column By Row Value")]
    [Attributes.ClassAttributes.Description("This command allows you to Filter Columns by reference to Row values.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Filter Columns by reference to Row values.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class FilterDataTableColumnByRowValueCommand : ADataTableCreateFromDataTableCommands, IDataTableRowPositionProperties, IFilterValueProperties, IHaveDataTableElements
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        [PropertyDescription("DataTable Variable Name to Filter")]
        [PropertyValidationRule("DataTable to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to Filter")]
        public override string v_TargetDataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        [PropertyParameterOrder(6000)]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterValueType))]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        [PropertyParameterOrder(7000)]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterAction))]
        [PropertySelectionChangeEvent(nameof(cmbFilterAction_SelectionChangeCommited))]
        [PropertyParameterOrder(8000)]
        public string v_FilterAction { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ActionParameterTable))]
        [PropertyParameterOrder(9000)]
        public DataTable v_FilterActionParameterTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_NewOutputDataTableName))]
        //public string v_NewDataTable { get; set; }

        public FilterDataTableColumnByRowValueCommand()
        {
            //this.CommandName = "FilterDataTableColumnByRowValueCommand";
            //this.SelectionName = "Filter DataTable Column By Row Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //this.v_TargetType = "Text";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var targetDT, var rowIndex) = this.ExpandUserVariablesAsDataTableAndRowIndex(nameof(v_TargetDataTable), nameof(v_RowIndex), engine);
            var targetDT = this.ExpandUserVariableAsDataTable(engine);
            var rowIndex = this.ExpandValueOrUserVariableAsDataTableRow(targetDT, engine);

            var parameters = DataTableControls.GetFieldValues(v_FilterActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_ValueType), nameof(v_FilterAction), parameters, engine, this);

            var newDT = new DataTable();

            int cols = targetDT.Columns.Count;
            int rows = targetDT.Rows.Count;

            for (int i = 0; i < cols; i++)
            {
                string value = targetDT.Rows[rowIndex][i]?.ToString() ?? "";
                if (checkFunc(value, parameters))
                {
                    if (newDT.Rows.Count == 0)
                    {
                        // first add column
                        newDT.Columns.Add(targetDT.Columns[i].ColumnName);
                        for (int j = 0; j < rows; j++)
                        {
                            newDT.Rows.Add(targetDT.Rows[j][i]);
                        }
                    }
                    else
                    {
                        int c = newDT.Columns.Count;
                        newDT.Columns.Add(targetDT.Columns[i].ColumnName);
                        for (int j = 0; j < rows; j++)
                        {
                            newDT.Rows[j][c] = targetDT.Rows[j][i];
                        }
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