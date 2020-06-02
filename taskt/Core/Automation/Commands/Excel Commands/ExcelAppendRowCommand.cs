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
    [Attributes.ClassAttributes.Description("Append to last row of sheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command will take in a comma seprerated value and append it to the end of an excel sheet.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automations.")]
    public class ExcelAppendRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **excelInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the Row to set")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter the text value that will be set (This could be a DataRow).")]
        [Attributes.PropertyAttributes.SampleUsage("Hello,world or [vText]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_TextToSet { get; set; }

        public ExcelAppendRowCommand()
        {
            this.CommandName = "ExcelAppendRowCommand";
            this.SelectionName = "Append Row";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var splittext = v_TextToSet.Split(',');
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);
            int i = 1;

            Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
            Microsoft.Office.Interop.Excel.Worksheet excelSheet = excelInstance.ActiveSheet;
            int lastUsedRow;
            try
            {
                lastUsedRow = excelSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                 System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                 Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                 false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
            }
            catch(Exception ex)
            {
                lastUsedRow = 0;
            }

            var targetText = v_TextToSet.ConvertToUserVariable(sender);
            splittext = targetText.Split(',');

            foreach (var item in splittext)
            {
                string output = item;
                if (item.Contains("[") || item.Contains("]"))
                    output = item.Trim('[', ']');
                output = output.Trim('"');
                if (output=="null")
                {
                    output = string.Empty;
                }
                excelSheet.Cells[lastUsedRow + 1, i] = output;
                i++;
            }


        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Append Row '" +v_TextToSet+ " to last row of workboook with Instance Name: '" + v_InstanceName + "']";
        }
    }
}