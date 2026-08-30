using System;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop")]
    [Attributes.ClassAttributes.CommandSettings("Loop For Dictionary")]
    [Attributes.ClassAttributes.Description("This command allows you to Repeat actions on the values held by Dictionary. This command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class BeginLoopForDictionaryCommand : ADictionaryInputDictionaryCommands, IHaveLoopAdditionalCommands
    {
        //[XmlAttribute]
        //public string v_Dictionary { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Dictionary Value (Readonly)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Dictionary Value", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Dictionary Value")]
        [PropertyParameterOrder(6000)]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Dictionary Key (Readonly)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Dictionary Key", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Dictionary Key")]
        [PropertyParameterOrder(6001)]
        public string v_Key { get; set; }

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

        public BeginLoopForDictionaryCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            var loopCommand = (BeginLoopForDictionaryCommand)parentCommand.ScriptCommand;

            var rawVariable = v_Dictionary.GetRawVariable(engine);
            var dicToLoop = this.ExpandUserVariableAsDictionary(engine);
            var keys = dicToLoop.Keys.ToList();
            int loopTimes = keys.Count;

            Action<string> dicValueAction;
            if (string.IsNullOrEmpty(v_Value))
            {
                dicValueAction = new Action<string>(str => { });   // nothing
            }
            else
            {
                dicValueAction = new Action<string>(str => { str.StoreInUserVariable(engine, v_Value); });
            }

            Action<string> dicKeyAction;
            if (string.IsNullOrEmpty(v_Key))
            {
                dicKeyAction = new Action<string>(idx => { });  // nothing
            }
            else
            {
                dicKeyAction = new Action<string>(idx => { idx.StoreInUserVariable(engine, v_Key); });
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
                rawVariable.CurrentPosition = index;    // TODO: it's no good

                engine.ReportProgress($"Starting Loop Number {(count + 1)}/{loopTimes} From Line {loopCommand.LineNumber}");

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                    {
                        return;
                    }

                    var key = keys[index];

                    // store variables value
                    dicValueAction(dicToLoop[key]);
                    dicKeyAction(key);
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

            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ReverseLoop), engine))
            {
                // reverse loop
                for (int i = keys.Count - 1; i >= 0; i--, count++)
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
                for (int i = 0; i < keys.Count; i++, count++)
                {
                    loopBodyProcess(i);
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