using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Wait For UIElement To Exists By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to Wait until the UIElement exists using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Wait until the UIElement exists using by XPath.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationWaitForUIElementToExistsByXPathCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_XPath))]
        public string v_SearchXPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public UIAutomationWaitForUIElementToExistsByXPathCommand()
        {
            //this.CommandName = "UIAutomationWaitForElementExistByXPathCommand";
            //this.SelectionName = "Wait For Element Exist By XPath";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            UIElementControls.SearchGUIElementByXPath(this, engine);
        }
    }
}