using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command pauses the script for a set amount of time specified in milliseconds.")]
    [UsesDescription("Use this command when you want to pause your script for a specific amount of time.  After the specified time is finished, the script will resume execution.")]
    [ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class PauseScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Amount of time to pause for (in milliseconds)")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a specific amount of time in milliseconds (ex. to specify 8 seconds, one would enter 8000) or specify a variable containing a value.")]
        [SampleUsage("**8000** or **[vVariableWaitTime]**")]
        [Remarks("")]
        public string v_PauseLength { get; set; }

        public PauseScriptCommand()
        {
            CommandName = "PauseScriptCommand";
            SelectionName = "Pause Script";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var userPauseLength = v_PauseLength.ConvertToUserVariable(engine);
            var pauseLength = int.Parse(userPauseLength);
            System.Threading.Thread.Sleep(pauseLength);
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PauseLength", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Wait for " + v_PauseLength + "ms]";
        }
    }
}