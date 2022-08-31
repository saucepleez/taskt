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
    [Attributes.ClassAttributes.Description("This command allows you to concatenate 2 lists.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to concatenate 2 lists.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConcatenateListsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name to concatenate")]
        [InputSpecification("")]
        [SampleUsage("**vList1** or **{{{vList1}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List1", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List1")]
        public string v_InputListA { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name to concatenate")]
        [InputSpecification("")]
        [SampleUsage("**vList2** or **{{{vList2}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List2", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List2")]
        public string v_InputListB { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name of the New List")]
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

        public ConcatenateListsCommand()
        {
            this.CommandName = "ConcatenateListsCommand";
            this.SelectionName = "Concatenate Lists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> listA = v_InputListA.GetListVariable(engine);
            List<string> listB = v_InputListB.GetListVariable(engine);

            List<string> newList = new List<string>();
            newList.AddRange(listA);
            newList.AddRange(listB);
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
        //    return base.GetDisplayValue() + " [Concatenate Lists " + this.v_InputListA + " and " + this.v_InputListB + ", To: " + this.v_OutputList + "]";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_InputListA))
        //    {
        //        this.validationResult += "List1 is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_InputListB))
        //    {
        //        this.validationResult += "List2 is empty.\n";
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