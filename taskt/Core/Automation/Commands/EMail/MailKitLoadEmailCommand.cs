using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Load Email")]
    [Attributes.ClassAttributes.Description("This command allows you to load EMail from File.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to load EMail from File.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitLoadEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_EMailPath))]
        [PropertyDescription("Path to the EMail")]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[InputSpecification("Path", true)]
        ////[SampleUsage("**C:\\Temp\\mymail.eml** or **{{{vPath}}}**")]
        //[PropertyDetailSampleUsage("**C:\\temp\\mymail.eml**", PropertyDetailSampleUsage.ValueType.Value, "Path")]
        //[PropertyDetailSampleUsage("**{{{vPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Path")]
        //[PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Path")]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_OutputEMailName))]
        public string v_MailName { get; set; }

        public MailKitLoadEmailCommand()
        {
            //this.CommandName = "MailKitLoadEmailCommand";
            //this.SelectionName = "Load Email";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string path = FilePathControls.formatFilePath_NoFileCounter(v_FilePath, engine, new List<string>() { "eml", "msg", "txt" }, true);

            var mail = MimeKit.MimeMessage.Load(path);
            mail.StoreInUserVariable(engine, v_MailName);
        }
    }
}
