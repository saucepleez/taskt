using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace taskt.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command activates a specific Worksheet in an Excel Workbook.")]

    public class ExcelActivateSheetCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Worksheet")]
        [InputSpecification("Specify the Worksheet within the Workbook to activate.")]
        [SampleUsage("Sheet1 || {vSheet}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SheetName { get; set; }

        public ExcelActivateSheetCommand()
        {
            CommandName = "ExcelActivateSheetCommand";
            SelectionName = "Activate Sheet";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            string vSheetToDelete = v_SheetName.ConvertToUserVariable(engine);

            var excelObject = engine.GetAppInstance(vInstance);
            var excelInstance = (Application)excelObject;      
            var workSheet = excelInstance.Sheets[vSheetToDelete] as Worksheet;
            workSheet.Select(); 
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SheetName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Sheet '{v_SheetName}' - Instance Name '{v_InstanceName}']";
        }
    }
}