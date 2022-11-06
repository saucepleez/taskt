using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.Description("This command allows you to Filter Rows by reference to Column values.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Filter Rows by reference to Column values.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FilterDataTableRowByColumnValueCommand : ScriptCommand
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

        [XmlIgnore]
        [NonSerialized]
        private ComboBox TargetTypeComboboxHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox FilterActionComboboxHelper;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView FilterParametersGridViewHelper;

        public FilterDataTableRowByColumnValueCommand()
        {
            this.CommandName = "FilterDataTableRowByColumnValueCommand";
            this.SelectionName = "Filter DataTable Row By Column Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_TargetType = "Text";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetDT = v_InputDataTable.GetDataTableVariable(engine);

            string colType = v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine);
            int colIndex = 0;
            switch (colType)
            {
                case "column name":
                    colIndex = DataTableControls.GetColumnIndexFromName(targetDT, v_TargetColumnIndex, engine);
                    break;
                case "index":
                    colIndex = DataTableControls.GetColumnIndex(targetDT, v_TargetColumnIndex, engine);
                    break;
            }

            //string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);
            //string filterAction = v_FilterAction.GetUISelectionValue("v_FilterAction", this, engine);
            var parameters = DataTableControls.GetFieldValues(v_FilterActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_ColumnType), nameof(v_FilterAction), parameters, engine, this);

            var res = DataTableControls.CloneColumnName(targetDT);

            int cols = targetDT.Columns.Count;

            int rows = targetDT.Rows.Count;
            //for (int i = 0; i < rows; i++)
            //{
            //    string value = (targetDT.Rows[i][colIndex] == null) ? "" : targetDT.Rows[i][colIndex].ToString();
            //    if (ConditionControls.FilterDeterminStatementTruth(value, targetType, filterAction, v_FilterActionParameterTable, engine))
            //    {
            //        int r = res.Rows.Count;
            //        res.Rows.Add();
            //        for (int j = 0; j < cols; j++)
            //        {
            //            res.Rows[r][j] = targetDT.Rows[i][j];
            //        }
            //    }
            //}
            for (int i = 0; i < rows; i++)
            {
                //string value = (targetDT.Rows[i][colIndex] == null) ? "" : targetDT.Rows[i][colIndex].ToString();
                string value = targetDT.Rows[i][colIndex]?.ToString() ?? "";
                if (checkFunc(value, parameters))
                {
                    int r = res.Rows.Count;
                    res.Rows.Add();
                    for (int j = 0; j < cols; j++)
                    {
                        res.Rows[r][j] = targetDT.Rows[i][j];
                    }
                }
            }

            res.StoreInUserVariable(engine, v_OutputDataTable);
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
        }

        private void cmbFilterAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            TargetTypeComboboxHelper = (ComboBox)CommandControls.GetControlsByName(ctrls, "v_TargetType", CommandControls.CommandControlType.Body)[0];
            FilterActionComboboxHelper = (ComboBox)CommandControls.GetControlsByName(ctrls, "v_FilterAction", CommandControls.CommandControlType.Body)[0];
            FilterParametersGridViewHelper = (DataGridView)CommandControls.GetControlsByName(ctrls, "v_FilterActionParameterTable", CommandControls.CommandControlType.Body)[0];

            return RenderedControls;
        }

        public override void AfterShown()
        {
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
            ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
        }

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [ DataTable: '" + this.v_InputDataTable + "', Column: '" + this.v_TargetColumnIndex + "', Type: '" + this.v_OutputDataTable + "', Action: '" + this.v_FilterAction + "', Result: '" + this.v_OutputDataTable + "']";
        //}
    }
}