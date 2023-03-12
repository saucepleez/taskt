using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Split Text")]
    [Attributes.ClassAttributes.Description("This command allows you to split a Text")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to split a single Text or variable into multiple items")]
    [Attributes.ClassAttributes.ImplementationDescription("This command uses the String.Split method to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class SplitTextCommand : ScriptCommand
    {
        [XmlAttribute]
        //[PropertyDescription("Please select variable or text to split")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Select or provide a variable or text value")]
        //[SampleUsage("**Hello** or **{{{vText}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        [PropertyDescription("Text")]
        [InputSpecification("Text", true)]
        [Remarks("")]
        [PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Text")]
        [PropertyDetailSampleUsage("**{{{vText}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Text")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyDisplayText(true, "Text")]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delimiter (ex. [crLF] for new line, [chars] for each char, ',' , {{{vChar}}})")]
        [InputSpecification("Delimiter", true)]
        //[SampleUsage("[crLF], [chars], ',' (comma - with no single quote wrapper)")]
        [PropertyDetailSampleUsage("**,**", PropertyDetailSampleUsage.ValueType.Value, "Delimiter")]
        [PropertyDetailSampleUsage("**[crLF]**", "Specify **Line Break** for Delimiter")]
        [PropertyDetailSampleUsage("**[chars]**", "Split by one character")]
        [PropertyDetailSampleUsage("**{{{vChar}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Delimiter")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Delimiter", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Delimiter")]
        public string v_splitCharacter { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the list variable which will contain the results")]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        //[PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        //[PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        //[PropertyInstanceType(PropertyInstanceType.InstanceType.List)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        public string v_applyConvertToUserVariableName { get; set; }

        public SplitTextCommand()
        {
            //this.CommandName = "SplitTextCommand";
            //this.SelectionName = "Split Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var stringVariable = v_userVariableName.ConvertToUserVariable(engine);

            var split = v_splitCharacter.ConvertToUserVariable(engine);
            List<string> splitString;
            //if (split == "[crLF]")
            //{
            //    splitString = stringVariable.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            //}
            //else if (split == "[chars]")
            //{
            //    splitString = new List<string>();
            //    var chars = stringVariable.ToCharArray();
            //    foreach (var c in chars)
            //    {
            //        splitString.Add(c.ToString());
            //    }
            //}
            //else
            //{
            //    var splitCharacter = v_splitCharacter.ConvertToUserVariable(sender);
            //    splitString = stringVariable.Split(new string[] { splitCharacter }, StringSplitOptions.None).ToList();
            //}
            switch (split)
            {
                case "[crLF]":
                    splitString = stringVariable.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                    break;
                case "[chars]":
                    //splitString = new List<string>();
                    //var chars = stringVariable.ToCharArray();
                    //foreach (var c in chars)
                    //{
                    //    splitString.Add(c.ToString());
                    //}
                    splitString = stringVariable.ToCharArray().Select(c => c.ToString()).ToList();
                    break;
                default:
                    splitString = stringVariable.Split(new string[] { split }, StringSplitOptions.None).ToList();
                    break;
            }

            splitString.StoreInUserVariable(engine, v_applyConvertToUserVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_userVariableName", this, editor));

        //    //RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_splitCharacter", this, editor));

        //    //RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyConvertToUserVariableName", this));
        //    //var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyConvertToUserVariableName", this).AddVariableNames(editor);
        //    //RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_applyConvertToUserVariableName", this, new Control[] { VariableNameControl }, editor));
        //    //RenderedControls.Add(VariableNameControl);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Split '" + v_userVariableName + "' by '" + v_splitCharacter + "' and apply to '" + v_applyConvertToUserVariableName + "']";
        //}

        //public override bool IsValidate(frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    //if (String.IsNullOrEmpty(this.v_userVariableName))
        //    //{
        //    //    this.validationResult += "Text is empty.\n";
        //    //    this.IsValid = false;
        //    //}
        //    //if (String.IsNullOrEmpty(this.v_splitCharacter))
        //    //{
        //    //    this.validationResult += "Delimiter is empty.\n";
        //    //    this.IsValid = false;
        //    //}
        //    //else if (this.v_splitCharacter.Trim().Length == 0)
        //    //{
        //    //    this.validationResult += "Delimiter is white space only. It may not work correctry when 'Enable Automatic Caluculation'. Highly recommend 'Disable Automatic Calculation'.";
        //    //    this.IsWarning = true;
        //    //}
        //    //if (String.IsNullOrEmpty(this.v_applyConvertToUserVariableName))
        //    //{
        //    //    this.validationResult += "Variable is empty.\n";
        //    //    this.IsValid = false;
        //    //}

        //    if (this.v_splitCharacter.Trim().Length == 0)
        //    {
        //        this.validationResult += "Delimiter is white space only. It may not work correctry when 'Enable Automatic Caluculation'. Highly recommend 'Disable Automatic Calculation'.";
        //        this.IsWarning = true;
        //    }

        //    return this.IsValid;
        //}
    }
}