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
    [Attributes.ClassAttributes.Description("This command allows you to get an item from a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get an item from a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetListItemCommand : ScriptCommand
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
        [PropertyDescription("Please enter the index of the List item.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Enter a valid List index value")]
        [SampleUsage("**0** or **-1** or **{{{vIndex}}}**")]
        [Remarks("**-1** means index of the last row. If it is empty, it will be the value of Current Position, which can be used for Loop List command.")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "Current Position")]
        [PropertyDisplayText(true, "Index")]
        public string v_ItemIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify a Variable Name to Store Result")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_UserVariableName { get; set; }

        public GetListItemCommand()
        {
            this.CommandName = "GetListItemCommand";
            this.SelectionName = "Get List Item";
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

            dynamic listToIndex;
            var varType = listVariable.VariableValue.GetType();
            if (listVariable.VariableValue is List<string>)
            {
                listToIndex = (List<string>)listVariable.VariableValue;
            }
            else if (varType.IsGenericType && (varType.GetGenericTypeDefinition() == typeof(List<>)))
            {
                listToIndex = listVariable.VariableValue;
            }
            else
            {
                var listValue = listVariable.VariableValue;
                if ((listValue is string) &&
                        (listValue.ToString().StartsWith("[") && listValue.ToString().EndsWith("]") && listValue.ToString().Contains(",")))
                {
                    Newtonsoft.Json.Linq.JArray jsonArray = Newtonsoft.Json.JsonConvert.DeserializeObject(listVariable.VariableValue.ToString()) as Newtonsoft.Json.Linq.JArray;

                    var itemList = new List<string>();
                    foreach (var jsonItem in jsonArray)
                    {
                        var value = (Newtonsoft.Json.Linq.JValue)jsonItem;
                        itemList.Add(value.ToString());
                    }
                    listToIndex = itemList;
                }
                else
                {
                    throw new Exception(v_ListName + " is not List");
                }
            }

            int index = 0;
            if (String.IsNullOrEmpty(v_ItemIndex))
            {
                index = listVariable.CurrentPosition;
            }
            else
            {
                //var itemIndex = v_ItemIndex.ConvertToUserVariable(sender);
                //index = int.Parse(itemIndex);
                index = v_ItemIndex.ConvertToUserVariableAsInteger("Index", engine);
            }
            
            if (index < 0)
            {
                index = listToIndex.Count + index;
            }

            if ((index >= 0) && (index < listToIndex.Count))
            {
                if (listToIndex is List<string>)
                {
                    ((string)listToIndex[index]).StoreInUserVariable(engine, v_UserVariableName);
                }
                else
                {
                    // set new variable
                    "".StoreInUserVariable(engine, v_UserVariableName);
                    var targetVariable = v_UserVariableName.GetRawVariable(engine);
                    targetVariable.VariableValue = listToIndex[index];
                }
            }
            else
            {
                throw new Exception("Strange index " + v_ItemIndex + ", parsed " + index);
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
        //    return base.GetDisplayValue() + $" [From '{v_ListName}', Index: '{v_ItemIndex}', Store In: '{v_UserVariableName}']";
        //}

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
        //    //else
        //    //{
        //    //    int vIndex;
        //    //    if (int.TryParse(this.v_ItemIndex, out vIndex))
        //    //    {
        //    //        if (vIndex < 0)
        //    //        {
        //    //            this.validationResult += "Specify a value of 0 or more for index of List item.\n";
        //    //            this.IsValid = false;
        //    //        }
        //    //    }
        //    //}
        //    if (String.IsNullOrEmpty(this.v_UserVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}