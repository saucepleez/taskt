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
    [Attributes.ClassAttributes.Group("API")]
    [Attributes.ClassAttributes.CommandSettings("Execute REST API")]
    [Attributes.ClassAttributes.Description("This command allows you to show a message to the user.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to present or display a value on screen to the user.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'MessageBox' and invokes VariableCommand to find variable data.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_run_code))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class HTTPExecuteRESTAPICommand : ScriptCommand, IHaveDataTableElements
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Base URL")]
        [InputSpecification("Bsae URL", true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**https://example.com**", "URL")]
        [PropertyDetailSampleUsage("**192.168.1.2**", "URL")]
        [PropertyDetailSampleUsage("**{{{vURL}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "URL")]
        [PropertyValidationRule("URL", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "URL")]
        public string v_BaseURL { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Port")]
        [InputSpecification("Port", true)]
        [PropertyIsOptional(true, "")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**8080**", "URL")]
        [PropertyDetailSampleUsage("**{{{vPort}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "URL")]
        [PropertyValidationRule("URL", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Port")]
        public string v_Port { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Endpoint")]
        [InputSpecification("Endpoint", true)]
        [PropertyShowSampleUsageInDescription(true)]
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
        [PropertyUISelectionOption("PUT")]
        [PropertyUISelectionOption("DELETE")]
        [PropertyUISelectionOption("PATCH")]
        [PropertyValidationRule("Method Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Method")]
        public string v_APIMethodType { get; set; }

        [XmlElement]
        [PropertyDescription("Basic REST Parameters")]
        [InputSpecification("Specify default search parameters")]
        [SampleUsage("")]
        [Remarks("Once you have clicked on a valid window the search parameters will be populated.  Enable only the ones required to be a match at runtime.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true, 400, 140)]
        [PropertyDataGridViewColumnSettings("Parameter Type", "Type", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox, "HEADER\nPARAMETER\nJSON BODY\nFILE")]
        [PropertyDataGridViewColumnSettings("Parameter Name", "Name", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Parameter Value", "Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewCellEditEvent(nameof(DataTableControls) + "+" + nameof(DataTableControls.AllEditableDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_RESTParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }

        [XmlElement]
        [PropertyDescription("Advanced REST Parameters")]
        [InputSpecification("Specify a list of advanced parameters.")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(true, true, true, 400, 140)]
        [PropertyDataGridViewColumnSettings("Parameter Name", "Name", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Parameter Value", "Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Content Type", "Content Type", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("Parameter Type", "Parameter Type", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.ComboBox, "Cookie\nGetOrPost\nHttpHeader\nQueryString\nRequestBody\nURLSegment\nQueryStringWithoutEncode")]
        public DataTable v_AdvancedParameters { get; set; }

        [XmlAttribute]
        [PropertyDetailSampleUsage(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Format Type")]
        [PropertyUISelectionOption("JSON")]
        [PropertyUISelectionOption("XML")]
        [PropertyUISelectionOption("None")]
        [PropertyValidationRule("Format Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyFirstValue("JSON")]
        [PropertyDisplayText(true, "Format")]
        public string v_RequestFormat { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Timeout (sec)")]
        [InputSpecification("Timeout", true)]
        [PropertyIsOptional(true, "60")]
        [PropertyFirstValue("60")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**30**", PropertyDetailSampleUsage.ValueType.Value, "Timeout")]
        [PropertyDetailSampleUsage("**{{{vTime}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Timeout")]
        [PropertyValidationRule("Timeout", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Timeout")]
        public string v_Timeout { get; set; }

        public HTTPExecuteRESTAPICommand()
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

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
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

        public string ExecuteRESTRequest(Engine.AutomationEngineInstance engine)
        {
            // get parameters
            var targetURL = v_BaseURL.ExpandValueOrUserVariable(engine);
            var targetPort = v_Port.ExpandValueOrUserVariable(engine);
            
            if (!targetURL.StartsWith("https://") && !targetURL.StartsWith("http://"))
            {
                targetURL = $"http://{targetURL}";
            }

            if (!string.IsNullOrEmpty(targetPort))
            {
                if (int.TryParse(targetPort, out int p))
                {
                    if (p > 1 && p < 65535)
                    {
                        targetURL += $":{p}";
                    }
                    else
                    {
                        throw new Exception($"Port is Out of Range. Value: '{v_Port}', Expand Value: '{p}'");
                    }
                }
            }

            // client
            var client = new RestClient(targetURL);

            // end point
            var targetEndpoint = v_APIEndPoint.ExpandValueOrUserVariable(engine);

            // methods
            //Method method = (Method)Enum.Parse(typeof(Method), targetMethod);
            var targetMethod = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_APIMethodType), engine);
            var method = (Method)Enum.Parse(typeof(Method), targetMethod, true);

            // rest request
            var request = new RestRequest(targetEndpoint, method);

            // get parameters
            var apiParameters = (from rw in v_RESTParameters.AsEnumerable()
                                 where rw.Field<string>("Parameter Type") == "PARAMETER"
                                 select rw);
            // for each api parameter
            foreach (var param in apiParameters)
            {
                var paramName = ((string)param["Parameter Name"]).ExpandValueOrUserVariable(engine);
                var paramValue = ((string)param["Parameter Value"]).ExpandValueOrUserVariable(engine);

                request.AddParameter(paramName, paramValue);
            }

            // get headers
            var apiHeaders = (from rw in v_RESTParameters.AsEnumerable()
                              where rw.Field<string>("Parameter Type") == "HEADER"
                              select rw);
            // for each header
            foreach (var header in apiHeaders)
            {
                var paramName = ((string)header["Parameter Name"]).ExpandValueOrUserVariable(engine);
                var paramValue = ((string)header["Parameter Value"]).ExpandValueOrUserVariable(engine);

                request.AddHeader(paramName, paramValue);
            }

            // get json body
            var jsonBody = (from rw in v_RESTParameters.AsEnumerable()
                            where rw.Field<string>("Parameter Type") == "JSON BODY"
                            select rw.Field<string>("Parameter Value")).FirstOrDefault();
            // add json body
            if (jsonBody != null)
            {
                var json = jsonBody.ExpandValueOrUserVariable(engine);
                request.AddJsonBody(jsonBody);
            }

            // get file
            var file = (from rw in v_RESTParameters.AsEnumerable()
                        where rw.Field<string>("Parameter Type") == "FILE"
                        select rw).FirstOrDefault();
            // add file
            if (file != null)
            {
                var paramName = ((string)file["Parameter Name"]).ExpandValueOrUserVariable(engine);
                var paramValue = ((string)file["Parameter Value"]).ExpandValueOrUserVariable(engine);
                var fileData = paramValue.ExpandValueOrUserVariable(engine);
                request.AddFile(paramName, fileData);
            }

            // add advanced parameters
            foreach (DataRow rw in this.v_AdvancedParameters.Rows)
            {
                var paramName = rw.Field<string>("Parameter Name").ExpandValueOrUserVariable(engine);
                var paramValue = rw.Field<string>("Parameter Value").ExpandValueOrUserVariable(engine);
                var paramType = rw.Field<string>("Parameter Type").ExpandValueOrUserVariable(engine);
                var contentType = rw.Field<string>("Content Type").ExpandValueOrUserVariable(engine);

                request.AddParameter(paramName, paramValue, (ParameterType)Enum.Parse(typeof(ParameterType), paramType));
            }

            // request format
            var requestFormat = v_RequestFormat.ExpandValueOrUserVariable(engine);
            switch (requestFormat)
            {
                case "none":
                    break;
                default:
                    request.RequestFormat = (DataFormat)Enum.Parse(typeof(DataFormat), requestFormat, true);
                    break;
            }

            // timeout
            if (string.IsNullOrEmpty(v_Timeout))
            {
                v_Timeout = "60";
            }
            var timeout = this.ExpandValueOrUserVariableAsInteger(nameof(v_Timeout), engine);
            request.Timeout = TimeSpan.FromSeconds(timeout);

            // execute client request
            var response = client.Execute<RestResponse>(request);
            var content = response.Content;

            // add service response for tracking
            engine.ServiceResponses.Add(response);

            // return response.Content;
            try
            {
                // try to parse and return json content
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject(content);
                return result.ToString();
            }
            catch (Exception)
            {
                // content failed to parse simply return it
                return content;
            }
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();
            
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_RESTParameters)], v_RESTParameters);
            
            DataTableControls.BeforeValidate((DataGridView)ControlsList[nameof(v_AdvancedParameters)], v_AdvancedParameters);
        }
    }
}

