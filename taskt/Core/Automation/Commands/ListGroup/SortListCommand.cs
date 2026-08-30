using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Actions")]
    [Attributes.ClassAttributes.CommandSettings("Sort List")]
    [Attributes.ClassAttributes.Description("This command allows you to sort list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to sort list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SortListCommand : AListCreateFromListCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        [PropertyDescription("List Variable Name to Sort")]
        [PropertyValidationRule("List to Sort", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "List to Sort")]
        public override string v_TargetList { get; set; }

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
        [PropertyParameterOrder(6000)]
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
        [PropertyParameterOrder(7000)]
        public string v_TargetType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_NewOutputListName))]
        //public string v_NewList { get; set; }

        public SortListCommand()
        {
            //this.CommandName = "SortListCommand";
            //this.SelectionName = "Sort List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            string sortOrder = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_SortOrder), "Sort Order", engine);

            string targetType = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_TargetType), "Target Type", engine);

            switch (targetType)
            {
                case "text":
                    //var targetList = v_TargetList.ExpandUserVariableAsList(engine);
                    var targetList = this.ExpandUserVariableAsList(engine);
                    var newList = new List<string>(targetList);

                    newList.Sort();
                    if (sortOrder == "descending")
                    {
                        newList.Reverse();
                    }
                    //newList.StoreInUserVariable(engine, v_NewList);
                    this.StoreListInUserVariable(newList, engine);
                    break;

                case "number":
                    //var targetValueList = v_TargetList.ExpandUserVariableAsDecimalList(false, engine);
                    var targetValueList = this.ExpandUserVariableAsDecimalList(nameof(v_TargetList), false, engine);
                    var valueList = new List<decimal>(targetValueList);

                    valueList.Sort();
                    if (sortOrder == "descending")
                    {
                        valueList.Reverse();
                    }

                    var newValueList = new List<string>();
                    foreach(var v in valueList)
                    {
                        newValueList.Add(v.ToString());
                    }
                    //newList2.StoreInUserVariable(engine, v_NewList);
                    this.StoreListInUserVariable(newValueList, engine);

                    break;
            }
        }
    }
}