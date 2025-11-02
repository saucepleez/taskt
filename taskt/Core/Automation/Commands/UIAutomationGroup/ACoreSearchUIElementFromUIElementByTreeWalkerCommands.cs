using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for search UIElement from UIElement by TreeWalker commands
    /// </summary>
    public abstract class ACoreSearchUIElementFromUIElementByTreeWalkerCommands : ADoSomethingUIElementCommands, IUIElementCoreSearchParametersProperties
    {
        [XmlElement]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_SearchParameters))]
        [PropertyParameterOrder(6000)]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeForUIElement))]
        [PropertyParameterOrder(7990)]
        public string v_WaitTimeForUIElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxSiblings))]
        [PropertyParameterOrder(7991)]
        public string v_MaxSiblings { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxNumberUIElements))]
        [PropertyParameterOrder(7992)]
        public string v_MaxNumberUIElements { get; set; }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            UIElementControls.RenderSearchParameterDataGridView(ControlsList.GetPropertyControl<DataGridView>(nameof(v_SearchParameters)));
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_SearchParameters));
            DataTableControls.BeforeValidate_NoRowAdding(dgv, v_SearchParameters);
        }
    }
}
