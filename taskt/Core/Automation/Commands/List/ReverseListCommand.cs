using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to reverse list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to reverse list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ReverseListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name to Reverse")]
        [InputSpecification("")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List to Reverse", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List to Reverse")]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name of the Reverse List")]
        [InputSpecification("")]
        [SampleUsage("**vNewList** or **{{{vNewList}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("New List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "New List")]
        public string v_OutputList { get; set; }

        public ReverseListCommand()
        {
            this.CommandName = "ReverseListCommand";
            this.SelectionName = "Reverse List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> targetList = v_InputList.GetListVariable(engine);

            List<string> newList = new List<string>();
            newList.AddRange(targetList);
            newList.Reverse();
            newList.StoreInUserVariable(engine, v_OutputList);
        }
        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Reverse List " + this.v_InputList + ", To: " + this.v_OutputList + "]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InputList))
        //    {
        //        this.validationResult += "List is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_OutputList))
        //    {
        //        this.validationResult += "New list is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}