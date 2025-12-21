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
    [Attributes.ClassAttributes.CommandSettings("Search UIElements Tree XML From Window Name")]
    [Attributes.ClassAttributes.Description("This command allows you to Search UIElements Tree XML from Window Name.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Search UIElements Tree XML from Window Name. XML content is based on WinAppDriver UI Recorder.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementsTreeXMLFromWindowNameCommand : AOneWindowNameCommands, IGetUIElementsXMLTreeFromSomethingProperties
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to store XML")]
        [PropertyDetailSampleUsage("**vXML**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [PropertyDetailSampleUsage("**{{{vXML}}}**", PropertyDetailSampleUsage.ValueType.VariableName)]
        [Remarks("XML content is based on WinAppDriver UI Recorder.")]
        [PropertyParameterOrder(6500)]
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

        public UIAutomationSearchUIElementsTreeXMLFromWindowNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var winElem = new InnerScriptVariable(engine))
            {
                var getWin = new UIAutomationGetWindowUIElementCommand()
                {
                    v_WindowName = this.v_WindowName,
                    v_CheckMethod = this.v_CheckMethod,
                    v_SelectionMethod = this.v_SelectionMethod,
                    v_Result = winElem.VariableName,
                    v_TargetWindowIndex =this.v_TargetWindowIndex,
                    v_CaseSensitive = this.v_CaseSensitive,
                    v_TrimBeforeCheck = this.v_TrimBeforeCheck,
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                };
                getWin.RunCommand(engine);

                this.StoreUIElementsTreeXMLInUserVariableFromUIElement((AutomationElement)winElem.VariableValue, engine);
            }
        }
    }
}