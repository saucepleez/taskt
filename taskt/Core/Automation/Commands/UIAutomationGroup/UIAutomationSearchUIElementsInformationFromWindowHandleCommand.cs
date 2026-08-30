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
    [Attributes.ClassAttributes.CommandSettings("Search UIElements Information From Window Handle")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElements Information from Window Handle.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElements Information from Window Handle.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementsInformationFromWindowHandleCommand : ADescendantsSearchUIElementsFromWindowHandleByTreeWalkerCommands, IResultProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6200)]
        public string v_Result { get; set; }

        public UIAutomationSearchUIElementsInformationFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var winElem = new InnerScriptVariable(engine))
            {
                var winSearch = new UIAutomationGetWindowUIElementFromWindowHandleCommand()
                {
                    v_WindowHandle = this.v_WindowHandle,
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                    v_Result = winElem.VariableName,
                    v_WindowNameResult = this.v_WindowNameResult,
                };
                winSearch.RunCommand(engine);

                var searchInfo = new UIAutomationSearchUIElementsInformationFromUIElementCommand()
                {
                    v_TargetElement = winElem.VariableName,
                    v_SearchParameters = this.v_SearchParameters,
                    v_Result = this.v_Result,
                    v_MaxSiblings = this.v_MaxSiblings,
                    v_MaxDepth = this.v_MaxDepth,
                    v_MaxNumberUIElements = this.v_MaxNumberUIElements,
                    v_SiblingsDirection = this.v_SiblingsDirection,
                    v_WaitTimeForUIElement = this.v_WaitTimeForUIElement,
                };
                searchInfo.RunCommand(engine);

                this.StoreWindowUIElementInUserVariable((AutomationElement)winElem.VariableValue, engine);
            }
        }
    }
}