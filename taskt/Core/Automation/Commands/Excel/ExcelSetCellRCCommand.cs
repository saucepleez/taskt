using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.Description("This command sets the value of a cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelSetCellRCCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **{{{vInstance}}}**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.Excel)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to set")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text value that will be set.")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello World** or **{{{vText}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_TextToSet { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Row")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell row.")]
        [Attributes.PropertyAttributes.SampleUsage("**1** or **2** or **{{{vRow}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_ExcelCellRow { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Column")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell column.")]
        [Attributes.PropertyAttributes.SampleUsage("**1** or **2** or **{{{vColumn}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyTextBoxSetting(1, false)]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_ExcelCellColumn { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Value type (Default is Cell)")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Cell** or **Formula** or **Format** or **Color** or **Comment**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Cell")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Formula")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Format")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Font Color")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Back Color")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsOptional(true)]
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
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);

            var vRow = v_ExcelCellRow.ConvertToUserVariable(sender);
            var vCol = v_ExcelCellColumn.ConvertToUserVariable(sender);

            int row, col;
            row = int.Parse(vRow);
            col = int.Parse(vCol);
           
            var targetText = v_TextToSet.ConvertToUserVariable(sender);

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
            ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Value = targetText;

            var valueType = v_ValueType.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(valueType))
            {
                valueType = "Cell";
            }

            switch (valueType)
            {
                case "Cell":
                    ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Value = targetText;
                    break;
                case "Formula":
                    ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Formula = targetText;
                    break;
                case "Format":
                    ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).NumberFormatLocal = targetText;
                    break;
                case "Font Color":
                    ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Font.Color = long.Parse(targetText);
                    break;
                case "Back Color":
                    ((Microsoft.Office.Interop.Excel.Range)excelSheet.Cells[row, col]).Interior.Color = long.Parse(targetText);
                    break;
                default:
                    throw new Exception(valueType + " is not support.");
                    break;
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellRow", this, editor));
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellColumn", this, editor));

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
            }

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Cell " + v_ValueType + " Row: " + v_ExcelCellRow + ", Column: " + v_ExcelCellColumn + " to '" + v_TextToSet + "', Instance Name: '" + v_InstanceName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InstanceName))
            {
                this.validationResult += "Instance is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_ExcelCellRow))
            {
                this.validationResult += "Row is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_ExcelCellColumn))
            {
                this.validationResult += "Column is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}