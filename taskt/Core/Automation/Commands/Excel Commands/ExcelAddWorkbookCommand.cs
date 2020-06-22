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
    [Description("This command adds a new Excel Workbook to an Excel Instance.")]

    public class ExcelAddWorkbookCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        public ExcelAddWorkbookCommand()
        {
            CommandName = "ExcelAddWorkbookCommand";
            SelectionName = "Add Workbook";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);
            var excelInstance = (Application)excelObject;
            excelInstance.Workbooks.Add();
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
        }
    }
}