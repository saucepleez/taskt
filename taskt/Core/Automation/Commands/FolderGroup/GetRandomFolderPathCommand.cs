using System;
using System.IO;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{

    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Get Random Folder Path")]
    [Attributes.ClassAttributes.Description("This command allows you to get random folder name path.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get random folder name path.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetRandomFolderPathCommand : AFolderExistsFolderPathPathResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //public string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyParameterOrder(6000)]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Check Folder Does Not Exists")]
        [PropertyFirstValue("Yes")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyValidationRule("Check File", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(6500)]
        public string v_CheckFolderDoesNotExists { get; set; }

        //[XmlAttribute]
        //public string v_WaitTimeForFolder { get; set; }

        //[XmlAttribute]
        //public string v_ResultPath { get; set; }

        public GetRandomFolderPathCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            string GetRandomFolder(string baseFolder)
            {
                var fn = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
                return Path.Combine(baseFolder, fn);
            }

            this.FolderAction(engine,
                new Func<string, string>(folderPath =>
                {
                    string res = "";
                    if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_CheckFolderDoesNotExists), engine))
                    {
                        int i = 0;
                        while (true)
                        {
                            var p = GetRandomFolder(folderPath);
                            if (!Directory.Exists(p))
                            {
                                res = p;
                                break;
                            }
                            i++;
                            if (i > 256)
                            {
                                throw new Exception("Could not get the path to a folder that does not exist.");
                            }
                        }
                    }
                    else
                    {
                        res = GetRandomFolder(folderPath);
                    }
                    res.StoreInUserVariable(engine, v_Result);

                    return res;
                })
            );
        }
    }
}