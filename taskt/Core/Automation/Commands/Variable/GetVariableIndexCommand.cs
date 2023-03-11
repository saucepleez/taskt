using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Variable Index")]
    [Attributes.ClassAttributes.Description("This command allows you to get Variable Index.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Variable Index.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetVariableIndexCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VariableNameControls), nameof(VariableNameControls.v_VariableName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public GetVariableIndexCommand()
        {
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var variableName = VariableNameControls.GetVariableName(v_userVariableName, engine);
            var rawVarialbe = variableName.GetRawVariable(engine);
            rawVarialbe.CurrentPosition.ToString().StoreInUserVariable(engine, v_Result);
        }
    }
}