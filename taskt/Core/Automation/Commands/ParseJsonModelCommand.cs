using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Drawing;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to parse a JSON object into a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a JSON object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class ParseJsonModelCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Supply the value or variable requiring extraction (ex. [vSomeVariable])")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable or text value")]
        [Attributes.PropertyAttributes.SampleUsage("**Hello** or **vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_InputValue { get; set; }

        [XmlElement]
        [Attributes.PropertyAttributes.PropertyDescription("Assign Objects for Parsing. (ex: $.id)")]
        [Attributes.PropertyAttributes.InputSpecification("Inpu")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_ParseObjects { get; set; }


        [XmlIgnore]
        [NonSerialized]
        private DataGridView ParseObjectsGridViewHelper;

        public ParseJsonModelCommand()
        {
            this.CommandName = "ParseJsonModelCommand";
            this.SelectionName = "Parse JSON Model";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            v_ParseObjects = new DataTable();
            v_ParseObjects.Columns.Add("Json Selector");
            v_ParseObjects.Columns.Add("Output Variable");
            v_ParseObjects.TableName = $"ParseJsonObjectsTable{DateTime.Now.ToString("MMddyyhhmmss")}";

            ParseObjectsGridViewHelper = new DataGridView();
            ParseObjectsGridViewHelper.AllowUserToAddRows = true;
            ParseObjectsGridViewHelper.AllowUserToDeleteRows = true;
            ParseObjectsGridViewHelper.Size = new Size(400, 250);
            ParseObjectsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            ParseObjectsGridViewHelper.DataBindings.Add("DataSource", this, "v_ParseObjects", false, DataSourceUpdateMode.OnPropertyChanged);

        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
            
            //get variablized input
            var variableInput = v_InputValue.ConvertToUserVariable(sender);

            foreach (DataRow rw in v_ParseObjects.Rows)
            {
                var jsonSelector = rw.Field<string>("Json Selector").ConvertToUserVariable(sender);
                var targetVariableName = rw.Field<string>("Output Variable");

                //create objects
                Newtonsoft.Json.Linq.JObject o;
                IEnumerable<Newtonsoft.Json.Linq.JToken> searchResults;
                List<string> resultList = new List<string>();

                //parse json
                try
                {
                    o = Newtonsoft.Json.Linq.JObject.Parse(variableInput);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Occured Parsing Tokens: " + ex.ToString());
                }


                //select results
                try
                {
                    searchResults = o.SelectTokens(jsonSelector);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
                }

                //add results to result list since list<string> is supported
                foreach (var result in searchResults)
                {
                    resultList.Add(result.ToString());
                }

                //get variable
                var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == targetVariableName).FirstOrDefault();

                //create if var does not exist
                if (requiredComplexVariable == null)
                {
                    engine.VariableList.Add(new Script.ScriptVariable() { VariableName = targetVariableName, CurrentPosition = 0 });
                    requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == targetVariableName).FirstOrDefault();
                }

                //assign value to variable
                requiredComplexVariable.VariableValue = resultList;

            }





        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_ParseObjects", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_ParseObjects", this, new[] { ParseObjectsGridViewHelper }, editor));

            RenderedControls.Add(ParseObjectsGridViewHelper);

           

            return RenderedControls;

        }



        public override string GetDisplayValue()
        {
            return $"{base.GetDisplayValue()} [Select {v_ParseObjects.Rows.Count} item(s) from JSON]";
        }
    }
}