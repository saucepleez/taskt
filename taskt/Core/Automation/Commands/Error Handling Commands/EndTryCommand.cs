using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Error Handling Commands")]
    [Description("This command specifies the end of a try/catch block.")]
    public class EndTryCommand : ScriptCommand
    {
        public EndTryCommand()
        {
            CommandName = "EndTryCommand";
            SelectionName = "End Try";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //no execution required, used as a marker by the Automation Engine
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Comment", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}