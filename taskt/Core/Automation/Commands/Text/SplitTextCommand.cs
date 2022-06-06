using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.Description("This command allows you to split a Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to split a single Text or variable into multiple items")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Split method to achieve automation.")]
    public class SplitTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select variable or text to split")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or text value")]
        [SampleUsage("**Hello** or **{{{vText}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Input Delimiter (ex. [crLF] for new line, [chars] for each char, ',' , {{{vChar}}})")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Declare the character that will be used to seperate. [crLF] can be used for line breaks and [chars] can be used to split each digit/letter")]
        [SampleUsage("[crLF], [chars], ',' (comma - with no single quote wrapper)")]
        [Remarks("")]
        [PropertyValidationRule("Delimiter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_splitCharacter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please select the list variable which will contain the results")]
        [InputSpecification("Select or provide a variable from the variable list")]
        [SampleUsage("**vSomeVariable**")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_applyConvertToUserVariableName { get; set; }

        public SplitTextCommand()
        {
            this.CommandName = "SplitTextCommand";
            this.SelectionName = "Split Text";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }
        public override void RunCommand(object sender)
        {
            var stringVariable = v_userVariableName.ConvertToUserVariable(sender);

            List<string> splitString;
            if (v_splitCharacter == "[crLF]")
            {
                splitString = stringVariable.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }
            else if (v_splitCharacter == "[chars]")
            {
                splitString = new List<string>();
                var chars = stringVariable.ToCharArray();
                foreach (var c in chars)
                {
                    splitString.Add(c.ToString());
                }

            }
            else
            {
                var splitCharacter = v_splitCharacter.ConvertToUserVariable(sender);
                splitString = stringVariable.Split(new string[] { splitCharacter }, StringSplitOptions.None).ToList();
            }

            var engine = (Engine.AutomationEngineInstance)sender;

            //var v_receivingVariable = v_applyConvertToUserVariableName.Replace(engine.engineSettings.VariableStartMarker, "").Replace(engine.engineSettings.VariableEndMarker, "");
            ////get complex variable from engine and assign
            //var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_receivingVariable).FirstOrDefault();

            //if (requiredComplexVariable == null)
            //{
            //    engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_receivingVariable, CurrentPosition = 0 });
            //    requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_receivingVariable).FirstOrDefault();
            //}

            //requiredComplexVariable.VariableValue = splitString;

            splitString.StoreInUserVariable(engine, v_applyConvertToUserVariableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));

            //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_splitCharacter", this, editor));

            //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyConvertToUserVariableName", this));
            //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyConvertToUserVariableName", this).AddVariableNames(editor);
            //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyConvertToUserVariableName", this, new Control[] { VariableNameControl }, editor));
            //RenderedControls.Add(VariableNameControl);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;

        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Split '" + v_userVariableName + "' by '" + v_splitCharacter + "' and apply to '" + v_applyConvertToUserVariableName + "']";
        }

        public override bool IsValidate(frmCommandEditor editor)
        {
            base.IsValidate(editor);

            //if (String.IsNullOrEmpty(this.v_userVariableName))
            //{
            //    this.validationResult += "Text is empty.\n";
            //    this.IsValid = false;
            //}
            //if (String.IsNullOrEmpty(this.v_splitCharacter))
            //{
            //    this.validationResult += "Delimiter is empty.\n";
            //    this.IsValid = false;
            //}
            //else if (this.v_splitCharacter.Trim().Length == 0)
            //{
            //    this.validationResult += "Delimiter is white space only. It may not work correctry when 'Enable Automatic Caluculation'. Highly recommend 'Disable Automatic Calculation'.";
            //    this.IsWarning = true;
            //}
            //if (String.IsNullOrEmpty(this.v_applyConvertToUserVariableName))
            //{
            //    this.validationResult += "Variable is empty.\n";
            //    this.IsValid = false;
            //}

            if (this.v_splitCharacter.Trim().Length == 0)
            {
                this.validationResult += "Delimiter is white space only. It may not work correctry when 'Enable Automatic Caluculation'. Highly recommend 'Disable Automatic Calculation'.";
                this.IsWarning = true;
            }

            return this.IsValid;
        }
    }
}