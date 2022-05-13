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
    [Attributes.ClassAttributes.Description("This command allows you to get Element Tree XML from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Element Tree XML from AutomationElement. XML content is based on WinAppDriver UI Recorder.")]
    public class UIAutomationGetElementTreeXMLFromElementCommand : ScriptCommand
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
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify a Variable to store XML")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vXML** or **{{{vXML}}}**")]
        [Remarks("XML content is based on WinAppDriver UI Recorder.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_XMLVariable { get; set; }

        public UIAutomationGetElementTreeXMLFromElementCommand()
        {
            this.CommandName = "UIAutomationGetElementTreeXMLFromElementCommand";
            this.SelectionName = "Get Element Tree XML From Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);

            Dictionary<int, AutomationElement> elems;
            var xml = AutomationElementControls.GetXmlFromElement(targetElement, out elems);
            using(System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                xml.Save(sw);
                sw.ToString().StoreInUserVariable(engine, v_XMLVariable);
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
            return base.GetDisplayValue() + " [Target Element: '" + v_TargetElement + "', Store: '" + v_XMLVariable + "']";
        }

    }
}