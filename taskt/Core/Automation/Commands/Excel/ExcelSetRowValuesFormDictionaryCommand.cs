using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.Description("This command set Row values from Dictionary.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a Row values from Dictionary.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetRowValuesFromDictionaryCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        //[SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyValidationRule("Instance Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyFirstValue("%kwd_default_excel_instance%")]
        //[PropertyDisplayText(true, "Instance")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the Row Index")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        //[Remarks("")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyValidationRule("Row Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        //[PropertyDisplayText(true, "Row")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_RowLocation))]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Specify Column Type")]
        //[InputSpecification("")]
        //[SampleUsage("**Range** or **RC**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "Range")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Range")]
        //[PropertyUISelectionOption("RC")]
        //[PropertySelectionValueSensitive(false)]
        //[PropertyDisplayText(true, "Column Type")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnType))]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the Start Column Location")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "A or 1")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "Start Column")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnStart))]
        public string v_ColumnStart { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please Enter the End Column Location")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "End of Dictionary")]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyDisplayText(true, "End Column")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ColumnEnd))]
        public string v_ColumnEnd { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Dictionary Variable Name to set")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary)]
        //[PropertyValidationRule("Dictionary", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(DictionaryControls), nameof(DictionaryControls.v_InputDictionaryName))]
        public string v_DictionaryVariable { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify the Value type to set")]
        //[InputSpecification("")]
        //[SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        //[Remarks("")]
        //[PropertyUISelectionOption("Cell")]
        //[PropertyUISelectionOption("Formula")]
        //[PropertyUISelectionOption("Format")]
        //[PropertyUISelectionOption("Font Color")]
        //[PropertyUISelectionOption("Back Color")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsOptional(true, "Cell")]
        //[PropertySelectionValueSensitive(false)]
        //[PropertyDisplayText(true, "Value Type")]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_ValueType))]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify If Dictionary Items not enough")]
        //[InputSpecification("")]
        //[SampleUsage("**Ignore** or **Error**")]
        //[Remarks("")]
        //[PropertyIsOptional(true, "Ignore")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyUISelectionOption("Error")]
        //[PropertySelectionValueSensitive(false)]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_WhenItemNotEnough))]
        [PropertyDescription("When Dictionary Items Not Enough")]
        public string v_IfDictionaryNotEnough { get; set; }

        public ExcelSetRowValuesFromDictionaryCommand()
        {
            this.CommandName = "ExcelSetRowValuesFromDictionaryCommand";
            this.SelectionName = "Set Row Values From Dictionary";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            (_, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            Dictionary<string, string> myDic = v_DictionaryVariable.GetDictionaryVariable(engine);

            (int rowIndex, int columnStartIndex, int columnEndIndex, string valueType) =
                ExcelControls.GetRangeIndeiesRowDirection(
                    nameof(v_RowIndex), nameof(v_ColumnType),
                    nameof(v_ColumnStart), nameof(v_ColumnEnd),
                    nameof(v_ValueType), engine, excelSheet, this,
                    myDic
                );

            //string ifListNotEnough = v_IfDictionaryNotEnough.GetUISelectionValue("v_IfDictionaryNotEnough", this, engine);
            string ifListNotEnough = this.GetUISelectionValue(nameof(v_IfDictionaryNotEnough), "If Dictionary Not Enough", engine);
            int range = columnEndIndex - columnStartIndex + 1;
            if (ifListNotEnough == "error")
            {
                if (range > myDic.Count)
                {
                    throw new Exception("Dictionary Items not enough");
                }
            }

            int max = range;
            if (range > myDic.Count)
            {
                max = myDic.Count;
            }

            Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.SetCellValueFunction(v_ValueType.GetUISelectionValue("v_ValueType", this, engine));

            string[] keys = new string[myDic.Count];
            myDic.Keys.CopyTo(keys, 0);

            for (int i = 0; i < max; i++)
            {
                setFunc(myDic[keys[i]], excelSheet, columnStartIndex + i, rowIndex);
            }
        }
    }
}