using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("This command allows you want to get list index from value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get list index from value")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class GetListIndexFromValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please indicate the List Variable Name.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Enter a existing List.")]
        [Attributes.PropertyAttributes.SampleUsage("**myList** or **{{{myList}}}** or **[1,2,3]**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the value to search.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**0** or **{{{vValue}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        public string v_SearchItem { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify search method")]
        [Attributes.PropertyAttributes.InputSpecification("**First Index** or **Last Index**")]
        [Attributes.PropertyAttributes.SampleUsage("")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("First Index")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Last Index")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "First Index")]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please specify the variable to apply index")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Attributes.PropertyAttributes.Remarks("If list does not contains value, result is -1.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
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
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //var listVariable = v_ListName.GetRawVariable(engine);
            //if (listVariable == null)
            //{
            //    throw new System.Exception("Complex Variable '" + v_ListName + "' or '" + v_ListName.ApplyVariableFormatting(engine) + "' not found. Ensure the variable exists before attempting to modify it.");
            //}
            //List<string> targetList;
            //if (listVariable.VariableValue is List<string>)
            //{
            //    targetList = (List<string>)listVariable.VariableValue;
            //}
            //else
            //{
            //    throw new Exception(v_ListName + " is not List");
            //}
            List<string> targetList = v_ListName.GetListVariable(engine);

            var searchedValue = v_SearchItem.ConvertToUserVariable(sender);

            //var searchMethod = v_SearchMethod.ConvertToUserVariable(sender);
            //if (String.IsNullOrEmpty(searchMethod))
            //{
            //    searchMethod = "First Index";
            //}
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

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Search Value in '{v_ListName}', Value: '{v_SearchItem}', Result: '{v_Result}']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_ListName))
            {
                this.validationResult += "List Name is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_SearchItem))
            {
                this.validationResult += "Search Value is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_Result))
            {
                this.validationResult += "Result Variable is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}