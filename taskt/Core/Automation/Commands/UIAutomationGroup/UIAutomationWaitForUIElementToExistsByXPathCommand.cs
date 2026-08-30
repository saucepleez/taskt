using System;
using System.Windows.Automation;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement By XPath")]
    [Attributes.ClassAttributes.CommandSettings("Wait For UIElement To Exists By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to Wait until the UIElement exists using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Wait until the UIElement exists using by XPath.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationWaitForUIElementToExistsByXPathCommand : ADescendantsSearchUIElementsFromUIElementByXPathCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_XPath))]
        //public string v_SearchXPath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        //public string v_WaitTimeForUIElement { get; set; }

        public UIAutomationWaitForUIElementToExistsByXPathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //UIElementControls.SearchGUIElementByXPath(this, engine);
            var targetElement = this.ExpandUserVariableAsUIElement(engine);

            this.DeepSearchUIElementAction(engine, targetElement, new Action<AutomationElement>(elem =>
            {
                // nothing
            }));
        }
    }
}