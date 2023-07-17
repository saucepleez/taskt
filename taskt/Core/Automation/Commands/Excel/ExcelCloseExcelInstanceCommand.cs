using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Instance")]
    [Attributes.ClassAttributes.CommandSettings("Close Excel Instance")]
    [Attributes.ClassAttributes.Description("This command allows you to close Excel instance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to close an open instance of Excel.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ExcelCloseExcelInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ExcelControls), nameof(ExcelControls.v_InputInstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("If the Workbook should be Saved")]
        [PropertyUISelectionOption("True")]
        [PropertyUISelectionOption("False")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**True**", "If the Workbook needs to be Saved, Save it and then Close it")]
        [PropertyDetailSampleUsage("**False**", "Whether the Workbook needs to be Saved or not, Close it without saving")]
        [PropertyIsOptional(true, "False")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_ExcelSaveOnExit { get; set; }

        public ExcelCloseExcelInstanceCommand()
        {
            //this.CommandName = "ExcelCloseApplicationCommand";
            //this.SelectionName = "Close Excel Application";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelInstance = v_InstanceName.GetExcelInstance(engine);

            //check if workbook exists and save
            if (excelInstance.ActiveWorkbook != null)
            {
                string vSaved = this.GetUISelectionValue(nameof(v_ExcelSaveOnExit), "Save Setting", engine);

                excelInstance.ActiveWorkbook.Close((vSaved == "true"));
            }

            //close excel
            excelInstance.Quit();

            //remove instance
            engine.RemoveAppInstance(vInstance);
        }
    }
}