using System;

namespace taskt.Core.Automation.Commands
{




    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions continuously.  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a series of commands an endless amount of times.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    public class BeginContinousLoopCommand : ScriptCommand
    {

        public BeginContinousLoopCommand()
        {
            this.CommandName = "BeginContinousLoopCommand";
            this.SelectionName = "Loop Continuously";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {
            Core.Automation.Commands.BeginContinousLoopCommand loopCommand = (Core.Automation.Commands.BeginContinousLoopCommand)parentCommand.ScriptCommand;

            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;


            engine.ReportProgress("Starting Continous Loop From Line " + loopCommand.LineNumber);

            while (true)
            {


                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopCancelled = false;
                        return;
                    }
                }
            }
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}