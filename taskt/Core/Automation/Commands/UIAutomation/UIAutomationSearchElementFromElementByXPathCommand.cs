using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.CommandSettings("Search Element From Element By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to get AutomationElement from AutomationElement using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get AutomationElement from AutomationElement. XPath does not support to use parent and sibling for root element.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSearchElementFromElementByXPathCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        [PropertyDescription("AutomationElement Variable Name to Search")]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_XPath))]
        public string v_SearchXPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_NewOutputAutomationElementName))]
        public string v_AutomationElementVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private TextBox XPathTextBox;

        public UIAutomationSearchElementFromElementByXPathCommand()
        {
            //this.CommandName = "UIAutomationGetElementFromElementByXPathCommand";
            //this.SelectionName = "Get Element From Element By XPath";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var elem = AutomationElementControls.SearchGUIElementByXPath(this, engine);
            elem.StoreInUserVariable(engine, v_AutomationElementVariable);
        }
    }
}