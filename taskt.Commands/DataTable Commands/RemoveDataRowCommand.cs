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
    [Description("This command removes specific DataRows from a DataTable.")]

    public class RemoveDataRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("DataTable")]
        [InputSpecification("Enter an existing DataTable.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Removal Tuple")]
        [InputSpecification("Enter a tuple containing the column name and item you would like to remove.")]
        [SampleUsage("(ColumnName1,Item1),(ColumnName2,Item2) || ({vColumn1},{vItem1}),({vCloumn2},{vItem2}) || {vRemovalTuple}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SearchItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Overwrite Option")]
        [PropertyUISelectionOption("And")]
        [PropertyUISelectionOption("Or")]
        [InputSpecification("Indicate whether this command should remove rows with all the constraints or remove them with 1 or more constraints.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_AndOr { get; set; }

        public RemoveDataRowCommand()
        {
            CommandName = "RemoveDataRowCommand";
            SelectionName = "Remove DataRow";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var dataSetVariable = LookupVariable(engine);
            var vSearchItem = v_SearchItem.ConvertToUserVariable(engine);

            DataTable Dt = (DataTable)dataSetVariable.VariableValue;
            var t = new List<Tuple<string, string>>();
            var listPairs = vSearchItem.Split(')');
            int i = 0;

            listPairs = listPairs.Take(listPairs.Count() - 1).ToArray();
            foreach (string item in listPairs)
            {
                string temp;
                temp = item.Trim('(', '"');
                var tempList = temp.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                for (int z = 0; z < tempList.Count; z++)
                {
                    tempList[z] = tempList[z].Trim('(');
                }
                t.Insert(i, Tuple.Create(tempList[0], tempList[1]));
                i++;
            }

            List<DataRow> listrows = Dt.AsEnumerable().ToList();
            if (v_AndOr == "Or")
            {
                List<DataRow> templist = new List<DataRow>();
                //for each filter
                foreach (Tuple<string, string> tuple in t)
                {
                    //for each datarow
                    foreach (DataRow item in listrows)
                    {
                        if (item[tuple.Item1] != null)
                        {
                            if (item[tuple.Item1].ToString() == tuple.Item2.ToString())
                            {
                                //add to list if filter matches
                                if (!templist.Contains(item))
                                    templist.Add(item);
                            }
                        }
                    }
                }
                foreach (DataRow item in templist)
                {
                    Dt.Rows.Remove(item);
                }
                Dt.AcceptChanges();
                dataSetVariable.VariableValue = Dt;
            }

            //If And operation is checked
            if (v_AndOr == "And")
            {
                List<DataRow> templist = new List<DataRow>(listrows);

                //for each tuple
                foreach (Tuple<string, string> tuple in t)
                {
                    //for each datarow
                    foreach (DataRow drow in Dt.AsEnumerable())
                    {
                        if (drow[tuple.Item1].ToString() != tuple.Item2)
                        {
                            //remove from list if filter matches
                            templist.Remove(drow);
                        }
                    }
                }

                foreach (DataRow item in templist)
                {
                    Dt.Rows.Remove(item);
                }
                Dt.AcceptChanges();

                //Assigns Datatable to newly updated Datatable
                dataSetVariable.VariableValue = Dt;
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SearchItem", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_AndOr", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Remove Rows With '{v_SearchItem}' From '{v_DataTable}']";
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