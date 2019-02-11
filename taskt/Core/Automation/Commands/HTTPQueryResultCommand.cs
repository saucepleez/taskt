using System;
using System.Xml.Serialization;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("API Commands")]
    [Attributes.ClassAttributes.Description("This command processes an HTML source object")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to parse and extract data from a successful **HTTP Request Command**")]
    [Attributes.ClassAttributes.ImplementationDescription("TBD")]
    public class HTTPQueryResultCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select variable containing HTML")]
        [Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        [Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("XPath Query")]
        [Attributes.PropertyAttributes.InputSpecification("Enter the XPath Query and the item will be extracted.")]
        [Attributes.PropertyAttributes.SampleUsage("@//*[@id=\"aso_search_form_anchor\"]/div/input")]
        [Attributes.PropertyAttributes.Remarks("You can use Chrome Dev Tools to click an element and copy the XPath.")]
        public string v_xPathQuery { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Apply Query Result To Variable")]
        public string v_applyToVariableName { get; set; }

        public HTTPQueryResultCommand()
        {
            this.CommandName = "HTTPRequestQueryCommand";
            this.SelectionName = "HTTP Result Query";
            this.CommandEnabled = true;
        }

        public override void RunCommand(object sender)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(v_userVariableName.ConvertToUserVariable(sender));

            var div = doc.DocumentNode.SelectSingleNode(v_xPathQuery);
            string divString = div.InnerText;

            divString.StoreInUserVariable(sender, v_applyToVariableName);


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Query Variable '" + v_userVariableName + "' and apply result to '" + v_applyToVariableName + "']";
        }
    }
}