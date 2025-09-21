using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Index")]
    [Attributes.ClassAttributes.CommandSettings("Get List Index From Value")]
    [Attributes.ClassAttributes.Description("This command allows you want to get list index from value")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get list index from value")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetListIndexFromValueCommand : AListGetFromValueCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //public string v_List { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_SearchValue))]
        //public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Search Method")]
        [InputSpecification("", true)]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("First Index")]
        [PropertyUISelectionOption("Last Index")]
        [PropertyIsOptional(true, "First Index")]
        [PropertyDisplayText(true, "Search Method")]
        [PropertyParameterOrder(8000)]
        public string v_SearchMethod { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [Remarks("When List does not have value, Result is **-1**")]
        public override string v_Result { get; set; }

        public GetListIndexFromValueCommand()
        {
            //this.CommandName = "GetListIndexFromValueCommand";
            //this.SelectionName = "Get List Index From Value";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //List<string> targetList = v_List.ExpandUserVariableAsList(engine);
            var targetList = this.ExpandUserVariableAsList(engine);

            var searchedValue = v_Value.ExpandValueOrUserVariable(engine);

            //string searchMethod = this.GetUISelectionValue(nameof(v_SearchMethod), "Search Method", engine);
            string searchMethod = this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_SearchMethod), engine);

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
    }
}