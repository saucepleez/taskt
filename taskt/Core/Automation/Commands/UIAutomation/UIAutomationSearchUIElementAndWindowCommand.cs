﻿using System;
using System.Data;
using System.Xml.Serialization;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search UIElement & Window")]
    [Attributes.ClassAttributes.CommandSettings("Search UIElement And Window")]
    [Attributes.ClassAttributes.Description("This command allows you to get UIElement from Window Name.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get UIElement from Window Name.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_window))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationSearchUIElementAndWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowName))]
        public string v_WindowName { get; set; }

        [XmlElement]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_OutputUIElementName))]
        public string v_AutomationElementVariable { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_CompareMethod))]
        public string v_CompareMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_MatchMethod_Single))]
        [PropertySelectionChangeEvent(nameof(MatchMethodComboBox_SelectionChangeCommitted))]
        public string v_MatchMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_TargetWindowIndex))]
        public string v_TargetWindowIndex { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WaitTime))]
        public string v_WaitTimeForWindow { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(UIElementControls), nameof(UIElementControls.v_WaitTime))]
        public string v_ElementWaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_WindowNameResult))]
        public string v_NameResult { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WindowControls), nameof(WindowControls.v_OutputWindowHandle))]
        public string v_HandleResult { get; set; }

        public UIAutomationSearchUIElementAndWindowCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var winElem = AutomationElementControls.GetWindowAutomationElement(this, engine);

            //var waitTile = this.ConvertToUserVariableAsInteger(nameof(v_ElementWaitTime), engine);

            //var elem = AutomationElementControls.SearchGUIElement(winElem, v_SearchParameters, waitTile, engine);

            //elem.StoreInUserVariable(engine, v_AutomationElementVariable);

            var varName = VariableNameControls.GetInnerVariableName(0, engine, false);

            var winSearch = new UIAutomationSearchUIElementFromWindowCommand()
            {
                v_WindowName = this.v_WindowName,
                v_CompareMethod = this.v_CompareMethod,
                v_MatchMethod = this.v_MatchMethod,
                v_TargetWindowIndex = this.v_TargetWindowIndex,
                v_WaitTimeForWindow = this.v_WaitTimeForWindow,
                v_AutomationElementVariable = varName,
                v_NameResult = this.v_NameResult,
                v_HandleResult = this.v_HandleResult,
            };
            winSearch.RunCommand(engine);

            var searchElem = new UIAutomationSearchUIElementFromUIElementCommand()
            {
                v_TargetElement = varName,
                v_SearchParameters = this.v_SearchParameters,
                v_AutomationElementVariable = this.v_AutomationElementVariable,
                v_WaitTime = this.v_ElementWaitTime
            };
            searchElem.RunCommand(engine);
        }

        private void MatchMethodComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WindowControls.MatchMethodComboBox_SelectionChangeCommitted(ControlsList, (ComboBox)sender, nameof(v_TargetWindowIndex));
        }

        public override void Refresh(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            ControlsList.GetPropertyControl<ComboBox>(nameof(v_WindowName)).AddWindowNames();
        }

        public override void AfterShown(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        {
            //AutomationElementControls.RenderSearchParameterDataGridView((DataGridView)ControlsList[nameof(v_SearchParameters)]);
            UIElementControls.RenderSearchParameterDataGridView(ControlsList.GetPropertyControl<DataGridView>(nameof(v_SearchParameters)));
        }
    }
}