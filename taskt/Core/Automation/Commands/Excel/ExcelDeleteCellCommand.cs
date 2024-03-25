﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Linq;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Excel Commands")]
    [Attributes.ClassAttributes.SubGruop("Cell")]
    [Attributes.ClassAttributes.CommandSettings("Delete Cell")]
    [Attributes.ClassAttributes.Description("This command allows you to delete a specified cell in Excel")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete a specific cell from the current sheet.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Excel Interop to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    public class ExcelDeleteCellCommand : AExcelInstanceCommands
    {
        //[XmlAttribute]
        //[PropertyDescription("Please Enter the instance name")]
        //[InputSpecification("Enter the unique instance name that was specified in the **Create Excel** command")]
        //[SampleUsage("**myInstance** or **excelInstance**")]
        //[Remarks("Failure to enter the correct instance name or failure to first call **Create Excel** command will cause an error")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.Excel)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyDescription("Indicate the range to delete ex. A1 or A1:C1")]
        [InputSpecification("Enter the actual location of the cell.")]
        [SampleUsage("A1, B10, {vAddress}")]
        [Remarks("")]
        [PropertyParameterOrder(6000)]
        public string v_CellLocation { get; set; }

        [XmlAttribute]
        [PropertyDescription("Should the cells below shift upward after deletion?")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the row below will be shifted up to replace the old row.")]
        [SampleUsage("Select 'Yes' or 'No'")]
        [Remarks("")]
        [PropertyParameterOrder(6001)]
        public string v_Shift { get; set; }

        public ExcelDeleteCellCommand()
        {
            //this.CommandName = "ExcelDeleteCellCommand";
            //this.SelectionName = "Delete Cell";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(_, var excelSheet) = v_InstanceName.ExpandValueOrUserVariableAsExcelInstanceAndWorksheet(engine);
            (_, var excelSheet) = this.ExpandValueOrVariableAsExcelInstanceAndCurrentWorksheet(engine);

            string range = v_CellLocation.ExpandValueOrUserVariable(engine);
            var cells = excelSheet.Range[range, Type.Missing];

            if (v_Shift == "Yes")
            {
                cells.Delete();
            }
            else
            {
                cells.Clear();
            }
        }

        public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            var instanceCtrls = CommandControls.CreateDefaultDropdownGroupFor("v_InstanceName", this, editor);
            CommandControls.AddInstanceNames((ComboBox)instanceCtrls.Where(t => (t.Name == "v_InstanceName")).FirstOrDefault(), editor, PropertyInstanceType.InstanceType.Excel);
            RenderedControls.AddRange(instanceCtrls);
            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_Shift", this, editor));

            if (editor.creationMode == UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor.CreationMode.Add)
            {
                this.v_InstanceName = editor.appSettings.ClientSettings.DefaultExcelInstanceName;
            }

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Range: " + v_CellLocation + ", Instance Name: '" + v_InstanceName + "']";
        }
    }
}