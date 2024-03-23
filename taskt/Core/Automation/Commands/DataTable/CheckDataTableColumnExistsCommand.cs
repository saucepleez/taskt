using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Column Action")]
    [Attributes.ClassAttributes.CommandSettings("Check DataTable Column Exists")]
    [Attributes.ClassAttributes.Description("This command allows you to check the column name existance")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check the column name existance.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckDataTableColumnExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Name of Column")]
        [InputSpecification("")]
        //[SampleUsage("**colName** or **{{{vColName}}}**")]
        [PropertyDetailSampleUsage("**name**", PropertyDetailSampleUsage.ValueType.Value, "Column Name to be Checked")]
        [PropertyDetailSampleUsage("**{{{vColName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Column Name to be Checked")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        public string v_ColumnName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        public string v_OutputVariableName { get; set; }

        public CheckDataTableColumnExistsCommand()
        {
            //this.CommandName = "CheckDataTableColumnExistsCommand";
            //this.SelectionName = "Check DataTable Column Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            DataTable myDT = v_DataTable.ExpandUserVariableAsDataTable(engine);

            string targetColumnName = v_ColumnName.ExpandValueOrUserVariable(engine);

            myDT.Columns.Contains(targetColumnName).StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}