using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Sort List")]
    [Attributes.ClassAttributes.Description("This command allows you to sort list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to sort list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SortListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List Variable Name to Sort")]
        [PropertyValidationRule("List to Sort", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List to Sort")]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyDescription("Sort Order")]
        [InputSpecification("", true)]
        [SampleUsage("**Ascending** or **Descending**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Ascending")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Ascending")]
        [PropertyUISelectionOption("Descending")]
        [PropertyDisplayText(true, "Order")]
        public string v_SortOrder { get; set; }

        [XmlAttribute]
        [PropertyDescription("Sort Target Value Type")]
        [InputSpecification("", true)]
        [SampleUsage("**Text** or **Number**")]
        [Remarks("")]
        [PropertyIsOptional(true, "Text")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Text")]
        [PropertyUISelectionOption("Number")]
        [PropertyDisplayText(true, "Type")]
        public string v_TargetType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        public string v_OutputList { get; set; }

        public SortListCommand()
        {
            //this.CommandName = "SortListCommand";
            //this.SelectionName = "Sort List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string sortOrder = this.GetUISelectionValue(nameof(v_SortOrder), "Sort Order", engine);

            string targetType = this.GetUISelectionValue(nameof(v_TargetType), "Target Type", engine);

            switch (targetType)
            {
                case "text":
                    List<string> targetList = v_InputList.GetListVariable(engine);
                    List<string> newList = new List<string>(targetList);

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