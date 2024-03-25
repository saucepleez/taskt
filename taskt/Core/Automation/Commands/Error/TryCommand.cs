﻿using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.CommandSettings("Try")]
    [Attributes.ClassAttributes.Description("This command allows embedding commands and will automatically move to the 'catch' handler")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to handle potential errors that could occur.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_try))]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class TryCommand : ScriptCommand
    {
        public TryCommand()
        {
            //this.CommandName = "TryCommand";
            //this.SelectionName = "Try";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            //get indexes of commands
            var startIndex = 0;
            var catchIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is CatchExceptionCommand);
            var finallyIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is FinallyCommand);
            var endTryIndex = parentCommand.AdditionalScriptCommands.FindIndex(a => a.ScriptCommand is EndTryCommand);
         
            var lineNumber = 0;
            for (int i = startIndex; i < catchIndex; i++)
            {
                if ((engine.IsCancellationPending) || (engine.CurrentLoopCancelled))
                    return;
                try
                {
                    Script.ScriptAction cmd = parentCommand.AdditionalScriptCommands[i];
                    lineNumber = cmd.ScriptCommand.LineNumber;
                    engine.ExecuteCommand(cmd);
                }
                catch (Exception ex)
                {
                    //error occured so start processing from catch index onwards
                    var catchCommandItem = parentCommand.AdditionalScriptCommands[catchIndex];
                    var catchCommand = (CatchExceptionCommand)catchCommandItem.ScriptCommand;

                    //catchCommand.StackTrace = ex.ToString();
                    //catchCommand.ErrorMessage = ex.Message;
                    //engine.AddVariable("Catch:StackTrace", catchCommand.StackTrace);
                    //engine.AddVariable("Catch:ErrorMessage", catchCommand.ErrorMessage);
                    SystemVariables.Update_ErrorCatch(catchCommand, ex);

                    //assify = (input >= 0) ? "nonnegative" : "negative";
                    var endCatch = (finallyIndex != -1) ? finallyIndex : endTryIndex;
                
                    for (int j = catchIndex; j < endCatch; j++)
                    {
                        engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[j]);
                    }
                    break;

                }
             
            }
            //handle finally block if exists
            if (finallyIndex != -1)
            {
                for (int k = finallyIndex; k < endTryIndex; k++)
                {
                    engine.ExecuteCommand(parentCommand.AdditionalScriptCommands[k]);
                }
            }
        }
    }
}