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
    [Attributes.ClassAttributes.CommandSettings("Replace List")]
    [Attributes.ClassAttributes.Description("This command allows you to relace List value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to relpace List value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReplaceListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_BothListName))]
        [PropertyDescription("List Variable Name to Replace")]
        [PropertyValidationRule("List to Replace", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetList { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValueType))]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValueType))]
        [PropertySelectionChangeEvent(nameof(cmbReplaceAction_SelectionChangeCommited))]
        public string v_ReplaceAction { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ActionParameterTable))]
        public DataTable v_ReplaceActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValue))]
        public string v_ReplaceValue { get; set; }

        public ReplaceListCommand()
        {
            //this.CommandName = "ReplaceListCommand";
            //this.SelectionName = "Replace List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> targetList = v_TargetList.GetListVariable(engine);

            var parameters = DataTableControls.GetFieldValues(v_ReplaceActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_TargetType), nameof(v_ReplaceAction), parameters, engine, this);

            string newValue = v_ReplaceValue.ConvertToUserVariable(engine);

            for (int i = targetList.Count - 1; i >= 0; i--)
            {
                if (checkFunc(targetList[i], parameters))
                {
                    targetList[i] = newValue;
                }
            }
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            //ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, ReplaceActionComboboxHelper);
            ConditionControls.AddFilterActionItems((ComboBox)ControlsList[nameof(v_TargetType)], (ComboBox)ControlsList[nameof(v_ReplaceAction)]);
        }

        private void cmbReplaceAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            //ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, (DataGridView)ControlsList[nameof(v_ReplaceActionParameterTable)], (ComboBox)ControlsList[nameof(v_ReplaceAction)], (ComboBox)ControlsList[nameof(v_TargetType)]);
        }

        public override void AfterShown()
        {
            //ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, ReplaceActionComboboxHelper);
            ConditionControls.AddFilterActionItems((ComboBox)ControlsList[nameof(v_TargetType)], (ComboBox)ControlsList[nameof(v_ReplaceAction)]);
            //ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, (DataGridView)ControlsList[nameof(v_ReplaceActionParameterTable)], (ComboBox)ControlsList[nameof(v_ReplaceAction)], (ComboBox)ControlsList[nameof(v_TargetType)]);
        }
    }
}