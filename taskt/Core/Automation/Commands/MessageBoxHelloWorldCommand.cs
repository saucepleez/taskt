using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to show a message hello world.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user?")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data?")]
    public class MessageBoxHelloWorldCommand : ScriptCommand
    {
        [XmlIgnore]
        [NonSerialized]
        public int v_A;

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Close After X (Seconds) - 0 to bypass")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Specify how many seconds to display on screen. After the amount of seconds passes, the message box will be automatically closed and script will resume execution.")]
        [Attributes.PropertyAttributes.SampleUsage("**0** to remain open indefinitely or **5** to stay open for 5 seconds.")]
        [Attributes.PropertyAttributes.Remarks("")]

        public int v_AutoCloseAfter { get; set; }

 




        public MessageBoxHelloWorldCommand()
        {
            this.CommandName = "MessageBoxCommand";
            this.SelectionName = "Show Message Hello World";
            this.CommandEnabled = true;
            this.v_AutoCloseAfter = 0;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
           // string variableMessage = v_Message.ConvertToUserVariable(sender);

           // variableMessage = variableMessage.Replace("\\n", Environment.NewLine);

            if (engine.tasktEngineUI == null)
            {
                engine.ReportProgress("Complex Messagebox Supported With UI Only");
                System.Windows.Forms.MessageBox.Show("Hello World", "Message Box Command", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                return;
            }

            //automatically close messageboxes for server requests
            if (engine.serverExecution && v_AutoCloseAfter <= 0)
            {
                v_AutoCloseAfter = 10;
            }

            var result = engine.tasktEngineUI.Invoke(new Action(() =>
            {
                engine.tasktEngineUI.ShowMessage("Hello World", "MessageBox Command", UI.Forms.Supplemental.frmDialog.DialogType.OkOnly, v_AutoCloseAfter);
            }

            ));

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create message controls
            //var messageControlSet = CommandControls.CreateDefaultInputGroupFor("v_Message", this, editor);
            //RenderedControls.AddRange(messageControlSet);


            //create auto close control set
            var autocloseControlSet = CommandControls.CreateDefaultInputGroupFor("v_AutoCloseAfter", this, editor);
            RenderedControls.AddRange(autocloseControlSet);


            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Message: Hello world]";
        }

        //controls can be overriden and rendered individually
        public System.Windows.Forms.TextBox v_MessageControl()
        {
            var Textbox = new TextBox();
            Textbox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            //Textbox.DataBindings.Add("Text", this, "v_Message", false, DataSourceUpdateMode.OnPropertyChanged);
            Textbox.Height = 30;
            Textbox.Width = 300;

            return Textbox;
        }
    }
}
