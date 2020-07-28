using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Web Browser Commands")]
    [Description("This command downloads the HTML source of a web page for parsing without using browser automation.")]

    public class GetHTMLSourceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("URL")]
        [InputSpecification("Enter a valid URL that you want to collect data from.")]
        [SampleUsage("http://mycompany.com/news || {vCompany}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WebRequestURL { get; set; }

        [XmlAttribute]
        [PropertyDescription("Execute Request As Logged On User")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Sets currently logged on user authentication information for the request.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_WebRequestCredentials { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Response Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public GetHTMLSourceCommand()
        {
            CommandName = "GetHTMLSourceCommand";
            SelectionName = "Get HTML Source";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(v_WebRequestURL.ConvertToUserVariable(engine));
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            if (v_WebRequestCredentials == "Yes")
            {
                request.Credentials = CredentialCache.DefaultCredentials;
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string strResponse = reader.ReadToEnd();

            strResponse.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_WebRequestURL", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_WebRequestCredentials", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Target URL '{v_WebRequestURL}' - Store Response in '{v_OutputUserVariableName}']";
        }
    }
}