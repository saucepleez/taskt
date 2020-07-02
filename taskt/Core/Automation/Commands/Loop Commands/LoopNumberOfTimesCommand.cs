using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Loop Commands")]
    [Description("This command repeats the subsequent actions a specified number of times.")]
    public class LoopNumberOfTimesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Loop Count")]
        [InputSpecification("Enter the amount of times you would like to execute the encased commands.")]
        [SampleUsage("5 || {vLoopCount}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LoopParameter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Start Index")]
        [InputSpecification("Enter the starting index of the loop.")]
        [SampleUsage("5 || {vStartIndex}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LoopStart { get; set; }

        public LoopNumberOfTimesCommand()
        {
            CommandName = "LoopNumberOfTimesCommand";
            SelectionName = "Loop Number Of Times";
            CommandEnabled = true;
            CustomRendering = true;
            v_LoopStart = "0";
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            LoopNumberOfTimesCommand loopCommand = (LoopNumberOfTimesCommand)parentCommand.ScriptCommand;
            var engine = (AutomationEngineInstance)sender;

            if (!engine.VariableList.Any(f => f.VariableName == "Loop.CurrentIndex"))
            {
                engine.VariableList.Add(new ScriptVariable() { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
            }

            int loopTimes;
            ScriptVariable complexVarible = null;

            var loopParameter = loopCommand.v_LoopParameter.ConvertToUserVariable(sender);
            loopTimes = int.Parse(loopParameter);

            int startIndex = 0;
            int.TryParse(v_LoopStart.ConvertToUserVariable(sender), out startIndex);

            for (int i = startIndex; i < loopTimes; i++)
            {
                if (complexVarible != null)
                    complexVarible.CurrentPosition = i;

              //  (i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");

                engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    (i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress("Continuing Next Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }
                engine.ReportProgress("Finished Loop From Line " + loopCommand.LineNumber);
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LoopParameter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LoopStart", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            if (v_LoopStart != "0")
            {
                return "Loop From (" + v_LoopStart + "+1) to " + v_LoopParameter;
            }
            else
            {
                return "Loop " +  v_LoopParameter + " Times";
            }
        }
    }
}