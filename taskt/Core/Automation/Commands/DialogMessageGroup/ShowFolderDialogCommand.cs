using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Dialog/Message")]
    [Attributes.ClassAttributes.CommandSettings("Show Folder Dialog")]
    [Attributes.ClassAttributes.Description("Show FolderBrowserDialog")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to select folder.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_input))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class ShowFolderDialogCommand : AShowFileFolderDialogCommands
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Dialog Type")]
        [PropertyUISelectionOption("FolderBrowserDialog")]
        [PropertyUISelectionOption("CommonOpenFolderDialog")]
        [PropertyIsOptional(true, "CommonOpenFolderDialog")]
        [PropertyValidationRule("Dialog Type", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Dialog Type")]
        public string v_DialogType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //public string v_Result { get; set; }

        public ShowFolderDialogCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            engine.tasktEngineUI.Invoke(new Action(() =>
            {
                switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_DialogType), engine))
                {
                    case "folderbrowserdialog":
                        using (var dialog = new FolderBrowserDialog())
                        {
                            dialog.SelectedPath = this.GetInitialDirectory(engine);

                            this.ShowDialogProcess(dialog,
                                new Func<string>(() => dialog.SelectedPath),
                                "FolderBrowserDialog", engine);
                        }
                        break;
                    case "commonopenfolderdialog":
                        using (var dialog = new CommonOpenFileDialog())
                        {
                            dialog.IsFolderPicker = true;
                            dialog.InitialDirectory = this.GetInitialDirectory(engine);

                            this.ShowDialogProcess(dialog,
                                new Func<string>(() => dialog.FileName),
                                "CommonOpenFileDialog", engine);
                        }
                        break;
                }
            }));
        }
    }
}
