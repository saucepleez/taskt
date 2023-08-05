using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script Commands")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run PowerShell Script File")]
    [Attributes.ClassAttributes.Description("This command allows you to run a powershell script and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a powershell script and wait for it to close before taskt continues executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RunPowerShellScriptFileCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Path to the Powershell Script File")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.ps1**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vScriptPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [Remarks("This command differs from **Start Process** because this command blocks execution until the script has completed. If you do not want to stop while the script executes, consider using **Start Process** instead.\nIf file does not contain extensin, supplement ps1 or bat extension.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "ps1")]
        public string v_ScriptPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Arguments")]
        [InputSpecification("Arguments", true)]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        [PropertyDetailSampleUsage("**1 2 3**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        [PropertyDetailSampleUsage("**{{{vArgs}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Arguments")]
        [PropertyIsOptional(true)]
        [PropertyFirstValue("-NoProfile -ExecutionPolicy unrestricted")]
        [PropertyValidationRule("Arguments", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_PowerShellArgs { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Convert Variables before Execution")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        public string v_ReplaceScriptVariables { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Receive the Output")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        public string v_applyToVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitForFile { get; set; }

        public RunPowerShellScriptFileCommand()
        {
            //this.CommandName = "RunPowershellCommand";
            //this.SelectionName = "Run Powershell";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_PowerShellArgs = "-NoProfile -ExecutionPolicy unrestricted";
            //this.v_ReplaceScriptVariables = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //define script path
            //string scriptPath = FilePathControls.FormatFilePath_NoFileCounter(v_ScriptPath, engine, new List<string>() { "ps1", "bat" }, true);
            var scriptPath = FilePathControls.WaitForFile(this, nameof(v_ScriptPath), nameof(v_WaitForFile), engine);

            //get script text
            var psCommand = File.ReadAllText(scriptPath);

            //if (v_ReplaceScriptVariables.ToUpperInvariant() == "YES")
            if (this.GetUISelectionValue(nameof(v_ReplaceScriptVariables), engine) == "yes")
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
            
            if (!String.IsNullOrEmpty(v_applyToVariableName))
            {
                string output = reader.ReadToEnd();
                output.StoreRawDataInUserVariable(sender, v_applyToVariableName);
            }
        }
    }
}