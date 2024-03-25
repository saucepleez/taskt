﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core;
using taskt.Core.Automation.Engine;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to repeat actions several times (loop).  Any 'Begin Loop' command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to perform a series of commands a specified amount of times.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command recursively calls the underlying 'BeginLoop' Command to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    public class BeginNumberOfTimesLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Enter how many times to perform the loop (ex. 5, {{{vNum}}})")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the amount of times you would like to perform the encased commands.")]
        [Attributes.PropertyAttributes.SampleUsage("**5** or **10** or **{{{vNum}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_LoopParameter { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyDescription("Optional - Define Start Index (Default: 0)")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the starting index of the loop.")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **1** or **{{{vStartValue}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_LoopStart { get; set; }

        public BeginNumberOfTimesLoopCommand()
        {
            this.CommandName = "BeginNumberOfTimesLoopCommand";
            this.SelectionName = "Loop Number Of Times";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_LoopStart = "0";
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Core.Script.ScriptAction parentCommand)
        {
            Core.Automation.Commands.BeginNumberOfTimesLoopCommand loopCommand = (Core.Automation.Commands.BeginNumberOfTimesLoopCommand)parentCommand.ScriptCommand;

            //if (!engine.VariableList.Any(f => f.VariableName == "Loop.CurrentIndex"))
            //{
            //    engine.VariableList.Add(new Script.ScriptVariable() { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
            //}


            int loopTimes;
            Script.ScriptVariable complexVarible = null;

            var loopParameter = loopCommand.v_LoopParameter.ExpandValueOrUserVariable(engine);

            loopTimes = int.Parse(loopParameter);


            int startIndex = 0;
            int.TryParse(v_LoopStart.ExpandValueOrUserVariable(engine), out startIndex);


            for (int i = startIndex; i < loopTimes; i++)
            {
                if (complexVarible != null)
                {
                    complexVarible.CurrentPosition = i;
                }

                // (i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");

                engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                    {
                        return;
                    }

                    //(i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");
                    SystemVariables.Update_LoopCurrentIndex(i + 1);

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
        public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
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

        public override bool IsValidate(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_LoopParameter))
            {
                this.validationResult += "Times is empty.\n";
            }
            else
            {
                int v;
                if (int.TryParse(this.v_LoopParameter, out v))
                {
                    if (v < 0)
                    {
                        this.validationResult += "Specify a value of 0 or more for Times.\n";
                        this.IsValid = false;
                    }
                }
            }

            return this.IsValid;
        }
    }
}