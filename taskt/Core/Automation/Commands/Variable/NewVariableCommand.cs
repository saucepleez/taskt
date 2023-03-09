using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("New Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to explicitly add a variable if you are not using **Set Variable* with the setting **Create Missing Variables** at runtime.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class NewVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VariableNameControls), nameof(VariableNameControls.v_VariableName))]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(VariableNameControls), nameof(VariableNameControls.v_VariableValue))]
        public string v_Input { get; set; }

        [XmlAttribute]
        [PropertyDescription("When the Variable Already Exists")]
        [InputSpecification("", true)]
        [PropertyUISelectionOption("Do Nothing If Variable Exists")]
        [PropertyUISelectionOption("Error If Variable Exists")]
        [PropertyUISelectionOption("Replace If Variable Exists")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "Replace If Variable Exists")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_IfExists { get; set; }

        public NewVariableCommand()
        {
            //this.CommandName = "AddVariableCommand";
            //this.SelectionName = "New Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            var variableValue = v_Input.ConvertToUserVariable(engine);

            var ifExists = this.GetUISelectionValue(nameof(v_IfExists), engine);
            var variableName = VariableNameControls.GetVariableName(v_userVariableName, engine);
            if (VariableNameControls.IsVariableExists(variableName, engine))
            {
                switch (ifExists)
                {
                    case "do nothing if variable exists":
                        // nothing to do
                        break;

                    case "replace if variable exists":
                        variableValue.StoreInUserVariable(engine, variableName);
                        break;

                    case "error if variable exists":
                        throw new Exception("Variable Name '" + variableName + "' is already exists! Use 'Set Variable' instead or change the Exception Setting in the 'Add Variable' Command.");
                }
            }
            else
            {
                variableValue.StoreInUserVariable(engine, variableName);
            }
        }
    }
}