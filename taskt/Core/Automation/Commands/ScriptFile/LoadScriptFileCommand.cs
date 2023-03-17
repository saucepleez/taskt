using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Script File Commands")]
    [Attributes.ClassAttributes.CommandSettings("Load Script File")]
    [Attributes.ClassAttributes.Description("This command pre-loads tasks for future execution.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to load a task but not immediately execute it.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class LoadScriptFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Script File. After, Use 'Run Task' with the Same Path to Execute.")]
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

            //var startFile = v_taskPath.ConvertToUserVariable(sender);
            string startFile = FilePathControls.formatFilePath_NoFileCounter(v_taskPath, engine, "xml", true);
            
            Script.Script deserializedScript = Script.Script.DeserializeFile(startFile, engine.engineSettings);

            engine.PreloadedTasks.Add(startFile, deserializedScript);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create file path and helpers
        //    //RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_taskPath", this));
        //    //var taskPathControl = UI.CustomControls.CommandControls.CreateDefaultInputFor("v_taskPath", this);
        //    //RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_taskPath", this, new Control[] { taskPathControl }, editor));
        //    //RenderedControls.Add(taskPathControl);

        //    var ctrls = UI.CustomControls.CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;
        //}

      
        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [" + v_taskPath + "]";
        //}
    }
}