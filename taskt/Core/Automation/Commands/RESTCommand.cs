using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using RestSharp;
using System.Data;
using System.Drawing;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("API Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to show a message to the user.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    public class RESTCommand : ScriptCommand
    {
        //[XmlAttribute]
        //[Attributes.PropertyAttributes.PropertyDescription("Please enter an instance name")]
        //[Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[Attributes.PropertyAttributes.InputSpecification("")]
        //[Attributes.PropertyAttributes.SampleUsage("")]
        //[Attributes.PropertyAttributes.Remarks("")]
        //public string v_InstanceName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the Base URL (ex. http://mysite.com)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define any API endpoint which contains the full URL.")]
        [Attributes.PropertyAttributes.SampleUsage("**https://example.com** or **{vMyUrl}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_BaseURL { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the endpoint (Ex. /v2/endpoint)")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Define any API endpoint which contains the full URL.")]
        [Attributes.PropertyAttributes.SampleUsage("**/v2/getUser/1** or **{vMyUrl}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_APIEndPoint { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select method type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("GET")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("POST")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary method type.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_APIMethodType { get; set; }

        [Attributes.PropertyAttributes.PropertyDescription("Advanced REST Parameters")]
        [Attributes.PropertyAttributes.InputSpecification("Specify a list of advanced parameters.")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("")]
        public DataTable v_AdvancedParameters { get; set; }

        [Attributes.PropertyAttributes.PropertyDescription("Basic REST Parameters")]
        [Attributes.PropertyAttributes.InputSpecification("Specify default search parameters")]
        [Attributes.PropertyAttributes.SampleUsage("n/a")]
        [Attributes.PropertyAttributes.Remarks("Once you have clicked on a valid window the search parameters will be populated.  Enable only the ones required to be a match at runtime.")]
        public DataTable v_RESTParameters { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply Result To Variable")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select method type")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Json")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Xml")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("None")]
        [Attributes.PropertyAttributes.InputSpecification("Select the necessary method type.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_RequestFormat { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView RESTParametersGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView AdvancedRESTParametersGridViewHelper;
        public RESTCommand()
        {
            this.CommandName = "RESTCommand";
            this.SelectionName = "Execute REST API";
            this.CommandEnabled = true;
            this.CustomRendering = true;
            this.v_RequestFormat = "Json";
            this.v_RESTParameters = new DataTable();
            this.v_RESTParameters.Columns.Add("Parameter Type");
            this.v_RESTParameters.Columns.Add("Parameter Name");
            this.v_RESTParameters.Columns.Add("Parameter Value");
            this.v_RESTParameters.TableName = DateTime.Now.ToString("RESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            //advanced parameters

            this.v_AdvancedParameters = new DataTable();
         
            this.v_AdvancedParameters.Columns.Add("Parameter Name");
            this.v_AdvancedParameters.Columns.Add("Parameter Value");
            this.v_AdvancedParameters.Columns.Add("Content Type");
            this.v_AdvancedParameters.Columns.Add("Parameter Type");
            this.v_AdvancedParameters.TableName = DateTime.Now.ToString("AdvRESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
        }

        public override void RunCommand(object sender)
        {
           // var engine = (Engine.AutomationEngineInstance)sender;

            try
            {
                //make REST Request and store into variable
                var restContent = ExecuteRESTRequest(sender);
                restContent.StoreInUserVariable(sender, v_userVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
         
            

        }
        public string ExecuteRESTRequest(object sender)
        {
            //get engine instance
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

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

                var param = new Parameter();

                if (!string.IsNullOrEmpty(contentType))
                    param.ContentType = contentType;

                if (!string.IsNullOrEmpty(paramType))
                    param.Type = (ParameterType)System.Enum.Parse(typeof(ParameterType), paramType);
                
                   
                param.Name = paramName;
                param.Value = paramValue;

                request.Parameters.Add(param);
            }

            var requestFormat = v_RequestFormat.ConvertToUserVariable(sender);
            if (string.IsNullOrEmpty(requestFormat))
            {
                requestFormat = "Xml";
            }
            request.RequestFormat = (DataFormat)System.Enum.Parse(typeof(DataFormat), requestFormat);
     
            
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

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

           var baseURLControlSet = CommandControls.CreateDefaultInputGroupFor("v_BaseURL", this, editor);
            RenderedControls.AddRange(baseURLControlSet);

            var apiEndPointControlSet = CommandControls.CreateDefaultInputGroupFor("v_APIEndPoint", this, editor);
            RenderedControls.AddRange(apiEndPointControlSet);
      

            var apiMethodLabel = CommandControls.CreateDefaultLabelFor("v_APIMethodType", this);
            var apiMethodDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_APIMethodType", this);
     
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_RequestFormat", this, editor));

            foreach (Method method in (Method[])Enum.GetValues(typeof(Method)))
            {
                apiMethodDropdown.Items.Add(method.ToString());
            }

     
            RenderedControls.Add(apiMethodLabel);
            RenderedControls.Add(apiMethodDropdown);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_RESTParameters", this));

            RESTParametersGridViewHelper = new DataGridView();
            RESTParametersGridViewHelper.Width = 500;
            RESTParametersGridViewHelper.Height = 140;

            RESTParametersGridViewHelper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            RESTParametersGridViewHelper.AutoGenerateColumns = false;

            var selectColumn = new DataGridViewComboBoxColumn();
            selectColumn.HeaderText = "Type";
            selectColumn.DataPropertyName = "Parameter Type";
            selectColumn.DataSource = new string[] { "HEADER", "PARAMETER", "JSON BODY", "FILE" };
            RESTParametersGridViewHelper.Columns.Add(selectColumn);

            var paramNameColumn = new DataGridViewTextBoxColumn();
            paramNameColumn.HeaderText = "Name";
            paramNameColumn.DataPropertyName = "Parameter Name";
            RESTParametersGridViewHelper.Columns.Add(paramNameColumn);

            var paramValueColumn = new DataGridViewTextBoxColumn();
            paramValueColumn.HeaderText = "Value";
            paramValueColumn.DataPropertyName = "Parameter Value";
            RESTParametersGridViewHelper.Columns.Add(paramValueColumn);
       
            RESTParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_RESTParameters", false, DataSourceUpdateMode.OnPropertyChanged);
            RenderedControls.Add(RESTParametersGridViewHelper);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_AdvancedParameters", this));

            //advanced parameters
            AdvancedRESTParametersGridViewHelper = new DataGridView();
            AdvancedRESTParametersGridViewHelper.Width = 500;
            AdvancedRESTParametersGridViewHelper.Height = 140;

            AdvancedRESTParametersGridViewHelper.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            AdvancedRESTParametersGridViewHelper.AutoGenerateColumns = false;

            var advParamNameColumn = new DataGridViewTextBoxColumn();
            advParamNameColumn.HeaderText = "Name";
            advParamNameColumn.DataPropertyName = "Parameter Name";
            AdvancedRESTParametersGridViewHelper.Columns.Add(advParamNameColumn);

            var advParamValueColumns = new DataGridViewTextBoxColumn();
            advParamValueColumns.HeaderText = "Value";
            advParamValueColumns.DataPropertyName = "Parameter Value";
            AdvancedRESTParametersGridViewHelper.Columns.Add(advParamValueColumns);

            var advParamContentType = new DataGridViewTextBoxColumn();
            advParamContentType.HeaderText = "Content Type";
            advParamContentType.DataPropertyName = "Content Type";
            AdvancedRESTParametersGridViewHelper.Columns.Add(advParamContentType);


            var advParamType = new DataGridViewComboBoxColumn();
            advParamType.HeaderText = "Parameter Type";
            advParamType.DataPropertyName = "Parameter Type";
            advParamType.DataSource = new string[] { "Cookie", "GetOrPost", "HttpHeader", "QueryString", "RequestBody", "URLSegment", "QueryStringWithoutEncode"};
            AdvancedRESTParametersGridViewHelper.Columns.Add(advParamType);

            AdvancedRESTParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_AdvancedParameters", false, DataSourceUpdateMode.OnPropertyChanged);
            RenderedControls.Add(AdvancedRESTParametersGridViewHelper);


            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
            var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_userVariableName", this, new Control[] { VariableNameControl }, editor));
            RenderedControls.Add(VariableNameControl);

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [URL: " + v_BaseURL +  v_APIEndPoint + "]";
        }

    }

}

