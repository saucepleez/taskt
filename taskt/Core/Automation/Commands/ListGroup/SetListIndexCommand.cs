using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Set List Index")]
    [Attributes.ClassAttributes.Description("This command allows you to modify List Index.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify List Index.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class SetListIndexCommand : AListIndexCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_BothListName))]
        //public string v_List { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        [PropertyIsOptional(false)]
        public override string v_Index { get; set; }

        public SetListIndexCommand()
        {
            //this.CommandName = "SetListIndexCommand";
            //this.SelectionName = "Set List Index";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //(var _, var index) = this.ExpandUserVariablesAsListAndIndex(nameof(v_List), nameof(v_Index), engine);

            (_, var index, _) = this.ExpandValueOrUserVariableAsListAndIndexAndValue(engine);

            // TODO: i want to be better
            var rawVariable = v_List.GetRawVariable(engine);
            rawVariable.CurrentPosition = index;
        }
    }
}