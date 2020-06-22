using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace taskt.Core.Automation.Commands
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
        [PropertyDescription("Visible")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the Excel automation should be visible or not.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Visible { get; set; }

        public ExcelCreateApplicationCommand()
        {
            CommandName = "ExcelCreateApplicationCommand";
            SelectionName = "Create Excel Application";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_Visible = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var newExcelSession = new Application();
            if (v_Visible == "Yes")
                newExcelSession.Visible = true;
            else
                newExcelSession.Visible = false;

            engine.AddAppInstance(v_InstanceName, newExcelSession); 
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_Visible", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Visible '{v_Visible}' - Instance Name '{v_InstanceName}']";
        }
    }
}