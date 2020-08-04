using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Switch Commands")]
    [Description("This command specifies the end of a switch block.")]
    public class EndSwitchCommand : ScriptCommand
    {
        public EndSwitchCommand()
        {
            CommandName = "EndSwitchCommand";
            SelectionName = "End Switch";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //no execution required, used as a marker by the Automation Engine
        }

        public override List<Control> Render(IfrmCommandEditor editor)
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
