using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.Description("This command set Row values from List.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a Row values from List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ExcelSetRowValuesFromListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("Instance Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyFirstValue("%kwd_default_excel_instance%")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Row Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Row Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Specify Column Type")]
        [InputSpecification("")]
        [SampleUsage("**Range** or **RC**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Range")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Range")]
        [PropertyUISelectionOption("RC")]
        [PropertyValueSensitive(false)]
        public string v_ColumnType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Start Column Location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Start Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero | PropertyValidationRule.ValidationRuleFlags.EqualsZero)]
        public string v_ColumnStart { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the End Column Location")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**A** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "End of List")]
        public string v_ColumnEnd { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the List Variable Name to set")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ListVariable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the Value type to set")]
        [InputSpecification("")]
        [SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        [Remarks("")]
        [PropertyUISelectionOption("Cell")]
        [PropertyUISelectionOption("Formula")]
        [PropertyUISelectionOption("Format")]
        [PropertyUISelectionOption("Font Color")]
        [PropertyUISelectionOption("Back Color")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsOptional(true, "Cell")]
        [PropertyValueSensitive(false)]
        public string v_ValueType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify If List Items not enough")]
        [InputSpecification("")]
        [SampleUsage("**Ignore** or **Error**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ignore")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ignore")]
        [PropertyUISelectionOption("Error")]
        [PropertyValueSensitive(false)]
        public string v_IfListNotEnough { get; set; }

        public ExcelSetRowValuesFromListCommand()
        {
            this.CommandName = "ExcelSetRowValuesFromListCommand";
            this.SelectionName = "Set Row Values From List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var excelInstance = ExcelControls.getExcelInstance(engine, v_InstanceName.ConvertToUserVariable(engine));
            var excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelInstance.ActiveSheet;

            //int rowIndex = int.Parse(v_RowIndex.ConvertToUserVariable(engine));
            int rowIndex = v_RowIndex.ConvertToUserVariableAsInteger("Row Index", engine);
            if (rowIndex < 1)
            {
                throw new Exception("Row index is less than 1");
            }

            // get list
            List<string> myList = v_ListVariable.GetListVariable(engine);

            int columnStartIndex = 0;
            int columnEndIndex = 0;
            switch (v_ColumnType.GetUISelectionValue("v_ColumnType", this, engine))
            {
                case "range":
                    columnStartIndex = ExcelControls.getColumnIndex(excelSheet, v_ColumnStart.ConvertToUserVariable(engine));

                    if (String.IsNullOrEmpty(v_ColumnEnd))
                    {
                        columnEndIndex = columnStartIndex + myList.Count - 1;
                    }
                    else
                    {
                        columnEndIndex = ExcelControls.getColumnIndex(excelSheet, v_ColumnEnd.ConvertToUserVariable(engine));
                    }
                    break;

                case "rc":
                    //columnStartIndex = int.Parse(v_ColumnStart.ConvertToUserVariable(engine));
                    //columnEndIndex = int.Parse(v_ColumnEnd.ConvertToUserVariable(engine));
                    columnStartIndex = v_ColumnStart.ConvertToUserVariableAsInteger("Column Start", engine);

                    if (String.IsNullOrEmpty(v_ColumnEnd))
                    {
                        columnEndIndex = columnStartIndex + myList.Count - 1;
                    }
                    else
                    {
                        columnEndIndex = v_ColumnEnd.ConvertToUserVariableAsInteger("Column End", engine);
                    }

                    if ((columnStartIndex < 0) || (columnEndIndex < 0))
                    {
                        throw new Exception("Column is less than 0");
                    }
                    break;
            }
            if (columnStartIndex > columnEndIndex)
            {
                int t = columnStartIndex;
                columnStartIndex = columnEndIndex;
                columnEndIndex = t;
            }

            string ifListNotEnough = v_IfListNotEnough.GetUISelectionValue("v_IfListNotEnough", this, engine);
            int range = columnEndIndex - columnStartIndex + 1;
            if (ifListNotEnough == "error")
            {
                if (range > myList.Count)
                {
                    throw new Exception("List Items not enough");
                }
            }

            int max = range;
            if (range > myList.Count)
            {
                max = myList.Count;
            }

            Action<string, Microsoft.Office.Interop.Excel.Worksheet, int, int> setFunc = ExcelControls.setCellValueFunction(v_ValueType.GetUISelectionValue("v_ValueType", this, engine));

            for (int i = 0; i < max; i++)
            {
                setFunc(myList[i], excelSheet, columnStartIndex + i, rowIndex);
            }
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctls);

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set " + v_ValueType + " Values From '" + v_ColumnStart + "' to '" + v_ColumnEnd + "' Row '" + v_RowIndex + "' from List '" + v_ListVariable + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}