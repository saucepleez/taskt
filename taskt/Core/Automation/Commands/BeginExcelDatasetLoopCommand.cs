using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Loop Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to loop through an Excel Dataset")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to iterate over a series of Excel cells.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to loop through a known Excel DataSet")]
    public class BeginExcelDatasetLoopCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the Excel DataSet Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a unique dataset name that will be used later to traverse over the data")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataSetName { get; set; }

        public BeginExcelDatasetLoopCommand()
        {
            this.CommandName = "BeginExcelDataSetLoopCommand";
            this.SelectionName = "Loop Excel Dataset";
            //this command is no longer required as list loop can successfully loop.
            this.CommandEnabled = false;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender, Core.Script.ScriptAction parentCommand)
        {

            Core.Automation.Commands.BeginExcelDatasetLoopCommand loopCommand = (Core.Automation.Commands.BeginExcelDatasetLoopCommand)parentCommand.ScriptCommand;
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            var dataSetVariable = engine.VariableList.Where(f => f.VariableName == v_DataSetName).FirstOrDefault();

            if (dataSetVariable == null)
                throw new Exception("DataSet Name Not Found - " + v_DataSetName);

            if (!engine.VariableList.Any(f => f.VariableName == "Loop.CurrentIndex"))
            {
                engine.VariableList.Add(new Script.ScriptVariable() { VariableName = "Loop.CurrentIndex", VariableValue = "0" });
            }


            DataTable excelTable = (DataTable)dataSetVariable.VariableValue;


            var loopTimes = excelTable.Rows.Count;

            for (int i = 0; i < excelTable.Rows.Count; i++)
            {
                dataSetVariable.CurrentPosition = i;

                (i + 1).ToString().StoreInUserVariable(engine, "Loop.CurrentIndex");

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    //bgw.ReportProgress(0, new object[] { loopCommand.LineNumber, "Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber });

                    if (engine.IsCancellationPending)
                        return;
                    engine.ExecuteCommand(cmd);
                    // bgw.ReportProgress(0, new object[] { loopCommand.LineNumber, "Finished Loop From Line " + loopCommand.LineNumber });
                }
            }

        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataSetName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}