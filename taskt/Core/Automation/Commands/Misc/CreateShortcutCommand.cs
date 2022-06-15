using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Misc Commands")]
    [Attributes.ClassAttributes.SubGruop("Other")]
    [Attributes.ClassAttributes.Description("This command allow to create shortcut file")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create shortcut file")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CreateShortcutCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyDescription("Please specify Shortcut Target File, Folder, or URL")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**C:\\temp\\target.txt** or **C:\\temp** or **http://example.com** or **{{{vPath}}** or **{{{vURL}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Target", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_TargetPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify saved Shortcut Path")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**C:\\temp\\shortcut.lnk** or **C:\\temp\\shortcut.url** or **{{{vShortcut}}}**")]
        [PropertyShowSampleUsageInDescription(true)]
        [Remarks("")]
        [PropertyValidationRule("Shortcut", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_SavePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Shortcut Description")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        public string v_Description { get; set; }

        public CreateShortcutCommand()
        {
            this.CommandName = "CreateShortcutCommand";
            this.SelectionName = "Create Shortcut";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string targetPath = v_TargetPath.ConvertToUserVariable(engine);
            bool isURL = (targetPath.StartsWith("http:") || (targetPath.StartsWith("https:")));

            //string savePath = v_SavePath.ConvertToUserVariable(engine);
            string savePath;

            if (!isURL)
            {
                //savePath = FilePathControls.formatFilePath(savePath, engine);
                //if (!FilePathControls.hasExtension(savePath))
                //{
                //    savePath += ".lnk";
                //}
                if (FilePathControls.containsFileCounter(v_SavePath, engine))
                {
                    savePath = FilePathControls.formatFilePath_ContainsFileCounter(v_SavePath, engine, "lnk");
                }
                else
                {
                    savePath = FilePathControls.formatFilePath_NoFileCounter(v_SavePath, engine, "lnk");
                }

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
                //savePath = FilePathControls.formatFilePath(savePath, engine);
                //if (!FilePathControls.hasExtension(savePath))
                //{
                //    savePath += ".url";
                //}
                if (FilePathControls.containsFileCounter(v_SavePath, engine))
                {
                    savePath = FilePathControls.formatFilePath_ContainsFileCounter(v_SavePath, engine, "url");
                }
                else
                {
                    savePath = FilePathControls.formatFilePath_NoFileCounter(v_SavePath, engine, "url");
                }

                string outputText = "[InternetShortcut]\nURL=" + targetPath;
                WriteTextFileCommand writeText = new WriteTextFileCommand
                {
                    v_FilePath = savePath,
                    v_TextToWrite = outputText
                };
                writeText.RunCommand(engine);
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            var ctls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Target: '" + v_TargetPath + "', Shortcut: '" + v_SavePath + "']";
        }
    }
}