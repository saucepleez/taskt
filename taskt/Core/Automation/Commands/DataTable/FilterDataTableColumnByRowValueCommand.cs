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
    [Attributes.ClassAttributes.Description("This command allows you to Filter Columns by reference to Row values.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Filter Columns by reference to Row values.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FilterDataTableColumnByRowValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a DataTable Variable Name to Filter")]
        [InputSpecification("")]
        [SampleUsage("**vTable** or **{{{vTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyValidationRule("DataTable to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "DataTable to Filter")]
        public string v_InputDataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the Index of the Row")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid Column index value")]
        [SampleUsage("**id** or **0** or **{{{vRow}}}** or **-1**")]
        [Remarks("If **-1** is specified for Row Index, it means the last row.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Row", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Row")]
        public string v_TargetRowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select filter target value type")]
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
        [PropertyDescription("Please select filter action")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertySelectionChangeEvent(nameof(cmbFilterAction_SelectionChangeCommited))]
        [PropertyValidationRule("Filter Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Action")]
        public string v_FilterAction { get; set; }

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
        public DataTable v_FilterActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a DataTable Variable Name of the Filtered DataTable")]
        [InputSpecification("")]
        [SampleUsage("**vNewTable** or **{{{vNewTable}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Filtered DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New DataTable")]
        public string v_OutputDataTable { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private ComboBox TargetTypeComboboxHelper;

        //[XmlIgnore]
        //[NonSerialized]
        //private ComboBox FilterActionComboboxHelper;

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView FilterParametersGridViewHelper;

        public FilterDataTableColumnByRowValueCommand()
        {
            this.CommandName = "FilterDataTableColumnByRowValueCommand";
            this.SelectionName = "Filter DataTable Column By Row Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_TargetType = "Text";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var targetDT = v_InputDataTable.GetDataTableVariable(engine);
            //int rowIndex = DataTableControls.GetRowIndex(v_InputDataTable, v_TargetRowIndex, engine);
            (var targetDT, var rowIndex) = this.GetDataTableVariableAndRowIndex(nameof(v_InputDataTable), nameof(v_TargetRowIndex), engine);

            //string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);
            //string filterAction = v_FilterAction.GetUISelectionValue("v_FilterAction", this, engine);

            var parameters = DataTableControls.GetFieldValues(v_FilterActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_TargetType), nameof(v_FilterAction), parameters, engine, this);

            var res = new DataTable();

            int cols = targetDT.Columns.Count;
            int rows = targetDT.Rows.Count;

            //for (int i = 0; i < cols; i++)
            //{
            //    string value = (targetDT.Rows[rowIndex][i] == null) ? "" : targetDT.Rows[rowIndex][i].ToString();
            //    if (ConditionControls.FilterDeterminStatementTruth(value, targetType, filterAction, v_FilterActionParameterTable, engine))
            //    {
            //        if (res.Rows.Count == 0)
            //        {
            //            // first add column
            //            res.Columns.Add(targetDT.Columns[i].ColumnName);
            //            for (int j = 0; j < rows; j++)
            //            {
            //                res.Rows.Add(targetDT.Rows[j][i]);
            //            }
            //        }
            //        else
            //        {
            //            int c = res.Columns.Count;
            //            res.Columns.Add(targetDT.Columns[i].ColumnName);
            //            for (int j = 0; j < rows; j++)
            //            {
            //                res.Rows[j][c] = targetDT.Rows[j][i];
            //            }
            //        }
            //    }
            //}
            for (int i = 0; i < cols; i++)
            {
                string value = targetDT.Rows[rowIndex][i]?.ToString() ?? "";
                if (checkFunc(value, parameters))
                {
                    if (res.Rows.Count == 0)
                    {
                        // first add column
                        res.Columns.Add(targetDT.Columns[i].ColumnName);
                        for (int j = 0; j < rows; j++)
                        {
                            res.Rows.Add(targetDT.Rows[j][i]);
                        }
                    }
                    else
                    {
                        int c = res.Columns.Count;
                        res.Columns.Add(targetDT.Columns[i].ColumnName);
                        for (int j = 0; j < rows; j++)
                        {
                            res.Rows[j][c] = targetDT.Rows[j][i];
                        }
                    }
                }
            }

            res.StoreInUserVariable(engine, v_OutputDataTable);
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_TargetType)];
            var FilterActionComboboxHelper = (ComboBox)ControlsList[nameof(v_FilterAction)];
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
        }

        private void cmbFilterAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            var FilterParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_FilterActionParameterTable)];
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_TargetType)];
            var FilterActionComboboxHelper = (ComboBox)ControlsList[nameof(v_FilterAction)];
            ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
        }
        public override void AfterShown()
        {
            var FilterParametersGridViewHelper = (DataGridView)ControlsList[nameof(v_FilterActionParameterTable)];
            var TargetTypeComboboxHelper = (ComboBox)ControlsList[nameof(v_TargetType)];
            var FilterActionComboboxHelper = (ComboBox)ControlsList[nameof(v_FilterAction)];
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
            ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    TargetTypeComboboxHelper = (ComboBox)CommandControls.GetControlsByName(ctrls, "v_TargetType", CommandControls.CommandControlType.Body)[0];
        //    FilterActionComboboxHelper = (ComboBox)CommandControls.GetControlsByName(ctrls, "v_FilterAction", CommandControls.CommandControlType.Body)[0];
        //    FilterParametersGridViewHelper = (DataGridView)CommandControls.GetControlsByName(ctrls, "v_FilterActionParameterTable", CommandControls.CommandControlType.Body)[0];

        //    return RenderedControls;
        //}
    }
}