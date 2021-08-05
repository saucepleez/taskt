using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command enables user to break and exit from the current loop")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to break from the current loop")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class NextLoopCommand : ScriptCommand
    {
        public NextLoopCommand()
        {
            this.DefaultPause = 0;
            this.CommandName = "NextLoopCommand";
            this.SelectionName = "Next Loop";
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
            return base.GetDisplayValue();
        }
    }
}