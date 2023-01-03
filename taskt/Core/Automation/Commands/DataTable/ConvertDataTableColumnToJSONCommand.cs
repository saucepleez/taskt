using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Column")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Column to JSON")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Column to JSON.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableColumnToJSONCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate the DataTable Variable Name")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a existing DataTable to fet rows from.")]
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
        //[PropertyDescription("Please specify Column type")]
        //[InputSpecification("")]
        //[SampleUsage("**Column Name** or **Index**")]
        //[Remarks("")]
        //[PropertyUISelectionOption("Column Name")]
        //[PropertyUISelectionOption("Index")]
        //[PropertyIsOptional(true, "Column Name")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyDisplayText(true, "Column Type")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please enter the Name or Index of the Column")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a valid Column index value")]
        //[SampleUsage("**id** or **0** or **{{{vColumn}}}** or **-1**")]
        //[Remarks("If **-1** is specified for Column Index, it means the last column.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Column")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_ColumnNameIndex))]
        public string v_DataColumnIndex { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify the Variable Name To Assign The JSON")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.JSON, true)]
        //[PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(JSONControls), nameof(JSONControls.v_OutputJSONName))]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableColumnToJSONCommand()
        {
            this.CommandName = "ConvertDataTableColumnToJSONCommand";
            this.SelectionName = "Convert DataTable Column To JSON";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var listCommand = new ConvertDataTableColumnToListCommand
            {
                v_DataTableName = this.v_DataTableName,
                v_ColumnType = this.v_ColumnType,
                v_DataColumnIndex = this.v_DataColumnIndex,
                v_OutputVariableName = ExtensionMethods.GetInnerVariableName(0, engine)
            };
            listCommand.RunCommand(engine);

            List<string> myList = (List<string>)ExtensionMethods.GetInnerVariable(0, engine).VariableValue;

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(myList);
            json.StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}