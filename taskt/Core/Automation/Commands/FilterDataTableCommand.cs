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
    [Attributes.ClassAttributes.Description("This command allows you filter a DataTable into a new Datatable")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get specific rows of a DataTable.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command attempts to filter a Datatable into a new Datatable")]
    public class FilterDataTableCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the DataTable Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the DataTable name you would like to filter through.")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_DataTableName { get; set; }
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the output DataTable Name")]
        [Attributes.PropertyAttributes.InputSpecification("Enter a unique DataTable name for future reference.")]
        [Attributes.PropertyAttributes.SampleUsage("**myData**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_OutputDTName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate tuples to filter by.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a tuple containing the column name and item you would like to filter by.")]
        [Attributes.PropertyAttributes.SampleUsage("{ColumnName1,Item1},{ColumnName2,Item2}")]
        [Attributes.PropertyAttributes.Remarks("")]
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
            Script.ScriptVariable newDatatable = new Script.ScriptVariable
            {
                VariableName = v_OutputDTName,
                VariableValue = outputDT
            };

            //Overwrites variable if it already exists
            if (engine.VariableList.Exists(y => y.VariableName == newDatatable.VariableName))
            {
                Script.ScriptVariable temp = engine.VariableList.Where(y => y.VariableName == newDatatable.VariableName).FirstOrDefault();
                engine.VariableList.Remove(temp);
            }

            engine.VariableList.Add(newDatatable);
            dataSetVariable.VariableValue = Dt;


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
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_OutputDTName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SearchItem", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue()+ "[Filter all datarows with the filter: " + v_SearchItem + " from DataTable: " + v_DataTableName + " and put them in DataTable: "+ v_OutputDTName+"]";
        }
    }
}