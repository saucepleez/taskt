﻿using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.CommandSettings("Replace DataTable Row Value")]
    [Attributes.ClassAttributes.Description("This command allows you to Replace Row values.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Replace Row values.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReplaceDataTableRowValueCommand : ScriptCommand, IHaveDataTableElements
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        public string v_TargetRowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValueType))]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceAction))]
        [PropertySelectionChangeEvent(nameof(cmbReplaceAction_SelectionChangeCommited))]
        public string v_ReplaceAction { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ActionParameterTable))]
        public DataTable v_ReplaceActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValue))]
        public string v_NewValue { get; set; }

        public ReplaceDataTableRowValueCommand()
        {
            //this.CommandName = "ReplaceDataTableRowValueCommand";
            //this.SelectionName = "Replace DataTable Row Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var targetDT, var rowIndex) = this.ExpandUserVariablesAsDataTableAndRowIndex(nameof(v_DataTable), nameof(v_TargetRowIndex), engine);

            var parameters = DataTableControls.GetFieldValues(v_ReplaceActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_TargetType), nameof(v_ReplaceAction), parameters, engine, this);

            string newValue = v_NewValue.ExpandValueOrUserVariable(engine);

            int cols = targetDT.Columns.Count;

            for (int i = 0; i < cols; i++)
            {
                string value = targetDT.Rows[rowIndex][i]?.ToString() ?? "";
                if (checkFunc(value, parameters))
                {
                    targetDT.Rows[rowIndex][i] = newValue;
                }
            }
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_TargetType)];
            var ReplaceActionComboboxHelper = (ComboBox)ControlsList[nameof(v_ReplaceAction)];
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, ReplaceActionComboboxHelper);
        }

        private void cmbReplaceAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            var ReplaceParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_ReplaceActionParameterTable)];
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_TargetType)];
            var ReplaceActionComboboxHelper = (ComboBox)ControlsList[nameof(v_ReplaceAction)];
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            var ReplaceParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_ReplaceActionParameterTable)];
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_TargetType)];
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