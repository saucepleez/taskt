using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
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
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List Variable Name to Filter")]
        [PropertyValidationRule("List to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyDescription("Filter Target Value Type")]
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
        [PropertyDescription("Filter Action")]
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
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.FirstColumnReadonlySubsequentEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_FilterActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("New List Variable Name")]
        [PropertyValidationRule("New List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New List")]
        public string v_OutputList { get; set; }

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

            var parameters = DataTableControls.GetFieldValues(v_FilterActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_TargetType), nameof(v_FilterAction), parameters, engine, this);

            List<string> res = new List<string>();

            foreach(string item in targetList)
            {
                if (checkFunc(item, parameters))
                {
                    res.Add(item);
                }
            }

            res.StoreInUserVariable(engine, v_OutputList);
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            //ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
            ConditionControls.AddFilterActionItems((ComboBox)ControlsList[nameof(v_TargetType)], (ComboBox)ControlsList[nameof(v_FilterAction)]);
        }

        private void cmbFilterAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            //ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
            ConditionControls.RenderFilter(v_FilterActionParameterTable, (DataGridView)ControlsList[nameof(v_FilterActionParameterTable)], (ComboBox)ControlsList[nameof(v_FilterAction)], (ComboBox)ControlsList[nameof(v_TargetType)]);
        }

        public override void AfterShown()
        {
            //ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
            ConditionControls.AddFilterActionItems((ComboBox)ControlsList[nameof(v_TargetType)], (ComboBox)ControlsList[nameof(v_FilterAction)]);
            //ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
            ConditionControls.RenderFilter(v_FilterActionParameterTable, (DataGridView)ControlsList[nameof(v_FilterActionParameterTable)], (ComboBox)ControlsList[nameof(v_FilterAction)], (ComboBox)ControlsList[nameof(v_TargetType)]);
        }
    }
}