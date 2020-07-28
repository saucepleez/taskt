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
    [Description("This command declares the seperation between the actions based on the 'true' or 'false' condition.")]
    public class ElseCommand : ScriptCommand
    {
        public ElseCommand()
        {
            DefaultPause = 0;
            CommandName = "ElseCommand";
            SelectionName = "Else";
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
            return "Else";
        }
    }
}