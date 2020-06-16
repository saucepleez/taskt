using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Group("API Commands")]
    [Description("This command calls a REST API with a specific HTTP method.")]

    public class ExecuteRESTAPICommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Base URL")]
        [InputSpecification("Provide the base URL of the API.")]
        [SampleUsage("https://example.com || {vMyUrl}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_BaseURL { get; set; }

        [XmlAttribute]
        [PropertyDescription("Endpoint")]
        [InputSpecification("Define any API endpoint which contains the full URL.")]
        [SampleUsage("/v2/getUser/1 || {vMyUrl}")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_APIEndPoint { get; set; }

        [XmlAttribute]
        [PropertyDescription("Method Type")]
        [PropertyUISelectionOption("GET")]
        [PropertyUISelectionOption("POST")]
        [InputSpecification("Select the necessary method type.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_APIMethodType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Request Format")]
        [PropertyUISelectionOption("Json")]
        [PropertyUISelectionOption("Xml")]
        [PropertyUISelectionOption("None")]
        [InputSpecification("Select the necessary request format.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_RequestFormat { get; set; }

        [PropertyDescription("Basic REST Parameters")]
        [InputSpecification("Specify default search parameters.")]
        [SampleUsage("")]
        [Remarks("Once you have clicked on a valid window the search parameters will be populated." +
                 " Enable only the ones required to be a match at runtime.")]
        public DataTable v_RESTParameters { get; set; }

        [PropertyDescription("Advanced REST Parameters")]
        [InputSpecification("Specify a list of advanced parameters.")]
        [SampleUsage("")]
        [Remarks("")]
        public DataTable v_AdvancedParameters { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Response Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _RESTParametersGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _advancedRESTParametersGridViewHelper;

        public ExecuteRESTAPICommand()
        {
            CommandName = "ExecuteRESTAPICommand";
            SelectionName = "Execute REST API";
            CommandEnabled = true;
            CustomRendering = true;
            v_RequestFormat = "Json";

            v_RESTParameters = new DataTable();
            v_RESTParameters.Columns.Add("Parameter Type");
            v_RESTParameters.Columns.Add("Parameter Name");
            v_RESTParameters.Columns.Add("Parameter Value");
            v_RESTParameters.TableName = DateTime.Now.ToString("RESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            //advanced parameters
            v_AdvancedParameters = new DataTable();
            v_AdvancedParameters.Columns.Add("Parameter Name");
            v_AdvancedParameters.Columns.Add("Parameter Value");
            v_AdvancedParameters.Columns.Add("Content Type");
            v_AdvancedParameters.Columns.Add("Parameter Type");
            v_AdvancedParameters.TableName = DateTime.Now.ToString("AdvRESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
        }

        public override void RunCommand(object sender)
        {
            try
            {
                //make REST Request and store into variable
                string restContent;

                //get engine instance
                var engine = (AutomationEngineInstance)sender;

                //get parameters
                var targetURL = v_BaseURL.ConvertToUserVariable(sender);
                var targetEndpoint = v_APIEndPoint.ConvertToUserVariable(sender);
                var targetMethod = v_APIMethodType.ConvertToUserVariable(sender);

                //client
                var client = new RestClient(targetURL);

                //methods
                Method method = (Method)Enum.Parse(typeof(Method), targetMethod);

                //rest request
                var request = new RestRequest(targetEndpoint, method);

                //get parameters
                var apiParameters = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "PARAMETER");

                //get headers
                var apiHeaders = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "HEADER");

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
                var jsonBody = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "JSON BODY")
                                               .Select(rw => rw.Field<string>("Parameter Value")).FirstOrDefault();

                //add json body
                if (jsonBody != null)
                {
                    var json = jsonBody.ConvertToUserVariable(sender);
                    request.AddJsonBody(jsonBody);
                }

                //get json body
                var file = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "FILE").FirstOrDefault();

                //get file
                if (file != null)
                {
                    var paramName = ((string)file["Parameter Name"]).ConvertToUserVariable(sender);
                    var paramValue = ((string)file["Parameter Value"]).ConvertToUserVariable(sender);
                    var fileData = paramValue.ConvertToUserVariable(sender);
                    request.AddFile(paramName, fileData);

                }

                //add advanced parameters
                foreach (DataRow rw in v_AdvancedParameters.Rows)
                {
                    var paramName = rw.Field<string>("Parameter Name").ConvertToUserVariable(sender);
                    var paramValue = rw.Field<string>("Parameter Value").ConvertToUserVariable(sender);
                    var paramType = rw.Field<string>("Parameter Type").ConvertToUserVariable(sender);
                    var contentType = rw.Field<string>("Content Type").ConvertToUserVariable(sender);

                    var param = new Parameter();

                    if (!string.IsNullOrEmpty(contentType))
                        param.ContentType = contentType;

                    if (!string.IsNullOrEmpty(paramType))
                        param.Type = (ParameterType)Enum.Parse(typeof(ParameterType), paramType);


                    param.Name = paramName;
                    param.Value = paramValue;

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
                    var result = JsonConvert.DeserializeObject(content);
                    restContent = result.ToString();
                }
                catch (Exception)
                {
                    //content failed to parse simply return it
                    restContent = content;
                }

                restContent.StoreInUserVariable(sender, v_OutputUserVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_BaseURL", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_APIEndPoint", this, editor));

            var apiMethodLabel = CommandControls.CreateDefaultLabelFor("v_APIMethodType", this);
            var apiMethodDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_APIMethodType", this);
            foreach (Method method in (Method[])Enum.GetValues(typeof(Method)))
            {
                apiMethodDropdown.Items.Add(method.ToString());
            }
            RenderedControls.Add(apiMethodLabel);
            RenderedControls.Add(apiMethodDropdown);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_RequestFormat", this, editor));
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_RESTParameters", this));

            _RESTParametersGridViewHelper = new DataGridView();
            _RESTParametersGridViewHelper.Width = 500;
            _RESTParametersGridViewHelper.Height = 140;

            _RESTParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _RESTParametersGridViewHelper.AutoGenerateColumns = false;

            var selectColumn = new DataGridViewComboBoxColumn();
            selectColumn.HeaderText = "Type";
            selectColumn.DataPropertyName = "Parameter Type";
            selectColumn.DataSource = new string[] { "HEADER", "PARAMETER", "JSON BODY", "FILE" };
            _RESTParametersGridViewHelper.Columns.Add(selectColumn);

            var paramNameColumn = new DataGridViewTextBoxColumn();
            paramNameColumn.HeaderText = "Name";
            paramNameColumn.DataPropertyName = "Parameter Name";
            _RESTParametersGridViewHelper.Columns.Add(paramNameColumn);

            var paramValueColumn = new DataGridViewTextBoxColumn();
            paramValueColumn.HeaderText = "Value";
            paramValueColumn.DataPropertyName = "Parameter Value";
            _RESTParametersGridViewHelper.Columns.Add(paramValueColumn);

            _RESTParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_RESTParameters", false, DataSourceUpdateMode.OnPropertyChanged);
            RenderedControls.Add(_RESTParametersGridViewHelper);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_AdvancedParameters", this));

            //advanced parameters
            _advancedRESTParametersGridViewHelper = new DataGridView();
            _advancedRESTParametersGridViewHelper.Width = 500;
            _advancedRESTParametersGridViewHelper.Height = 140;

            _advancedRESTParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _advancedRESTParametersGridViewHelper.AutoGenerateColumns = false;

            var advParamNameColumn = new DataGridViewTextBoxColumn();
            advParamNameColumn.HeaderText = "Name";
            advParamNameColumn.DataPropertyName = "Parameter Name";
            _advancedRESTParametersGridViewHelper.Columns.Add(advParamNameColumn);

            var advParamValueColumns = new DataGridViewTextBoxColumn();
            advParamValueColumns.HeaderText = "Value";
            advParamValueColumns.DataPropertyName = "Parameter Value";
            _advancedRESTParametersGridViewHelper.Columns.Add(advParamValueColumns);

            var advParamContentType = new DataGridViewTextBoxColumn();
            advParamContentType.HeaderText = "Content Type";
            advParamContentType.DataPropertyName = "Content Type";
            _advancedRESTParametersGridViewHelper.Columns.Add(advParamContentType);

            var advParamType = new DataGridViewComboBoxColumn();
            advParamType.HeaderText = "Parameter Type";
            advParamType.DataPropertyName = "Parameter Type";
            advParamType.DataSource = new string[] { "Cookie", "GetOrPost", "HttpHeader", "QueryString", "RequestBody", "URLSegment", "QueryStringWithoutEncode" };
            _advancedRESTParametersGridViewHelper.Columns.Add(advParamType);

            _advancedRESTParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_AdvancedParameters", false, DataSourceUpdateMode.OnPropertyChanged);
            RenderedControls.Add(_advancedRESTParametersGridViewHelper);

            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Target URL '{v_BaseURL}{v_APIEndPoint}' - Store Response in '{v_OutputUserVariableName}']";
        }
    }
}

