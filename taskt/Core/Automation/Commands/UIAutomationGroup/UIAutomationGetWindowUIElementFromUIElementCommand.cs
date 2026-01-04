using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Window UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Get Window UIElement From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get Window UIElement from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Window UIElement from UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationGetWindowUIElementFromUIElementCommand : AGetFromUIElementCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_OutputUIElementName))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        public UIAutomationGetWindowUIElementFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var whnd = new InnerScriptVariable(engine))
            {
                var getWinHandle = new UIAutomationGetWindowHandleFromUIElementCommand()
                {
                    v_TargetElement = this.v_TargetElement,
                    v_WindowHandleResult = whnd.VariableName,
                    v_WindowNameResult = this.v_WindowNameResult,
                };
                getWinHandle.RunCommand(engine);

                var getWinElem = new UIAutomationGetWindowUIElementFromWindowHandleCommand()
                {
                    v_WindowHandle = whnd.VariableValue.ToString(),
                    v_Result = this.v_Result,
                };
                getWinElem.RunCommand(engine);
                if (!string.IsNullOrEmpty(this.v_WindowHandleResult))
                {
                    whnd.VariableValue.ToString().StoreInUserVariable(engine, v_WindowHandleResult);
                }
            }
        }
    }
}