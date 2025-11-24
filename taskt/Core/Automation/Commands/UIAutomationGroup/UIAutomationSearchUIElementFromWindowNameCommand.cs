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
    [Attributes.ClassAttributes.CommandSettings("Search UIElement From Window Name")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement from Window Name.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement from Window Name.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementFromWindowNameCommand : ADeepSearchAnyUIElementFromWindowNameByTreeWalkerCommands, IWindowUIElementResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        //[PropertyParameterOrder(5000)]
        //public string v_WindowName { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_SearchParameters))]
        //public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_OutputUIElementName))]
        [PropertyParameterOrder(6200)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_CheckMethod))]
        //[PropertyParameterOrder(8100)]
        //public string v_CheckMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_SelectionMethod_Single))]
        //[PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        //[PropertyParameterOrder(8200)]
        //public string v_SelectionMethod { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_TargetWindowIndex))]
        //[PropertyParameterOrder(8300)]
        //public string v_TargetWindowIndex { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        //[PropertyParameterOrder(8300)]
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
        //[PropertyParameterOrder(10200)]
        //public string v_WindowUIElement { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_CaseSensitive))]
        //[PropertyParameterOrder(11000)]
        //public string v_CaseSensitive { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_TrimBeforeCheck))]
        //[PropertyParameterOrder(11100)]
        //public string v_TrimBeforeCheck { get; set; }

        public UIAutomationSearchUIElementFromWindowNameCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            using (var winElem = new InnerScriptVariable(engine))
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

                var searchElem = new UIAutomationSearchUIElementFromUIElementCommand()
                {
                    v_TargetElement = winElem.VariableName,
                    v_SearchParameters = this.v_SearchParameters,
                    v_TargetUIElementIndex = this.v_TargetUIElementIndex,
                    v_Result = this.v_Result,
                    v_WaitTimeForUIElement = this.v_WaitTimeForUIElement,
                    v_MaxSiblings = this.v_MaxSiblings,
                    v_MaxDepth = this.v_MaxDepth,
                    v_MaxNumberUIElements = this.v_MaxNumberUIElements,
                    v_SiblingsDirection = this.v_SiblingsDirection,
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

        //public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    //AutomationElementControls.RenderSearchParameterDataGridView((DataGridView)ControlsList[nameof(v_SearchParameters)]);
        //    UIElementControls.RenderSearchParameterDataGridView(ControlsList.GetPropertyControl<DataGridView>(nameof(v_SearchParameters)));
        //}

        //public override void BeforeValidate()
        //{
        //    var dgvSearch = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_SearchParameters));
        //    DataTableControls.BeforeValidate(dgvSearch, v_SearchParameters);
        //}
    }
}