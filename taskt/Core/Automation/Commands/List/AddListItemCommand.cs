using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("List Item")]
    [Attributes.ClassAttributes.CommandSettings("Add List Item")]
    [Attributes.ClassAttributes.Description("This command allows you to add list item.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to add list item.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class AddListItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_BothListName))]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_OneLineTextBox))]
        [PropertyDescription("Value to be added the List")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "the Value to be added List")]
        [PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "the Value to be added List")]
        [PropertyDisplayText(true, "Value")]
        public string v_Input { get; set; }

        public AddListItemCommand()
        {
            //this.CommandName = "AddListItemCommand";
            //this.SelectionName = "Add List Item";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            List<string> lst = v_ListName.GetListVariable(engine);

            var variableInput = v_Input.ConvertToUserVariable(sender);
            lst.Add(variableInput);
        }
    }
}