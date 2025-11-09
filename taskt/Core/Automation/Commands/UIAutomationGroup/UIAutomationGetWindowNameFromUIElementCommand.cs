using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Get From UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Window Name From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Window Name from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Window Name from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetWindowNameFromUIElementCommand : AGetFromUIElementCommands
    {
        
        [XmlAttribute]
        [PropertyIsOptional(false)]
        [PropertyValidationRule("Window Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyParameterOrder(6000)]
        public override string v_WindowNameResult { get; set; }

        public UIAutomationGetWindowNameFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.UIElementAction(engine,
                new Action<AutomationElement>((targetElement) =>
                {
                    // nothing
                })
            );
        }
    }
}