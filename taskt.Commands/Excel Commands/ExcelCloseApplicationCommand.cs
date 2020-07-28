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
    [Description("This command closes an open Excel Workbook and Instance.")]

    public class ExcelCloseApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Save Workbook")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the Workbook should be saved before closing.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ExcelSaveOnExit { get; set; }

        public ExcelCloseApplicationCommand()
        {
            CommandName = "ExcelCloseApplicationCommand";
            SelectionName = "Close Excel Application";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_ExcelSaveOnExit = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);
            var excelInstance = (Application)excelObject;
            bool saveOnExit;
            if (v_ExcelSaveOnExit == "Yes")
                saveOnExit = true;
            else
                saveOnExit = false;

            //check if workbook exists and save
            if (excelInstance.ActiveWorkbook != null)
            {
                excelInstance.ActiveWorkbook.Close(saveOnExit);
            }

            //close excel
            excelInstance.Quit();
            //remove instance
            engine.RemoveAppInstance(vInstance);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ExcelSaveOnExit", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Save on Close '{v_ExcelSaveOnExit}' - Instance Name '{v_InstanceName}']";
        }
    }
}