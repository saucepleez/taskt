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
    [Description("This command signifies that the current loop should exit and resume execution outside the current loop.")]
    public class ExitLoopCommand : ScriptCommand
    {
        public ExitLoopCommand()
        {
            DefaultPause = 0;
            CommandName = "ExitLoopCommand";
            SelectionName = "Exit Loop";
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
            return "Exit Loop";
        }
    }
}