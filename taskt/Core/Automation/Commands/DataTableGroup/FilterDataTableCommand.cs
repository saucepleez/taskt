using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DataTable")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("This command allows you filter a DataTable into a new Datatable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get specific rows of a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to filter a Datatable into a new Datatable")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_spreadsheet))]
    public sealed class FilterDataTableCommand : ADataTableCreateFromDataTableCommands
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [InputSpecification("Enter the DataTable name you would like to filter through.")]
        //[SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public override string v_TargetDataTable { get; set; }

        //[XmlAttribute]
        //[PropertyDescription("Please indicate the output DataTable Variable Name")]
        //[InputSpecification("Enter a unique DataTable name for future reference.")]
        //[SampleUsage("**newData** or **{{{vNewData}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //public string v_NewDataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate tuples to filter by.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a tuple containing the column name and item you would like to filter by.")]
        [SampleUsage("{ColumnName1,Item1},{ColumnName2,Item2}")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyParameterOrder(11000)]
        public string v_SearchItem { get; set; }

        public FilterDataTableCommand()
        {
            this.CommandName = "FilterDataTableCommand";
            this.SelectionName = "Filter DataTable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetDT = (DataTable)v_TargetDataTable.GetRawVariable(engine).VariableValue;
            var targetDT = this.ExpandUserVariableAsDataTable(engine);

            var vSearchItem = v_SearchItem.ExpandValueOrUserVariable(engine);

            //var searchConditions = new List<Tuple<string, string>>();
            var searchConditions = new List<(string, string)>();

            var listPairs = vSearchItem.Split('}');
            //int i = 0;

            //listPairs = listPairs.Take(listPairs.Count() - 1).ToArray();
            foreach (string item in listPairs)
            {
                string temp;
                temp = item.Trim('{');
                var tempList = temp.Split(',');
                //searchConditions.Insert(i, Tuple.Create(tempList[0], tempList[1]));
                searchConditions.Add((tempList[0], tempList[1]));
                //i++;
            }

            var listrows = targetDT.AsEnumerable().ToList();
            var templist = new List<DataRow>();

            foreach ((string col, string value) in searchConditions)
            {
                foreach (DataRow item in listrows)
                {
                    if (item[col] != null)
                    {
                        if (item[col].ToString() == value.ToString())
                        {
                            templist.Add(item);
                        }
                    }
                }
            }

            var newDT = new DataTable();
            int x = 0;
            foreach(DataColumn column in targetDT.Columns)
            {
                newDT.Columns.Add(targetDT.Columns[x].ToString());
                x++;
            }

            foreach (DataRow item in templist)
            {
                newDT.Rows.Add(item.ItemArray);
            }

            targetDT.AcceptChanges();

            //newDT.StoreInUserVariable(engine, v_NewDataTable);
            this.StoreDataTableInUserVariable(newDT, engine);
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
            return base.GetDisplayValue()+ "[Filter all datarows with the filter: " + v_SearchItem + " from DataTable: " + v_TargetDataTable + " and put them in DataTable: "+ v_NewDataTable+"]";
        }
    }
}