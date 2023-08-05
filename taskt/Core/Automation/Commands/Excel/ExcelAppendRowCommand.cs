using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Row")]
    [Attributes.ClassAttributes.CommandSettings("Append Row")]
    [Attributes.ClassAttributes.Description("Append to last row of sheet.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command will take in a comma seprerated value and append it to the end of an excel sheet.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automations.")]
    public class ExcelAppendRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the instance name")]
        [InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [SampleUsage("**myInstance** or **excelInstance**")]
        [Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Row to set")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter the text value that will be set (This could be a DataRow).")]
        [SampleUsage("Hello,world or {vText}")]
        [Remarks("")]
        public string v_TextToSet { get; set; }

        public ExcelAppendRowCommand()
        {
            //this.CommandName = "ExcelAppendRowCommand";
            //this.SelectionName = "Append Row";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            this.v_InstanceName = "";
        }
        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            var splittext = v_TextToSet.Split(',');

            (_, var excelSheet) = v_InstanceName.GetExcelInstanceAndWorksheet(engine);

            int i = 1;            
            int lastUsedRow;
            try
            {
                lastUsedRow = excelSheet.Cells.Find("*", System.Reflection.Missing.Value,
                                 System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                                 Microsoft.Office.Interop.Excel.XlSearchOrder.xlByRows, Microsoft.Office.Interop.Excel.XlSearchDirection.xlPrevious,
                                 false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
            }
            catch(Exception)
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

            var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
            CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.Excel);
            RenderedControls.AddRange(instanceCtrls);
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

            if (editor.creationMode == frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
            }

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Append Row '" +v_TextToSet+ " to last row of workboook with Instance Name: '" + v_InstanceName + "']";
        }
    }
}