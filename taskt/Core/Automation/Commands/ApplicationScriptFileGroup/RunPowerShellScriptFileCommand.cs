using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run PowerShell Script File")]
    [Attributes.ClassAttributes.Description("This command allows you to run a powershell script and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a powershell script and wait for it to close before taskt continues executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_script))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RunPowerShellScriptFileCommand : ARunScriptFileCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Powershell Script File Path")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.ps1**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vScriptPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [Remarks("This command differs from **Start Process** because this command blocks execution until the script has completed. If you do not want to stop while the script executes, consider using **Start Process** instead.\nIf file does not contain extensin, supplement ps1 or bat extension.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "ps1")]
        public override string v_TargetFilePath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Arguments")]
        //[InputSpecification("Arguments", true)]
        //[PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**1 2 3**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**{{{vArgs}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Arguments")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Arguments", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(6000)]
        //public override string v_Arguments { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Script Execution Method")]
        [PropertyUISelectionOption("EncodedCommand Base64")]
        [PropertyUISelectionOption("ExecutionPolicy Unrestricted")]
        [PropertyUISelectionOption("ExecutionPolicy Bypass")]
        [PropertyDetailSampleUsage("EncodedCommand Base64", "Encode the Script to Base64 and execute it by -EncodedCommand parameters. Arguments are sent to PowerShell, they cannot be retrieved by the Script. But you can expand the value of the taskt Variables in the Script.")]
        [PropertyDetailSampleUsage("ExecutionPolicy Unrestricted", "'-ExecutionPolicy Unrestricted' is set up in PowerShell and a Script is specified and executed by the -File parameter. Arguments are sent to the Script.")]
        [PropertyDetailSampleUsage("ExecutionPolicy Bypass", "'-ExecutionPolicy Bypass' is set up in PowerShell and a Script is specified and executed by the -File parameter. Arguments are sent to the Script.")]
        [PropertyIsOptional(true, "EncodedCommand Base64")]
        [PropertyValidationRule("Execution Method", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Execution Method")]
        [PropertySelectionChangeEvent(nameof(cmbExecutionMethod_SelectionChangeCommited))]
        [PropertyParameterOrder(7000)]
        public string v_ExecutionMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Expand taskt Variables In Script File")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [Remarks("This parameter is enabled when Execution Method is Base64")]
        [PropertyParameterOrder(7100)]
        public string v_ReplaceScriptVariables { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Receive the Output")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(8000)]
        //public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }

        public RunPowerShellScriptFileCommand()
        {
            //this.CommandName = "RunPowershellCommand";
            //this.SelectionName = "Run Powershell";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_PowerShellArgs = "-NoProfile -ExecutionPolicy unrestricted";
            //this.v_ReplaceScriptVariables = "No";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            // define script path
            var scriptPath = this.WaitForFile(engine);

            var arguments = this.ExpandValueOrUserVariable(nameof(v_Arguments), "Arguments", engine);

            //ProcessStartInfo startInfo;
            string sendArgs;
            switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ExecutionMethod), engine))
            {
                case "encodedcommand base64":
                    // get script text
                    var psCommand = File.ReadAllText(scriptPath);
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ReplaceScriptVariables), engine))
                    {
                        // convert variables
                        psCommand = psCommand.ExpandValueOrUserVariable(engine);
                    }

                    // convert ps script
                    var psCommandBytes = System.Text.Encoding.Unicode.GetBytes(psCommand);
                    var psCommandBase64 = Convert.ToBase64String(psCommandBytes);

                    if (!arguments.ToLower().Contains("-noprofile"))
                    {
                        arguments = $"-NoProfile {arguments}";
                    }

                    //// execute
                    //startInfo = new ProcessStartInfo()
                    //{
                    //    FileName = "powershell.exe",
                    //    Arguments = $"{arguments} -EncodedCommand {psCommandBase64}",
                    //    UseShellExecute = false,
                    //    RedirectStandardOutput = true
                    //};

                    sendArgs = $"{arguments} -EncodedCommand {psCommandBase64}";
                    break;

                case "executionpolicy unrestricted":
                    //startInfo = new ProcessStartInfo()
                    //{
                    //    FileName = "powershell.exe",
                    //    Arguments = $"-ExecutionPolicy Unrestricted -File \"{scriptPath}\" {arguments}",
                    //    UseShellExecute = false,
                    //    RedirectStandardOutput = true
                    //};
                    sendArgs = $"-ExecutionPolicy Unrestricted -File \"{scriptPath}\" {arguments}";
                    break;

                case "executionpolicy bypass":
                    //startInfo = new ProcessStartInfo()
                    //{
                    //    FileName = "powershell.exe",
                    //    Arguments = $"-ExecutionPolicy ByPass -File \"{scriptPath}\" {arguments}",
                    //    UseShellExecute = false,
                    //    RedirectStandardOutput = true
                    //};
                    sendArgs = $"-ExecutionPolicy ByPass -File \"{scriptPath}\" {arguments}";
                    break;

                default:
                    throw new Exception("bad execution method");    //
            }

            var startInfo = new ProcessStartInfo()
            {
                FileName = "powershell.exe",
                Arguments = sendArgs,
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var proc = System.Diagnostics.Process.Start(startInfo);

            // url: https://stackoverflow.com/questions/2285288/calling-a-ruby-script-in-c-sharp/12848337#12848337
            var reader = proc.StandardOutput;
            var output = reader.ReadToEnd();

            proc.WaitForExit();
            proc.Close();

            // store output into variable
            if (!string.IsNullOrEmpty(v_Result))
            {
                output.StoreRawDataInUserVariable(engine, v_Result);
            }
        }

        private void cmbExecutionMethod_SelectionChangeCommited(object sender, EventArgs e)
        {
            bool visible = true;
            switch (((ComboBox)sender).SelectedItem.ToString().ToLower())
            {
                case "executionpolicy unrestricted":
                case "executionpolicy bypass":
                    visible = false;
                    break;
            }
            FormUIControls.SetVisibleParameterControlGroup(this.ControlsList, nameof(v_ReplaceScriptVariables), visible);
        }
    }
}
