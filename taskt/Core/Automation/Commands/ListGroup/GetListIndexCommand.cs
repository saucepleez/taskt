using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Get List Index")]
    [Attributes.ClassAttributes.Description("This command allows you to get List Index.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to ge List Index.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetListIndexCommand : AListGetFromListCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        //public string v_List { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public override string v_Result { get; set; }

        public GetListIndexCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            // TODO: i want to remove current position

            var rawVariable = v_List.GetRawVariable(engine);
            if (rawVariable.VariableValue is List<string>)
            {
                //rawVariable.CurrentPosition.ToString().StoreInUserVariable(engine, v_Result);
                rawVariable.CurrentPosition.StoreInUserVariable(engine, v_Result);
            }
            else
            {
                throw new Exception($"Variable '{v_List}' is not a List. Type: {rawVariable.GetType()}");
            }
        }
    }
}