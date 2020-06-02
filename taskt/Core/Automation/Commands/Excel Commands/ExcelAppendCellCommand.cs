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
    [Attributes.ClassAttributes.Description("Append input to last row of sheet into the first cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to the last cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ExcelAppendCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **excelInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter text to set")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text value that will be set.")]
        [Attributes.PropertyAttributes.SampleUsage("Hello World or [vText]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_TextToSet { get; set; }

        public ExcelAppendCellCommand()
        {
            this.CommandName = "ExcelAppendCellCommand";
            this.SelectionName = "Append Cell";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {


            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);


            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
            int test = 0;
            test = excelSheet.Columns.Count;
            var lastUsedRow = excelSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                 System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                 Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                 false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
            var targetAddress = "A" + (lastUsedRow + 1);
            var targetText = v_TextToSet.ConvertToUserVariable(sender);
            excelSheet.Range[targetAddress].Value = targetText;

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Append last cell to: " + v_TextToSet +"]";
        }
    }
}