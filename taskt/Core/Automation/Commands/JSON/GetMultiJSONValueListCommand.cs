using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("JSON Commands")]
    [Attributes.ClassAttributes.SubGruop("Get/Set")]
    [Attributes.ClassAttributes.Description("This command allows you to parse a JSON object into a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to extract data from a JSON object")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetMultiJSONValueListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please Supply the JSON value or variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or text value")]
        [SampleUsage("**{ \"id\": 123, \"name\": \"john\" }** or **{{{vJSONVariable}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.JSON)]
        [PropertyValidationRule("JSON", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "JSON")]
        public string v_InputValue { get; set; }

        [XmlElement]
        [PropertyDescription("Please Assign Objects for Parsing.")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyDisplayText(true, "Get")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true, 400, 250)]
        [PropertyDataGridViewColumnSettings("Json Selector", "Json Selector", false)]
        [PropertyDataGridViewColumnSettings("Output Variable", "Output Variable", false)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_ParseObjects { get; set; }


        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView ParseObjectsGridViewHelper;

        public GetMultiJSONValueListCommand()
        {
            this.CommandName = "GetMultiJSONValueListCommand";
            this.SelectionName = "Get Multi JSON Value List";
            this.CommandEnabled = true;
            this.CustomRendering = true;

            //v_ParseObjects = new DataTable();
            //v_ParseObjects.Columns.Add("Json Selector");
            //v_ParseObjects.Columns.Add("Output Variable");
            //v_ParseObjects.TableName = $"ParseJsonObjectsTable{DateTime.Now.ToString("MMddyyhhmmss")}";

            //ParseObjectsGridViewHelper = new DataGridView();
            //ParseObjectsGridViewHelper.AllowUserToAddRows = true;
            //ParseObjectsGridViewHelper.AllowUserToDeleteRows = true;
            //ParseObjectsGridViewHelper.Size = new Size(400, 250);
            //ParseObjectsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //ParseObjectsGridViewHelper.DataBindings.Add("DataSource", this, "v_ParseObjects", false, DataSourceUpdateMode.OnPropertyChanged);

        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            ////get variablized input
            //var variableInput = v_InputValue.ConvertToUserVariable(sender);

            //foreach (DataRow rw in v_ParseObjects.Rows)
            //{
            //    var jsonSelector = rw.Field<string>("Json Selector").ConvertToUserVariable(sender);
            //    var targetVariableName = rw.Field<string>("Output Variable");

            //    //create objects
            //    Newtonsoft.Json.Linq.JObject o;
            //    IEnumerable<Newtonsoft.Json.Linq.JToken> searchResults;
            //    List<string> resultList = new List<string>();

            //    //parse json
            //    try
            //    {
            //        o = Newtonsoft.Json.Linq.JObject.Parse(variableInput);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Error Occured Parsing Tokens: " + ex.ToString());
            //    }


            //    //select results
            //    try
            //    {
            //        searchResults = o.SelectTokens(jsonSelector);
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
            //    }

            //    //add results to result list since list<string> is supported
            //    foreach (var result in searchResults)
            //    {
            //        resultList.Add(result.ToString());
            //    }

            //    //get variable
            //    var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == targetVariableName).FirstOrDefault();

            //    //create if var does not exist
            //    if (requiredComplexVariable == null)
            //    {
            //        engine.VariableList.Add(new Script.ScriptVariable() { VariableName = targetVariableName, CurrentPosition = 0 });
            //        requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == targetVariableName).FirstOrDefault();
            //    }

            //    //assign value to variable
            //    requiredComplexVariable.VariableValue = resultList;

            //}

            var table = DataTableControls.GetFieldValues(v_ParseObjects, "Json Selector", "Output Variable", false);
            foreach(var row in table)
            {
                new GetJSONValueListCommand
                {
                    v_InputValue = this.v_InputValue,
                    v_JsonExtractor = row.Key,
                    v_applyToVariableName = row.Value
                }.RunCommand(engine);
            }
        }
        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    ParseObjectsGridViewHelper = CommandControls.CreateDataGridView(this, "v_ParseObjects");
        //    ParseObjectsGridViewHelper.CellClick += ParseObjectsGridViewHelper_CellClick;

        //    //create standard group controls
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));

        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_ParseObjects", this));
        //    RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_ParseObjects", this, new[] { ParseObjectsGridViewHelper }, editor));

        //    RenderedControls.Add(ParseObjectsGridViewHelper);

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return $"{base.GetDisplayValue()} [Select {v_ParseObjects.Rows.Count} item(s) from JSON]";
        //}

        //public void ParseObjectsGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex >= 0)
        //    {
        //        ParseObjectsGridViewHelper.BeginEdit(false);
        //    }
        //    else
        //    {
        //        ParseObjectsGridViewHelper.EndEdit();
        //    }
        //}

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_ParseObjects)], v_ParseObjects);
        }
    }
}