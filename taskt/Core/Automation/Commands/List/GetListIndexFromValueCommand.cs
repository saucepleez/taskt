using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("This command allows you want to get list index from value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get list index from value")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetListIndexFromValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the List Variable Name.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing List.")]
        [SampleUsage("**myList** or **{{{myList}}}** or **[1,2,3]**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List")]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the value to search.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Search Value")]
        public string v_SearchItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify search method")]
        [InputSpecification("**First Index** or **Last Index**")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("First Index")]
        [PropertyUISelectionOption("Last Index")]
        [PropertyIsOptional(true, "First Index")]
        [PropertyDisplayText(true, "Search Method")]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify the variable to apply index")]
        [InputSpecification("")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("If list does not contains value, result is -1.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public string v_Result { get; set; }

        public GetListIndexFromValueCommand()
        {
            this.CommandName = "GetListIndexFromValueCommand";
            this.SelectionName = "Get List Index From Value";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> targetList = v_ListName.GetListVariable(engine);

            var searchedValue = v_SearchItem.ConvertToUserVariable(sender);

            string searchMethod = v_SearchMethod.GetUISelectionValue("v_SearchMethod", this, engine);

            switch (searchMethod)
            {
                case "first index":
                    targetList.IndexOf(searchedValue).ToString().StoreInUserVariable(engine, v_Result);
                    break;
                case "last index":
                    targetList.LastIndexOf(searchedValue).ToString().StoreInUserVariable(engine, v_Result);
                    break;
            }
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + $" [Search Value in '{v_ListName}', Value: '{v_SearchItem}', Result: '{v_Result}']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_ListName))
        //    {
        //        this.validationResult += "List Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_SearchItem))
        //    {
        //        this.validationResult += "Search Value is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_Result))
        //    {
        //        this.validationResult += "Result Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}