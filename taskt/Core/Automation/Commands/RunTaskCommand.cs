using System;
using System.Xml.Serialization;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Task Commands")]
    [Attributes.ClassAttributes.Description("This command runs tasks.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run another task.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class RunTaskCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select a Task")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter or Select the valid path to the file.")]
        [Attributes.PropertyAttributes.SampleUsage("c:\\temp\\mytask.xml or [vScriptPath]")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_taskPath { get; set; }

        public RunTaskCommand()
        {
            this.CommandName = "RunTaskCommand";
            this.SelectionName = "Run Task";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            //get assembly reference
            var assembly = System.Reflection.Assembly.GetEntryAssembly().Location;

            //get variable
            var startFile = v_taskPath.ConvertToUserVariable(sender);
            startFile = @"""" + startFile + @"""";

            //start process
            var p = System.Diagnostics.Process.Start(assembly, startFile);

            //wait for exit
            p.WaitForExit();

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [" + v_taskPath + "]";
        }
    }
}