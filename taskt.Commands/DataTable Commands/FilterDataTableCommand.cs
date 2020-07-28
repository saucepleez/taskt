using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command filters specific rows from a DataTable into a new Datatable.")]

    public class FilterDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Input DataTable")]
        [InputSpecification("Enter the DataTable to filter through.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Filtered DataTable Name")]
        [InputSpecification("Enter a unique DataTable name for future reference.")]
        [SampleUsage("FilteredDataTable")]
        [Remarks("")]
        public string v_FilteredDataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Filter Tuple")]
        [InputSpecification("Enter a tuple containing the column name and item you would like to filter by.")]
        [SampleUsage("(ColumnName1,Item1),(ColumnName2,Item2) || ({vColumn1},{vItem1}),({vCloumn2},{vItem2}) || {vFilterTuple}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SearchItem { get; set; }

        public FilterDataTableCommand()
        {
            CommandName = "FilterDataTableCommand";
            SelectionName = "Filter DataTable";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var dataSetVariable = LookupVariable(engine);
            var vSearchItem = v_SearchItem.ConvertToUserVariable(engine);

            DataTable Dt = new DataTable();
            Dt = (DataTable)dataSetVariable.VariableValue;
            var t = new List<Tuple<string, string>>();
            var listPairs = vSearchItem.Split(')');
            int i = 0;

            listPairs = listPairs.Take(listPairs.Count() - 1).ToArray();
            foreach (string item in listPairs)
            {
                string temp;
                temp = item.Trim('(');
                var tempList = temp.Split(',');
                t.Insert(i, Tuple.Create(tempList[0], tempList[1]));
                i++;
            }

            List<DataRow> listrows = Dt.AsEnumerable().ToList();
            List<DataRow> templist = new List<DataRow>();

            foreach (Tuple<string, string> tuple in t)
            {
                foreach (DataRow item in listrows)
                {
                    if (item[tuple.Item1] != null)
                    {
                        if (item[tuple.Item1].ToString() == tuple.Item2.ToString())
                        {
                            templist.Add(item);
                        }
                    }
                }
            }

            DataTable outputDT = new DataTable();
            int x = 0;
            foreach(DataColumn column in Dt.Columns)
            {
                outputDT.Columns.Add(Dt.Columns[x].ToString());
                x++;
            }
            foreach (DataRow item in templist)
            {
                outputDT.Rows.Add(item.ItemArray);
            }

            engine.AddVariable(v_FilteredDataTableName, outputDT);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FilteredDataTableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SearchItem", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue()+ $" [Filter Rows With '{v_SearchItem}' From '{v_DataTable}' - Store Filtered DataTable in '{v_FilteredDataTableName}']";
        }

        private ScriptVariable LookupVariable(AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTable).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if (requiredVariable == null && v_DataTable.StartsWith(sendingInstance.EngineSettings.VariableStartMarker)
                                         && v_DataTable.EndsWith(sendingInstance.EngineSettings.VariableEndMarker))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTable.Replace(sendingInstance.EngineSettings.VariableStartMarker, "")
                                                     .Replace(sendingInstance.EngineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
    }
}