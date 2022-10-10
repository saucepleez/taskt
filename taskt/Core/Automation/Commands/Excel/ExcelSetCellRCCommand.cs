using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.Description("This command sets the value of a cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelSetCellRCCommand : ScriptCommand
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
        [PropertyValidationRule("Instance", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Instance")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter text to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the text value that will be set.")]
        [SampleUsage("**Hello World** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Text")]
        public string v_TextToSet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Cell Row")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the actual location of the cell row.")]
        [SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Row", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Row")]
        public string v_ExcelCellRow { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Cell Column")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the actual location of the cell column.")]
        [SampleUsage("**1** or **2** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Column")]
        public string v_ExcelCellColumn { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value type")]
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
        [PropertyDisplayText(true, "Value Type")]
        public string v_ValueType { get; set; }

        public ExcelSetCellRCCommand()
        {
            this.CommandName = "ExcelSetCellRCCommand";
            this.SelectionName = "Set Cell RC";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            //var excelObject = engine.GetAppInstance(vInstance);
            //Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            //var vRow = v_ExcelCellRow.ConvertToUserVariable(sender);
            //var vCol = v_ExcelCellColumn.ConvertToUserVariable(sender);
            //int row, col;
            //row = int.Parse(vRow);
            //col = int.Parse(vCol);

            int row = v_ExcelCellRow.ConvertToUserVariableAsInteger("v_ExcelCellRow", "Row", engine, this);
            int col = v_ExcelCellColumn.ConvertToUserVariableAsInteger("v_ExcelCellColumn", "Column", engine, this);

            var targetText = v_TextToSet.ConvertToUserVariable(sender);

            Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
            //((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Value = targetText;

            //var valueType = v_ValueType.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(valueType))
            //{
            //    valueType = "Cell";
            //}

            var valueType = v_ValueType.GetUISelectionValue("v_ValueType", this, engine);

            long colorToSet = 0;
            switch (valueType)
            {
                case "fore color":
                case "back color":
                    if (!long.TryParse(targetText, out colorToSet))
                    {
                        throw new Exception("Text to set '" + targetText + "' is not color.");
                    }
                    break;
            }

            // set range
            Microsoft.Office.Interop.Excel.Range rg = (Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col];

            switch (valueType)
            {
                case "cell":
                    //((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Value = targetText;
                    rg.Value = targetText;
                    break;
                case "formula":
                    //((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Formula = targetText;
                    rg.Formula = targetText;
                    break;
                case "format":
                    //((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).NumberFormatLocal = targetText;
                    rg.NumberFormatLocal = targetText;
                    break;
                case "font color":
                    //((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Font.Color = long.Parse(targetText);
                    //rg.Font.Color = long.Parse(targetText);
                    rg.Font.Color = colorToSet;
                    break;
                case "back color":
                    //((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Interior.Color = long.Parse(targetText);
                    //rg.Interior.Color = long.Parse(targetText);
                    rg.Interior.Color = colorToSet;
                    break;
                //default:
                //    throw new Exception(valueType + " is not support.");
                //    break;
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellRow", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellColumn", this, editor));

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    if (editor.creationMode == frmCommandEditor.CreationMode.Add)
        //    {
        //        this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
        //    }

        //    return RenderedControls;
        //}
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Set Cell " + v_ValueType + " Row: " + v_ExcelCellRow + ", Column: " + v_ExcelCellColumn + " to '" + v_TextToSet + "', Instance Name: '" + v_InstanceName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InstanceName))
        //    {
        //        this.validationResult += "Instance is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_ExcelCellRow))
        //    {
        //        this.validationResult += "Row is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_ExcelCellColumn))
        //    {
        //        this.validationResult += "Column is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}