using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Search UIElement From UIElement")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement from UIElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement from UIElement. Search for Descendants Elements.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationSearchUIElementFromUIElementCommand : ADeepSearchAnyUIElementFromUIElementByTreeWalkerCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //[PropertyDescription("UIElement Variable Name to Search")]
        //public string v_TargetElement { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_SearchParameters))]
        //public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_NewOutputUIElementName))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        //public string v_WaitTimeForUIElement { get; set; }

        public UIAutomationSearchUIElementFromUIElementCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var elem = UIElementControls.SearchGUIElement(this, engine);
            //elem.StoreInUserVariable(engine, v_Result);

            var targetElement = this.ExpandUserVariableAsUIElement(engine);
            var ret = this.GetUIElementFromDeepSearchUIElements(targetElement, engine);
            ret.StoreInUserVariable(engine, v_Result);
        }

        //public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    //AutomationElementControls.RenderSearchParameterDataGridView((DataGridView)ControlsList[nameof(v_SearchParameters)]);
        //    UIElementControls.RenderSearchParameterDataGridView(ControlsList.GetPropertyControl<DataGridView>(nameof(v_SearchParameters)));
        //}

        //public override void BeforeValidate()
        //{
        //    base.BeforeValidate();

        //    var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_SearchParameters));
        //    DataTableControls.BeforeValidate_NoRowAdding(dgv, v_SearchParameters);
        //}
    }
}