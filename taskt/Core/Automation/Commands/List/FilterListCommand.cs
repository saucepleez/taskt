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
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to filter List value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to filter List value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FilterListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name to Filter")]
        [InputSpecification("")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select filter target value type")]
        [InputSpecification("")]
        [SampleUsage("**Text** or **Number**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Numeric")]
        [PropertyControlIntoCommandField("TargetTypeComboboxHelper")]
        [PropertySelectionChangeEvent("cmbTargetType_SelectionChangeCommited")]
        [PropertyValidationRule("Target Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select filter action")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyControlIntoCommandField("FilterActionComboboxHelper")]
        [PropertySelectionChangeEvent("cmbFilterAction_SelectionChangeCommited")]
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
        [PropertyControlIntoCommandField("FilterParametersGridViewHelper")]
        public DataTable v_FilterActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name of the Filtered List")]
        [InputSpecification("")]
        [SampleUsage("**vNewList** or **{{{vNewList}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Filtered List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public string v_OutputList { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox TargetTypeComboboxHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox FilterActionComboboxHelper;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView FilterParametersGridViewHelper;

        public FilterListCommand()
        {
            this.CommandName = "FilterListCommand";
            this.SelectionName = "Filter List";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_TargetType = "Text";
            //this.v_FilterAction = "Contains";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> targetList = v_InputList.GetListVariable(engine);

            string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);
            string filterAction = v_FilterAction.GetUISelectionValue("v_FilterAction", this, engine);

            List<string> res = new List<string>();

            foreach(string item in targetList)
            {
                if (ConditionControls.FilterDeterminStatementTruth(item, targetType, filterAction, v_FilterActionParameterTable, engine))
                {
                    res.Add(item);
                }
            }

            res.StoreInUserVariable(engine, v_OutputList);
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
        }

        private void cmbFilterAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
        }

        public override void AfterShown()
        {
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

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [ List: '" + this.v_InputList + "', Type: '" + this.v_OutputList + "', Action: '" + this.v_FilterAction + "', Result: '" + this.v_OutputList + "']";
        //}
    }
}