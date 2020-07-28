using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command allows you to show a message to the user.")]
    [UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class ShowMessageCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Enter the message to be displayed.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Specify any text that should be displayed on screen.  You may also include variables for display purposes.")]
        [SampleUsage("**Hello World** or **[vMyText]**")]
        [Remarks("")]
        public string v_Message { get; set; }

        [XmlAttribute]
        [PropertyDescription("Close After X (Seconds) - 0 to bypass")]
        [InputSpecification("Specify how many seconds to display on screen. After the amount of seconds passes, the message box will be automatically closed and script will resume execution.")]
        [SampleUsage("**0** to remain open indefinitely or **5** to stay open for 5 seconds.")]
        [Remarks("")]
        public int v_AutoCloseAfter { get; set; }
        public ShowMessageCommand()
        {
            CommandName = "MessageBoxCommand";
            SelectionName = "Show Message";
            CommandEnabled = true;
            v_AutoCloseAfter = 0;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;
            string variableMessage = v_Message.ConvertToUserVariable(engine);

            variableMessage = variableMessage.Replace("\\n", Environment.NewLine);

            if (engine.TasktEngineUI == null)
            {
                engine.ReportProgress("Complex Messagebox Supported With UI Only");
                MessageBox.Show(variableMessage, "Message Box Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //automatically close messageboxes for server requests
            if (engine.ServerExecution && v_AutoCloseAfter <= 0)
            {
                v_AutoCloseAfter = 10;
            }

            var result = ((frmScriptEngine)engine.TasktEngineUI).Invoke(new Action(() =>
                {
                    engine.TasktEngineUI.ShowMessage(variableMessage, "MessageBox Command", DialogType.OkOnly, v_AutoCloseAfter);
                }
            ));

        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create message controls
            var messageControlSet = CommandControls.CreateDefaultInputGroupFor("v_Message", this, editor);
            RenderedControls.AddRange(messageControlSet);


            //create auto close control set
            var autocloseControlSet = CommandControls.CreateDefaultInputGroupFor("v_AutoCloseAfter", this, editor);
            RenderedControls.AddRange(autocloseControlSet);


            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Message: " + v_Message + "]";
        }

        //controls can be overriden and rendered individually
        public TextBox v_MessageControl()
        {
            var Textbox = new TextBox();
            Textbox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            Textbox.DataBindings.Add("Text", this, "v_Message", false, DataSourceUpdateMode.OnPropertyChanged);
            Textbox.Height = 30;
            Textbox.Width = 300;

            return Textbox;
        }
    }
}
