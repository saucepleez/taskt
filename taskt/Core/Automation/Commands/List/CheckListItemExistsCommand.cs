using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.Description("This command allows you want to check list has a value you specify")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check list has a value you specify")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckListItemExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please indicate the List Variable Name.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a existing List.")]
        [SampleUsage("**myList** or **{{{myList}}}**")]
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
        [PropertyDescription("Please specify the variable to apply Result")]
        [InputSpecification("")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Result")]
        public string v_Result { get; set; }

        public CheckListItemExistsCommand()
        {
            this.CommandName = "CheckListItemExistsCommand";
            this.SelectionName = "Check List Item Exists";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> targetList = v_ListName.GetListVariable(engine);

            var searchedValue = v_SearchItem.ConvertToUserVariable(sender);

            targetList.Contains(searchedValue).StoreInUserVariable(engine, v_Result);
        }
    }
}