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
    [Attributes.ClassAttributes.CommandSettings("Filter List")]
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
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterValueType))]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterAction))]
        [PropertySelectionChangeEvent(nameof(cmbFilterAction_SelectionChangeCommited))]
        public string v_FilterAction { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ActionParameterTable))]
        public DataTable v_FilterActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        public string v_OutputList { get; set; }

        public FilterListCommand()
        {
            //this.CommandName = "FilterListCommand";
            //this.SelectionName = "Filter List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
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