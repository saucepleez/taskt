using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation")]
    [Attributes.ClassAttributes.CommandSettings("Get File Information")]
    [Attributes.ClassAttributes.Description("This command returns a list of file paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a list of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetFileInformationCommand : AFileExistsFilePathPathResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        //[PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        //public string v_TargetFilePath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Information Type.")]
        [PropertyUISelectionOption("File Size")]
        [PropertyUISelectionOption("File Size (KB)")]
        [PropertyUISelectionOption("File Size (MB)")]
        [PropertyUISelectionOption("File Size (GB)")]
        [PropertyUISelectionOption("Readonly File")]
        [PropertyUISelectionOption("Hidden File")]
        [PropertyUISelectionOption("Creation Time")]
        [PropertyUISelectionOption("Last Write Time")]
        [PropertyUISelectionOption("Last Access Time")]
        [PropertyValidationRule("Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        [PropertyParameterOrder(6000)]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(7000)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        //public string v_ResultPath { get; set; }

        public GetFileInformationCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            this.FileAction(engine, 
                new Func<string, string>(path =>
                {
                    var fileInfo = new FileInfo(path);

                    string ret = "";
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_InfoType), engine))
                    {
                        case "file size":
                            ret = fileInfo.Length.ToString();
                            break;
                        case "file size (kb)":
                            ret = (fileInfo.Length / 1024.0).ToString();
                            break;
                        case "file size (mb)":
                            ret = (fileInfo.Length / 1048576.0).ToString();
                            break;
                        case "file size (gb)":
                            ret = (fileInfo.Length / 1073741824.0).ToString();
                            break;
                        case "readonly file":
                            ret = fileInfo.IsReadOnly ? "TRUE" : "FALSE";
                            break;
                        case "hidden file":
                            ret = ((fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden) ? "TRUE" : "FALSE";
                            break;
                        case "creation time":
                            ret = fileInfo.CreationTime.ToString();
                            break;
                        case "last write time":
                            ret = fileInfo.LastWriteTime.ToString();
                            break;
                        case "last access time":
                            ret = fileInfo.LastAccessTime.ToString();
                            break;
                    }

                    ret.StoreInUserVariable(engine, v_Result);

                    return path;
                })
            );
        }

        public override void AddInstance(InstanceCounter counter)
        {
            string type = (string.IsNullOrEmpty(v_InfoType) ? "" : v_InfoType.ToLower());

            switch(type)
            {
                case "readonly file":
                case "hidden file":
                    var boolType = new Automation.Attributes.PropertyAttributes.PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true);
                    var ins = (string.IsNullOrEmpty(v_Result) ? "" : v_Result);
                    counter.AddInstance(ins, boolType, true);
                    counter.AddInstance(ins, boolType, false);
                    break;
            }
        }

        public override void RemoveInstance(InstanceCounter counter)
        {
            string type = (string.IsNullOrEmpty(v_InfoType) ? "" : v_InfoType.ToLower());

            switch (type)
            {
                case "readonly file":
                case "hidden file":
                    var boolType = new Automation.Attributes.PropertyAttributes.PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true);
                    var ins = (string.IsNullOrEmpty(v_Result) ? "" : v_Result);
                    counter.RemoveInstance(ins, boolType, true);
                    counter.RemoveInstance(ins, boolType, false);
                    break;
            }
        }
    }
}