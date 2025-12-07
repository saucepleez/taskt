using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement By XPath")]
    [Attributes.ClassAttributes.CommandSettings("Search Child UIElement From UIElement By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to get Child UIElement from UIElement using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Child UIElement from UIElement. XPath does not support to use parent and sibling for root element.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchChildUIElementFromUIElementByXPathCommand : ACoreSearchUIElementsFromUIElementByXPathCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_NewOutputUIElementName))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        public UIAutomationSearchChildUIElementFromUIElementByXPathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var targetElement = this.ExpandUserVariableAsUIElement(engine);
            this.SearchChildrenUIElementAction(engine, targetElement, new Action<System.Windows.Automation.AutomationElement>(elem =>
            {
                elem.StoreInUserVariable(engine, v_Result);
            }));
        }
    }
}