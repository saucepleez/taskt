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
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to select a Item in AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to select a Item in AutomationElement.")]
    public class UIAutomationSelectItemInElementCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify AutomationElement Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vElement** or **{{{vElement}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyValidationRule("AutomationElement", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetElement { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Item value to Select")]
        [InputSpecification("")]
        [SampleUsage("**Hello** or **{{{vItem}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Item", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Item { get; set; }

        public UIAutomationSelectItemInElementCommand()
        {
            this.CommandName = "UIAutomationSelectItemInElementCommand";
            this.SelectionName = "Select Item In Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);

            var itemName = v_Item.ConvertToUserVariable(engine);

            var items = AutomationElementControls.GetSelectionItems(targetElement, true);
            bool isSelected = false;
            foreach(var item in items)
            {
                if (item.Current.Name == itemName)
                {
                    SelectionItemPattern selPtn = (SelectionItemPattern)item.GetCurrentPattern(SelectionItemPattern.Pattern);
                    selPtn.Select();
                    isSelected = true;
                    break;
                }
            }

            if (!isSelected)
            {
                throw new Exception("Item '" + v_Item + "' does not exists");
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
            return base.GetDisplayValue() + " [Root Element: '" + v_TargetElement + "', Store: '" + v_Item + "']";
        }

    }
}