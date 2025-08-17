using System;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Replace List")]
    [Attributes.ClassAttributes.Description("This command allows you to relace List value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to relpace List value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ReplaceListCommand : AListBothListCommands, IReplaceValueProperties, IHaveDataTableElements
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_BothListName))]
        [PropertyDescription("List Variable Name to Replace")]
        [PropertyValidationRule("List to Replace", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public override string v_List { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValueType))]
        [PropertySelectionChangeEvent(nameof(cmbTargetType_SelectionChangeCommited))]
        [PropertyParameterOrder(6000)]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValueType))]
        [PropertySelectionChangeEvent(nameof(cmbReplaceAction_SelectionChangeCommited))]
        [PropertyParameterOrder(6001)]
        public string v_ReplaceAction { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ActionParameterTable))]
        [PropertyParameterOrder(6002)]
        public DataTable v_ReplaceActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ConditionControls), nameof(ConditionControls.v_ReplaceValue))]
        [PropertyParameterOrder(6003)]
        public string v_NewValue { get; set; }

        public ReplaceListCommand()
        {
            //this.CommandName = "ReplaceListCommand";
            //this.SelectionName = "Replace List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetList = v_List.ExpandUserVariableAsList(engine);
            var targetList = this.ExpandUserVariableAsList(engine);

            var parameters = DataTableControls.GetFieldValues(v_ReplaceActionParameterTable, "ParameterName", "ParameterValue", engine);
            var checkFunc = ConditionControls.GetFilterDeterminStatementTruthFunc(nameof(v_ValueType), nameof(v_ReplaceAction), parameters, engine, this);

            string newValue = v_NewValue.ExpandValueOrUserVariable(engine);

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
            ConditionControls.AddFilterActionItems((ComboBox)ControlsList[nameof(v_ValueType)], (ComboBox)ControlsList[nameof(v_ReplaceAction)]);
        }

        private void cmbReplaceAction_SelectionChangeCommited(object sender, EventArgs e)
        {
            //ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, (DataGridView)ControlsList[nameof(v_ReplaceActionParameterTable)], (ComboBox)ControlsList[nameof(v_ReplaceAction)], (ComboBox)ControlsList[nameof(v_ValueType)]);
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            //ConditionControls.AddFilterActionItems(TargetTypeComboboxHelper, ReplaceActionComboboxHelper);
            ConditionControls.AddFilterActionItems((ComboBox)ControlsList[nameof(v_ValueType)], (ComboBox)ControlsList[nameof(v_ReplaceAction)]);
            //ConditionControls.RenderFilter(v_ReplaceActionParameterTable, ReplaceParametersGridViewHelper, ReplaceActionComboboxHelper, TargetTypeComboboxHelper);
            ConditionControls.RenderFilter(v_ReplaceActionParameterTable, (DataGridView)ControlsList[nameof(v_ReplaceActionParameterTable)], (ComboBox)ControlsList[nameof(v_ReplaceAction)], (ComboBox)ControlsList[nameof(v_ValueType)]);
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_ReplaceActionParameterTable));
            DataTableControls.BeforeValidate_NoRowAdding(dgv, v_ReplaceActionParameterTable);
        }
    }
}