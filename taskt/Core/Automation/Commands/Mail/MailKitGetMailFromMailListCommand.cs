using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Mail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to get MailKitMail from MailKitMailList.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get MailKitMail from MailKitMailList.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class MailKitGetMailFromMailListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify MailList Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vMailList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("MailList", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        public string v_MailListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Mail Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **{{{vIndex}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "CurrentPosition")]
        [PropertyTextBoxSetting(1, false)]
        public string v_Index { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store Mail")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vMail** or **{{{vMail}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Mail Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_MailVariable { get; set; }

        public MailKitGetMailFromMailListCommand()
        {
            this.CommandName = "MailKitGetMailFromMailListCommand";
            this.SelectionName = "Get Mail From MailList";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var mails = v_MailListName.GetMailKitMailListVariable(engine);

            int index;
            if (String.IsNullOrEmpty(v_Index))
            {
                var mailsVariable = v_MailListName.GetRawVariable(engine);
                index = mailsVariable.CurrentPosition;
            }
            else
            {
                index = v_Index.ConvertToUserVariableAsInteger("Index", engine);   
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
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            string index = (v_Index == null) ? "" : v_Index;
            return base.GetDisplayValue() + "[MailList: '" + v_MailListName + "', Index: '" + index + "', Store: '" + v_MailVariable + "']";
        }
    }
}
