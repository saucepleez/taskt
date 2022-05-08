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
    [Attributes.ClassAttributes.Description("This command allows you to set Text Value from AutomationElement.")]
    [Attributes.ClassAttributes.ImplementationDescription("Use this command when you want to set Text Value from AutomationElement.")]
    public class UIAutomationSelectElementCommand : ScriptCommand
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

        public UIAutomationSelectElementCommand()
        {
            this.CommandName = "UIAutomationSelectElementCommand";
            this.SelectionName = "Select Element";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var targetElement = v_TargetElement.GetAutomationElementVariable(engine);

            object checkPtn;
            if (targetElement.TryGetCurrentPattern(TogglePattern.Pattern, out checkPtn))
            {
                TogglePattern ptn = (TogglePattern)checkPtn;
                switch (ptn.Current.ToggleState)
                {
                    case ToggleState.Off:
                    case ToggleState.Indeterminate:
                        do
                        {
                            ptn.Toggle();
                        } while (ptn.Current.ToggleState != ToggleState.On);
                        break;
                }
            }
            else if (targetElement.TryGetCurrentPattern(SelectionItemPattern.Pattern, out checkPtn))
            {
                ((SelectionItemPattern)checkPtn).Select();
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
            return base.GetDisplayValue() + " [Root Element: '" + v_TargetElement + "']";
        }

    }
}