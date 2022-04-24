using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.Description("This command allows you to sort list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to sort list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class SortListCommand : ScriptCommand
    {
        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a List Variable Name to sort")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vList** or **{{{vList}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyShowSampleUsageInDescription(true)]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select sort order")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Ascending** or **Descending**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Ascending")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Ascending")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Descending")]
        public string v_SortOrder { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select sort target value type")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**Text** or **Number**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyIsOptional(true, "Text")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Text")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Number")]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please select a List Variable Name of the Sorted List")]
        [Attributes.PropertyAttributes.InputSpecification("")]
        [Attributes.PropertyAttributes.SampleUsage("**vNewList** or **{{{vNewList}}}**")]
        [Attributes.PropertyAttributes.Remarks("")]
        [Attributes.PropertyAttributes.PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [Attributes.PropertyAttributes.PropertyIsVariablesList(true)]
        [Attributes.PropertyAttributes.PropertyInstanceType(Attributes.PropertyAttributes.PropertyInstanceType.InstanceType.List)]
        [Attributes.PropertyAttributes.PropertyParameterDirection(Attributes.PropertyAttributes.PropertyParameterDirection.ParameterDirection.Output)]
        public string v_OutputList { get; set; }

        public SortListCommand()
        {
            this.CommandName = "SortListCommand";
            this.SelectionName = "Sort List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;

            //get variable by regular name
            //Script.ScriptVariable listVariable = v_InputList.GetRawVariable(engine);
            ////if still null then throw exception
            //if (listVariable == null)
            //{
            //    throw new System.Exception("Complex Variable '" + v_InputList + "' or '" + v_InputList.ApplyVariableFormatting(engine) + "' not found. Ensure the variable exists before attempting to modify it.");
            //}
            List<string> targetList = v_InputList.GetListVariable(engine);

            //string sortOrder;
            //if (String.IsNullOrEmpty(v_SortOrder))
            //{
            //    sortOrder = "Ascending";
            //}
            //else
            //{
            //    sortOrder = v_SortOrder.ConvertToUserVariable(engine);
            //}
            string sortOrder = v_SortOrder.GetUISelectionValue("v_SortOrder", this, engine);

            //string targetType;
            //if (String.IsNullOrEmpty(v_TargetType))
            //{
            //    targetType = "Text";
            //}
            //else
            //{
            //    targetType = v_TargetType.ConvertToUserVariable(engine);
            //}
            string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);

            //if (!(listVariable.VariableValue is List<string>))
            //{
            //    throw new Exception(v_InputList + " is not List or not-supported List.");
            //}

            switch (targetType)
            {
                case "text":
                    List<string> newList = new List<string>();
                    newList.AddRange(targetList);

                    newList.Sort();
                    if (sortOrder == "descending")
                    {
                        newList.Reverse();
                    }
                    newList.StoreInUserVariable(engine, v_OutputList);
                    break;

                case "number":
                    List<decimal> valueList = new List<decimal>();
                    foreach(var v in targetList)
                    {
                        valueList.Add(decimal.Parse(v));
                    }
                    valueList.Sort();
                    if (sortOrder == "descending")
                    {
                        valueList.Reverse();
                    }

                    List<string> newList2 = new List<string>();
                    foreach(var v in valueList)
                    {
                        newList2.Add(v.ToString());
                    }
                    newList2.StoreInUserVariable(engine, v_OutputList);

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
            return base.GetDisplayValue() + " [Sort List " + this.v_InputList + ", To: " + this.v_OutputList + ", Order: " + this.v_SortOrder + "]";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            if (String.IsNullOrEmpty(this.v_InputList))
            {
                this.validationResult += "List is empty.\n";
                this.IsValid = false;
            }
            if (String.IsNullOrEmpty(this.v_OutputList))
            {
                this.validationResult += "Sorted list is empty.\n";
                this.IsValid = false;
            }

            return this.IsValid;
        }
    }
}