using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Get")]
    [Attributes.ClassAttributes.Description("This command allows you to get Children Elements Information from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Children Elements Information from AutomationElement.")]
    public class UIAutomationGetChildrenElementsInformationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify AutomationElement Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vElement}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.AutomationElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_RootElement { get; set; }

        [XmlElement]
        [PropertyDescription("Set Search Parameters")]
        [PropertyCustomUIHelper("GUI Inspect Tool", "lnkGUIInspectTool_Click")]
        [PropertyCustomUIHelper("Inspect Tool Parser", "lnkInspectToolParser_Click")]
        [PropertyCustomUIHelper("Add Empty Parameters", "lnkAddEmptyParameter_Click")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true)]
        [PropertyDataGridViewColumnSettings("Enabled", "Enabled", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.CheckBox)]
        [PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify a Variable to store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ResultVariable { get; set; }

        public UIAutomationGetChildrenElementsInformationCommand()
        {
            this.CommandName = "UIAutomationGetChildrenElementsInformationCommand";
            this.SelectionName = "Get Children Elements Information";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_RootElement.GetAutomationElementVariable(engine);

            var elems = AutomationElementControls.GetChildrenElements(targetElement, v_SearchParameters, engine);

            string result = "";

            int counts = elems.Count;
            for (int i = 0; i < counts; i++)
            {
                var elem = elems[i];
                result += "Index: " + i + ", Name: " + elem.Current.Name + ", LocalizedControlType: " + elem.Current.LocalizedControlType + ", ControlType: " + AutomationElementControls.GetControlTypeText(elem.Current.ControlType) + "\n";
            }
            result.Trim().StoreInUserVariable(engine, v_ResultVariable);
        }

        private void lnkAddEmptyParameter_Click(object sender, EventArgs e)
        {
            AutomationElementControls.CreateEmptyParamters(v_SearchParameters);
        }

        private void lnkInspectToolParser_Click(object sender, EventArgs e)
        {
            AutomationElementControls.InspectToolParserClicked(v_SearchParameters);
        }
        private void lnkGUIInspectTool_Click(object sender, EventArgs e)
        {
            AutomationElementControls.GUIInspectTool_UsedByInspectResult_Clicked(v_SearchParameters);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrl = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrl);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Root Element: '" + v_RootElement + "', Result: '" + v_ResultVariable + "']";
        }

    }
}