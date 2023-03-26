using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Encrypt Decrypt Text")]
    [Attributes.ClassAttributes.Description("This command handles text encryption")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to store some data encrypted")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class EncryptDecryptTextCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Action")]
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
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public EncryptDecryptTextCommand()
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
            var variableInput = v_InputValue.ConvertToUserVariable(engine);
            var passphrase = v_PassPhrase.ConvertToUserVariable(engine);

            string resultData = "";

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

            resultData.StoreInUserVariable(engine, v_applyToVariableName);
        }
    }
}