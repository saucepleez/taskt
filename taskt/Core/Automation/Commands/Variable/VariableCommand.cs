using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("Set Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class VariableCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please select a variable to modify")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyDescription("Variable Name")]
        [InputSpecification("Variable Name", true)]
        [PropertyDetailSampleUsage("**vSomeVariable**", PropertyDetailSampleUsage.ValueType.Value, "Variable Name")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Variable Value")]
        [InputSpecification("Variable Value", true)]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Variable Value")]
        [PropertyDetailSampleUsage("**{{{vNum}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Variable Value")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.MultiLineTextBox)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDisplayText(true, "Value")]
        public string v_Input { get; set; }

        [XmlAttribute]
        [PropertyDescription("Convert Variables in Input Text Above")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("", true)]
        [Remarks("If **{{{vNum}}}** has **'1'** and You select **'Yes'**, Variable will be Assigned **'1'**. If You Select **'No'**, Variable will be assigned **'{{{vNum}}}'**.")]
        [PropertyIsOptional(true, "Yes")]
        public string v_ReplaceInputVariables { get; set; }

        public VariableCommand()
        {
            //this.CommandName = "VariableCommand";
            //this.SelectionName = "Set Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_ReplaceInputVariables = "Yes";
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            //v_userVariableName = v_userVariableName.ConvertToUserVariable(engine);

            //if (String.IsNullOrEmpty(v_ReplaceInputVariables))
            //{
            //    v_ReplaceInputVariables = "YES";
            //}
            //string variableInput;
            //if (v_ReplaceInputVariables.ToUpperInvariant() == "YES")
            //{
            //    variableInput = v_Input.ConvertToUserVariable(sender);
            //}
            //else
            //{
            //    variableInput = v_Input;
            //}
            //if (variableInput.StartsWith("{{") && variableInput.EndsWith("}}"))
            //{
            //    var itemList = variableInput.Replace("{{", "").Replace("}}", "").Split('|').Select(s => s.Trim()).ToList();
            //    itemList.StoreInUserVariable(engine, v_userVariableName);
            //}
            //else
            //{
            //    variableInput.StoreInUserVariable(engine, v_userVariableName);
            //}

            var isRepalce = this.GetUISelectionValue(nameof(v_ReplaceInputVariables), engine);
            string variableValue;
            if (isRepalce == "yes")
            {
                variableValue = v_Input.ConvertToUserVariable(engine);
            }
            else
            {
                variableValue = v_Input;
            }

            var variableName = VariableControls.GetVariableName(v_userVariableName, engine);
            if (VariableControls.IsVariableExists(variableName, engine))
            {
                variableValue.StoreInUserVariable(engine, variableName);
            }
            else
            {
                throw new Exception("Variable Name '" + variableName + "' does not exists.");
            }
        }

        //private Script.ScriptVariable LookupVariable(Engine.AutomationEngineInstance sendingInstance)
        //{
        //    //search for the variable
        //    var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_userVariableName).FirstOrDefault();

        //    return requiredVariable;
        //}

        //private string parseVariableName(string variableName, Engine.AutomationEngineInstance engine)
        //{
        //    var settings = engine.engineSettings;
        //    if (variableName.StartsWith(settings.VariableStartMarker) && variableName.EndsWith(settings.VariableEndMarker))
        //    {
        //        if (engine.engineSettings.IgnoreFirstVariableMarkerInOutputParameter)
        //        {
        //            variableName = variableName.Substring(settings.VariableStartMarker.Length, variableName.Length - settings.VariableStartMarker.Length - settings.VariableEndMarker.Length);
        //        }
        //    }
        //    if (variableName.Contains(settings.VariableStartMarker) && variableName.Contains(settings.VariableEndMarker))
        //    {
        //        variableName = variableName.ConvertToUserVariable(engine);
        //    }

        //    return variableName;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Apply '" + v_Input + "' to Variable '" + v_userVariableName + "']";
        //}

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    //custom rendering
        //    base.Render(editor);


        //    //create control for variable name
        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_userVariableName", this, VariableNameControl, editor));
        //    RenderedControls.Add(VariableNameControl);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Input", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ReplaceInputVariables", this, editor));
        //    return RenderedControls;
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_userVariableName))
        //    {
        //        this.validationResult += "Variable is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}