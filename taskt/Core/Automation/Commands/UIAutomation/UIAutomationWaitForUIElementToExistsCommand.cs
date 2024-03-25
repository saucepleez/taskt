﻿using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Windows.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement")]
    [Attributes.ClassAttributes.CommandSettings("Wait For UIElement To Exists")]
    [Attributes.ClassAttributes.Description("This command allows you to Wait until the UIElement exists.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Wait until the UIElement exists.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationWaitForUIElementToExistsCommand : ScriptCommand, IHaveDataTableElements
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_InputUIElementName))]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        public UIAutomationWaitForUIElementToExistsCommand()
        {
            //this.CommandName = "UIAutomationWaitForElementExistCommand";
            //this.SelectionName = "Wait For Element Exist";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            UIElementControls.SearchGUIElement(this, engine);
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            //AutomationElementControls.RenderSearchParameterDataGridView((DataGridView)ControlsList[nameof(v_SearchParameters)]);
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