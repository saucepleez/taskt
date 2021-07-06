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
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to show a message to the user.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class ShowEngineContextCommand : ScriptCommand
    {

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Close After X (Seconds) - 0 to bypass")]
        [Attributes.PropertyAttributes.InputSpecification("Specify how many seconds to display on screen. After the amount of seconds passes, the message box will be automatically closed and script will resume execution.")]
        [Attributes.PropertyAttributes.SampleUsage("**0** to remain open indefinitely or **5** to stay open for 5 seconds.")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_AutoCloseAfter { get; set; }
        public ShowEngineContextCommand()
        {
            this.CommandName = "ShowEngineContextCommand";
            this.SelectionName = "Show Engine Context";
            this.CommandEnabled = true;
            this.v_AutoCloseAfter = "0";
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;


            if (engine.tasktEngineUI == null)
            {           
                return;
            }

            v_AutoCloseAfter = v_AutoCloseAfter.ConvertToUserVariable(sender);
            int closeValue;
            if (!int.TryParse(v_AutoCloseAfter, out closeValue))
            {
                closeValue = 10;
            }

            //automatically close messageboxes for server requests
            if (engine.serverExecution && closeValue <= 0)
            {
                closeValue = 10;
            }

            var result = engine.tasktEngineUI.Invoke(new Action(() =>
                {
                    engine.tasktEngineUI.ShowEngineContext(engine.GetEngineContext(), closeValue);
                }
            ));

        }
        public override List<Control> Render(frmCommandEditor editor)
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

        public override bool IsValidate(frmCommandEditor editor)
        {
            this.IsValid = true;
            this.validationResult = "";

            if (String.IsNullOrEmpty(v_AutoCloseAfter))
            {
                this.validationResult += "Close time is empty.\n";
                this.IsValid = true;
            }
            else
            {
                int v;
                if (!int.TryParse(v_AutoCloseAfter, out v))
                {
                    if (v < 0)
                    {
                        this.validationResult += "Close time less than zero.\n";
                        this.IsValid = true;
                    }
                }
            }

            return this.IsValid;
        }

    }
}
