using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop")]
    [Attributes.ClassAttributes.CommandSettings("Loop For List")]
    [Attributes.ClassAttributes.Description("This command allows you to Repeat actions on the values held by List. This command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class BeginLoopForListCommand : AListInputListCommands, IHaveLoopAdditionalCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //public string v_List { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store List Value (Readonly)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("List Value", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "List Value")]
        [PropertyParameterOrder(6000)]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store List Index (Readonly)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("List Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "List Index")]
        [PropertyParameterOrder(6001)]
        public string v_Index { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Reverse Loop")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyDisplayText(false, "")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(6003)]
        public string v_ReverseLoop { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Current Loop Times (First Time Value is '1')")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Loop Current Times Variable", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Current Times")]
        [PropertyParameterOrder(6004)]
        public string v_CurrentTimes { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store the Number of Loops (First Time Value is 0)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Number of Loops", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Number of Loops")]
        [PropertyParameterOrder(6005)]
        public string v_NumberOfLoops { get; set; }

        public BeginLoopForListCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            var loopCommand = (BeginLoopForListCommand)parentCommand.ScriptCommand;

            var rawVariable = v_List.GetRawVariable(engine);
            var listToLoop = this.ExpandUserVariableAsList(engine);
            int loopTimes = listToLoop.Count;

            Action<string> listValueAction;
            if (string.IsNullOrEmpty(v_Value))
            {
                listValueAction = new Action<string>(str => { });   // nothing
            }
            else
            {
                listValueAction = new Action<string>(str => { str.StoreInUserVariable(engine, v_Value); });
            }

            Action<int> listIndexAction;
            if (string.IsNullOrEmpty(v_Index))
            {
                listIndexAction = new Action<int>(idx => { });  // nothing
            }
            else
            {
                listIndexAction = new Action<int>(idx => { idx.StoreInUserVariable(engine, v_Index); });
            }

            Action<int> loopTimesAction;
            if (string.IsNullOrEmpty(v_CurrentTimes))
            {
                loopTimesAction = new Action<int>(num =>
                {
                    SystemVariables.Update_LoopCurrentIndex(num);
                });
            }
            else
            {
                loopTimesAction = new Action<int>(num =>
                {
                    SystemVariables.Update_LoopCurrentIndex(num);
                    num.StoreInUserVariable(engine, v_CurrentTimes);
                });
            }

            Action<int> loopNumAction;
            if (string.IsNullOrEmpty(v_NumberOfLoops))
            {
                loopNumAction = new Action<int>(num => { });    // nothing
            }
            else
            {
                loopNumAction = new Action<int>(num => { num.StoreInUserVariable(engine, v_NumberOfLoops); });
            }

            int count = 0;  // loop counter
            var loopBodyProcess = new Action<int>(index =>
            {
                rawVariable.CurrentPosition = index;

                engine.ReportProgress($"Starting Loop Number {(count + 1)}/{loopTimes} From Line {loopCommand.LineNumber}");

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                    {
                        return;
                    }

                    // store variables value
                    listValueAction(listToLoop[index]);
                    listIndexAction(index);
                    loopTimesAction(count + 1);
                    loopNumAction(count);

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress($"Exiting Loop From Line {loopCommand.LineNumber}");
                        //engine.CurrentLoopCancelled = false;
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
            });

            //DBG
            //var dummyAction = new Action<int>(cnt => { });

            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ReverseLoop), engine))
            {
                // reverse loop
                for (int i = listToLoop.Count - 1; i >= 0; i--, count++)
                {
                    loopBodyProcess(i);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.CurrentLoopCancelled = false;
                        break;
                    }
                }
            }
            else
            {
                // normal loop
                for (int i = 0; i < listToLoop.Count; i++, count++)
                {
                    loopBodyProcess(i);
                    //dummyAction(i);
                    //Console.WriteLine(i);
                    if (engine.CurrentLoopCancelled)
                    {
                        engine.CurrentLoopCancelled = false;
                        break;
                    }
                }
            }
        }
    }
}