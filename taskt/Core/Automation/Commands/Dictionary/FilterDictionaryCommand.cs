using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Action")]
    [Attributes.ClassAttributes.Description("This command allows you to filter Dictionary value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to filter Dictionary value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FilterDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please select a Dictionary Variable Name to Filter")]
        //[InputSpecification("")]
        //[SampleUsage("**vDic** or **{{{vDic}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        //[PropertyValidationRule("Dictionary to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Dictionary to Filter")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        [PropertyDescription("Dictionary Variable Name to Filter")]
        [PropertyValidationRule("Dictionary to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Dictionary to Filter")]
        public string v_InputDictionary { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select filter target value type")]
        //[InputSpecification("")]
        //[SampleUsage("**Text** or **Number**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Text")]
        //[PropertyUISelectionOption("Numeric")]
        //[PropertyValidationRule("Target Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Type")]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterValueType))]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select filter action")]
        //[InputSpecification("")]
        //[SampleUsage("")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("Filter Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Action")]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterAction))]
        [PropertySelectionChangeEvent(nameof(cmbFilterAction_SelectionChangeCommited))]
        public string v_FilterAction { get; set; }

        [XmlElement]
        //[PropertyDescription("Additional Parameters")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        //[PropertyDataGridViewSetting(false, false, true, 400, 120)]
        //[PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true)]
        //[PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false)]
        //[PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        //[PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ActionParameterTable))]
        public DataTable v_FilterActionParameterTable { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select a Dictionary Variable Name of the Filtered List")]
        //[InputSpecification("")]
        //[SampleUsage("**vNewDic** or **{{{vNewDic}}}**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyValidationRule("Filtered Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Result")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_NewOutputDictionaryName))]
        //[PropertyDescription("New Dictionary Variable Name")]
        //[PropertyValidationRule("New Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "New Dictionary")]
        public string v_OutputDictionary { get; set; }

        public FilterDictionaryCommand()
        {
            this.CommandName = "FilterDictionaryCommand";
            this.SelectionName = "Filter Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //this.v_TargetType = "Text";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetDic = v_InputDictionary.GetDictionaryVariable(engine);

            var parameters = DataTableControls.GetFieldValues(v_FilterActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_TargetType), nameof(v_FilterAction), parameters, engine, this);

            var res = new Dictionary<string, string>();

            foreach(var item in targetDic)
            {
                if (checkFunc(item.Value, parameters))
                {
                    res.Add(item.Key, item.Value);
                }
            }

            res.StoreInUserVariable(engine, v_OutputDictionary);
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
    }
}