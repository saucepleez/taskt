using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands.UIAutomationGroup
{
    /// <summary>
    /// for search UIElement from UIElements by TreeWalker commands
    /// </summary>
    public abstract class ACoreSearchUIElementsFromUIElementByTreeWalkerCommands : ADoSomethingUIElementCommands, IUIElementCoreSearchParametersProperties
    {
        [XmlElement]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_SearchParameters))]
        [PropertyParameterOrder(6000)]
        public virtual DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_WaitTimeForUIElement))]
        [PropertyParameterOrder(7990)]
        public virtual string v_WaitTimeForUIElement { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxSiblings))]
        [PropertyParameterOrder(7991)]
        public virtual string v_MaxSiblings { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_MaxNumberUIElements))]
        [PropertyParameterOrder(7995)]
        public virtual string v_MaxNumberUIElements { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VP_UIElementControls), nameof(VP_UIElementControls.v_SiblingsDirection))]
        [PropertyParameterOrder(7996)]
        public virtual string v_SiblingsDirection { get; set; }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            EM_UIElementCoreSearchParametersPropertiesExtensionMethods.RenderUIElementSearchParameter(ControlsList.GetPropertyControl<DataGridView>(nameof(v_SearchParameters)));
        }

        public override void BeforeValidate()
        {
            base.BeforeValidate();

            var dgv = FormUIControls.GetPropertyControl<DataGridView>(ControlsList, nameof(v_SearchParameters));
            DataTableControls.BeforeValidate_NoRowAdding(dgv, v_SearchParameters);
        }
    }
}
