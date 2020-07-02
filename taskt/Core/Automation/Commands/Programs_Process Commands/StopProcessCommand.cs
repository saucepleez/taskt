using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Programs/Process Commands")]
    [Description("This command stops a program or process.")]

    public class StopProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Program Name or Path")]
        [InputSpecification("Provide a valid program name or enter a full path to the script/executable including the extension.")]
        [SampleUsage(@"notepad || excel || {vApp} || C:\temp\myapp.exe || {ProjectPath}\myapp.exe")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_ProgramName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Stop Option")]
        [PropertyUISelectionOption("Close")]
        [PropertyUISelectionOption("Kill")]
        [InputSpecification("Indicate whether the program should be closed or killed.")]
        [SampleUsage("")]
        [Remarks("*Close* will close any open process windows while *Kill* will close all processes, including background ones.")]
        public string v_StopOption { get; set; }

        public StopProcessCommand()
        {
            CommandName = "StopProgramCommand";
            SelectionName = "Stop Process";
            CommandEnabled = true;
            CustomRendering = true;
            v_StopOption = "Kill";
        }

        public override void RunCommand(object sender)
        {
            string shortName = v_ProgramName.ConvertToUserVariable(sender);
            var processes = Process.GetProcessesByName(shortName);

            foreach (var prc in processes)
            {
                if (v_StopOption == "Close")
                    prc.CloseMainWindow();
                else if (v_StopOption == "Kill")
                    prc.Kill();
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ProgramName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_StopOption", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_StopOption} Process '{v_ProgramName}']";
        }
    }
}