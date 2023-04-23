using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using RestSharp;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("API Commands")]
    [Attributes.ClassAttributes.CommandSettings("Execute REST API")]
    [Attributes.ClassAttributes.Description("This command allows you to show a message to the user.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RESTCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Base URL")]
        [InputSpecification("Bsae URL", true)]
        //[SampleUsage("**https://example.com** or **{vMyUrl}**")]
        [PropertyDetailSampleUsage("**https://example.com**", "URL")]
        [PropertyDetailSampleUsage("**{{{vURL}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "URL")]
        [PropertyValidationRule("URL", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "URL")]
        public string v_BaseURL { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Endpoint")]
        [InputSpecification("Endpoint", true)]
        //[SampleUsage("**/v2/getUser/1** or **{vMyUrl}**")]
        [PropertyDetailSampleUsage("**/v2/getUser/1**", PropertyDetailSampleUsage.ValueType.Value, "Endpoint")]
        [PropertyDetailSampleUsage("**{{{vMyURL}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Endpoint")]
        [PropertyValidationRule("Endpoint", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Endpint")]
        public string v_APIEndPoint { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Method Type")]
        [PropertyUISelectionOption("GET")]
        [PropertyUISelectionOption("POST")]
        [PropertyValidationRule("Method Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Method")]
        public string v_APIMethodType { get; set; }

        [XmlElement]
        [PropertyDescription("Advanced REST Parameters")]
        [InputSpecification("Specify a list of advanced parameters.")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true, 400, 140)]
        [PropertyDataGridViewColumnSettings("Parameter Type", "Type", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox, "HEADER\nPARAMETER\nJSON BODY\nFILE")]
        [PropertyDataGridViewColumnSettings("Parameter Name", "Name", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Parameter Value", "Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_AdvancedParameters { get; set; }

        [XmlElement]
        [PropertyDescription("Basic REST Parameters")]
        [InputSpecification("Specify default search parameters")]
        [SampleUsage("")]
        [Remarks("Once you have clicked on a valid window the search parameters will be populated.  Enable only the ones required to be a match at runtime.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true, 400, 140)]
        [PropertyDataGridViewColumnSettings("Parameter Name", "Name", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Parameter Value", "Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Content Type", "Content Type", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Parameter Type", "Parameter Type", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox, "Cookie\nGetOrPost\nHttpHeader\nQueryString\nRequestBody\nURLSegment\nQueryStringWithoutEncode")]
        public DataTable v_RESTParameters { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Apply Result To Variable")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDetailSampleUsage(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Format Type")]
        [PropertyUISelectionOption("Json")]
        [PropertyUISelectionOption("Xml")]
        [PropertyUISelectionOption("None")]
        [PropertyValidationRule("Format Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyFirstValue("Json")]
        [PropertyDisplayText(true, "Format")]
        public string v_RequestFormat { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView RESTParametersGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView AdvancedRESTParametersGridViewHelper;

        public RESTCommand()
        {
            //this.CommandName = "RESTCommand";
            //this.SelectionName = "Execute REST API";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_RequestFormat = "Json";
            //this.v_RESTParameters = new DataTable();
            //this.v_RESTParameters.Columns.Add("Parameter Type");
            //this.v_RESTParameters.Columns.Add("Parameter Name");
            //this.v_RESTParameters.Columns.Add("Parameter Value");
            //this.v_RESTParameters.TableName = DateTime.Now.ToString("RESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            ////advanced parameters

            //this.v_AdvancedParameters = new DataTable();
         
            //this.v_AdvancedParameters.Columns.Add("Parameter Name");
            //this.v_AdvancedParameters.Columns.Add("Parameter Value");
            //this.v_AdvancedParameters.Columns.Add("Content Type");
            //this.v_AdvancedParameters.Columns.Add("Parameter Type");
            //this.v_AdvancedParameters.TableName = DateTime.Now.ToString("AdvRESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            try
            {
                //make REST Request and store into variable
                var restContent = ExecuteRESTRequest(engine);
                restContent.StoreInUserVariable(engine, v_userVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ExecuteRESTRequest(object sender)
        {
            //get engine instance
            var engine = (Engine.AutomationEngineInstance)sender;

            //get parameters
            var targetURL = v_BaseURL.ConvertToUserVariable(engine);
            var targetEndpoint = v_APIEndPoint.ConvertToUserVariable(engine);
            var targetMethod = v_APIMethodType.ConvertToUserVariable(engine);

            //client
            var client = new RestClient(targetURL);

            //methods
            Method method = (Method)Enum.Parse(typeof(Method), targetMethod);
            
            //rest request
            var request = new RestRequest(targetEndpoint, method);

            //get parameters
            var apiParameters = (from rw in v_RESTParameters.AsEnumerable()
                                 where rw.Field<string>("Parameter Type") == "PARAMETER"
                                 select rw);

            //get headers
            var apiHeaders = (from rw in v_RESTParameters.AsEnumerable()
                              where rw.Field<string>("Parameter Type") == "HEADER"
                              select rw);

            //for each api parameter
            foreach (var param in apiParameters)
            {
                var paramName = ((string)param["Parameter Name"]).ConvertToUserVariable(sender);
                var paramValue = ((string)param["Parameter Value"]).ConvertToUserVariable(sender);

                request.AddParameter(paramName, paramValue);
            }

            //for each header
            foreach (var header in apiHeaders)
            {
                var paramName = ((string)header["Parameter Name"]).ConvertToUserVariable(sender);
                var paramValue = ((string)header["Parameter Value"]).ConvertToUserVariable(sender);

                request.AddHeader(paramName, paramValue);
            }

            //get json body
            var jsonBody = (from rw in v_RESTParameters.AsEnumerable()
                            where rw.Field<string>("Parameter Type") == "JSON BODY"
                            select rw.Field<string>("Parameter Value")).FirstOrDefault();
            //add json body
            if (jsonBody != null)
            {
                var json = jsonBody.ConvertToUserVariable(sender);
                request.AddJsonBody(jsonBody);
            }

            //get json body
            var file = (from rw in v_RESTParameters.AsEnumerable()
                        where rw.Field<string>("Parameter Type") == "FILE"
                        select rw).FirstOrDefault();

            //get file
            if (file != null)
            {
                var paramName = ((string)file["Parameter Name"]).ConvertToUserVariable(sender);
                var paramValue = ((string)file["Parameter Value"]).ConvertToUserVariable(sender);
                var fileData = paramValue.ConvertToUserVariable(sender);
                request.AddFile(paramName, fileData);

            }

            //add advanced parameters
            foreach (DataRow rw in this.v_AdvancedParameters.Rows)
            {
                var paramName = rw.Field<string>("Parameter Name").ConvertToUserVariable(sender);
                var paramValue = rw.Field<string>("Parameter Value").ConvertToUserVariable(sender);
                var paramType = rw.Field<string>("Parameter Type").ConvertToUserVariable(sender);
                var contentType = rw.Field<string>("Content Type").ConvertToUserVariable(sender);

                var param = new Parameter(paramName, paramValue, (ParameterType)Enum.Parse(typeof(ParameterType), paramType));

                request.Parameters.Add(param);
            }

            var requestFormat = v_RequestFormat.ConvertToUserVariable(sender);
            if (string.IsNullOrEmpty(requestFormat))
            {
                requestFormat = "Xml";
            }
            request.RequestFormat = (DataFormat)Enum.Parse(typeof(DataFormat), requestFormat);
     
            
            //execute client request
            IRestResponse response = client.Execute(request);
            var content = response.Content;

            //add service response for tracking
            engine.ServiceResponses.Add(response);

            // return response.Content;
            try
            {
                //try to parse and return json content
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                return result.ToString();
            }
            catch (Exception)
            {
                //content failed to parse simply return it
                return content;
            }
        }
        public override void BeforeValidate()
        {
            base.BeforeValidate();
            //if (RESTParametersGridViewHelper.IsCurrentCellDirty || RESTParametersGridViewHelper.IsCurrentRowDirty)
            //{
            //    RESTParametersGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //    var newRow = v_RESTParameters.NewRow();
            //    v_RESTParameters.Rows.Add(newRow);

            //    for (var i = v_RESTParameters.Rows.Count - 1; i >= 0; i--)
            //    {
            //        if (v_RESTParameters.Rows[i][0].ToString() == "" && v_RESTParameters.Rows[i][1].ToString() == "" && v_RESTParameters.Rows[i][2].ToString() == "")
            //        {
            //            v_RESTParameters.Rows[i].Delete();
            //        }
            //    }
            //}

            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_RESTParameters)], v_RESTParameters);

            //if (AdvancedRESTParametersGridViewHelper.IsCurrentCellDirty || AdvancedRESTParametersGridViewHelper.IsCurrentRowDirty)
            //{
            //    AdvancedRESTParametersGridViewHelper.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //    var newRow = v_AdvancedParameters.NewRow();
            //    v_AdvancedParameters.Rows.Add(newRow);

            //    for (var i = v_AdvancedParameters.Rows.Count - 1; i >= 0; i--)
            //    {
            //        if (v_AdvancedParameters.Rows[i][0].ToString() == "" && v_AdvancedParameters.Rows[i][1].ToString() == "" && v_AdvancedParameters.Rows[i][2].ToString() == "" && v_AdvancedParameters.Rows[i][3].ToString() == "")
            //        {
            //            v_AdvancedParameters.Rows[i].Delete();
            //        }
            //    }
            //}

            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_AdvancedParameters)], v_AdvancedParameters);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var baseURLControlSet = CommandControls.CreateDefaultInputGroupFor("v_BaseURL", this, editor);
        //    RenderedControls.AddRange(baseURLControlSet);

        //    var apiEndPointControlSet = CommandControls.CreateDefaultInputGroupFor("v_APIEndPoint", this, editor);
        //    RenderedControls.AddRange(apiEndPointControlSet);


        //    var apiMethodLabel = CommandControls.CreateDefaultLabelFor("v_APIMethodType", this);
        //    var apiMethodDropdown = (ComboBox)CommandControls.CreateDefaultDropdownFor("v_APIMethodType", this);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_RequestFormat", this, editor));

        //    foreach (Method method in (Method[])Enum.GetValues(typeof(Method)))
        //    {
        //        apiMethodDropdown.Items.Add(method.ToString());
        //    }


        //    RenderedControls.Add(apiMethodLabel);
        //    RenderedControls.Add(apiMethodDropdown);

        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_RESTParameters", this));

        //    //RESTParametersGridViewHelper = new DataGridView();
        //    //RESTParametersGridViewHelper.Width = 500;
        //    //RESTParametersGridViewHelper.Height = 140;

        //    //RESTParametersGridViewHelper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        //    //RESTParametersGridViewHelper.AutoGenerateColumns = false;

        //    RESTParametersGridViewHelper = CommandControls.CreateDefaultDataGridViewFor("v_RESTParameters", this, true, true, false, -1, 140, false );
        //    RESTParametersGridViewHelper.CellClick += RESTParametersGridViewHelper_CellClick;

        //    var selectColumn = new DataGridViewComboBoxColumn();
        //    selectColumn.HeaderText = "Type";
        //    selectColumn.DataPropertyName = "Parameter Type";
        //    selectColumn.DataSource = new string[] { "HEADER", "PARAMETER", "JSON BODY", "FILE" };
        //    RESTParametersGridViewHelper.Columns.Add(selectColumn);

        //    var paramNameColumn = new DataGridViewTextBoxColumn();
        //    paramNameColumn.HeaderText = "Name";
        //    paramNameColumn.DataPropertyName = "Parameter Name";
        //    RESTParametersGridViewHelper.Columns.Add(paramNameColumn);

        //    var paramValueColumn = new DataGridViewTextBoxColumn();
        //    paramValueColumn.HeaderText = "Value";
        //    paramValueColumn.DataPropertyName = "Parameter Value";
        //    RESTParametersGridViewHelper.Columns.Add(paramValueColumn);

        //    //RESTParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_RESTParameters", false, DataSourceUpdateMode.OnPropertyChanged);
        //    RenderedControls.Add(RESTParametersGridViewHelper);

        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_AdvancedParameters", this));

        //    //advanced parameters
        //    //AdvancedRESTParametersGridViewHelper = new DataGridView();
        //    //AdvancedRESTParametersGridViewHelper.Width = 500;
        //    //AdvancedRESTParametersGridViewHelper.Height = 140;

        //    //AdvancedRESTParametersGridViewHelper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
        //    //AdvancedRESTParametersGridViewHelper.AutoGenerateColumns = false;
        //    AdvancedRESTParametersGridViewHelper = CommandControls.CreateDefaultDataGridViewFor("v_AdvancedParameters", this, true, true, false, -1, 140, false);
        //    AdvancedRESTParametersGridViewHelper.CellClick += AdvancedRESTParametersGridViewHelper_CellClick;

        //    var advParamNameColumn = new DataGridViewTextBoxColumn();
        //    advParamNameColumn.HeaderText = "Name";
        //    advParamNameColumn.DataPropertyName = "Parameter Name";
        //    AdvancedRESTParametersGridViewHelper.Columns.Add(advParamNameColumn);

        //    var advParamValueColumns = new DataGridViewTextBoxColumn();
        //    advParamValueColumns.HeaderText = "Value";
        //    advParamValueColumns.DataPropertyName = "Parameter Value";
        //    AdvancedRESTParametersGridViewHelper.Columns.Add(advParamValueColumns);

        //    var advParamContentType = new DataGridViewTextBoxColumn();
        //    advParamContentType.HeaderText = "Content Type";
        //    advParamContentType.DataPropertyName = "Content Type";
        //    AdvancedRESTParametersGridViewHelper.Columns.Add(advParamContentType);


        //    var advParamType = new DataGridViewComboBoxColumn();
        //    advParamType.HeaderText = "Parameter Type";
        //    advParamType.DataPropertyName = "Parameter Type";
        //    advParamType.DataSource = new string[] { "Cookie", "GetOrPost", "HttpHeader", "QueryString", "RequestBody", "URLSegment", "QueryStringWithoutEncode"};
        //    AdvancedRESTParametersGridViewHelper.Columns.Add(advParamType);

        //    //AdvancedRESTParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_AdvancedParameters", false, DataSourceUpdateMode.OnPropertyChanged);
        //    RenderedControls.Add(AdvancedRESTParametersGridViewHelper);


        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_userVariableName", this, VariableNameControl, editor));
        //    RenderedControls.Add(VariableNameControl);

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [URL: " + v_BaseURL +  v_APIEndPoint + "]";
        //}

        //public void RESTParametersGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex >= 0)
        //    {
        //        if (e.ColumnIndex != 0)
        //        {
        //            RESTParametersGridViewHelper.BeginEdit(false);
        //        }
        //        else if (RESTParametersGridViewHelper.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
        //        {
        //            // Parameter type is empty
        //            SendKeys.Send("{F4}");  // show combobox list
        //        }
        //    }
        //    else
        //    {
        //        RESTParametersGridViewHelper.EndEdit();
        //    }
        //}

        //public void AdvancedRESTParametersGridViewHelper_CellClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex >= 0)
        //    {
        //        if (e.ColumnIndex != 3)
        //        {
        //            AdvancedRESTParametersGridViewHelper.BeginEdit(false);
        //        }
        //        else if (AdvancedRESTParametersGridViewHelper.Rows[e.RowIndex].Cells[3].Value.ToString() == "")
        //        {
        //            // parameter type is empty
        //            SendKeys.Send("{F4}");  // show combobx list
        //        }
        //    }
        //    else
        //    {
        //        AdvancedRESTParametersGridViewHelper.EndEdit();
        //    }
        //}
    }
}

