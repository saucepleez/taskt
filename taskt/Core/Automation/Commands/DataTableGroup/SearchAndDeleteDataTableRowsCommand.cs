using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Row Action")]
    [Attributes.ClassAttributes.CommandSettings("Search And Delete DataTable Rows")]
    [Attributes.ClassAttributes.Description("This command allows you Delete specified DataTable Rows.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Delete a specific Row.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to delete a DataTable Row")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    public sealed class SearchAndDeleteDataTableRowsCommand : ADataTableBothDataTableCommands
    {
        //[XmlAttribute]
        //[PropertyDescription("Please indicate the DataTable Variable Name")]
        //[InputSpecification("Enter the name of your DataTable")]
        //[SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        //[Remarks("")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate tuples to delete column rows")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a tuple containing the column name and item you would like to remove.")]
        [SampleUsage("{ColumnName1,Item1},{ColumnName2,Item2}")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyParameterOrder(6000)]
        public string v_SearchItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select overwrite option")]
        [PropertyUISelectionOption("And")]
        [PropertyUISelectionOption("Or")]
        [InputSpecification("Indicate whether this command should remove rows with all the constraints or remove them with 1 or more constraints")]
        [SampleUsage("Select from **And** or **Or**")]
        [Remarks("")]
        [PropertyParameterOrder(7000)]
        public string v_AndOr { get; set; }

        public SearchAndDeleteDataTableRowsCommand()
        {
            //this.CommandName = "RemoveDataRowCommand";
            //this.SelectionName = "Remove DataRow";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //DataTable Dt = (DataTable)v_DataTable.GetRawVariable(engine).VariableValue;
            var myDT = this.ExpandUserVariableAsDataTable(engine);

            var vSearchItem = v_SearchItem.ExpandValueOrUserVariable(engine);

            //var searchConditions = new List<Tuple<string, string>>();
            var searchConditions = new List<(string, string)>();
            var listPairs = vSearchItem.Split('}');
            //int i = 0;

            //listPairs = listPairs.Take(listPairs.Count() - 1).ToArray();
            foreach (string item in listPairs)
            {
                string temp;
                temp = item.Trim('{', '"');
                var tempList = temp.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                for (int z = 0; z < tempList.Count; z++)
                {
                    tempList[z] = tempList[z].Trim('{');
                }
                //searchConditions.Insert(i, Tuple.Create(tempList[0], tempList[1]));
                searchConditions.Add((tempList[0], tempList[1]));
                //i++;
            }


            List<DataRow> listrows = new List<DataRow>();
            listrows = myDT.AsEnumerable().ToList();
            if (v_AndOr == "Or")
            {
                var templist = new List<DataRow>();
                //for each filter
                //foreach (Tuple<string, string> tuple in searchConditions)
                foreach((string colName, string value) in searchConditions)
                {
                    //for each datarow
                    foreach (DataRow item in listrows)
                    {
                        if (item[colName] != null)
                        {

                            if (item[colName].ToString() == value.ToString())
                            {
                                //add to list if filter matches
                                if (!templist.Contains(item))
                                {
                                    templist.Add(item);
                                }
                            }
                        }
                    }
                }
                foreach (DataRow item in templist)
                {
                    myDT.Rows.Remove(item);
                }
                myDT.AcceptChanges();

                //dataSetVariable.VariableValue = Dt;
            }
            //If And operation is checked
            else if (v_AndOr == "And")
            {
                var templist = new List<DataRow>(listrows);

                //for each tuple
                //foreach (Tuple<string, string> tuple in searchConditions)
                foreach((string colName, string value) in searchConditions)
                {
                    //for each datarow
                    foreach (DataRow drow in myDT.AsEnumerable())
                    {
                        if (drow[colName].ToString() != value)
                        {
                            //remove from list if filter matches
                            templist.Remove(drow);
                        }
                    }
                }
                foreach (DataRow item in templist)
                {
                    myDT.Rows.Remove(item);
                }
                myDT.AcceptChanges();

                //Assigns Datatable to newly updated Datatable
                //dataSetVariable.VariableValue = Dt;
            }
        }
        
        public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + "[Remove all datarows with the filter: " + v_SearchItem + " from DataTable: " + v_DataTable + "]";
        }
    }
}