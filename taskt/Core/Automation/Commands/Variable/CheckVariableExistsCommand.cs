using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("Check Variable Exists")]
    [Attributes.ClassAttributes.Description("This command allows you to check variable existance.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check variable existance.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CheckVariableExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VariableNameControls), nameof(VariableNameControls.v_VariableName))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When the Variable Exists, Result is **True**")]
        public string v_Result { get; set; }

        public CheckVariableExistsCommand()
        {
            //this.CommandName = "CheckVariableExistsCommand";
            //this.SelectionName = "Check Variable Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var variableName = VariableNameControls.GetVariableName(v_userVariableName, engine);
            VariableNameControls.IsVariableExists(variableName, engine).StoreInUserVariable(engine, v_Result);
        }
    }
}