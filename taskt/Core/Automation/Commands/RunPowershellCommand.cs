using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to run a powershell script and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a powershell script and wait for it to close before taskt continues executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    public class RunPowershellCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter the path to the powershell script (ex. C:\\temp\\myscript.ps, {{{vScriptPath}}})")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a fully qualified path to the script, including the script extension.")]
        [Attributes.PropertyAttributes.SampleUsage("**C:\\temp\\myscript.ps** or **{{{vScriptPath}}}**")]
        [Attributes.PropertyAttributes.Remarks("This command differs from **Start Process** because this command blocks execution until the script has completed.  If you do not want to stop while the script executes, consider using **Start Process** instead.")]
        public string v_ScriptPath { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Enter Powershell Command Arguments")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter any necessary arguments")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_PowerShellArgs { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Convert variables before execution")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Yes")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("No")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary option.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_ReplaceScriptVariables { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Select the variable to receive the output")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_applyToVariableName { get; set; }
        public RunPowershellCommand()
        {
            this.CommandName = "RunPowershellCommand";
            this.SelectionName = "Run Powershell";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_PowerShellArgs = "-NoProfile -ExecutionPolicy unrestricted";
            this.v_ReplaceScriptVariables = "No";
        }

        public override void RunCommand(object sender)
        {
            {
               
                //define script path
                var scriptPath = v_ScriptPath.ConvertToUserVariable(sender);

                //get script text
                var psCommand = System.IO.File.ReadAllText(scriptPath);

                  if (v_ReplaceScriptVariables.ToUpperInvariant() == "YES")
                {
                    //convert variables
                    psCommand = psCommand.ConvertToUserVariable(sender);
                }
             
                //convert ps script
                var psCommandBytes = System.Text.Encoding.Unicode.GetBytes(psCommand);
                var psCommandBase64 = Convert.ToBase64String(psCommandBytes);

                //execute
                var startInfo = new ProcessStartInfo()
                {
                    FileName = "powershell.exe",
                    Arguments = $"{v_PowerShellArgs} -EncodedCommand {psCommandBase64}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true                  
                };


               var proc =  Process.Start(startInfo);

                proc.WaitForExit();

                //store output into variable
                StreamReader reader = proc.StandardOutput;
                string output = reader.ReadToEnd();
                output.StoreRawDataInUserVariable(sender, v_applyToVariableName);
         
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ScriptPath", this, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PowerShellArgs", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ReplaceScriptVariables", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyToVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Script Path: " + v_ScriptPath + "]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_ScriptPath))
            {
                this.validationResult += "Script is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}