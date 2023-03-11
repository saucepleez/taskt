using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Get List Index")]
    [Attributes.ClassAttributes.Description("This command allows you to get List Index.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to ge List Index.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetListIndexCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public GetListIndexCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var rawVariable = v_ListName.GetRawVariable(engine);
            if (rawVariable.VariableValue is List<string>)
            {
                rawVariable.CurrentPosition.ToString().StoreInUserVariable(engine, v_Result);
            }
            else
            {
                throw new Exception("Variable '" + v_ListName + "' is not LIST.");
            }
        }
    }
}