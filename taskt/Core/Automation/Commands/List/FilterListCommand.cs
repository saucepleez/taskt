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
    [Attributes.ClassAttributes.Description("This command allows you to sort list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to sort list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class FilterListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name to Filter")]
        [InputSpecification("")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List to Filter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select filter target value type")]
        [InputSpecification("")]
        [SampleUsage("**Text** or **Number**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Text")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Number")]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select filter action")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_ActionType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name of the Filtered List")]
        [InputSpecification("")]
        [SampleUsage("**vNewList** or **{{{vNewList}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Filtered List", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_OutputList { get; set; }

        public FilterListCommand()
        {
            this.CommandName = "FilterListCommand";
            this.SelectionName = "Filter List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> targetList = v_InputList.GetListVariable(engine);

            string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);

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
    }
}