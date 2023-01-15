using System;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.Description("This command allows you to get AutomationElement from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get AutomationElement from AutomationElement. Search for Descendants Elements.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetElementFromElementCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please specify AutomationElement Variable")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**{{{vElement}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.AutomationElement, true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        //[PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Root Element")]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        [PropertyDescription("AutomationElement Variable Name to Search")]
        public string v_TargetElement { get; set; }

        [XmlElement]
        //[PropertyDescription("Set Search Parameters")]
        //[PropertyCustomUIHelper("GUI Inspect Tool", nameof(lnkGUIInspectTool_Click))]
        //[PropertyCustomUIHelper("Inspect Tool Parser", nameof(lnkInspectToolParser_Click))]
        //[PropertyCustomUIHelper("Add Empty Parameters", nameof(lnkAddEmptyParameter_Click))]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        //[PropertyDataGridViewSetting(false, false, true)]
        //[PropertyDataGridViewColumnSettings("Enabled", "Enabled", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.CheckBox)]
        //[PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        //[PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        ////[PropertyControlIntoCommandField("SearchParametersGridViewHelper")]
        //[PropertyDataGridViewCellEditEvent(nameof(AutomationElementControls) + "+" + nameof(AutomationElementControls.UIAutomationDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        //[PropertyDataGridViewCellEditEvent(nameof(AutomationElementControls) + "+" + nameof(AutomationElementControls.UIAutomationDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_SearchParameters))]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify a Variable to store Result AutomationElement")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**vElement** or **{{{vElement}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.AutomationElement, true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Result AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Store")]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_NewOutputAutomationElementName))]
        public string v_AutomationElementVariable { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView SearchParametersGridViewHelper;

        public UIAutomationGetElementFromElementCommand()
        {
            this.CommandName = "UIAutomationGetElementFromElementCommand";
            this.SelectionName = "Get Element From Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            AutomationElement elem = AutomationElementControls.SearchGUIElement(rootElement, v_SearchParameters, engine);
            if (elem != null)
            {
                elem.StoreInUserVariable(engine, v_AutomationElementVariable);
            }
            else
            {
                throw new Exception("AutomationElement not found");
            }
        }

        //private void lnkAddEmptyParameter_Click(object sender, EventArgs e)
        //{
        //    AutomationElementControls.CreateEmptyParamters(v_SearchParameters);
        //}

        //private void lnkInspectToolParser_Click(object sender, EventArgs e)
        //{
        //    AutomationElementControls.InspectToolParserClicked(v_SearchParameters);
        //}
        //private void lnkGUIInspectTool_Click(object sender, EventArgs e)
        //{
        //    AutomationElementControls.GUIInspectTool_UsedByInspectResult_Clicked(v_SearchParameters);
        //}
    }
}