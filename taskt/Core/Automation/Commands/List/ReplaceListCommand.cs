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
    [Attributes.ClassAttributes.Description("This command allows you to relace List value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to relpace List value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ReplaceListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name to Replace")]
        [InputSpecification("")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetList { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select replace target value type")]
        [InputSpecification("")]
        [SampleUsage("**Text** or **Number**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Numeric")]
        [PropertySelectionChangeEvent("cmbTargetType_SelectionChangeCommited")]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select replace action")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertySelectionChangeEvent("cmbReplaceAction_SelectionChangeCommited")]
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
        [PropertyDescription("Please specify Replace value")]
        [InputSpecification("")]
        [SampleUsage("**vNewList** or **{{{vNewList}}}**")]
        [Remarks("")]
        public string v_ReplaceValue { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox TargetTypeComboboxHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox ReplaceActionComboboxHelper;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView ReplaceParametersGridViewHelper;

        public ReplaceListCommand()
        {
            this.CommandName = "ReplaceListCommand";
            this.SelectionName = "Replace List";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_TargetType = "Text";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> targetList = v_TargetList.GetListVariable(engine);

            string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);
            string filterAction = v_ReplaceAction.GetUISelectionValue("v_ReplaceAction", this, engine);

            string newValue = v_ReplaceValue.ConvertToUserVariable(engine);

            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                if (ConditionControls.FilterDeterminStatementTruth(targetList[i], targetType, filterAction, v_ReplaceActionParameterTable, engine))
                {
                    targetList[i] = newValue;
                }
            }
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, ReplaceActionComboboxHelper);
        }

        private void cmbReplaceAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            TargetTypeComboboxHelper = (ComboBox)CommandControls.GetControlsByName(ctrls, "v_TargetType", CommandControls.CommandControlType.Body)[0];
            ReplaceActionComboboxHelper = (ComboBox)CommandControls.GetControlsByName(ctrls, "v_ReplaceAction", CommandControls.CommandControlType.Body)[0];
            ReplaceParametersGridViewHelper = (DataGridView)CommandControls.GetControlsByName(ctrls, "v_ReplaceActionParameterTable", CommandControls.CommandControlType.Body)[0];

            return RenderedControls;
        }

        public override void AfterShown()
        {
            ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, ReplaceActionComboboxHelper);
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [ List: '" + this.v_TargetList + "', Type: '" + this.v_ReplaceValue + "', Action: '" + this.v_ReplaceAction + "', Replace: '" + this.v_ReplaceValue + "']";
        }
    }
}