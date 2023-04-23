using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get HTML Text By XPath")]
    [Attributes.ClassAttributes.Description("This command processes an HTML source object")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to parse and extract data from a successful **HTTP Request Command**")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class HTMLGetHTMLTextByXPath : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        [PropertyDescription("HTML")]
        [InputSpecification("HTML", true)]
        [PropertyDetailSampleUsageBehavior(MultiAttributesBehavior.Overwrite)]
        [PropertyDetailSampleUsage("**{{{vHTML}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "HTML")]
        [PropertyDetailSampleUsage("**<html><head>...**", PropertyDetailSampleUsage.ValueType.Value, "HTML")]
        [PropertyValidationRule("HTML", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "HTML")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("XPath Query")]
        [InputSpecification("XPath Query", true)]
        [SampleUsage("@//*[@id=\"aso_search_form_anchor\"]/div/input")]
        [Remarks("You can use Chrome Dev Tools to click an element and copy the XPath.")]
        [PropertyValidationRule("XPath", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "XPath")]
        public string v_xPathQuery { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }


        public HTMLGetHTMLTextByXPath()
        {
            //this.CommandName = "HTTPRequestQueryCommand";
            //this.SelectionName = "HTTP Result Query";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(v_userVariableName.ConvertToUserVariable(sender));

            var div = doc.DocumentNode.SelectSingleNode(v_xPathQuery);
            string divString = div.InnerText;

            divString.StoreInUserVariable(sender, v_applyToVariableName);
        }
    }
}