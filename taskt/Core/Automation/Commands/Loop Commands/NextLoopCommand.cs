using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Loop Commands")]
    [Description("This command enables user to break and exit from the current loop.")]
    public class NextLoopCommand : ScriptCommand
    {
        public NextLoopCommand()
        {
            DefaultPause = 0;
            CommandName = "NextLoopCommand";
            SelectionName = "Next Loop";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_Comment", this));
            RenderedControls.Add(CommandControls.CreateDefaultInputFor("v_Comment", this, 100, 300));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}