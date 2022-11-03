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
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SortListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name to sort")]
        [InputSpecification("")]
        [SampleUsage("**vList** or **{{{vList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("List to Sort", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List to Sort")]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select sort order")]
        [InputSpecification("")]
        [SampleUsage("**Ascending** or **Descending**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ascending")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ascending")]
        [PropertyUISelectionOption("Descending")]
        [PropertyDisplayText(true, "Order")]
        public string v_SortOrder { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select sort target value type")]
        [InputSpecification("")]
        [SampleUsage("**Text** or **Number**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Text")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Number")]
        [PropertyDisplayText(true, "Type")]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select a List Variable Name of the Sorted List")]
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

        public SortListCommand()
        {
            this.CommandName = "SortListCommand";
            this.SelectionName = "Sort List";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //string sortOrder = v_SortOrder.GetUISelectionValue("v_SortOrder", this, engine);
            string sortOrder = this.GetUISelectionValue(nameof(v_SortOrder), "Sort Order", engine);

            //string targetType = v_TargetType.GetUISelectionValue("v_TargetType", this, engine);
            string targetType = this.GetUISelectionValue(nameof(v_TargetType), "Target Type", engine);

            switch (targetType)
            {
                case "text":
                    List<string> targetList = v_InputList.GetListVariable(engine);
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
                    List<decimal> targetValueList = v_InputList.GetDecimalListVariable(false, engine);
                    List<decimal> valueList = new List<decimal>(targetValueList);
                    //foreach(var v in targetValueList)
                    //{
                    //    valueList.Add(decimal.Parse(v));
                    //}
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
    }
}