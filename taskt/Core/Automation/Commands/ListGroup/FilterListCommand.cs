using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Filter List")]
    [Attributes.ClassAttributes.Description("This command allows you to filter List value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to filter List value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class FilterListCommand : AListCreateFromListCommands, IFilterValueProperties, IHaveDataTableElements
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List Variable Name to Filter")]
        [PropertyValidationRule("List to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public override string v_TargetList { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterValueType))]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        [PropertyParameterOrder(6000)]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_FilterAction))]
        [PropertySelectionChangeEvent(nameof(cmbFilterAction_SelectionChangeCommited))]
        [PropertyParameterOrder(7000)]
        public string v_FilterAction { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ActionParameterTable))]
        [PropertyParameterOrder(8000)]
        public DataTable v_FilterActionParameterTable { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        //public string v_NewList { get; set; }

        public FilterListCommand()
        {
            //this.CommandName = "FilterListCommand";
            //this.SelectionName = "Filter List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //List<string> targetList = v_TargetList.ExpandUserVariableAsList(engine);
            var targetList = this.ExpandUserVariableAsList(engine);

            var parameters = DataTableControls.GetFieldValues(v_FilterActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_ValueType), nameof(v_FilterAction), parameters, engine, this);

            var newList = new List<string>();

            foreach(string item in targetList)
            {
                if (checkFunc(item, parameters))
                {
                    newList.Add(item);
                }
            }

            //res.StoreInUserVariable(engine, v_NewList);
            this.StoreListInUserVariable(newList, engine);
        }

        private void cmbTargetType_SelectionChangeCommited(object sender, EventArgs e)
        {
            //ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
            ConditionControls.AddFilterActionItems((ComboBox)ControlsList[nameof(v_ValueType)], (ComboBox)ControlsList[nameof(v_FilterAction)]);
        }

        private void cmbFilterAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            //ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
            ConditionControls.RenderFilter(v_FilterActionParameterTable, (DataGridView)ControlsList[nameof(v_FilterActionParameterTable)], (ComboBox)ControlsList[nameof(v_FilterAction)], (ComboBox)ControlsList[nameof(v_ValueType)]);
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            //ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, FilterActionComboboxHelper);
            ConditionControls.AddFilterActionItems((ComboBox)ControlsList[nameof(v_ValueType)], (ComboBox)ControlsList[nameof(v_FilterAction)]);
            //ConditionControls.RenderFilter(v_FilterActionParameterTable, FilterParametersGridViewHelper, FilterActionComboboxHelper, TargetTypeComboboxHelper);
            ConditionControls.RenderFilter(v_FilterActionParameterTable, (DataGridView)ControlsList[nameof(v_FilterActionParameterTable)], (ComboBox)ControlsList[nameof(v_FilterAction)], (ComboBox)ControlsList[nameof(v_ValueType)]);
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_FilterActionParameterTable));
            DataTableControls.BeforeValidate_NoRowAdding(dgv, v_FilterActionParameterTable);
        }
    }
}