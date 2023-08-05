using System;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Text Commands")]
    [Attributes.ClassAttributes.SubGruop("Action")]
    [Attributes.ClassAttributes.CommandSettings("Modify Text")]
    [Attributes.ClassAttributes.Description("This command allows you to trim Text, convert Text, etc.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to trim Text, convert Text, etc.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ModifyTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text_MultiLine))]
        public string v_userVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Modify Method")]
        [PropertyUISelectionOption("To Upper Case")]
        [PropertyUISelectionOption("To Lower Case")]
        [PropertyUISelectionOption("To Base64 String")]
        [PropertyUISelectionOption("From Base64 String")]
        [PropertyUISelectionOption("Trim")]
        [PropertyUISelectionOption("Trim Start")]
        [PropertyUISelectionOption("Trim End")]
        [PropertyUISelectionOption("Reverse")]
        [PropertyValidationRule("Modify Method", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Modify Method")]
        public string v_ConvertType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_applyToVariableName { get; set; }

        public ModifyTextCommand()
        {
            //this.CommandName = "ModifyTextCommand";
            //this.SelectionName = "Modify Text";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var stringValue = v_userVariableName.ConvertToUserVariable(engine);

            //var caseType = v_ConvertType.ConvertToUserVariable(sender);
            var caseType = this.GetUISelectionValue(nameof(v_ConvertType), engine);
            switch (caseType)
            {
                case "to upper case":
                    stringValue = stringValue.ToUpper();
                    break;
                case "to lower case":
                    stringValue = stringValue.ToLower();
                    break;
                case "to base64 string":
                    byte[] textAsBytes = System.Text.Encoding.ASCII.GetBytes(stringValue);
                    stringValue = Convert.ToBase64String(textAsBytes);
                    break;
                case "from base64 string":
                    byte[] encodedDataAsBytes = Convert.FromBase64String(stringValue);
                    stringValue = System.Text.Encoding.ASCII.GetString(encodedDataAsBytes);
                    break;
                case "trim":
                    stringValue = stringValue.Trim();
                    break;
                case "trim start":
                    stringValue = stringValue.TrimStart();
                    break;
                case "trim end":
                    stringValue = stringValue.TrimEnd();
                    break;
                case "reverse":
                    stringValue = new string(stringValue.Reverse().ToArray());
                    break;
            }

            stringValue.StoreInUserVariable(engine, v_applyToVariableName);
        }
    }
}