using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Programs/Process Commands")]
    [Attributes.ClassAttributes.CommandSettings("Run Batch Script File")]
    [Attributes.ClassAttributes.Description("This command allows you to run a script or program and wait for it to exit before proceeding.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a script (such as vbScript, javascript, or executable) but wait for it to close before taskt continues executing.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.Start' and waits for the script/program to exit before proceeding.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RunBatchScriptFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Path to the Batch Script File")]
        [InputSpecification("Path", true)]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.bat**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.vbs**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.js**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vScriptPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [Remarks("This command differs from **Start Process** because this command blocks execution until the script has completed. If you do not want to stop while the script executes, consider using **Start Process** instead.\nIf file does not contain extensin, supplement ps1 or bat extension.\nIf file does not contain folder path, file will be opened in the same folder as script file.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Path")]
        public string v_ScriptPath { get; set; }

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

            string scriptPath = FilePathControls.formatFilePath_NoFileCounter(v_ScriptPath, engine, new List<string>() { "bat", "vbs", "js", "wsf" }, true);

            System.Diagnostics.Process scriptProc = new System.Diagnostics.Process();
            scriptProc.StartInfo.FileName = scriptPath;
            scriptProc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            scriptProc.Start();
            scriptProc.WaitForExit();

            scriptProc.Close();
        }
    }
}