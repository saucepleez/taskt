using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Loop")]
    [Attributes.ClassAttributes.CommandSettings("Loop Continuously")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions continuously.  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a series of commands an endless amount of times.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class BeginContinousLoopCommand : ScriptCommand, IHaveLoopAdditionalCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store the Number of Loops (First Time Value is 0)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Number of Loops", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Number of Loops")]
        public string v_NumberOfLoops { get; set; }

        public BeginContinousLoopCommand()
        {
            //this.CommandName = "BeginContinousLoopCommand";
            //this.SelectionName = "Loop Continuously";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            var loopCommand = (BeginContinousLoopCommand)parentCommand.ScriptCommand;

            Action<decimal> loopTimesAction;
            if (!string.IsNullOrEmpty(v_NumberOfLoops))
            {
                loopTimesAction = new Action<decimal>(num =>
                {
                    num.StoreInUserVariable(engine, v_NumberOfLoops);
                    SystemVariables.Update_LoopCurrentIndex(num);
                });
            }
            else
            {
                loopTimesAction = new Action<decimal>(num =>
                {
                    SystemVariables.Update_LoopCurrentIndex(num);
                });
            }

            engine.ReportProgress($"Starting Continous Loop From Line {loopCommand.LineNumber}");

            decimal count = 0;
            while (true)
            {
                //loopTimesAction(count);

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                    {
                        return;
                    }

                    loopTimesAction(count); // update loop count

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress($"Exiting Loop From Line {loopCommand.LineNumber}");
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress($"Continuing Next Loop From Line {loopCommand.LineNumber}");
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }

                try
                {
                    count++;
                }
                catch
                {
                    count = decimal.MinValue;
                }
            }
        }

        //public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_Comment", this));
        //    RenderedControls.Add(CommandControls.CreateDefaultInputFor("v_Comment", this, 100, 300));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue();
        //}
    }
}
