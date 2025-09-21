using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.CommandSettings("Remove List Item")]
    [Attributes.ClassAttributes.Description("This command allows you to Remove an item from a List")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Remove an item from a List.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RemoveListItemCommand : AListIndexCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //public string v_List { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        //public string v_Index { get; set; }

        public RemoveListItemCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            (var lst, var index, _) = this.ExpandValueOrUserVariableAsListAndIndexAndValue(engine);
            lst.RemoveAt(index);
        }
    }
}