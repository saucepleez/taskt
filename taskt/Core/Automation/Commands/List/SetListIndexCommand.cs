using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Set List Index")]
    [Attributes.ClassAttributes.Description("This command allows you to modify List Index.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify List Index.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetListIndexCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_BothListName))]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_ListIndex))]
        [PropertyIsOptional(false)]
        public string v_Index { get; set; }

        public SetListIndexCommand()
        {
            //this.CommandName = "SetListIndexCommand";
            //this.SelectionName = "Set List Index";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            (var _, var index) = this.GetListVariableAndIndex(nameof(v_ListName), nameof(v_Index), engine);

            var rawVariable = v_ListName.GetRawVariable(engine);
            rawVariable.CurrentPosition = index;
        }
    }
}