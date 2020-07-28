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
        public override List<Control> Render(IfrmCommandEditor editor)
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