using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get File Info")]
    [Attributes.ClassAttributes.Description("This command returns a list of file paths from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a list of file paths from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetFileInfoCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePath))]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.AllowNoExtension, PropertyFilePathSetting.FileCounterBehavior.NoSupport)]
        public string v_TargetFileName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Information Type.")]
        [PropertyUISelectionOption("File size")]
        [PropertyUISelectionOption("Readonly file")]
        [PropertyUISelectionOption("Hidden file")]
        [PropertyUISelectionOption("Creation time")]
        [PropertyUISelectionOption("Last write time")]
        [PropertyUISelectionOption("Last access time")]
        [PropertyValidationRule("Type", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Type")]
        public string v_InfoType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_UserVariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        public string v_WaitTime { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_FilePathResult))]
        public string v_ResultPath { get; set; }

        public GetFileInfoCommand()
        {
            //this.CommandName = "GetFileInfoCommand";
            //this.SelectionName = "Get File Info";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var targetFile = FilePathControls.WaitForFile(this, nameof(v_TargetFileName), nameof(v_WaitTime), engine);
            //var fileInfo = new System.IO.FileInfo(targetFile);

            //var infoType = this.GetUISelectionValue(nameof(v_InfoType), engine);
            //string ret = "";
            //switch (infoType)
            //{
            //    case "file size":
            //        ret = fileInfo.Length.ToString();
            //        break;
            //    case "readonly file":
            //        ret = fileInfo.IsReadOnly ? "TRUE" : "FALSE";
            //        break;
            //    case "hidden file":
            //        ret = (((System.IO.FileAttributes)fileInfo.Attributes & System.IO.FileAttributes.Hidden) == System.IO.FileAttributes.Hidden) ? "TRUE": "FALSE";
            //        break;
            //    case "creation time":
            //        ret = fileInfo.CreationTime.ToString();
            //        break;
            //    case "last write time":
            //        ret = fileInfo.LastWriteTime.ToString();
            //        break;
            //    case "last access time":
            //        ret = fileInfo.LastAccessTime.ToString();
            //        break;
            //    //default:
            //    //    throw new Exception(infoType + " is not support.");
            //}

            //ret.StoreInUserVariable(engine, v_UserVariableName);

            FilePathControls.FileAction(this, engine,
                new Action<string>(path =>
                {
                    var fileInfo = new System.IO.FileInfo(path);

                    var infoType = this.GetUISelectionValue(nameof(v_InfoType), engine);
                    string ret = "";
                    switch (infoType)
                    {
                        case "file size":
                            ret = fileInfo.Length.ToString();
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

                    ret.StoreInUserVariable(engine, v_UserVariableName);
                })
            );
        }

        public override void AddInstance(InstanceCounter counter)
        {
            string type = (String.IsNullOrEmpty(v_InfoType) ? "" : v_InfoType.ToLower());

            switch(type)
            {
                case "readonly file":
                case "hidden file":
                    var boolType = new Automation.Attributes.PropertyAttributes.PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true);
                    var ins = (String.IsNullOrEmpty(v_UserVariableName) ? "" : v_UserVariableName);
                    counter.addInstance(ins, boolType, true);
                    counter.addInstance(ins, boolType, false);
                    break;
            }
        }

        public override void RemoveInstance(InstanceCounter counter)
        {
            string type = (String.IsNullOrEmpty(v_InfoType) ? "" : v_InfoType.ToLower());

            switch (type)
            {
                case "readonly file":
                case "hidden file":
                    var boolType = new Automation.Attributes.PropertyAttributes.PropertyInstanceType(PropertyInstanceType.InstanceType.Boolean, true);
                    var ins = (String.IsNullOrEmpty(v_UserVariableName) ? "" : v_UserVariableName);
                    counter.removeInstance(ins, boolType, true);
                    counter.removeInstance(ins, boolType, false);
                    break;
            }
        }
    }
}