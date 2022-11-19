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
    [Attributes.ClassAttributes.Description("This command allows you to Replace values.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Replace values.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReplaceDataTableValueCommand : ScriptCommand
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

        public ReplaceDataTableValueCommand()
        {
            this.CommandName = "ReplaceDataTableValueCommand";
            this.SelectionName = "Replace DataTable Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_TargetType = "Text";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetDT = v_InputDataTable.GetDataTableVariable(engine);

            //string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);
            //string filterAction = v_ReplaceAction.GetUISelectionValue("v_ReplaceAction", this, engine);
            var parameters = DataTableControls.GetFieldValues(v_ReplaceActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_TargetType), nameof(v_ReplaceAction), parameters, engine, this);

            string newValue = v_NewValue.ConvertToUserVariable(engine);

            int cols = targetDT.Columns.Count;
            int rows = targetDT.Rows.Count;

            //for (int i = 0; i < rows; i++)
            //{
            //    for (int j = 0; j < cols; j++)
            //    {
            //        string value = (targetDT.Rows[i][j] == null) ? "" : targetDT.Rows[i][j].ToString();
            //        if (ConditionControls.FilterDeterminStatementTruth(value, targetType, filterAction, v_ReplaceActionParameterTable, engine))
            //        {
            //            targetDT.Rows[i][j] = newValue;
            //        }
            //    }
            //}
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    string value = targetDT.Rows[i][j]?.ToString() ?? "";
                    if (checkFunc(value, parameters))
                    {
                        targetDT.Rows[i][j] = newValue;
                    }
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    TargetTypeComboboxHelper = (ComboBox)CommandControls.GetControlsByName(ctrls, "v_TargetType", CommandControls.CommandControlType.Body)[0];
        //    ReplaceActionComboboxHelper = (ComboBox)CommandControls.GetControlsByName(ctrls, "v_ReplaceAction", CommandControls.CommandControlType.Body)[0];
        //    ReplaceParametersGridViewHelper = (DataGridView)CommandControls.GetControlsByName(ctrls, "v_ReplaceActionParameterTable", CommandControls.CommandControlType.Body)[0];

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [ DataTable: '" + this.v_InputDataTable + "', Type: '" + this.v_NewValue + "', Action: '" + this.v_ReplaceAction + "', Replace: '" + this.v_NewValue + "']";
        //}
    }
}