using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.Description("This command allows you to set a DataTable Row values to a DataTable by a Dictionary")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a DataTable Row values to a DataTable by a Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetDataTableRowValuesByDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please indicate the DataTable Variable Name to be setted a row")]
        //[InputSpecification("Enter a existing DataTable Variable Name")]
        //[SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("DataTable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "DataTable")]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_BothDataTableName))]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Row index to set values")]
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
        //[PropertyDescription("Please specify the Dictionary Variable Name to set to the DataTable")]
        //[InputSpecification("")]
        //[SampleUsage("")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        //[PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Dictionary")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_RowValues { get; set; }

        [XmlAttribute]
        [PropertyDescription("When Dictionary Key does not Exists")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**Ignore**", "Do not Set a Value")]
        [PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyIsOptional(true, "Ignore")]
        public string v_NotExistsKey { get; set; }

        public SetDataTableRowValuesByDictionaryCommand()
        {
            this.CommandName = "SetDataTableRowValuesByDictionaryCommand";
            this.SelectionName = "Set DataTable Row Values By Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (var myDT, var rowIndex) = this.GetDataTableVariableAndRowIndex(nameof(v_DataTableName), nameof(v_RowIndex), engine);

            var myDic = v_RowValues.GetDictionaryVariable(engine);

            string ifKeyNotExists = this.GetUISelectionValue(nameof(v_NotExistsKey), "Key Not Exists", engine);

            // get columns list
            new GetDataTableColumnListCommand
            {
                v_DataTableName = this.v_DataTableName,
                v_OutputList = ExtensionMethods.GetInnerVariableName(0, engine)
            }.RunCommand(engine);
            var columns = (List<string>)ExtensionMethods.GetInnerVariable(0, engine).VariableValue;

            if (ifKeyNotExists == "error")
            {
                // check key and throw exception
                foreach(var item in myDic)
                {
                    if (!columns.Contains(item.Key))
                    {
                        throw new Exception("Column name " + item.Key + " does not exists");
                    }
                }
            }

            foreach(var item in myDic)
            {
                if (columns.Contains(item.Key))
                {
                    myDT.Rows[rowIndex][item.Key] = item.Value;
                }
            }
        }
    }
}