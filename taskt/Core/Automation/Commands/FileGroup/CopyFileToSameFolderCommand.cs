using System;
using System.Xml.Serialization;
using System.IO;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation")]
    [Attributes.ClassAttributes.CommandSettings("Copy File To Same Folder")]
    [Attributes.ClassAttributes.Description("This command allows to copy a file to same folder.")]
    [Attributes.ClassAttributes.UsesDescription("This command allows to copy a file to same folder.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class CopyFileToSameFolderCommand : AFileCopySameRenameFileCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        //[PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        //public string v_TargetFilePath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("New File Name")]
        //[InputSpecification("New File Name", true)]
        //[PropertyDetailSampleUsage("**newfile**", PropertyDetailSampleUsage.ValueType.Value, "File Name")]
        //[PropertyDetailSampleUsage("**{{{vNewFileName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name")]
        //[PropertyValidationRule("New FileName", PropertyValidationRule.ValidationRuleFlags.Empty)]
        //[PropertyDisplayText(true, "New FileName")]
        //[PropertyParameterOrder(6000)]
        //public string v_NewFileName { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("Extension Option")]
        //[PropertyUISelectionOption("Auto")]
        //[PropertyUISelectionOption("Force Combine New Extension")]
        //[PropertyUISelectionOption("Contains New File Name")]
        //[PropertyUISelectionOption("Use Before Rename Path")]
        //[PropertyAddtionalParameterInfo("Auto", "If the New File Name does not contain an Extension and Not specified New Extension, it will automatically be given the extension of the path before the Rename.")]
        //[PropertyAddtionalParameterInfo("Force Combine New Extension", "Forces combining the specified extensions with the New Extension")]
        //[PropertyAddtionalParameterInfo("Contains New File Name", "Determine that New File Name contains the Extension and Do NOT add the Extension to the New File Name")]
        //[PropertyAddtionalParameterInfo("Use Before Rename Path", "Forces before Rename File Path extensions to be combined")]
        //[PropertySecondaryLabel(true)]
        //[PropertyIsOptional(true, "Auto")]
        //[PropertySelectionChangeEvent(nameof(cmbExtensionOption_SelectionChange))]
        //[PropertyParameterOrder(7000)]
        //public string v_ExtensionOption { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("New File Extension")]
        //[InputSpecification("New File Extension", true)]
        //[PropertyDetailSampleUsage("**txt**", PropertyDetailSampleUsage.ValueType.Value, "Extension")]
        //[PropertyDetailSampleUsage("**{{{vExtension}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Extension")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("New Extension", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(8000)]
        //public string v_NewExtension { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("When File Name Same After the Change")]
        //[PropertyUISelectionOption("Ignore")]
        //[PropertyUISelectionOption("Error")]
        //[PropertyDetailSampleUsage("**Ignore**", "Nothing to do")]
        //[PropertyDetailSampleUsage("**Error**", "Rise a Error")]
        //[PropertyIsOptional(true, "Ignore")]
        //[PropertyParameterOrder(9000)]
        //public string v_WhenFileNameSame { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //[PropertyDescription("Variable Name to Store File Path Before Rename")]
        //public string v_BeforeFilePathResult { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Store File Path After Rename")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //public string v_AfterFilePathResult { get; set; }

        public CopyFileToSameFolderCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.FileAction(engine,
                this.CreateActionFunc(File.Copy, engine)
            );
        }
    }
}
