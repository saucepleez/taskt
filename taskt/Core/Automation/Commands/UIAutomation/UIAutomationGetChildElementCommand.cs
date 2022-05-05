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
    [Attributes.ClassAttributes.Description("")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class UIAutomationGetChildElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify AutomationElement Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Input or Type the name of the window that you want to activate or bring forward.")]
        [SampleUsage("**Untitled - Notepad** or **%kwd_current_window%** or **{{{vWindowName}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_RootElement { get; set; }

        [XmlElement]
        [PropertyDescription("Set Search Parameters")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyCustomUIHelper("Add Empty Parameters", "lnkAddEmptyParameter_Click")]
        [InputSpecification("Use the Element Recorder to generate a listing of potential search parameters.")]
        [SampleUsage("n/a")]
        [Remarks("Once you have clicked on a valid window the search parameters will be populated.  Enable only the ones required to be a match at runtime.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.DataGridView)]
        [PropertyDataGridViewSetting(false, false, true)]
        [PropertyDataGridViewColumnSettings("Enabled", "Enabled", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.CheckBox)]
        [PropertyDataGridViewColumnSettings("ParameterName", "Parameter Name", true, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        [PropertyDataGridViewColumnSettings("ParameterValue", "Parameter Value", false, PropertyDataGridViewColumnSettings.DataGridViewColumnType.TextBox)]
        public DataTable v_SearchParameters { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify a Child Element Index")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Index { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify a Variable to store Result AutomationElement")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_AutomationElementVariable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView SearchParametersGridViewHelper;

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

            var elems = AutomationElementControls.GetAllGUIElements(rootElement, v_SearchParameters, engine);
            if (elems.Count > 0)
            {
                elems[index].StoreInUserVariable(engine, v_AutomationElementVariable);
            }
            else
            {
                throw new Exception("AutomationElement not found");
            }
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
            return base.GetDisplayValue() + " [Root Element: '" + v_RootElement + "', Store: '" + v_AutomationElementVariable + "']";
        }

    }
}