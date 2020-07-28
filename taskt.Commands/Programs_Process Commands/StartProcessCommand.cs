using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
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

namespace taskt.Commands
{
    [Serializable]
    [Group("Programs/Process Commands")]
    [Description("This command starts a program or process.")]

    public class StartProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Program Name or Path")]
        [InputSpecification("Provide a valid program name or enter a full path to the script/executable including the extension.")]
        [SampleUsage(@"notepad || excel || {vApp} || C:\temp\myapp.exe || {ProjectPath}\myapp.exe")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_ProgramName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Arguments")]
        [InputSpecification("Enter any arguments or flags if applicable.")]
        [SampleUsage("-a || -version || {vArg}")]
        [Remarks("You will need to consult documentation to determine if your executable supports arguments or flags on startup.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ProgramArgs { get; set; }

        [XmlAttribute]
        [PropertyDescription("Wait For Exit")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether to wait for the process to be completed.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_WaitForExit { get; set; }

        public StartProcessCommand()
        {
            CommandName = "StartProcessCommand";
            SelectionName = "Start Process";
            CommandEnabled = true;
            CustomRendering = true;
            v_WaitForExit = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            string vProgramName = v_ProgramName.ConvertToUserVariable(engine);
            string vProgramArgs = v_ProgramArgs.ConvertToUserVariable(engine);
            Process newProcess;

            if (v_ProgramArgs == "")
                newProcess = Process.Start(vProgramName);
            else
                newProcess = Process.Start(vProgramName, vProgramArgs);

            if (v_WaitForExit == "Yes")
                newProcess.WaitForExit();

            Thread.Sleep(2000);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ProgramName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ProgramArgs", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_WaitForExit", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Process '{v_ProgramName}' - Wait For Exit '{v_WaitForExit}']";
        }
    }
}