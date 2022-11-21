using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Xml.Linq;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("UIAutomation Commands")]
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.Description("This command allows you to Wait until the AutomationElement exists.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to Wait until the AutomationElement exists.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationWaitForElementExistCommand : ScriptCommand
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
        [PropertyDisplayText(true, "Root Element")]
        public string v_TargetElement { get; set; }

        [XmlElement]
        [PropertyDescription("Set Search Parameters")]
        [PropertyCustomUIHelper("GUI Inspect Tool", nameof(lnkGUIInspectTool_Click))]
        [PropertyCustomUIHelper("Inspect Tool Parser", nameof(lnkInspectToolParser_Click))]
        [PropertyCustomUIHelper("Add Empty Parameters", nameof(lnkAddEmptyParameter_Click))]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true)]
        [PropertyDataGridViewColumnSettings("Enabled", "Enabled", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.CheckBox)]
        [PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        //[PropertyControlIntoCommandField("SearchParametersGridViewHelper")]
        [PropertyDataGridViewCellEditEvent(nameof(AutomationElementControls) + "+" + nameof(AutomationElementControls.UIAutomationDataGridView_CellBeginEdit), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellBeginEdit)]
        [PropertyDataGridViewCellEditEvent(nameof(AutomationElementControls) + "+" + nameof(AutomationElementControls.UIAutomationDataGridView_CellClick), PropertyDataGridViewCellEditEvent.DataGridViewCellEvent.CellClick)]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify how many seconds to wait for the AutomationElement to exist")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**10** or **{{{vWait}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Wait Time", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.EqualsZero | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Wait", "s")]
        public string v_WaitTime { get; set; }

        //[XmlIgnore]
        //[NonSerialized]
        //private DataGridView SearchParametersGridViewHelper;

        public UIAutomationWaitForElementExistCommand()
        {
            this.CommandName = "UIAutomationWaitForElementExistCommand";
            this.SelectionName = "Wait For Element Exist";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var rootElement = v_TargetElement.GetAutomationElementVariable(engine);

            int pauseTime = v_WaitTime.ConvertToUserVariableAsInteger("Wait Time", engine);
            var stopWaiting = DateTime.Now.AddSeconds(pauseTime);

            bool elementFound = false;
            while (!elementFound)
            {
                AutomationElement elem = AutomationElementControls.SearchGUIElement(rootElement, v_SearchParameters, engine);
                if (elem != null)
                {
                    elementFound = true;
                }

                if (DateTime.Now > stopWaiting)
                {
                    throw new Exception("AutomationElement was not found in time!");
                }

                engine.ReportProgress("AutomationElement Not Yet Found... " + (int)((stopWaiting - DateTime.Now).TotalSeconds) + "s remain");
                System.Threading.Thread.Sleep(1000);
            }
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
    }
}