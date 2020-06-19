using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Error Handling Commands")]
    [Description("This command defines a try/catch block which will execute the associated catch block if any " +
                 "exceptions are thrown.")]
    public class TryCommand : ScriptCommand
    {
        public TryCommand()
        {
            CommandName = "TryCommand";
            SelectionName = "Try";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            //get engine
            var engine = (AutomationEngineInstance) sender;

            //get indexes of commands
            var startIndex = 0;
            var startCatchIndex =
                parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is CatchExceptionCommand);
            var startFinallyIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is FinallyCommand);
            var endTryIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is EndTryCommand);

            for (var tryIndex = startIndex; tryIndex < startCatchIndex; tryIndex++)
            {
                if (engine.IsCancellationPending || engine.CurrentLoopCancelled)
                    return;

                try
                {
                    var cmd = parentCommand.AdditionalScriptCommands[tryIndex];
                    engine.ExecuteCommand(cmd);
                }
                catch (Exception ex)
                {
                    //error occured so start processing from catch index onwards
                    var catchCommandItem = parentCommand.AdditionalScriptCommands[startCatchIndex];
                    var catchCommand = (CatchExceptionCommand) catchCommandItem.ScriptCommand;

                    catchCommand.StackTrace = ex.ToString();
                    catchCommand.ErrorMessage = ex.Message;
                    engine.AddVariable("Catch:StackTrace", catchCommand.StackTrace);
                    engine.AddVariable("Catch:ErrorMessage", catchCommand.ErrorMessage);

                    var endCatch = startFinallyIndex != -1 ? startFinallyIndex : endTryIndex;

                    for (var catchIndex = startCatchIndex; catchIndex < endCatch; catchIndex++)
                    {
                        engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[catchIndex]);
                    }
                    break;
                }
            }

            //handle finally block if exists
            if (startFinallyIndex != -1)
            {
                for (var finallyIndex = startFinallyIndex; finallyIndex < endTryIndex; finallyIndex++)
                {
                    engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[finallyIndex]);
                }
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
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