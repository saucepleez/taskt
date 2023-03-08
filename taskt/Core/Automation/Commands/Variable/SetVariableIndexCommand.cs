using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Variable Commands")]
    [Attributes.ClassAttributes.CommandSettings("Set Variable Index")]
    [Attributes.ClassAttributes.Description("This command allows you to modify variables.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to modify the value of variables.  You can even use variables to modify other variables.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements actions against VariableList from the scripting engine.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SetVariableIndexCommand : ScriptCommand
    {
        [XmlAttribute]
        //[Attributes.PropertyAttributes.PropertyDescription("Please select a variable to modify")]
        //[Attributes.PropertyAttributes.InputSpecification("Select or provide a variable from the variable list")]
        //[Attributes.PropertyAttributes.SampleUsage("**vSomeVariable**")]
        //[Attributes.PropertyAttributes.Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
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
        [PropertyDescription("Index")]
        [InputSpecification("Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specify the First Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Index", PropertyValidationRule.ValidationRuleFlags.Empty | PropertyValidationRule.ValidationRuleFlags.LessThanZero)]
        [PropertyDisplayText(true, "Index")]
        public string v_Index { get; set; }

        public SetVariableIndexCommand()
        {
            //this.CommandName = "SetVariableIndexCommand";
            //this.SelectionName = "Set Variable Index";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            //var requiredVariable = LookupVariable(engine);

            ////if still not found and user has elected option, create variable at runtime
            //if ((requiredVariable == null) && (engine.engineSettings.CreateMissingVariablesDuringExecution))
            //{
            //    engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_userVariableName });
            //    requiredVariable = LookupVariable(engine);
            //}

            //if (requiredVariable != null)
            //{

            //    var index = int.Parse(v_Index.ConvertToUserVariable(sender));

            //    requiredVariable.CurrentPosition = index;
            //}
            //else
            //{
            //    throw new Exception("Attempted to update variable index, but variable was not found. Enclose variables within brackets, ex. {vVariable}");
            //}

            var variableName = VariableControls.GetVariableName(v_userVariableName, engine);
            var rawVariable = variableName.GetRawVariable(engine);

            var index = this.ConvertToUserVariableAsInteger(nameof(v_Index), engine);
            rawVariable.CurrentPosition = index;
        }

        //private Script.ScriptVariable LookupVariable(Core.Automation.Engine.AutomationEngineInstance sendingInstance)
        //{
        //    //search for the variable
        //    var requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == v_userVariableName).FirstOrDefault();

        //    //if variable was not found but it starts with variable naming pattern
        //    if ((requiredVariable == null) && (v_userVariableName.StartsWith(sendingInstance.engineSettings.VariableStartMarker)) && (v_userVariableName.EndsWith(sendingInstance.engineSettings.VariableEndMarker)))
        //    {
        //        //reformat and attempt
        //        var reformattedVariable = v_userVariableName.Replace(sendingInstance.engineSettings.VariableStartMarker, "").Replace(sendingInstance.engineSettings.VariableEndMarker, "");
        //        requiredVariable = sendingInstance.VariableList.Where(var => var.VariableName == reformattedVariable).FirstOrDefault();
        //    }

        //    return requiredVariable;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Update Variable '" + v_userVariableName + "' index to '" + v_Index + "']";
        //}

        //public override List<Control> Render(UI.Forms.frmCommandEditor editor)
        //{
        //    //custom rendering
        //    base.Render(editor);


        //    //create control for variable name
        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_userVariableName", this));
        //    var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_userVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_userVariableName", this, VariableNameControl, editor));
        //    RenderedControls.Add(VariableNameControl);

        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Index", this, editor));

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
        //    if (String.IsNullOrEmpty(this.v_Index))
        //    {
        //        this.validationResult += "Index is empty.\n";
        //        this.IsValid = false;
        //    }
        //    else
        //    {
        //        int idx;
        //        if (int.TryParse(this.v_Index, out idx))
        //        {
        //            if (idx < 0)
        //            {
        //                this.validationResult += "Specify a value of 0 or more for index.\n";
        //                this.IsValid = false;
        //            }
        //        }
        //    }

        //    return this.IsValid;
        //}
    }
}