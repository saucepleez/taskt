using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script Commands")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run Batch Script File")]
    [Attributes.ClassAttributes.Description("This command allows you to run a script or program and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a script (such as vbScript, javascript, or executable) but wait for it to close before taskt continues executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RunBatchScriptFileCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Path to the Batch Script File")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.bat**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.vbs**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.js**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vScriptPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [Remarks("This command differs from **Start Application** because this command blocks execution until the script has completed. If you do not want to stop while the script executes, consider using **Start Application** instead.\nIf file does not contain extensin, supplement ps1 or bat extension.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "bat,vbs,js,wsh")]
        public string v_ScriptPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitForFile { get; set; }

        public RunBatchScriptFileCommand()
        {
            //this.CommandName = "RunScriptCommand";
            //this.SelectionName = "Run Script";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //string scriptPath = FilePathControls.FormatFilePath_NoFileCounter(v_ScriptPath, engine, new List<string>() { "bat", "vbs", "js", "wsf" }, true);
            string scriptPath = FilePathControls.WaitForFile(this, nameof(v_ScriptPath), nameof(v_WaitForFile), engine);

            System.Diagnostics.Process scriptProc = new System.Diagnostics.Process();
            scriptProc.StartInfo.FileName = scriptPath;
            scriptProc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            scriptProc.Start();
            scriptProc.WaitForExit();

            scriptProc.Close();
        }
    }
}