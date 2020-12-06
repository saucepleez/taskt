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
    [Attributes.ClassAttributes.Description("This command runs tasks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run another task.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class UnloadTaskCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select a Task to Pre-Load.  Use Run Task with the same path to execute.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the valid path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("c:\\temp\\mytask.xml or [vScriptPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_taskPath { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Unload Error Preference")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Error if not found")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Continue if not found")]
        [Attributes.PropertyAttributes.InputSpecification("Select the appropriate corresponding action to take once the element has been located")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **Invoke Click**, **Left Click**, **Right Click**, **Middle Click**, **Double Left Click**, **Clear Element**, **Set Text**, **Get Text**, **Get Attribute**, **Wait For Element To Exist**, **Get Count**")]
        [Attributes.PropertyAttributes.Remarks("Selecting this field changes the parameters that will be required in the next step")]
        public string v_ErrorPreference { get; set; }

        public UnloadTaskCommand()
        {
            this.CommandName = "UnloadTaskCommand";
            this.SelectionName = "Unload Task";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_ErrorPreference = "Continue if not found";
        }

        public override void RunCommand(object sender)
        {
            
            //deserialize task
            Core.Automation.Engine.AutomationEngineInstance currentScriptEngine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var startFile = v_taskPath.ConvertToUserVariable(sender);
            var errorPreference = v_ErrorPreference.ConvertToUserVariable(sender).ToUpperInvariant();

            if (currentScriptEngine.PreloadedTasks.ContainsKey(startFile))
            {
                currentScriptEngine.PreloadedTasks.Remove(startFile);
            }
            else if (errorPreference == "ERROR IF NOT FOUND")
            {
                throw new Exception($"The task {startFile} was not loaded.  Throwing error due to selected preference.");
            }


        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create file path and helpers
            RenderedControls.Add(UI.CustomControls.CommandControls.CreateDefaultLabelFor("v_taskPath", this));
            var taskPathControl = UI.CustomControls.CommandControls.CreateDefaultInputFor("v_taskPath", this);
            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateUIHelpersFor("v_taskPath", this, new Control[] { taskPathControl }, editor));
            RenderedControls.Add(taskPathControl);

            RenderedControls.AddRange(UI.CustomControls.CommandControls.CreateDefaultDropdownGroupFor("v_ErrorPreference", this, editor));

            return RenderedControls;
        }

      
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_taskPath + "]";
        }
    }
}