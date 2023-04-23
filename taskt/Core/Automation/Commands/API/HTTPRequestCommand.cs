using System;
using System.Xml.Serialization;
using System.Net;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("API Commands")]
    [Attributes.ClassAttributes.CommandSettings("HTTP Request")]
    [Attributes.ClassAttributes.Description("This command downloads the HTML source of a web page for parsing")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to retrieve HTML of a web page without using browser automation.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements System.Web to achieve automation")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class HTTPRequestCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("URL")]
        [InputSpecification("URL", true)]
        //[SampleUsage("http://mycompany.com/news or {vURL}")]
        [PropertyDetailSampleUsage("**http://mycompany.com/news**", PropertyDetailSampleUsage.ValueType.Value, "URL")]
        [PropertyDetailSampleUsage("**{{{vURL}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "URL")]
        [Remarks("")]
        [PropertyValidationRule("URL", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "URL")]
        public string v_WebRequestURL { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionControls), nameof(SelectionControls.v_YesNoComboBox))]
        [PropertyDescription("Execute Request as the currently logged on user?")]
        //[PropertyUISelectionOption("Yes")]
        //[PropertyUISelectionOption("No")]
        //[InputSpecification("Sets currently logged on user authentication information for the request.")]
        //[SampleUsage("Select 'Yes' or 'No'")]
        //[Remarks("")]
        public string v_WebRequestCredentials { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Apply Result To Variable")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_userVariableName { get; set; }


        //[XmlIgnore]
        //[NonSerialized]
        //public ComboBox VariableNameControl;

        public HTTPRequestCommand()
        {
            //this.CommandName = "HTTPRequestCommand";
            //this.SelectionName = "HTTP Request";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(v_WebRequestURL.ConvertToUserVariable(sender));
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";

            //if (v_WebRequestCredentials == "Yes")
            if (this.GetYesNoSelectionValue(nameof(v_WebRequestCredentials), engine))
            {
                request.Credentials = CredentialCache.DefaultCredentials;
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string strResponse = reader.ReadToEnd();

            strResponse.StoreInUserVariable(sender, v_userVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create inputs for request url
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_WebRequestURL", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_WebRequestCredentials", this, editor));

        //    //create window name helper control
        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_userVariableName", this, VariableNameControl, editor));
        //    RenderedControls.Add(VariableNameControl);

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Target URL: '" + v_WebRequestURL + "' and apply result to '" + v_userVariableName + "']";
        //}

    }
}