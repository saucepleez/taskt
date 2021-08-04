using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Engine;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.Description("This command allows embedding commands and will automatically move to the 'catch' handler")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to handle potential errors that could occur.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class TryCommand : ScriptCommand
    {
        public TryCommand()
        {
            this.CommandName = "TryCommand";
            this.SelectionName = "Try";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender, Script.ScriptAction parentCommand)
        {
           //get engine
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

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

                    catchCommand.StackTrace = ex.ToString();
                    catchCommand.ErrorMessage = ex.Message;
                    engine.AddVariable("Catch:StackTrace", catchCommand.StackTrace);
                    engine.AddVariable("Catch:ErrorMessage", catchCommand.ErrorMessage);

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