using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    [Description("This command creates an Excel Instance.")]

    public class ExcelCreateApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter a unique name that will represent the application instance.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("This unique name allows you to refer to the instance by name in future commands, " +
                 "ensuring that the commands you specify run against the correct application.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("New/Open Workbook")]
        [PropertyUISelectionOption("New Workbook")]
        [PropertyUISelectionOption("Open Workbook")]
        [InputSpecification("Indicate whether to create a new Workbook or to open an existing Workbook.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_NewOpenWorkbook { get; set; }

        [XmlAttribute]
        [PropertyDescription("Workbook File Path")]
        [InputSpecification("Enter or Select the path to the Workbook file.")]
        [SampleUsage(@"C:\temp\myfile.xlsx || {vFilePath} || {ProjectPath}\myfile.xlsx")]
        [Remarks("This input should only be used for opening existing Workbooks.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Visible")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the Excel automation should be visible or not.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Visible { get; set; }

        [XmlAttribute]
        [PropertyDescription("Close All Existing Excel Instances")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether to close any existing Excel instances before executing Excel Automation.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_CloseAllInstances { get; set; }

        public ExcelCreateApplicationCommand()
        {
            CommandName = "ExcelCreateApplicationCommand";
            SelectionName = "Create Excel Application";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_NewOpenWorkbook = "New Workbook";
            v_Visible = "No";
            v_CloseAllInstances = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vFilePath = v_FilePath.ConvertToUserVariable(engine);

            if (v_CloseAllInstances == "Yes")
            {
                var processes = Process.GetProcessesByName("excel");
                foreach (var prc in processes)
                {
                    prc.Kill();
                }
            }

            var newExcelSession = new Application();
            if (v_Visible == "Yes")
                newExcelSession.Visible = true;
            else
                newExcelSession.Visible = false;

            engine.AddAppInstance(v_InstanceName, newExcelSession); 

            if (v_NewOpenWorkbook == "New Workbook")
            {
                if (!string.IsNullOrEmpty(vFilePath))
                    throw new InvalidDataException("File path should not be provided for a new Excel Workbook");
                else
                    newExcelSession.Workbooks.Add();
            }
            else if (v_NewOpenWorkbook == "Open Workbook")
            {
                if (string.IsNullOrEmpty(vFilePath))
                    throw new NullReferenceException("File path for Excel Workbook not provided");
                else
                    newExcelSession.Workbooks.Open(vFilePath);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_NewOpenWorkbook", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_Visible", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_CloseAllInstances", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_NewOpenWorkbook} - Visible '{v_Visible}' - Close Instances '{v_CloseAllInstances}' - New Instance Name '{v_InstanceName}']";
        }
    }
}