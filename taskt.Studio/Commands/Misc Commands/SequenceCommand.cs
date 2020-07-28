using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{

    [Serializable]
    [Group("Misc Commands")]
    [Description("Command that groups multiple actions")]
    [UsesDescription("Use this command when you want to group multiple commands together.")]
    [ImplementationDescription("This command implements many commands in a list.")]
    public class SequenceCommand : ScriptCommand
    {
        public List<ScriptCommand> v_scriptActions = new List<ScriptCommand>();


        public SequenceCommand()
        {
            CommandName = "SequenceCommand";
            SelectionName = "Sequence Command";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {

            var engine = (AutomationEngineInstance)sender;

            foreach (var item in v_scriptActions)
            {

                //exit if cancellation pending
                if (engine.IsCancellationPending)
                {
                    return;
                }

                //only run if not commented
                if (!item.IsCommented)
                    item.RunCommand(sender);



            }

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
            return base.GetDisplayValue() + " [" + v_scriptActions.Count() + " embedded commands]";
        }
    }
}