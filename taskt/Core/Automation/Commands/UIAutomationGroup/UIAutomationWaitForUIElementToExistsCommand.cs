using System;
using taskt.Core.Automation.Commands.UIAutomationGroup;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Wait For UIElement To Exists")]
    [Attributes.ClassAttributes.Description("This command allows you to Wait until the UIElement exists.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Wait until the UIElement exists.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class UIAutomationWaitForUIElementToExistsCommand : ADeepSearchOneUIElementFromUIElementByTreeWalkerCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        //public string v_TargetElement { get; set; }

        //[XmlElement]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_SearchParameters))]
        //public DataTable v_SearchParameters { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        //public string v_WaitTimeForUIElement { get; set; }

        public UIAutomationWaitForUIElementToExistsCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //UIElementControls.SearchGUIElement(this, engine);

            var targetElement = this.ExpandUserVariableAsUIElement(engine);
            this.DeepSearchUIElements(targetElement, engine);
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