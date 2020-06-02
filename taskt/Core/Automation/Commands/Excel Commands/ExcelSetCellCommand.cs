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
    [Attributes.ClassAttributes.Description("This command sets the value of a cell.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set a value to a specific cell.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelSetCellCommand : ScriptCommand
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
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Cell Location (ex. A1 or B2)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the actual location of the cell.")]
        [Attributes.PropertyAttributes.SampleUsage("A1, B10, [vAddress]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ExcelCellAddress { get; set; }
        public ExcelSetCellCommand()
        {
            this.CommandName = "ExcelSetCellCommand";
            this.SelectionName = "Set Cell";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);

           
                var targetAddress = v_ExcelCellAddress.ConvertToUserVariable(sender);
                var targetText = v_TextToSet.ConvertToUserVariable(sender);

                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
                excelSheet.Range[targetAddress].Value = targetText;
            
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ExcelCellAddress", this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Set Cell '" + v_ExcelCellAddress + "' to '" + v_TextToSet + "', Instance Name: '" + v_InstanceName + "']";
        }
    }
}