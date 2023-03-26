using System;
using System.Xml.Serialization;
using taskt.Core.Server;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Engine Commands")]
    [Attributes.ClassAttributes.CommandSettings("Upload BotStore Data")]
    [Attributes.ClassAttributes.Description("This command allows you to upload data to a local tasktServer bot store")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to upload or share data across bots.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class UploadBotStoreDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Name of the Key to Create")]
        [InputSpecification("Key", true)]
        [PropertyDetailSampleUsage("**myKey**", PropertyDetailSampleUsage.ValueType.Value, "Key")]
        [PropertyDetailSampleUsage("**{{{vKey}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Key")]
        [PropertyValidationRule("Key", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Key")]
        public string v_KeyName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextControls), nameof(TextControls.v_Text))]
        [PropertyDescription("Value to Upload")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Result")]
        public string v_Result { get; set; }

        public UploadBotStoreDataCommand()
        {
            //this.CommandName = "UploadDataCommand";
            //this.SelectionName = "Upload BotStore Data";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var keyName = v_KeyName.ConvertToUserVariable(engine);
            var keyValue = v_InputValue.ConvertToUserVariable(engine);

            try
            {
                var result = HttpServerClient.UploadData(keyName, keyValue);
                if (!String.IsNullOrEmpty(v_Result))
                {
                    result.StoreInUserVariable(engine, v_Result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}