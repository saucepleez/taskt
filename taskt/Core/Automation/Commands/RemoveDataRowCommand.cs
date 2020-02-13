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
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.Description("This command allows you remove specified data rows.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete a specific row.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to delete a DataTable Row")]
    public class RemoveDataRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the name of your DataTable")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select overwrite option")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("And")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Or")]
        [Attributes.PropertyAttributes.InputSpecification("Indicate whether this command should remove rows with all the constraints or remove them with 1 or more constraints")]
        [Attributes.PropertyAttributes.SampleUsage("Select from **And** or **Or**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_AndOr { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate tuples to delete column rows.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a tuple containing the column name and item you would like to remove.")]
        [Attributes.PropertyAttributes.SampleUsage("{ColumnName1,Item1},{ColumnName2,Item2}")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_SearchItem { get; set; }

        public RemoveDataRowCommand()
        {
            this.CommandName = "RemoveDataRowCommand";
            this.SelectionName = "Remove DataRow";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {

            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            var dataSetVariable = LookupVariable(engine);
            var vSearchItem = v_SearchItem.ConvertToUserVariable(engine);

            DataTable Dt = new DataTable();
            Dt = (DataTable)dataSetVariable.VariableValue;
            var t = new List<Tuple<string, string>>();
            var listPairs = vSearchItem.Split('}');
            int i = 0;

            listPairs = listPairs.Take(listPairs.Count() - 1).ToArray();
            foreach (string item in listPairs)
            {
                string temp;
                temp = item.Trim('{', '"');
                var tempList = temp.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                for (int z = 0; z < tempList.Count; z++)
                {
                    tempList[z] = tempList[z].Trim('{');
                }
                t.Insert(i, Tuple.Create(tempList[0], tempList[1]));
                i++;
            }


            List<DataRow> listrows = new List<DataRow>();
            listrows = Dt.AsEnumerable().ToList();
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
        private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        {
            //search for the variable
            var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_DataTableName).FirstOrDefault();

            //if variable was not found but it starts with variable naming pattern
            if ((requiredVariable == null) && (v_DataTableName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_DataTableName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
            {
                //reformat and attempt
                var reformattedVariable = v_DataTableName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
                requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
            }

            return requiredVariable;
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DataTableName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SearchItem", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_AndOr", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + "[Remove all datarows with the filter: " + v_SearchItem + " from DataTable: " + v_DataTableName + "]";
        }
    }
}