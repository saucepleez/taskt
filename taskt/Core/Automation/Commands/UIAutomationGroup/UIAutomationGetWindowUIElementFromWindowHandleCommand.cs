using System;
using System.Windows.Automation;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Window UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Window UIElement From Window Handle")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement from Window Handle")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement from Window Handle")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetWindowUIElementFromWindowHandleCommand : AWindowHandleCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_OutputUIElementName))]
        [PropertyParameterOrder(5100)]
        public string v_Result { get; set; }

        public UIAutomationGetWindowUIElementFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.WindowHandleAction(engine, new Action<IntPtr>(whnd =>
            {
                var ele = AutomationElement.FromHandle(whnd);
                ele.StoreInUserVariable(engine, v_Result);
            }));
        }
    }
}