using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("DataTable Action")]
    [Attributes.ClassAttributes.Description("This command allows you to set the DataTable value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set the DataTable value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDataTableValueCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate the DataTable Variable Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a existing DataTable.")]
        //[SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "DataTable")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_InputDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Column value type")]
        //[InputSpecification("")]
        //[SampleUsage("**Index** or **Column Name**")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Index")]
        //[PropertyUISelectionOption("Column Name")]
        //[PropertyIsOptional(true, "Column Name")]
        //[PropertyDisplayText(true, "Column Type")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Column Name or Index")]
        //[InputSpecification("")]
        //[SampleUsage("**0** or **id** or **{{{vColumn}}}** or **-1**")]
        //[Remarks("If **-1** is specified for Column Index, it means the last column.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Column")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Row Index")]
        //[InputSpecification("")]
        //[SampleUsage("**0** or **1** or **-1** or **{{{vIndex}}}**")]
        //[Remarks("**-1** means index of the last row.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyIsOptional(true, "Current Row")]
        //[PropertyDisplayText(true, "Row")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value to Set DataTable")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**123**", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDisplayText(true, "Value to Set")]
        public string v_NewValue { get; set; }

        public SetDataTableValueCommand()
        {
            this.CommandName = "SetDataTableValueCommand";
            this.SelectionName = "Set DataTable Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var myDT, var rowIndex, var columnIndex) = this.GetDataTableVariableAndRowColumnIndeies(nameof(v_DataTableName), nameof(v_RowIndex), nameof(v_ColumnType), nameof(v_ColumnIndex), engine);

            string newValue = v_NewValue.ConvertToUserVariable(engine);

            myDT.Rows[rowIndex][columnIndex] = newValue;
        }
    }
}