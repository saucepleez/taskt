using System;
using System.Xml.Serialization;
using System.Collections.Generic;
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
        //[PropertyDescription("Please indicate the List Variable Name.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a existing List.")]
        //[SampleUsage("**myList** or **{{{myList}}}** or **[1,2,3]**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        //[PropertyValidationRule("List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "List")]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value to Search.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        //[SampleUsage("**0** or **{{{vValue}}}**")]
        [PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Value to Search")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Value to Search")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Value to Search")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value to Search")]
        public string v_SearchItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Search Method")]
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
        //[PropertyDescription("Please specify the variable to apply index")]
        //[InputSpecification("")]
        //[SampleUsage("**vResult** or **{{{vResult}}}**")]
        //[Remarks("If list does not contains value, result is -1.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Result")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("When List does not have value, Result is **-1**")]
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

            string searchMethod = this.GetUISelectionValue(nameof(v_SearchMethod), "Search Method", engine);

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
    }
}