using System;
using System.Collections.Generic;
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
    [Description("This command parses and extracts data from an HTML source object or a successful **GetHTMLSourceCommand**.")]

    public class QueryHTMLSourceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("HTML")]
        [InputSpecification("Enter the HTML to be queried.")]
        [SampleUsage("<!DOCTYPE html><html><head><title>Example</title></head></html> || {vMyHTML}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_HTMLVariable { get; set; }

        [XmlAttribute]
        [PropertyDescription("XPath Query")]
        [InputSpecification("Enter the XPath Query and the item will be extracted.")]
        [SampleUsage("@//*[@id=\"aso_search_form_anchor\"]/div/input || {vMyXPath}")]
        [Remarks("You can use Chrome Dev Tools to click an element and copy the XPath.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_XPathQuery { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Query Result Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required" +
                 " to pre-define your variables; however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public QueryHTMLSourceCommand()
        {
            CommandName = "QueryHTMLSourceCommand";
            SelectionName = "Query HTML Source";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(v_HTMLVariable.ConvertToUserVariable(engine));

            var div = doc.DocumentNode.SelectSingleNode(v_XPathQuery);
            string divString = div.InnerText;

            divString.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_HTMLVariable", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_XPathQuery", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Query '{v_HTMLVariable}' - Store Result in '{v_OutputUserVariableName}']";
        }
    }
}