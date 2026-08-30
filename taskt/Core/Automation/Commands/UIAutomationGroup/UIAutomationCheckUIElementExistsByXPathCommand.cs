using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement By XPath")]
    [Attributes.ClassAttributes.CommandSettings("Check UIElement Exists By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to check UIElement existence.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to check UIElement existence.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationCheckUIElementExistsByXPathCommand : ADescendantsSearchUIElementsFromUIElementByXPathCommands, IResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_XPath))]
        //public string v_SearchXPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the UIElement exists, Result value is **True**")]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        //[PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        //[PropertyIsOptional(true, "0")]
        //[PropertyFirstValue("0")]
        //public string v_WaitTimeForUIElement { get; set; }

        public UIAutomationCheckUIElementExistsByXPathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //try
            //{
            //    UIElementControls.SearchGUIElementByXPath(this, engine);
            //    true.StoreInUserVariable(engine, v_Result);
            //}
            //catch
            //{
            //    false.StoreInUserVariable(engine, v_Result);
            //}

            var targetElement = this.ExpandUserVariableAsUIElement(engine);
            this.DeepSearchUIElementAction(engine, targetElement,
                new Action<System.Windows.Automation.AutomationElement>(elem =>
                {
                    true.StoreInUserVariable(engine, v_Result);
                }),
                new Action<Exception>(ex =>
                {
                    false.StoreInUserVariable(engine, v_Result);
                })
            );
        }
    }
}