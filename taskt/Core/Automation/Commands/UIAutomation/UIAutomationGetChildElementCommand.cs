using System;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.Description("This command allows you to get Child Element from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Child Element from AutomationElement. Search only for Child Elements.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetChildElementCommand : ScriptCommand
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
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Root Element")]
        [PropertyVirtualProperty(nameof(AutomationElementControls), nameof(AutomationElementControls.v_InputAutomationElementName))]
        [PropertyDescription("Root AutomationElement Variable")]
        public string v_RootElement { get; set; }

        [XmlElement]
        //[PropertyDescription("Set Search Parameters")]
        //[PropertyCustomUIHelper("GUI Inspect Tool", nameof(lnkGUIInspectTool_Click))]
        //[PropertyCustomUIHelper("Inspect Tool Parser", nameof(lnkInspectToolParser_Click))]
        //[PropertyCustomUIHelper("Add Empty Parameters", nameof(lnkAddEmptyParameter_Click))]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("")]
        //[SampleUsage("")]
        //[Remarks("")]
        //[PropertyIsOptional(true)]
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
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Child Element Index")]
        [InputSpecification("Index", true)]
        //[SampleUsage("**0** or **1** or **{{{vIndex}}}**")]
        [PropertyDetailSampleUsage("**0**", "Specfity the First AutomationElement")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Index")]
        public string v_Index { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please specify a Variable to store Result AutomationElement")]
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
        [PropertyDescription("AutomationElemnet Variable Name to Store Child Element")]
        public string v_AutomationElementVariable { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView SearchParametersGridViewHelper;

        public UIAutomationGetChildElementCommand()
        {
            this.CommandName = "UIAutomationGetChildElementCommand";
            this.SelectionName = "Get Child Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_RootElement.GetAutomationElementVariable(engine);
            int index = v_Index.ConvertToUserVariableAsInteger("v_Index", engine);

            var elems = AutomationElementControls.GetChildrenElements(rootElement, v_SearchParameters, engine);
            if (elems.Count > 0)
            {
                elems[index].StoreInUserVariable(engine, v_AutomationElementVariable);
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