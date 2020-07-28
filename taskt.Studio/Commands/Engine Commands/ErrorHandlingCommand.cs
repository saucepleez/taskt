using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command specifies what to do  after an error is encountered.")]
    [UsesDescription("Use this command when you want to define how your script should behave when an error is encountered.")]
    [ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class ErrorHandlingCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Select an action to take in the event an error occurs")]
        [PropertyUISelectionOption("Stop Processing")]
        [PropertyUISelectionOption("Continue Processing")]
        [InputSpecification("Select the action you want to take when you come across an error.")]
        [SampleUsage("**Stop Processing** to end the script if an error is encountered or **Continue Processing** to continue running the script")]
        [Remarks("**If Command** allows you to specify and test if a line number encountered an error. In order to use that functionality, you must specify **Continue Processing**")]
        public string v_ErrorHandlingAction { get; set; }

        public ErrorHandlingCommand()
        {
            CommandName = "ErrorHandlingCommand";
            SelectionName = "Error Handling";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            engine.ErrorHandlingAction = v_ErrorHandlingAction;
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);


            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_ErrorHandlingAction", this));
            var dropdown = CommandControls.CreateDropdownFor("v_ErrorHandlingAction", this);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_ErrorHandlingAction", this, new Control[] { dropdown }, editor));
            RenderedControls.Add(dropdown);

            return RenderedControls;
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Action: " + v_ErrorHandlingAction + "]";
        }
    }
}