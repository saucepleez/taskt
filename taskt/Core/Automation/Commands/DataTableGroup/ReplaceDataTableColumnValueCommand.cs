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
    [Attributes.ClassAttributes.CommandSettings("Replace DataTable Column Value")]
    [Attributes.ClassAttributes.Description("This command allows you to Replace Column values.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Replace Column values.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ReplaceDataTableColumnValueCommand : ADataTableColumnCommands, IReplaceValueProperties, IHaveDataTableElements
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        //public string v_DataTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        //public string v_ColumnType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        //public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValueType))]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        [PropertyParameterOrder(8000)]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceAction))]
        [PropertyParameterOrder(8001)]
        public string v_ReplaceAction { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ActionParameterTable))]
        [PropertyParameterOrder(8002)]
        public DataTable v_ReplaceActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValue))]
        [PropertyParameterOrder(8003)]
        public string v_NewValue { get; set; }

        public ReplaceDataTableColumnValueCommand()
        {
            //this.CommandName = "ReplaceDataTableColumnValueCommand";
            //this.SelectionName = "Replace DataTable Column Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var targetDT, var colIndex) = this.ExpandUserVariablesAsDataTableAndColumnIndex(nameof(v_DataTable), nameof(v_ColumnType), nameof(v_ColumnIndex), engine);
            (var targetDT, var colIndex, _) = this.ExpandValueOrUserVariableAsDataTableAndColumn(engine);

            var parameters = DataTableControls.GetFieldValues(v_ReplaceActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_ValueType), nameof(v_ReplaceAction), parameters, engine, this);

            string newValue = v_NewValue.ExpandValueOrUserVariable(engine);

            int rows = targetDT.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                string value = targetDT.Rows[i][colIndex]?.ToString() ?? "";
                if (checkFunc(value, parameters))
                {
                    targetDT.Rows[i][colIndex] = newValue;
                }
            }
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_ValueType)];
            var ReplaceActionComboboxHelper = (ComboBox)ControlsList[nameof(v_ReplaceAction)];
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, ReplaceActionComboboxHelper);
        }

        private void cmbReplaceAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            var ReplaceParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_ReplaceActionParameterTable)];
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_ValueType)];
            var ReplaceActionComboboxHelper = (ComboBox)ControlsList[nameof(v_ReplaceAction)];
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var ReplaceParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_ReplaceActionParameterTable)];
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_ValueType)];
            var ReplaceActionComboboxHelper = (ComboBox)ControlsList[nameof(v_ReplaceAction)];
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, ReplaceActionComboboxHelper);
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_ReplaceActionParameterTable));
            DataTableControls.BeforeValidate_NoRowAdding(dgv, v_ReplaceActionParameterTable);
        }
    }
}