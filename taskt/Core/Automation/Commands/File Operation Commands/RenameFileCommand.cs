﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("File Operation Commands")]
    [Description("This command renames an existing file.")]
    public class RenameFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("File Path")]
        [InputSpecification("Enter or Select the path to the file.")]
        [SampleUsage(@"C:\temp\myfile.txt || {ProjectPath}\myfile.txt || {vTextFilePath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowFileSelectionHelper)] 
        public string v_SourceFilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("New File Name (with extension)")]
        [InputSpecification("Specify new file name with extension.")]
        [SampleUsage("newfile.txt || {vNewFileName}")]
        [Remarks("Changing the file extension will not automatically convert files.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_NewName { get; set; }

        public RenameFileCommand()
        {
            CommandName = "RenameFileCommand";
            SelectionName = "Rename File";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertToUserVariable(sender);
            var newFileName = v_NewName.ConvertToUserVariable(sender);

            //get source file name and info
            FileInfo sourceFileInfo = new FileInfo(sourceFile);

            //create destination
            var destinationPath = Path.Combine(sourceFileInfo.DirectoryName, newFileName);

            //rename file
            File.Move(sourceFile, destinationPath);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_NewName", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Rename '{v_SourceFilePath}' to '{v_NewName}']";
        }
    }
}