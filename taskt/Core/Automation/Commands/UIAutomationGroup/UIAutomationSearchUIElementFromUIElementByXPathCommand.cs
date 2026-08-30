using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement By XPath")]
    [Attributes.ClassAttributes.CommandSettings("Search UIElement From UIElement By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement from UIElement using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement from UIElement. XPath does not support to use parent and sibling for root element.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementFromUIElementByXPathCommand : ADescendantsSearchUIElementsFromUIElementByXPathCommands, IResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //[PropertyDescription("UIElement Variable Name to Search")]
        //public string v_TargetElement { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_XPath))]
        //public string v_SearchXPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_NewOutputUIElementName))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        //public string v_WaitTimeForUIElement { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private TextBox XPathTextBox;

        public UIAutomationSearchUIElementFromUIElementByXPathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var elem = UIElementControls.SearchGUIElementByXPath(this, engine);
            //elem.StoreInUserVariable(engine, v_Result);

            var targetElement = this.ExpandUserVariableAsUIElement(engine);
            this.DeepSearchUIElementAction(engine, targetElement, new Action<System.Windows.Automation.AutomationElement>(elem =>
            {
                elem.StoreInUserVariable(engine, v_Result);
            }));
        }
    }
}