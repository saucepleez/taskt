using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Encrypt Text")]
    [Attributes.ClassAttributes.Description("This command handles text encryption")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to store some data encrypted")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class EncryptTextCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Encryption Action")]
        [PropertyUISelectionOption("Encrypt")]
        [PropertyUISelectionOption("Decrypt")]
        [PropertyIsOptional(true, "Encrypt")]
        [PropertyFirstValue("Encrypt")]
        [PropertyDisplayText(true, "Action")]
        public string v_EncryptionType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        [PropertyIsOptional(true, "")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Pass Phrase")]
        [InputSpecification("Pass Phrage", true)]
        [PropertyFirstValue("TASKT")]
        [PropertyValidationRule("Pass Phrase", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(false, "")]
        public string v_PassPhrase { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please select the variable to receive the encrypted data")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Select or provide a variable from the variable list")]
        //[SampleUsage("**vSomeVariable**")]
        //[Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required to pre-define your variables, however, it is highly recommended.")]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public EncryptTextCommand()
        {
            //this.CommandName = "EncryptionCommand";
            //this.SelectionName = "Encryption Command";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
            //this.v_EncryptionType = "Encrypt";
            //this.v_PassPhrase = "TASKT";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //get variablized input
            var variableInput = v_InputValue.ConvertToUserVariable(sender);
            var passphrase = v_PassPhrase.ConvertToUserVariable(sender);

            string resultData = "";

            //if (v_EncryptionType.ConvertToUserVariable(sender) == "Encrypt")
            //{
            //    //encrypt data
            //    resultData = EncryptionServices.EncryptString(variableInput, passphrase);
            //}
            //else if (v_EncryptionType.ConvertToUserVariable(sender) == "Decrypt")
            //{
            //    //encrypt data
            //    resultData = EncryptionServices.DecryptString(variableInput, passphrase);
            //}
            //else
            //{
            //    throw new NotImplementedException($"Encryption Service Requested '{v_EncryptionType.ConvertToUserVariable(sender)}' has not been implemented");
            //}
            var encType = this.GetUISelectionValue(nameof(v_EncryptionType), engine);
            switch (encType)
            {
                case "encrypt":
                    resultData = EncryptionServices.EncryptString(variableInput, passphrase);
                    break;

                case "decrypt":
                    resultData = EncryptionServices.DecryptString(variableInput, passphrase);
                    break;
            }

            ////get variable
            //var requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();

            ////create if var does not exist
            //if (requiredComplexVariable == null)
            //{
            //    engine.VariableList.Add(new Script.ScriptVariable() { VariableName = v_applyToVariableName, CurrentPosition = 0 });
            //    requiredComplexVariable = engine.VariableList.Where(x => x.VariableName == v_applyToVariableName).FirstOrDefault();
            //}

            ////assign value to variable
            //requiredComplexVariable.VariableValue = resultData;

            resultData.StoreInUserVariable(engine, v_applyToVariableName);
        }

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_EncryptionType", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PassPhrase", this, editor));

        //    RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_applyToVariableName", this));
        //    var VariableNameControl = CommandControls.CreateStandardComboboxFor("v_applyToVariableName", this).AddVariableNames(editor);
        //    RenderedControls.AddRange(CommandControls.CreateDefaultUIHelpersFor("v_applyToVariableName", this, VariableNameControl, editor));
        //    RenderedControls.Add(VariableNameControl);

        //    return RenderedControls;

        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + $" [{v_EncryptionType} Data, apply to '{v_applyToVariableName}']";
        //}
    }
}