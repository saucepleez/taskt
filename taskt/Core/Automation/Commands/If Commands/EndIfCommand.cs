using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.ClassAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("If Commands")]
    [Description("This command signifies the exit point of If action(s) and is required for all the Begin If commands.")]
    public class EndIfCommand : ScriptCommand
    {
        public EndIfCommand()
        {
            DefaultPause = 0;
            CommandName = "EndIfCommand";
            SelectionName = "End If";
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
            return "End If";
        }
    }
}