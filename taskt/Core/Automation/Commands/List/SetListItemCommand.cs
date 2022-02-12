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
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.Description("This command allows you want to set an item in a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to set an item in a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SetListItemCommand : ScriptCommand
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
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please enter the index of the List item.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid List index value")]
        [SampleUsage("**0** or **-1** or **{{{vIndex}}}**")]
        [Remarks("If it is empty, it will be the value of Current Position, which can be used for Loop List command.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Position")]
        public string v_ItemIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Enter the Value of the Set")]
        [InputSpecification("")]
        [SampleUsage("**1** or **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyValidationRule("Value", PropertyValidationRule.ValidationRuleFlags.Empty)]
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
                throw new Exception("Strange index value : " + v_ItemIndex);
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_ListName}', Index: '{v_ItemIndex}', Store In: '{v_NewValue}']";
        }

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_ListName))
        //    {
        //        this.validationResult += "List Name is empty.\n";
        //        this.IsValid = false;
        //    }
        //    if (String.IsNullOrEmpty(this.v_ItemIndex))
        //    {
        //        this.validationResult += "Index of List item is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}