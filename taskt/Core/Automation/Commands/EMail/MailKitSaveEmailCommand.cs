using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("EMail Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Save Email")]
    [Attributes.ClassAttributes.Description("This command allows you to Save EMail.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Save EMail.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class MailKitSaveEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_InputEMailName))]
        public string v_MailName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(EMailControls), nameof(EMailControls.v_EMailPath))]
        [PropertyDescription("Path to Save the File")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "eml")]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[InputSpecification("")]
        //[SampleUsage("**C:\\Temp\\mymail.eml** or **{{{vPath}}}**")]
        //[Remarks("")]
        //[PropertyShowSampleUsageInDescription(true)]
        //[PropertyTextBoxSetting(1, false)]
        //[PropertyValidationRule("Path", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "Path")]
        public string v_SavePath { get; set; }

        public MailKitSaveEmailCommand()
        {
            //this.CommandName = "MailKitSaveEmailCommand";
            //this.SelectionName = "Save Email";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var mail = v_MailName.GetMailKitEMailVariable(engine);

            //string path;
            //if (FilePathControls.ContainsFileCounter(v_SavePath, engine))
            //{
            //    path = FilePathControls.FormatFilePath_ContainsFileCounter(v_SavePath, engine, "eml");
            //}
            //else
            //{
            //    path = FilePathControls.FormatFilePath_NoFileCounter(v_SavePath, engine, "eml");
            //}
            var path = this.ConvertToUserVariableAsFilePath(nameof(v_SavePath), engine);

            mail.WriteTo(path);
        }
    }
}
