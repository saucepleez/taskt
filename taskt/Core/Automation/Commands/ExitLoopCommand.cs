using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command signifies the current loop should exit and resume work past the point of the current loop.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to signify that looping should end and commands outside the loop should resume execution.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is used by the engine to exit a loop")]
    public class ExitLoopCommand : ScriptCommand
    {
        public ExitLoopCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "ExitLoopCommand";
            this.SelectionName = "Exit Loop";
            this.CommandEnabled = true;
            this.CustomRendering = true;
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