using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run Batch Script File")]
    [Attributes.ClassAttributes.Description("This command allows you to run a script or program and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a script (such as vbScript, javascript, or executable) but wait for it to close before taskt continues executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_script))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RunBatchScriptFileCommand : ARunScriptFileCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Batch Script File Path")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.bat**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.vbs**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.js**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vScriptPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [Remarks("This command differs from **Start Application** because this command blocks execution until the script has completed. If you do not want to stop while the script executes, consider using **Start Application** instead.\nIf file does not contain extensin, supplement ps1 or bat extension.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "bat,vbs,js,wsh")]
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
        //public string v_Arguments { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Receive the Output")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(7000)]
        //public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }

        public RunBatchScriptFileCommand()
        {
            //this.CommandName = "RunScriptCommand";
            //this.SelectionName = "Run Script";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //string scriptPath = FilePathControls.FormatFilePath_NoFileCounter(v_ScriptPath, engine, new List<string>() { "bat", "vbs", "js", "wsf" }, true);
            //string scriptPath = FilePathControls.WaitForFile(this, nameof(v_TargetFilePath), nameof(v_WaitTimeForFile), engine);
            var scriptPath = this.WaitForFile(engine);

            string argments = "";
            if (!string.IsNullOrEmpty(v_Arguments))
            {
                argments = this.ExpandValueOrUserVariable(nameof(v_Arguments), "Arguments", engine);
            }

            var scriptProc = new System.Diagnostics.Process();
            scriptProc.StartInfo.FileName = scriptPath;

            if (!string.IsNullOrEmpty(argments))
            {
                scriptProc.StartInfo.Arguments = argments;
            }

            scriptProc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            scriptProc.StartInfo.UseShellExecute = false;
            scriptProc.StartInfo.RedirectStandardInput = false;
            scriptProc.StartInfo.RedirectStandardOutput = true;

            scriptProc.Start();
            string output = scriptProc.StandardOutput.ReadToEnd();
            scriptProc.WaitForExit();

            scriptProc.Close();

            if (!string.IsNullOrEmpty(v_Result))
            {
                output.StoreInUserVariable(engine, v_Result);
            }
        }
    }
}