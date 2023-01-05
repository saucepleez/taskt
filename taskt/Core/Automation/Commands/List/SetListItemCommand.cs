using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.Description("This command allows you want to set an item in a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set an item in a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetListItemCommand : ScriptCommand
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
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_BothListName))]
        public string v_ListName { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please enter the index of the List item.")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Enter a valid List index value")]
        //[SampleUsage("**0** or **-1** or **{{{vIndex}}}**")]
        //[Remarks("**-1** means index of the last row. If it is empty, it will be the value of Current Position, which can be used for Loop List command.")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyIsOptional(true, "Current Position")]
        //[PropertyDisplayText(true, "Index")]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        public string v_ItemIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value to Set")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Value to Set")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Value to Set")]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Value")]
        public string v_NewValue { get; set; }

        public SetListItemCommand()
        {
            this.CommandName = "SetListItemCommand";
            this.SelectionName = "Set List Item";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var listVariable = v_ListName.GetRawVariable(engine);
            if (listVariable == null)
            {
                throw new Exception("Complex Variable '" + v_ListName + "' or '" + v_ListName.ApplyVariableFormatting(engine) + "' not found. Ensure the variable exists before attempting to modify it.");
            }

            List<string> targetList;
            if (listVariable.VariableValue is List<string>)
            {
                targetList = (List<string>)listVariable.VariableValue;
            }
            else
            {
                throw new Exception(v_ListName + " is not List");
            }
            
            int index = 0;
            if (String.IsNullOrEmpty(v_ItemIndex))
            {
                index = listVariable.CurrentPosition;
            }
            else
            {
                string itemIndex = v_ItemIndex.ConvertToUserVariable(sender);
                index = int.Parse(itemIndex);
            }
            if (index < 0)
            {
                index = targetList.Count + index;
            }

            if ((index >= 0) && (index < targetList.Count))
            {
                targetList[index] = v_NewValue.ConvertToUserVariable(engine);
            }
            else
            {
                throw new Exception("Strange index " + v_ItemIndex + ", parsed " + index);
            }
        }
    }
}