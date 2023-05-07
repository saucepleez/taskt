using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Xml.Linq;
using System.Xml.XPath;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.CommandSettings("Check Element Exist By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to check AutomationElement existence.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to check AutomationElement existence.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationCheckElementExistByXPathCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_XPath))]
        public string v_SearchXPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Element exists, Result value is **True**")]
        public string v_Result { get; set; }

        public UIAutomationCheckElementExistByXPathCommand()
        {
            //this.CommandName = "UIAutomationCheckElementExistByXPathCommand";
            //this.SelectionName = "Check Element Exist By XPath";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            //XElement xml = AutomationElementControls.GetElementXml(rootElement, out _);
            (var xml, _) = AutomationElementControls.GetElementXml(rootElement);

            string xpath = v_SearchXPath.ConvertToUserVariable(engine);
            if (!xpath.StartsWith("."))
            {
                xpath = "." + xpath;
            }

            XElement resElem = xml.XPathSelectElement(xpath);
            (resElem != null).StoreInUserVariable(engine, v_Result);
        }
    }
}