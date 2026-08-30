using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Engine;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop")]
    [Attributes.ClassAttributes.CommandSettings("Loop Number Of Times")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions several times (loop).  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a series of commands a specified amount of times.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]

    public sealed class BeginNumberOfTimesLoopCommand : ScriptCommand, IHaveLoopAdditionalCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("How Many Times to perform the Loop")]
        [InputSpecification("Enter the amount of times you would like to perform the encased commands.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**5**", PropertyDetailSampleUsage.ValueType.Value, "Loop Times")]
        [PropertyDetailSampleUsage("**{{{vNum}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Loop Times")]
        //[SampleUsage("**5** or **10** or **{{{vNum}}}**")]
        [Remarks("")]
        [PropertyValidationRule("Loop Times", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Loop", "Times")]
        public string v_LoopParameter { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Define Start Value")]
        [InputSpecification("Enter the Starting Value of the loop.")]
        //[SampleUsage("**0** or **1** or **{{{vStartValue}}}**")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**5**", PropertyDetailSampleUsage.ValueType.Value, "Start Value")]
        [PropertyDetailSampleUsage("**{{{vStart}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Start Value")]
        [PropertyIsOptional(true, "0")]
        [PropertyFirstValue("0")]
        [Remarks("If Start Value is **0** and Loop Times is **5**, it Loops **5** times.\nIf Start Value is **1** and Loop Times is **5**, it Loops **4** times.")]
        [PropertyDisplayText(true, "Start Value")]
        public string v_LoopStart { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Current Loop Times (First Time Value is 'Start Value + 1')")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Loop Current Times Variable", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Current Times")]
        public string v_CurrentTimes { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store the Number of Loops (First Time Value is 0)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Number of Loops", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Number of Loops")]
        public string v_NumberOfLoops { get; set; }

        public BeginNumberOfTimesLoopCommand()
        {
            //this.CommandName = "BeginNumberOfTimesLoopCommand";
            //this.SelectionName = "Loop Number Of Times";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_LoopStart = "0";
        }

        public override void RunCommand(AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            var loopCommand = (BeginNumberOfTimesLoopCommand)parentCommand.ScriptCommand;

            //if (!engine.VariableList.Any(f => f.VariableName == "Loop.CurrentIndex"))
            //{
            //    engine.VariableList.Add(new Script.ScriptVariable() { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
            //}

            //Script.ScriptVariable complexVarible = null;

            //int loopTimes;
            //var loopParameter = loopCommand.v_LoopParameter.ExpandValueOrUserVariable(engine);
            //loopTimes = int.Parse(loopParameter);

            var loopTimes = v_LoopParameter.ExpandValueOrUserVariableAsInteger("Loop Times", engine);

            //int startValue;
            //int.TryParse(v_LoopStart.ExpandValueOrUserVariable(engine), out startValue);

            if (string.IsNullOrEmpty(v_LoopStart))
            {
                v_LoopStart = "0";
            }
            var startValue = v_LoopStart.ExpandValueOrUserVariableAsInteger("Start Value", engine);

            Action<int> numbefOfLoopsAction;
            if (!string.IsNullOrEmpty(v_NumberOfLoops))
            {
                numbefOfLoopsAction = new Action<int>(num =>
                {
                    num.StoreInUserVariable(engine, v_NumberOfLoops);
                });
            }
            else
            {
                numbefOfLoopsAction = new Action<int>(num =>
                {
                    // nothing to to ;-)
                });
            }
            Action<int> loopCountAction;
            if (!string.IsNullOrEmpty(v_CurrentTimes))
            {
                loopCountAction = new Action<int>(num =>
                {
                    num.StoreInUserVariable(engine, v_CurrentTimes);
                    SystemVariables.Update_LoopCurrentIndex(num);
                });
            }
            else
            {
                loopCountAction = new Action<int>(num =>
                {
                    SystemVariables.Update_LoopCurrentIndex(num);
                });
            }

            int count = 0;  // number of loops
            for (int i = startValue; i < loopTimes; i++)
            {
                //if (complexVarible != null)
                //{
                //    complexVarible.CurrentPosition = i;
                //}

                // (i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");

                engine.ReportProgress($"Starting Loop Number {(i + 1)}/{loopTimes} From Line {loopCommand.LineNumber}");

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                    {
                        return;
                    }

                    //(i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");
                    //SystemVariables.Update_LoopCurrentIndex(i + 1);
                    numbefOfLoopsAction(count);
                    loopCountAction(i + 1);

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
                engine.ReportProgress($"Finished Loop From Line {loopCommand.LineNumber}");
                count++;
            }
        }

        //public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LoopParameter", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LoopStart", this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    if (v_LoopStart != "0")
        //    {
        //        return "Loop From (" + v_LoopStart + "+1) to " + v_LoopParameter;

        //    }
        //    else
        //    {
        //        return "Loop " +  v_LoopParameter + " Times";
        //    }
        //}

        //public override bool IsValidate(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (string.IsNullOrEmpty(this.v_LoopParameter))
        //    {
        //        this.validationResult += "Times is empty.\n";
        //    }
        //    else
        //    {
        //        if (int.TryParse(this.v_LoopParameter, out int v))
        //        {
        //            if (v < 0)
        //            {
        //                this.validationResult += "Specify a value of 0 or more for Times.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }

        //    return this.IsValid;
        //}
    }
}