using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script Commands")]
    [Attributes.ClassAttributes.SubGruop("taskt Script File")]
    [Attributes.ClassAttributes.CommandSettings("Load Script File")]
    [Attributes.ClassAttributes.Description("This command pre-loads tasks for future execution.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to load a task but not immediately execute it.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class LoadScriptFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Path to the Script File. After, Use 'Run Script File' with the Same Path to Execute.")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.xml**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "xml")]
        public string v_taskPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitForFile { get; set; }

        public LoadScriptFileCommand()
        {
            //this.CommandName = "LoadTaskCommand";
            //this.SelectionName = "Load Task";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //deserialize task
            var engine = (Engine.AutomationEngineInstance)sender;

            //string startFile = FilePathControls.FormatFilePath_NoFileCounter(v_taskPath, engine, "xml", true);
            var startFile = FilePathControls.WaitForFile(this, nameof(v_taskPath), nameof(v_WaitForFile), engine);
            
            Script.Script deserializedScript = Script.Script.DeserializeFile(startFile, engine.engineSettings);

            engine.PreloadedTasks.Add(startFile, deserializedScript);
        }
    }
}