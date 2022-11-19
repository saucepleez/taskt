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
    [Attributes.ClassAttributes.Description("This command allows you to Replace Column values.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Replace Column values.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReplaceDataTableColumnValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a DataTable Variable Name to Replace")]
        [InputSpecification("")]
        [SampleUsage("**vTable** or **{{{vTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable to Replace", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable")]
        public string v_InputDataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Column type")]
        [InputSpecification("")]
        [SampleUsage("**Column Name** or **Index**")]
        [Remarks("")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Index")]
        [PropertyIsOptional(true, "Column Name")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyDisplayText(true, "Column Type")]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the Name or Index of the Column")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid Column index value")]
        [SampleUsage("**id** or **0** or **{{{vColumn}}}** or **-1**")]
        [Remarks("If **-1** is specified for Column Index, it means the last column.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        public string v_TargetColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select replace target value type")]
        [InputSpecification("")]
        [SampleUsage("**Text** or **Number**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Numeric")]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        [PropertyValidationRule("Target Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select replace action")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertySelectionChangeEvent(nameof(cmbReplaceAction_SelectionChangeCommited))]
        [PropertyValidationRule("Replace Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Action")]
        public string v_ReplaceAction { get; set; }

        [XmlElement]
        [PropertyDescription("Additional Parameters")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true, 400, 120)]
        [PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true)]
        [PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_ReplaceActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify replace value")]
        [InputSpecification("")]
        [SampleUsage("**newValue** or **{{{vNewValue}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDisplayText(true, "Replace Value")]
        public string v_NewValue { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private ComboBox TargetTypeComboboxHelper;

        //[XmlIgnore]
        //[NonSerialized]
        //private ComboBox ReplaceActionComboboxHelper;

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView ReplaceParametersGridViewHelper;

        public ReplaceDataTableColumnValueCommand()
        {
            this.CommandName = "ReplaceDataTableColumnValueCommand";
            this.SelectionName = "Replace DataTable Column Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_TargetType = "Text";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var targetDT = v_InputDataTable.GetDataTableVariable(engine);
            //string colType = v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine);
            //int colIndex = 0;
            //switch (colType)
            //{
            //    case "column name":
            //        colIndex = DataTableControls.GetColumnIndexFromName(targetDT, v_TargetColumnIndex, engine);
            //        break;
            //    case "index":
            //        colIndex = DataTableControls.GetColumnIndex(targetDT, v_TargetColumnIndex, engine);
            //        break;
            //}
            (var targetDT, var colIndex) = this.GetDataTableVariableAndColumnIndex(nameof(v_InputDataTable), nameof(v_ColumnType), nameof(v_TargetColumnIndex), engine);

            //string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);
            //string replaceAction = v_ReplaceAction.GetUISelectionValue("v_ReplaceAction", this, engine);
            var parameters = DataTableControls.GetFieldValues(v_ReplaceActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_TargetType), nameof(v_ReplaceAction), parameters, engine, this);

            string newValue = v_NewValue.ConvertToUserVariable(engine);

            //int rows = targetDT.Rows.Count;
            //for (int i = 0; i < rows; i++)
            //{
            //    string value = (targetDT.Rows[i][colIndex] == null) ? "" : targetDT.Rows[i][colIndex].ToString();
            //    if (ConditionControls.FilterDeterminStatementTruth(value, targetType, replaceAction, v_ReplaceActionParameterTable, engine))
            //    {
            //        targetDT.Rows[i][colIndex] = newValue;
            //    }
            //}
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

        public override void AfterShown()
        {
            var ReplaceParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_ReplaceActionParameterTable)];
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_TargetType)];
            var ReplaceActionComboboxHelper = (ComboBox)ControlsList[nameof(v_ReplaceAction)];
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, ReplaceActionComboboxHelper);
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
        }
    }
}