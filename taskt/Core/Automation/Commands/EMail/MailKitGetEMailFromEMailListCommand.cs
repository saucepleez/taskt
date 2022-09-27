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
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to get EMail from EMailList.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get EMail from EMailList.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitGetEMailFromEMailListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify EMailList Variable Name")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**{{{vMailList}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("EMailList", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMailList, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyDisplayText(true, "EMailList")]
        public string v_MailListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify EMailList Index")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**0** or **1** or **{{{vIndex}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsOptional(true, "CurrentPosition")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyDisplayText(true, "Index")]
        public string v_Index { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store EMail")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vEMail** or **{{{vEMail}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("EMail Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.MailKitEMail, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyDisplayText(true, "Store")]
        public string v_MailVariable { get; set; }

        public MailKitGetEMailFromEMailListCommand()
        {
            this.CommandName = "MailKitGetEMailFromMailListCommand";
            this.SelectionName = "Get EMail From MailList";
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

        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
        //    RenderedControls.AddRange(ctrls);

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    string index = (v_Index == null) ? "" : v_Index;
        //    return base.GetDisplayValue() + " [EMailList: '" + v_MailListName + "', Index: '" + index + "', Store: '" + v_MailVariable + "']";
        //}
    }
}
