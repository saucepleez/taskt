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
    [Attributes.ClassAttributes.CommandSettings("Search UIElements Tree XML From Window Handle")]
    [Attributes.ClassAttributes.Description("This command allows you to Search UIElements Tree XML from Window Handle.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Search UIElements Tree XML from Window Handle. XML content is based on WinAppDriver UI Recorder.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementsTreeXMLFromWindowHandleCommand : AWindowHandleCommands, IGetUIElementsXMLTreeFromSomethingProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to store XML")]
        [PropertyDetailSampleUsage("**vXML**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vXML}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("XML content is based on WinAppDriver UI Recorder.")]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeForUIElement))]
        [PropertyParameterOrder(7990)]
        public string v_WaitTimeForUIElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxSiblings))]
        [PropertyParameterOrder(7991)]
        public string v_MaxSiblings { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxDepth))]
        [PropertyParameterOrder(7992)]
        public string v_MaxDepth { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        [PropertyParameterOrder(8200)]
        public string v_WindowHandleResult { get; set; }

        public UIAutomationSearchUIElementsTreeXMLFromWindowHandleCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var winElem = new InnerScriptVariable(engine))
            {
                var getWin = new UIAutomationGetWindowUIElementFromWindowHandleCommand()
                {
                    v_WindowHandle = this.v_WindowHandle,
                    v_Result = winElem.VariableName,
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                };
                getWin.RunCommand(engine);

                this.StoreUIElementsTreeXMLInUserVariableFromUIElement((AutomationElement)winElem.VariableValue, engine);
            }
        }
    }
}