using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Task Commands")]
    [Attributes.ClassAttributes.Description("This command pre-loads tasks for future execution.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to load a task but not immediately execute it.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class LoadTaskCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select a task to load. After, use 'Run Task' with the same path to execute.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the valid path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("c:\\temp\\mytask.xml or [vScriptPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_taskPath { get; set; }

        public LoadTaskCommand()
        {
            this.CommandName = "LoadTaskCommand";
            this.SelectionName = "Load Task";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            
            //deserialize task
            Core.Automation.Engine.AutomationEngineInstance currentScriptEngine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var startFile = v_taskPath.ConvertToUserVariable(sender);
            Script.Script deserializedScript = Core.Script.Script.DeserializeFile(startFile);

            currentScriptEngine.PreloadedTasks.Add(startFile, deserializedScript);

        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create file path and helpers
            RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_taskPath", this));
            var taskPathControl = UI.CustomControls.CommandControls.CreateDefaultInputFor("v_taskPath", this);
            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_taskPath", this, new Control[] { taskPathControl }, editor));
            RenderedControls.Add(taskPathControl);

            return RenderedControls;
        }

      
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_taskPath + "]";
        }
    }
}