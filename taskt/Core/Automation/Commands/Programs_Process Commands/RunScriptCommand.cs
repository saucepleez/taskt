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
    [Attributes.ClassAttributes.Description("This command allows you to run a script or program and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a script (such as vbScript, javascript, or executable) but wait for it to close before taskt continues executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    public class RunScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the path to the script, or select a file.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a fully qualified path to the script, including the script extension.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myscript.vbs**")]
        [Attributes.PropertyAttributes.Remarks("This command differs from **Start Process** because this command blocks execution until the script has completed.  If you do not want to stop while the script executes, consider using **Start Process** instead.")]
        public string v_ScriptPath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Script Type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Powershell")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Python")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Default")]
        [Attributes.PropertyAttributes.InputSpecification("Select the type of script you want to execute.")]
        [Attributes.PropertyAttributes.SampleUsage("Select Powershell to run ps1 files, Default to run a file through the system default.")]
        [Attributes.PropertyAttributes.Remarks("Default executes with the system default for that file type.")]
        public string v_ScriptType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter script arguments, if any.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter any arguments as a single string.")]
        [Attributes.PropertyAttributes.SampleUsage("-message Hello -t 2")]
        public string v_ScriptArgs { get; set; }

        public RunScriptCommand()
        {
            this.CommandName = "RunScriptCommand";
            this.SelectionName = "Run Script";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            System.Diagnostics.Process scriptProc = new System.Diagnostics.Process();

            string scriptPath = v_ScriptPath.ConvertToUserVariable(sender);
            string scriptArgs = v_ScriptArgs.ConvertToUserVariable(sender);

            switch(v_ScriptType)
            {
                case "Powershell":
                    scriptProc.StartInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoProfile -ExecutionPolicy unrestricted -file \"{scriptPath}\" " + scriptArgs,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    break;
                case "Python":
                    scriptProc.StartInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = "python.exe",
                        Arguments = $"\"{scriptPath}\" " + scriptArgs,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    break;
                default:
                    scriptProc.StartInfo = new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = scriptPath,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    break;
            }
            scriptProc.Start();
            scriptProc.WaitForExit();

            string output = scriptProc.StandardOutput.ReadToEnd();
            string error = scriptProc.StandardError.ReadToEnd();
            scriptProc.Close();
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ScriptPath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ScriptArgs", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ScriptType", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Script Path: " + v_ScriptPath + "]";
        }
    }
}