using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("File Operation")]
    [Attributes.ClassAttributes.CommandSettings("Get Random File Path")]
    [Attributes.ClassAttributes.Description("This command allows you to get random file name path.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get random file name path.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetRandomFilePathCommand : AFolderExistsFolderPathCommands, ICanHandleFileName
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        [PropertyIsOptional(true, "Script File Folder")]
        [PropertyValidationRule("Folder", PropertyValidationRule.ValidationRuleFlags.None)]
        public override string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("File Extension")]
        [Remarks("")]
        [PropertyIsOptional(true)]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyDetailSampleUsage("**txt**", "Specify Text File")]
        [PropertyDetailSampleUsage("**{{{vExtension}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "File Name After Counter")]
        [PropertyValidationRule("Extension", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Extension")]
        [PropertyParameterOrder(6300)]
        public virtual string v_Extension { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6400)]
        public virtual string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Check File Does Not Exists")]
        [PropertyFirstValue("Yes")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyValidationRule("Check File", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(6500)]
        public string v_CheckFileDoesNotExists { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        //[PropertyFirstValue("10")]
        //public override string v_WaitTimeForFolder { get; set; }

        public GetRandomFilePathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var ext = this.ExpandValueOrUserVariable(nameof(v_Extension), "Extention", engine);
            var extensionAction = (string.IsNullOrEmpty(ext)) ?
                new Func<string, string>(p =>
                {
                    return p;
                }) :
                new Func<string, string>(p =>
                {
                    return Path.ChangeExtension(p, ext);
                });

            string GetRandomPath(string basePath)
            {
                var p = Path.Combine(basePath, Path.GetRandomFileName());
                return extensionAction(p);
            }

            this.FolderAction(engine,
                new Func<string, string>(folderPath =>
                {
                    string res = "";
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_CheckFileDoesNotExists), engine))
                    {
                        int i = 0;
                        while (true)
                        {
                            var p = GetRandomPath(folderPath);
                            if (!File.Exists(p))
                            {
                                res = p;
                                break;
                            }
                            i++;
                            if (i > 256)
                            {
                                throw new Exception("Could not get the path to a file that does not exist.");
                            }
                        }
                    }
                    else
                    {
                        res = GetRandomPath(folderPath);
                    }
                    res.StoreInUserVariable(engine, v_Result);

                    return folderPath;
                })
            );
        }
    }
}