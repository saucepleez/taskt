using System;
using System.Collections.Generic;
using System.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Sequence")]
    [Attributes.ClassAttributes.Description("Command that groups multiple actions")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to group multiple commands together.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements many commands in a list.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    public class SequenceCommand : ScriptCommand
    {
        public List<ScriptCommand> v_scriptActions = new List<ScriptCommand>();

        public SequenceCommand()
        {
            //this.CommandName = "SequenceCommand";
            //this.SelectionName = "Sequence Command";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            foreach (var item in v_scriptActions)
            {
                //exit if cancellation pending
                if (engine.IsCancellationPending)
                {
                    return;
                }

                //only run if not commented
                if (!item.IsCommented)
                {
                    item.RunCommand(engine);
                }
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [ " + v_scriptActions.Count() + " embedded commands ]";
        }
    }
}