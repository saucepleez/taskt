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
    [Attributes.ClassAttributes.SubGruop("Search")]
    [Attributes.ClassAttributes.Description("This command allows you to get Element from Table AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to get Element from Table AutomationElement.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UIAutomationGetElementFromTableElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify AutomationElement Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vElement}}}**")]
        [Remarks("Supported target Element is ListBox, DataGridView, etc.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.AutomationElement, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Element")]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Row Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** **{{{vRow}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Row", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Row")]
        public string v_Row { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Column Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **{{{vColumn}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Column", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Column")]
        public string v_Column { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify a Variable to store Result AutomationElement")]
        [InputSpecification("")]
        [SampleUsage("**vElement** or **{{{vElement}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_AutomationElementVariable { get; set; }

        public UIAutomationGetElementFromTableElementCommand()
        {
            this.CommandName = "UIAutomationGetElementFromTableElementCommand";
            this.SelectionName = "Get Element From Table Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);
            int row = v_Row.ConvertToUserVariableAsInteger("v_Row", engine);
            int column = v_Column.ConvertToUserVariableAsInteger("v_Column", engine);

            AutomationElement cellElem = AutomationElementControls.GetTableElement(targetElement, row, column);
            cellElem.StoreInUserVariable(engine, v_AutomationElementVariable);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrl = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrl);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Target Element: '" + v_TargetElement + "', Row: " + v_Row + ", Column: " + v_Column + ", Store: '" + v_AutomationElementVariable + "']";
        //}

    }
}