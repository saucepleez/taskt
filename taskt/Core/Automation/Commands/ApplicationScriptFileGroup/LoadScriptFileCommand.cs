using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("taskt Script File")]
    [Attributes.ClassAttributes.CommandSettings("Load Script File")]
    [Attributes.ClassAttributes.Description("This command pre-loads tasks for future execution.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to load a task but not immediately execute it.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_script))]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class LoadScriptFileCommand : AScriptFileCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Script File Path. After, Use 'Run Script File' with the Same Path to Execute.")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.xml**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "xml")]
        public override string v_TargetFilePath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }

        public LoadScriptFileCommand()
        {
            //this.CommandName = "LoadTaskCommand";
            //this.SelectionName = "Load Task";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //string startFile = FilePathControls.FormatFilePath_NoFileCounter(v_taskPath, engine, "xml", true);
            //var scriptFile = FilePathControls.WaitForFile(this, nameof(v_TargetFilePath), nameof(v_WaitTimeForFile), engine);

            var scriptFile = this.WaitForFile(engine);

            var deserializedScript = Script.Script.DeserializeFile(scriptFile, engine.engineSettings);

            engine.PreloadedTasks.Add(scriptFile, deserializedScript);
        }
    }
}