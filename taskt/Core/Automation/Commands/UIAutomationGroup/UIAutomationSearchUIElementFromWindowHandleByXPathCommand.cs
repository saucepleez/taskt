using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement From Window")]
    [Attributes.ClassAttributes.CommandSettings("Search UIElement From Window Handle By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement from Window Handle using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement from Window Handle. XPath does not support to use parent and sibling for root element.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementFromWindowHandleByXPathCommand : ADescendantsSearchUIElementsFromWindowHandleByXPathCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_OutputUIElementName))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        
        public UIAutomationSearchUIElementFromWindowHandleByXPathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using(var winElem = new InnerScriptVariable(engine))
            {
                var winSearch = new UIAutomationGetWindowUIElementFromWindowHandleCommand()
                {
                    v_WindowHandle = this.v_WindowHandle,
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                    v_Result = winElem.VariableName,
                    v_WindowNameResult = this.v_WindowNameResult,
                };
                winSearch.RunCommand(engine);

                var searchElem = new UIAutomationSearchUIElementFromUIElementByXPathCommand()
                {
                    v_TargetElement = winElem.VariableName,
                    v_SearchXPath = this.v_SearchXPath,
                    v_Result = this.v_Result,
                    v_WaitTimeForUIElement = this.v_WaitTimeForUIElement,
                    v_MaxDepth = this.v_MaxDepth,
                    v_MaxSiblings = this.v_MaxSiblings,
                };
                searchElem.RunCommand(engine);

                this.StoreWindowUIElementInUserVariable((AutomationElement)winElem.VariableValue, engine);
            }
        }
    }
}