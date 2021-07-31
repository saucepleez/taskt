using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

 
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to start a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to start applications by entering their name such as 'chrome.exe' or a fully qualified path to a file 'c:/some.exe'")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start'.")]
    public class StartProcessCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the name or path to the program (ex. notepad, calc, C:\\temp\\myapp.exe, {{{vPath}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Provide a valid program name or enter a full path to the script/executable including the extension")]
        [Attributes.PropertyAttributes.SampleUsage("**notepad** or **calc** or **c:\\temp\\myapp.exe** or **{{{vPath}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ProgramName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Please enter any arguments (ex. -a, -version, {{{vArgs}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter any arguments or flags if applicable.")]
        [Attributes.PropertyAttributes.SampleUsage("**-a** or **-version** or **{{{vArgs}}}**")]
        [Attributes.PropertyAttributes.Remarks("You will need to consult documentation to determine if your executable supports arguments or flags on startup.")]
        public string v_ProgramArgs { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Wait for the process to complete? (Default is No)")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Wait For Exit.")]
        [Attributes.PropertyAttributes.SampleUsage("Select **Yes** or **No**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_WaitForExit { get; set; }

        public StartProcessCommand()
        {
            this.CommandName = "StartProcessCommand";
            this.SelectionName = "Start Process";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {

            string vProgramName = v_ProgramName.ConvertToUserVariable(sender);
            string vProgramArgs = v_ProgramArgs.ConvertToUserVariable(sender);
            System.Diagnostics.Process p;

            if (v_ProgramArgs == "")
            {
                p = System.Diagnostics.Process.Start(vProgramName);
            }
            else
            {
                p = System.Diagnostics.Process.Start(vProgramName, vProgramArgs);
            }

            var waitForExit = v_WaitForExit.ConvertToUserVariable(sender);
            if (String.IsNullOrEmpty(waitForExit))
            {
                waitForExit = "No";
            }

            if (waitForExit == "Yes")
            {
                p.WaitForExit();
            }

            System.Threading.Thread.Sleep(2000);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ProgramName", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ProgramArgs", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_WaitForExit", this, editor));

            return RenderedControls;
        }
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Process: " + v_ProgramName + "]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_ProgramName))
            {
                this.validationResult += "Program is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}