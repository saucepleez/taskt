using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.Description("This command gets text from a specified Excel Cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get a value from a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Excel Interop' to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelGetCellRCCommand : ScriptCommand
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
        [PropertyDescription("Assign to Variable")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_userVariableName { get; set; }

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
        [PropertyDisplayText(true, "Type")]
        public string v_ValueType { get; set; }

        public ExcelGetCellRCCommand()
        {
            this.CommandName = "ExcelGetCellRCCommand";
            this.SelectionName = "Get Cell RC";
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

            //var excelInstance = v_InstanceName.getExcelInstance(engine);
            //Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
            (var excelInstance, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);


            //var vRow = v_ExcelCellRow.ConvertToUserVariable(sender);
            //var vColumn = v_ExcelCellColumn.ConvertToUserVariable(sender);

            //int row, col;
            //row = int.Parse(vRow);
            //col = int.Parse(vColumn);
            //int row = v_ExcelCellRow.ConvertToUserVariableAsInteger("v_ExcelCellRow", "Row", engine, this);
            //int col = v_ExcelCellColumn.ConvertToUserVariableAsInteger("v_ExcelCellColumn", "Column", engine, this);
            //(int row, int col) = ((v_ExcelCellRow, "v_ExcelCellRow"), (v_ExcelCellColumn, "v_ExcelCellColumn")).ConvertToUserVariableAsExcelRCLocation(engine, excelInstance, this);

            var rg = ((v_ExcelCellRow, "v_ExcelCellRow"), (v_ExcelCellColumn, "v_ExcelCellColumn")).GetExcelRange(engine, excelInstance, excelSheet, this);

            //var valueType = v_ValueType.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(valueType))
            //{
            //    valueType = "Cell";
            //}
            var valueType = v_ValueType.GetUISelectionValue("v_ValueType", this, engine);

            string cellValue = "";
            switch (valueType)
            {
                case "cell":
                    //cellValue = (string)((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Text;
                    cellValue = rg.Text;
                    break;
                case "formula":
                    //cellValue = (string)((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Formula;
                    cellValue = rg.Formula;
                    break;
                case "format":
                    //cellValue = (string)((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).NumberFormatLocal;
                    cellValue = rg.NumberFormatLocal;
                    break;
                case "font color":
                    //cellValue = ((long)((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Font.Color).ToString();
                    cellValue = rg.Font.Color.ToString();
                    break;
                case "back color":
                    //cellValue = ((long)((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Interior.Color).ToString();
                    cellValue = rg.Interior.Color.ToString();
                    break;
                //default:
                //    throw new Exception("Value type " + valueType + " is not support.");
                //    break;
            }

            cellValue.StoreInUserVariable(sender, v_userVariableName);            
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    ////create standard group controls
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellRow", this, editor));
        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellColumn", this, editor));

        //    ////create control for variable name
        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

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
        //    return base.GetDisplayValue() + " [Get " + v_ValueType + " Value From Row: " + v_ExcelCellRow + ", Column: " + v_ExcelCellColumn + " and apply to variable '" + v_userVariableName + "', Instance Name: '" + v_InstanceName + "']";
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
        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}