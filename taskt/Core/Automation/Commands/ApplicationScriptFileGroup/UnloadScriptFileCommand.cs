using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("taskt Script File")]
    [Attributes.ClassAttributes.CommandSettings("Unload Script File")]
    [Attributes.ClassAttributes.Description("This command runs tasks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run another task.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_stop_process))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UnloadScriptFileCommand : AScriptFileCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Script File Path to Pre-Load. Use 'Run Script File' with the same path to execute.")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.xml**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "xml")]
        public override string v_TargetFilePath { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Unload Error Preference")]
        [PropertyUISelectionOption("Error if not found")]
        [PropertyUISelectionOption("Continue if not found")]
        [Remarks("Selecting this field changes the parameters that will be required in the next step")]
        [PropertyIsOptional(true, "Continue if not found")]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(6000)]
        public string v_ErrorPreference { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }

        public UnloadScriptFileCommand()
        {
            //this.CommandName = "UnloadTaskCommand";
            //this.SelectionName = "Unload Task";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_ErrorPreference = "Continue if not found";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //string startFile = FilePathControls.FormatFilePath_NoFileCounter(v_taskPath, engine, "xml", true);
            // startFile = FilePathControls.WaitForFile(this, nameof(v_TargetFilePath), nameof(v_WaitTimeForFile), engine);
            var scriptFile = this.WaitForFile(engine);

            var errorPreference = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ErrorPreference), engine);

            if (engine.PreloadedTasks.ContainsKey(scriptFile))
            {
                engine.PreloadedTasks.Remove(scriptFile);
            }
            else if (errorPreference == "error if not found")
            {
                throw new Exception($"The task {scriptFile} was not loaded.  Throwing error due to selected preference.");
            }
        }
    }
}