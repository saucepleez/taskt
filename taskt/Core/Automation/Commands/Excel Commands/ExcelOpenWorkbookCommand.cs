using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command opens an Excel Workbook.")]

    public class ExcelOpenWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Workbook File Path")]
        [InputSpecification("Enter or Select the path to the Workbook file.")]
        [SampleUsage(@"C:\temp\myfile.xlsx || {vFilePath} || {ProjectPath}\myfile.xlsx")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        public ExcelOpenWorkbookCommand()
        {
            CommandName = "ExcelOpenWorkbookCommand";
            SelectionName = "Open Workbook";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var vFilePath = v_FilePath.ConvertToUserVariable(sender);
            var excelObject = engine.GetAppInstance(vInstance);
            var excelInstance = (Application)excelObject;
            excelInstance.Workbooks.Open(vFilePath);  
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Open '{v_FilePath}' - Instance Name '{v_InstanceName}']";
        }
    }
}