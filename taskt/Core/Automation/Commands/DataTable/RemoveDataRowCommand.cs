using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("This command allows you remove specified data rows.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to delete a specific row.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to delete a DataTable Row")]
    public class RemoveDataRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [InputSpecification("Enter the name of your DataTable")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DataTableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate tuples to delete column rows")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a tuple containing the column name and item you would like to remove.")]
        [SampleUsage("{ColumnName1,Item1},{ColumnName2,Item2}")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_SearchItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select overwrite option")]
        [PropertyUISelectionOption("And")]
        [PropertyUISelectionOption("Or")]
        [InputSpecification("Indicate whether this command should remove rows with all the constraints or remove them with 1 or more constraints")]
        [SampleUsage("Select from **And** or **Or**")]
        [Remarks("")]
        public string v_AndOr { get; set; }

        public RemoveDataRowCommand()
        {
            this.CommandName = "RemoveDataRowCommand";
            this.SelectionName = "Remove DataRow";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            DataTable Dt = (DataTable)v_DataTableName.GetRawVariable(engine).VariableValue;

            var vSearchItem = v_SearchItem.ConvertToUserVariable(engine);

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

                //dataSetVariable.VariableValue = Dt;
            }

            //If And operation is checked
            else if (v_AndOr == "And")
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
                //dataSetVariable.VariableValue = Dt;
            }
        }
        
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + "[Remove all datarows with the filter: " + v_SearchItem + " from DataTable: " + v_DataTableName + "]";
        }
    }
}