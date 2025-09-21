using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.CommandSettings("Check List Item Exists")]
    [Attributes.ClassAttributes.Description("This command allows you want to check list has a value you specify")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check list has a value you specify")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CheckListItemExistsCommand : AListGetFromValueCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //public string v_List { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_SearchValue))]
        //public string v_Value { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Item Exists, the Result is **True**")]
        public override string v_Result { get; set; }

        public CheckListItemExistsCommand()
        {
            //this.CommandName = "CheckListItemExistsCommand";
            //this.SelectionName = "Check List Item Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var targetList = v_List.ExpandUserVariableAsList(engine);
            var targetList = this.ExpandUserVariableAsList(engine);

            var searchedValue = v_Value.ExpandValueOrUserVariable(engine);

            targetList.Contains(searchedValue).StoreInUserVariable(engine, v_Result);
        }
    }
}