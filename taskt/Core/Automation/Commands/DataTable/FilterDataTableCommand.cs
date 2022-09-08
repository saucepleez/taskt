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
    [Attributes.ClassAttributes.Description("This command allows you filter a DataTable into a new Datatable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get specific rows of a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to filter a Datatable into a new Datatable")]
    public class FilterDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the DataTable Variable Name")]
        [InputSpecification("Enter the DataTable name you would like to filter through.")]
        [SampleUsage("**myDataTable** or **{{{vMyDataTable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_DataTableName { get; set; }
        [XmlAttribute]
        [PropertyDescription("Please indicate the output DataTable Variable Name")]
        [InputSpecification("Enter a unique DataTable name for future reference.")]
        [SampleUsage("**newData** or **{{{vNewData}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        public string v_OutputDTName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please indicate tuples to filter by.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a tuple containing the column name and item you would like to filter by.")]
        [SampleUsage("{ColumnName1,Item1},{ColumnName2,Item2}")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_SearchItem { get; set; }

        public FilterDataTableCommand()
        {
            this.CommandName = "FilterDataTableCommand";
            this.SelectionName = "Filter DataTable";
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
                temp = item.Trim('{');
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
            Dt.AcceptChanges();

            outputDT.StoreInUserVariable(engine, v_OutputDTName);
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
            return base.GetDisplayValue()+ "[Filter all datarows with the filter: " + v_SearchItem + " from DataTable: " + v_DataTableName + " and put them in DataTable: "+ v_OutputDTName+"]";
        }
    }
}