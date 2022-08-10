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
    [Attributes.ClassAttributes.Group("Dictionary Commands")]
    [Attributes.ClassAttributes.SubGruop("Dictionary Action")]
    [Attributes.ClassAttributes.Description("This command allows you to filter Dictionary value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to filter Dictionary value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class FilterDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a Dictionary Variable Name to Filter")]
        [InputSpecification("")]
        [SampleUsage("**vDic** or **{{{vDic}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyValidationRule("Dictionary to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_InputDictionary { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select filter target value type")]
        [InputSpecification("")]
        [SampleUsage("**Text** or **Number**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Numeric")]
        [PropertySelectionChangeEvent("cmbTargetType_SelectionChangeCommited")]
        [PropertyValidationRule("Target Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select filter action")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertySelectionChangeEvent("cmbFilterAction_SelectionChangeCommited")]
        [PropertyValidationRule("Filter Action", PropertyValidationRule.ValidationRuleFlags.Empty)]
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
        [PropertyDescription("Please select a Dictionary Variable Name of the Filtered List")]
        [InputSpecification("")]
        [SampleUsage("**vNewDic** or **{{{vNewDic}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Filtered Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_OutputDictionary { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox TargetTypeComboboxHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox FilterActionComboboxHelper;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView FilterParametersGridViewHelper;

        public FilterDictionaryCommand()
        {
            this.CommandName = "FilterDictionaryCommand";
            this.SelectionName = "Filter Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_TargetType = "Text";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetDic = v_InputDictionary.GetDictionaryVariable(engine);

            string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);
            string filterAction = v_FilterAction.GetUISelectionValue("v_FilterAction", this, engine);

            var res = new Dictionary<string, string>();

            foreach(var item in targetDic)
            {
                if (ConditionControls.FilterDeterminStatementTruth(item.Value, targetType, filterAction, v_FilterActionParameterTable, engine))
                {
                    res.Add(item.Key, item.Value);
                }
            }

            res.StoreInUserVariable(engine, v_OutputDictionary);
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

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [ Dictionary: '" + this.v_InputDictionary + "', Type: '" + this.v_OutputDictionary + "', Action: '" + this.v_FilterAction + "', Result: '" + this.v_OutputDictionary + "']";
        }
    }
}