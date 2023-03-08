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
    public class AddVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Variable Name")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vSomeVariable**", PropertyDetailSampleUsage.ValueType.Value, "Variable Name")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Both)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Variable Value")]
        [InputSpecification("Variable Value", true)]
        //[SampleUsage("**Hello** or **{{{vNum}}}**")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Variable Value")]
        [PropertyDetailSampleUsage("**{{{vNum}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Variable Value")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
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

        public AddVariableCommand()
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

            //if (!engine.VariableList.Any(f => f.VariableName == v_userVariableName))
            //{
            //    //variable does not exist so add to the list
            //    try
            //    {

            //        var variableValue = v_Input.ConvertToUserVariable(engine);

            //        engine.VariableList.Add(new Script.ScriptVariable
            //        {
            //            VariableName = v_userVariableName,
            //            VariableValue = variableValue
            //        });
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception("Encountered an error when adding variable '" + v_userVariableName + "': " + ex.ToString());
            //    }
            //}
            //else
            //{
            //    //variable exists so decide what to do
            //    var ifExists = v_IfExists.ConvertToUserVariable(sender);
            //    if (String.IsNullOrEmpty(ifExists))
            //    {
            //        ifExists = "Replace If Variable Exists";
            //    }

            //    switch (ifExists)
            //    {
            //        case "Replace If Variable Exists":
            //            v_Input.ConvertToUserVariable(sender).StoreInUserVariable(engine, v_userVariableName);
            //            break;
            //        case "Error If Variable Exists":
            //            throw new Exception("Attempted to create a variable that already exists! Use 'Set Variable' instead or change the Exception Setting in the 'Add Variable' Command.");
            //        default:
            //            break;
            //    }  
            //}

            var variableValue = v_Input.ConvertToUserVariable(engine);

            var ifExists = this.GetUISelectionValue(nameof(v_IfExists), engine);
            var variableName = VariableControls.GetVariableName(v_userVariableName, engine);
            if (VariableControls.IsVariableExists(variableName, engine))
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Input", this, editor));


        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_IfExists", this));
        //    var dropdown = CommandControls.CreateDefaultDropdownFor("v_IfExists", this);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_IfExists", this, dropdown, editor));
        //    RenderedControls.Add(dropdown);

        //    return RenderedControls;
        //}



        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Assign '" + v_Input + "' to New Variable '" + v_userVariableName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "New variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}