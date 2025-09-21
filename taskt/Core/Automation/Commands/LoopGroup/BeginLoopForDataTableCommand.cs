using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop")]
    [Attributes.ClassAttributes.CommandSettings("Loop For DataTable")]
    [Attributes.ClassAttributes.Description("This command allows you to Repeat actions on the values held by DataTable. This command must have a following 'End Loop' command.")]
    [Attributes.ClassAttributes.UsesDescription("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_startloop))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class BeginLoopForDataTableCommand : ADataTableInputDataTableCommands, IHaveLoopAdditionalCommands
    {
        //[XmlAttribute]
        //public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store DataTable Value (Readonly)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("DataTable Value", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "DataTable Value")]
        [PropertyParameterOrder(6000)]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Row Index (Readonly)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Row Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Row Index")]
        [PropertyParameterOrder(6001)]
        public string v_RowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Column Index")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Column Index", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Column Index")]
        [PropertyParameterOrder(6002)]
        public string v_ColumnIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Column Name (Readonly)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Column Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Column Name")]
        [PropertyParameterOrder(6003)]
        public string v_ColumnName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Loop Priority")]
        [PropertyUISelectionOption("Row First")]
        [PropertyUISelectionOption("Column First")]
        [PropertyIsOptional(true, "Row First")]
        [PropertyValidationRule("Loop nameof", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Loop Priority")]
        [PropertyParameterOrder(6004)]
        public string v_LoopDirection { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Row Reverse Loop")]
        [PropertyIsOptional(true)]
        [PropertyFirstValue("No")]
        [PropertyDisplayText(false, "")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(6005)]
        public string v_RowReverseLoop { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Column Reverse Loop")]
        [PropertyIsOptional(true)]
        [PropertyFirstValue("No")]
        [PropertyDisplayText(false, "")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyParameterOrder(6006)]
        public string v_ColumnReverseLoop { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Current Loop Times (First Time Value is '1')")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Loop Current Times Variable", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Current Times")]
        [PropertyParameterOrder(6007)]
        public string v_CurrentTimes { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store the Number of Loops (First Time Value is 0)")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Number of Loops", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Number of Loops")]
        [PropertyParameterOrder(6008)]
        public string v_NumberOfLoops { get; set; }

        public BeginLoopForDataTableCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            var loopCommand = (BeginLoopForDataTableCommand)parentCommand.ScriptCommand;

            var rawVariable = v_DataTable.GetRawVariable(engine);
            var tableToLoop = this.ExpandUserVariableAsDataTable(engine);
            var loopTimes = tableToLoop.Rows.Count * tableToLoop.Columns.Count;

            Action<string> tableValueAction;
            if (string.IsNullOrEmpty(v_Value))
            {
                tableValueAction = new Action<string>(str => { });   // nothing
            }
            else
            {
                tableValueAction = new Action<string>(str => { str.StoreInUserVariable(engine, v_Value); });
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

            Action<int> tableColumnIndexAction;
            if (string.IsNullOrEmpty(v_ColumnIndex))
            {
                tableColumnIndexAction = new Action<int>(idx => { });   // nothing
            }
            else
            {
                tableColumnIndexAction = new Action<int>(idx => { idx.StoreInUserVariable(engine, v_ColumnIndex); });
            }

            Action<string> tableColumnNameAction;
            if (string.IsNullOrEmpty(v_ColumnIndex))
            {
                tableColumnNameAction = new Action<string>(n => { });   // nothing
            }
            else
            {
                tableColumnNameAction = new Action<string>(n => { n.StoreInUserVariable(engine, v_ColumnName); });
            }

            int rowFirstValue, columnFirstValue;
            Func<int, bool> rowCondition, columnCondition;
            Func<int, int> rowNextValue, columnNextValue;
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_RowReverseLoop), engine))
            {
                rowFirstValue = tableToLoop.Rows.Count - 1;
                rowCondition = new Func<int, bool>(row => (row >= 0));
                rowNextValue = new Func<int, int>(row => (--row));
            }
            else
            {
                rowFirstValue = 0;
                rowCondition = new Func<int, bool>(row => (row < tableToLoop.Rows.Count));
                rowNextValue = new Func<int, int>(row => (++row));
            }
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ColumnReverseLoop), engine))
            {
                columnFirstValue = tableToLoop.Columns.Count - 1;
                columnCondition = new Func<int, bool>(column => (column >= 0));
                columnNextValue = new Func<int, int>(column => (--column));
            }
            else
            {
                columnFirstValue = 0;
                columnCondition = new Func<int, bool>(column => (column < tableToLoop.Columns.Count));
                columnNextValue = new Func<int, int>(column => (++column));
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
            var loopBodyProcess = new Action<int, int>((row, column) =>
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
                    tableValueAction(tableToLoop.Rows[row][column]?.ToString() ?? "");
                    tableRowIndexAction(row);
                    tableColumnIndexAction(column);
                    tableColumnNameAction(tableToLoop.Columns[column].ColumnName);
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

            switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_LoopDirection), engine))
            {
                case "row first":
                    for (int i = rowFirstValue; rowCondition(i); i = rowNextValue(i))
                    {
                        for (int j =  columnFirstValue; columnCondition(j); j = columnNextValue(j))
                        {
                            loopBodyProcess(i, j);
                            if (engine.CurrentLoopCancelled)
                            {
                                break;
                            }
                            count++;
                        }
                        if (engine.CurrentLoopCancelled)
                        {
                            engine.CurrentLoopCancelled = false;
                            break;
                        }
                    }
                    break;
                case "column first":
                    for (int j = columnFirstValue; columnCondition(j); j = columnNextValue(j))
                    {
                        for (int i = rowFirstValue; rowCondition(i); i = rowNextValue(i))
                        {
                        
                            loopBodyProcess(i, j);
                            if (engine.CurrentLoopCancelled)
                            {
                                break;
                            }
                            count++;
                        }
                        if (engine.CurrentLoopCancelled)
                        {
                            engine.CurrentLoopCancelled = false;
                            break;
                        }
                    }
                    break;
            }
        }
    }
}