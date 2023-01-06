using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Convert Row")]
    [Attributes.ClassAttributes.Description("This command allows you to convert DataTable Row to Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to convert DataTable Row to Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertDataTableRowToDictionaryCommand : ScriptCommand
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
        //[PropertyDescription("Please enter the index of the Row")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a valid DataRow index value")]
        //[SampleUsage("**0** or **1** or **-1** or **{{{vRowIndex}}}**")]
        //[Remarks("**-1** means index of the last row.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyIsOptional(true, "Current Row")]
        //[PropertyDisplayText(true, "Row")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_RowIndex))]
        public string v_DataRowIndex { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify the Variable Name To Assign The Dictionary")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        //[PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_OutputDictionaryName))]
        public string v_OutputVariableName { get; set; }

        public ConvertDataTableRowToDictionaryCommand()
        {
            this.CommandName = "ConvertDataTableRowToDictionaryCommand";
            this.SelectionName = "Convert DataTable Row To Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;         
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var srcDT, var index) = this.GetDataTableVariableAndRowIndex(nameof(v_DataTableName), nameof(v_DataRowIndex), engine);

            Dictionary<string, string> myDic = new Dictionary<string, string>();

            int cols = srcDT.Columns.Count;
            for (int i = 0; i < cols; i++)
            {
                myDic.Add(srcDT.Columns[i].ColumnName, srcDT.Rows[index][i]?.ToString() ?? "");
            }

            myDic.StoreInUserVariable(engine, v_OutputVariableName);
        }
    }
}