using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Get EMail From EMailList")]
    [Attributes.ClassAttributes.Description("This command allows you to get EMail from EMailList.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get EMail from EMailList.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetEMailFromEMailListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("EMailList Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("EMailList Variable Name", true)]
        [PropertyDetailSampleUsage("**vMailList**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyDetailSampleUsage("**{{{vMailList}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("EMailList", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMailList, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyDisplayText(true, "EMailList")]
        public string v_MailListName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("EMailList Index")]
        [InputSpecification("EMailList Index", true)]
        [PropertyDetailSampleUsage("**0**", "Specify the First EMail Index")]
        [PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Index")]
        [PropertyDetailSampleUsage("**-1**", "Specify the Last EMail Index")]
        [PropertyDetailSampleUsage("**{{{vIndex}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Index")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "CurrentPosition")]
        [PropertyDisplayText(true, "Index")]
        public string v_Index { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_OutputEMailName))]
        public string v_MailVariable { get; set; }

        public MailKitGetEMailFromEMailListCommand()
        {
            //this.CommandName = "MailKitGetEMailFromMailListCommand";
            //this.SelectionName = "Get EMail From EMailList";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var mails = v_MailListName.GetMailKitEMailListVariable(engine);

            int index;
            if (String.IsNullOrEmpty(v_Index))
            {
                var mailsVariable = v_MailListName.GetRawVariable(engine);
                index = mailsVariable.CurrentPosition;
            }
            else
            {  
                index = this.ConvertToUserVariableAsInteger(nameof(v_Index), engine);
            }

            if (index < 0)
            {
                index += mails.Count;
            }
            if (index >= mails.Count)
            {
                throw new Exception("Index '" + v_Index + "' out of range");
            }

            var mes = mails[index];

            // Clone MimeMessage
            using (var ms = new System.IO.MemoryStream())
            {
                mes.WriteTo(ms);

                ms.Position = 0;

                var newMes = MimeKit.MimeMessage.Load(ms);
                newMes.StoreInUserVariable(engine, v_MailVariable);
            }
        }
    }
}
