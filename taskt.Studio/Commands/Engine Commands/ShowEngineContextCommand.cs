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
using taskt.UI.Forms;

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command allows you to show a message to the user.")]
    [UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class ShowEngineContextCommand : ScriptCommand
    {

        [XmlAttribute]
        [PropertyDescription("Close After X (Seconds) - 0 to bypass")]
        [InputSpecification("Specify how many seconds to display on screen. After the amount of seconds passes, the message box will be automatically closed and script will resume execution.")]
        [SampleUsage("**0** to remain open indefinitely or **5** to stay open for 5 seconds.")]
        [Remarks("")]
        public int v_AutoCloseAfter { get; set; }
        public ShowEngineContextCommand()
        {
            CommandName = "ShowEngineContextCommand";
            SelectionName = "Show Engine Context";
            CommandEnabled = true;
            v_AutoCloseAfter = 0;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;


            if (engine.TasktEngineUI == null)
            {           
                return;
            }

            //automatically close messageboxes for server requests
            if (engine.ServerExecution && v_AutoCloseAfter <= 0)
            {
                v_AutoCloseAfter = 10;
            }

            var result = ((frmScriptEngine)engine.TasktEngineUI).Invoke(new Action(() =>
            {
                engine.TasktEngineUI.ShowEngineContext(engine.GetEngineContext(), v_AutoCloseAfter);
            }

            ));

        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);


            //create auto close control set
            var autocloseControlSet = CommandControls.CreateDefaultInputGroupFor("v_AutoCloseAfter", this, editor);
            RenderedControls.AddRange(autocloseControlSet);


            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }

    }
}
