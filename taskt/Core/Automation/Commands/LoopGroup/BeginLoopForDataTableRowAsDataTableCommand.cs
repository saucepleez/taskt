using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop")]
    [Attributes.ClassAttributes.CommandSettings("Loop For DataTable Row As DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to Repeat actions on the values held by DataTable. This command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class BeginLoopForDataTableRowAsDataTableCommand : ADataTableGetFromDataTableCommands, IHaveLoopAdditionalCommands
    {
        //[XmlAttribute]
        //public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DataTableControls), nameof(DataTableControls.v_OutputDataTableName))]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("DataTable Variable", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(6000)]
        public override string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Row Index (Readonly)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Row Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Row Index")]
        [PropertyParameterOrder(6001)]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Reverse Loop")]
        [PropertyIsOptional(true, "No")]
        [PropertyFirstValue("No")]
        [PropertyDisplayText(false, "")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(6002)]
        public string v_ReverseLoop { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Current Loop Times (First Time Value is '1')")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Loop Current Times Variable", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Current Times")]
        [PropertyParameterOrder(6003)]
        public string v_CurrentTimes { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store the Number of Loops (First Time Value is 0)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Number of Loops", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Number of Loops")]
        [PropertyParameterOrder(6004)]
        public string v_NumberOfLoops { get; set; }

        public BeginLoopForDataTableRowAsDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            var loopCommand = (BeginLoopForDataTableRowAsDataTableCommand)parentCommand.ScriptCommand;

            var rawVariable = v_DataTable.GetRawVariable(engine);
            var tableToLoop = this.ExpandUserVariableAsDataTable(engine);
            var loopTimes = tableToLoop.Rows.Count;

            Action<int> dataTableVariableAction;
            if (string.IsNullOrEmpty(v_Result))
            {
                dataTableVariableAction = new Action<int>(row => { }); // nothing
            }
            else
            {
                dataTableVariableAction = new Action<int>(row =>
                {
                    var cnbTbl = new ConvertDataTableRowToDataTableCommand()
                    {
                        v_DataTable = this.v_DataTable,
                        v_RowIndex = row.ToString(),
                        v_Result = this.v_Result,
                    };
                    cnbTbl.RunCommand(engine);
                });
            }

            Action<int> tableRowIndexAction;
            if (string.IsNullOrEmpty(v_RowIndex))
            {
                tableRowIndexAction = new Action<int>(idx => { });  // nothing
            }
            else
            {
                tableRowIndexAction = new Action<int>(idx => { idx.StoreInUserVariable(engine, v_RowIndex); });
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
            var loopBodyProcess = new Action<int>((row) =>
            {
                rawVariable.CurrentPosition = row;    // TODO: it's no good

                engine.ReportProgress($"Starting Loop Number {(count + 1)}/{loopTimes} From Line {loopCommand.LineNumber}");

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                    {
                        return;
                    }

                    // store variables value
                    dataTableVariableAction(row);
                    tableRowIndexAction(row);
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
                for (int i = tableToLoop.Rows.Count - 1; i >= 0; i--)
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
                for (int i = 0; i < tableToLoop.Rows.Count; i++)
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