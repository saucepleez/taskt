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
    [Attributes.ClassAttributes.Description("This command allows you to delete a specified row in Excel")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete an entire row from the current sheet.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    public class ExcelDeleteRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please Enter the instance name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        [Attributes.PropertyAttributes.SampleUsage("**myInstance** or **excelInstance**")]
        [Attributes.PropertyAttributes.Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InstanceName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Indicate the row number to delete")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the number of the row that should be deleted.")]
        [Attributes.PropertyAttributes.SampleUsage("1, 5, [vNumber]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_RowNumber { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Should the cells below shift upward after deletion?")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate whether the row below will be shifted up to replace the old row.")]
        [Attributes.PropertyAttributes.SampleUsage("Select 'Yes' or 'No'")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ShiftUp { get; set; }
        public ExcelDeleteRowCommand()
        {
            this.CommandName = "ExcelDeleteRowCommand";
            this.SelectionName = "Delete Row";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var vInstance = v_InstanceName.ConvertToUserVariable(engine);
            var excelObject = engine.GetAppInstance(vInstance);

          


                Microsoft.Office.Interop.Excel.Application excelInstance = (Microsoft.Office.Interop.Excel.Application)excelObject;
                Microsoft.Office.Interop.Excel.Worksheet workSheet = excelInstance.ActiveSheet;

                string rowToDelete = v_RowNumber.ConvertToUserVariable(sender);

                var cells = workSheet.Range["A" + rowToDelete, Type.Missing];
                var entireRow = cells.EntireRow;
                if (v_ShiftUp == "Yes")
                {
                    entireRow.Delete();
                }
                else
                {
                    entireRow.Clear();
                }


            
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RowNumber", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ShiftUp", this, editor));

            return RenderedControls;

        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Row Number: " + v_RowNumber + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
}