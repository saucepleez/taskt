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
    [Attributes.ClassAttributes.CommandSettings("Search UIElement From Window Name By XPath")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement from Window Name using by XPath.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement from Window Name. XPath does not support to use parent and sibling for root element.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementFromWindowNameByXPathCommand : ADescendantsSearchUIElementsFromWindowNameByXPathCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        //public string v_WindowName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_XPath))]
        //public string v_SearchXPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_OutputUIElementName))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_CheckMethod))]
        //public string v_CheckMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_SelectionMethod_Single))]
        //[PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        //public string v_SelectionMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_TargetWindowIndex))]
        //public string v_TargetWindowIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        //public string v_WaitTimeForWindow { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        //public string v_WaitTimeForUIElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowNameResult))]
        //public string v_WindowNameResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        //public string v_WindowHandleResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WindowUIElementName))]
        //public string v_WindowUIElement { get;set; }

        public UIAutomationSearchUIElementFromWindowNameByXPathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using(var winElem = new InnerScriptVariable(engine))
            {
                var winSearch = new UIAutomationGetWindowUIElementCommand()
                {
                    v_WindowName = this.v_WindowName,
                    v_CheckMethod = this.v_CheckMethod,
                    v_SelectionMethod = this.v_SelectionMethod,
                    v_TargetWindowIndex = this.v_TargetWindowIndex,
                    v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                    v_Result = winElem.VariableName,
                    v_WindowNameResult = this.v_WindowNameResult,
                    v_WindowHandleResult = this.v_WindowHandleResult,
                    v_CaseSensitive = this.v_CaseSensitive,
                    v_TrimBeforeCheck = this.v_TrimBeforeCheck,
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

        //private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        //{
        //    WindowControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        //}

        //public override void Refresh(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        //}
    }
}