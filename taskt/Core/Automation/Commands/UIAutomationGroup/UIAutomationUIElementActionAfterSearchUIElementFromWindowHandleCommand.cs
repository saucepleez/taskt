using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search And Action")]
    [Attributes.ClassAttributes.CommandSettings("UIElement Action After Search UIElement From Window Handle")]
    [Attributes.ClassAttributes.Description("This command searches for the UIElement in the specified Window Handle and then takes action on that UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command is used when you want to Search an UIElement by its Window Handle and perform an action on that UIElement.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationUIElementActionAfterSearchUIElementFromWindowHandleCommand : AUIElementActionFromWindowSomethingByTreeWalkerCommands, IWindowHandleProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_InputWindowHandle))]
        [PropertyParameterOrder(5000)]
        public string v_WindowHandle { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        [PropertyParameterOrder(6000)]
        public string v_WaitTimeForWindow { get; set; }

        public UIAutomationUIElementActionAfterSearchUIElementFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.UIElementActionProcess(engine,
                new Action<InnerScriptVariable>(v =>
                {
                    var winElem = new UIAutomationGetWindowUIElementFromWindowHandleCommand()
                    {
                        v_WindowHandle = this.v_WindowHandle,
                        v_Result = v.VariableName,
                        v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                    };
                    winElem.RunCommand(engine);
                }),
                new Action<InnerScriptVariable, string>((v, r) =>
                {
                    var chkElem = new UIAutomationCheckUIElementExistsCommand()
                    {
                        v_TargetElement = v.VariableName,
                        v_SearchParameters = this.v_SearchParameters,
                        v_TargetUIElementIndex = this.v_TargetUIElementIndex,
                        v_WaitTimeForUIElement = this.v_WaitTimeForUIElement,
                        v_Result = r,
                        v_MaxSiblings = this.v_MaxSiblings,
                        v_SiblingsDirection = this.v_SiblingsDirection,
                        v_MaxDepth = this.v_MaxDepth,
                        v_MaxNumberUIElements = this.v_MaxNumberUIElements,
                        v_WindowNameResult = this.v_WindowNameResult,
                        v_WindowHandleResult = this.v_WindowHandleResult,
                    };
                    chkElem.RunCommand(engine);
                }),
                new Action<InnerScriptVariable, InnerScriptVariable>((v, r) =>
                {
                    var trgElem = new UIAutomationSearchUIElementFromUIElementCommand()
                    {
                        v_TargetElement = v.VariableName,
                        v_SearchParameters = this.v_SearchParameters,
                        v_TargetUIElementIndex = this.v_TargetUIElementIndex,
                        v_WaitTimeForUIElement = this.v_WaitTimeForUIElement,
                        v_Result = r.VariableName,
                        v_MaxSiblings = this.v_MaxSiblings,
                        v_SiblingsDirection = this.v_SiblingsDirection,
                        v_MaxDepth = this.v_MaxDepth,
                        v_MaxNumberUIElements = this.v_MaxNumberUIElements,
                        v_WindowNameResult = this.v_WindowNameResult,
                        v_WindowHandleResult = this.v_WindowHandleResult,
                    };
                    trgElem.RunCommand(engine);
                })
            );
        }
    }
}