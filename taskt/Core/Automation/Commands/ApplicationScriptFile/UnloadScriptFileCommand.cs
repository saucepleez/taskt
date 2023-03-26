using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script Commands")]
    [Attributes.ClassAttributes.SubGruop("taskt Script File")]
    [Attributes.ClassAttributes.CommandSettings("Unload Script File")]
    [Attributes.ClassAttributes.Description("This command runs tasks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run another task.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UnloadScriptFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Script File to Pre-Load. Use 'Run Script File' with the same path to execute.")]
        [InputSpecification("Script File", true)]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.xml**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Script File", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Scrpit File")]
        public string v_taskPath { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Unload Error Preference")]
        [PropertyUISelectionOption("Error if not found")]
        [PropertyUISelectionOption("Continue if not found")]
        [Remarks("Selecting this field changes the parameters that will be required in the next step")]
        [PropertyIsOptional(true, "Continue if not found")]
        [PropertyDisplayText(false, "")]
        public string v_ErrorPreference { get; set; }

        public UnloadScriptFileCommand()
        {
            //this.CommandName = "UnloadTaskCommand";
            //this.SelectionName = "Unload Task";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_ErrorPreference = "Continue if not found";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string startFile = FilePathControls.formatFilePath_NoFileCounter(v_taskPath, engine, "xml", true);

            var errorPreference = this.GetUISelectionValue(nameof(v_ErrorPreference), engine);

            if (engine.PreloadedTasks.ContainsKey(startFile))
            {
                engine.PreloadedTasks.Remove(startFile);
            }
            else if (errorPreference == "error if not found")
            {
                throw new Exception($"The task {startFile} was not loaded.  Throwing error due to selected preference.");
            }
        }
    }
}