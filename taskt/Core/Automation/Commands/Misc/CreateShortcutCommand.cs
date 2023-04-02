using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.CommandSettings("Create Shortcut")]
    [Attributes.ClassAttributes.Description("This command allow to create shortcut file")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create shortcut file")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateShortcutCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Target File, Folder, or URL")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [InputSpecification("File, Folder, or URL", true)]
        [PropertyDetailSampleUsage("**C:\\temp\\target.txt", PropertyDetailSampleUsage.ValueType.Value, "Target File")]
        [PropertyDetailSampleUsage("**C:\\temp\\", PropertyDetailSampleUsage.ValueType.Value, "Target Folder")]
        [PropertyDetailSampleUsage("**http://example.com", PropertyDetailSampleUsage.ValueType.Value, "Target URL")]
        [PropertyDetailSampleUsage("**{{{vPath}}}", PropertyDetailSampleUsage.ValueType.VariableValue, "Target File, Folder, or URL")]
        [PropertyValidationRule("Target", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetPath { get; set; }

        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Saved Shortcut Path")]
        //[InputSpecification("Shortcut Path", true)]
        //[PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        //[PropertyDetailSampleUsage("**C:\\temp\\shortcut.lnk**", PropertyDetailSampleUsage.ValueType.Value, "Shortcut Path")]
        //[PropertyDetailSampleUsage("**C:\\temp\\shortcut.url**", PropertyDetailSampleUsage.ValueType.Value, "Shortcut Path")]
        //[PropertyDetailSampleUsage("**{{{vShortcut}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Shortcut Path")]
        //[PropertyValidationRule("Shortcut", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("Saved Shortcut Path")]
        [InputSpecification("Shortcut Path", true)]
        [PropertyDetailSampleUsage("**C:\\temp\\shortcut.lnk**", PropertyDetailSampleUsage.ValueType.Value, "Shortcut Path")]
        [PropertyDetailSampleUsage("**C:\\temp\\shortcut.url**", PropertyDetailSampleUsage.ValueType.Value, "Shortcut Path")]
        [PropertyDetailSampleUsage("**{{{vShortcut}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Shortcut Path")]
        [PropertyValidationRule("Shortcut", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "lnk,url")]
        public string v_SavePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Shortcut Description")]
        [InputSpecification("Description", true)]
        [PropertyIsOptional(true)]
        public string v_Description { get; set; }

        public CreateShortcutCommand()
        {
            //this.CommandName = "CreateShortcutCommand";
            //this.SelectionName = "Create Shortcut";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string targetPath = v_TargetPath.ConvertToUserVariable(engine);
            //bool isURL = (targetPath.StartsWith("http:") || (targetPath.StartsWith("https:")));

            if (!FilePathControls.IsURL(targetPath))
            {
                //if (FilePathControls.ContainsFileCounter(v_SavePath, engine))
                //{
                //    savePath = FilePathControls.FormatFilePath_ContainsFileCounter(v_SavePath, engine, "lnk");
                //}
                //else
                //{
                //    savePath = FilePathControls.FormatFilePath_NoFileCounter(v_SavePath, engine, "lnk");
                //}
                var savePath = v_SavePath.ConvertToUserVariableAsFilePath(new PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "lnk"), engine);

                string description = v_Description.ConvertToUserVariable(engine);

                // WshShell
                Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8"));
                dynamic shell = Activator.CreateInstance(t);
                var shortcut = shell.CreateShortcut(savePath);

                shortcut.TargetPath = targetPath;
                shortcut.Description = description;
                shortcut.Save();

                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shortcut);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(shell);
            }
            else
            {
                //if (FilePathControls.ContainsFileCounter(v_SavePath, engine))
                //{
                //    savePath = FilePathControls.FormatFilePath_ContainsFileCounter(v_SavePath, engine, "url");
                //}
                //else
                //{
                //    savePath = FilePathControls.FormatFilePath_NoFileCounter(v_SavePath, engine, "url");
                //}
                var savePath = v_SavePath.ConvertToUserVariableAsFilePath(new PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "url"), engine);

                string outputText = "[InternetShortcut]\nURL=" + targetPath;
                WriteTextFileCommand writeText = new WriteTextFileCommand
                {
                    v_FilePath = savePath,
                    v_TextToWrite = outputText
                };
                writeText.RunCommand(engine);
            }
        }
    }
}