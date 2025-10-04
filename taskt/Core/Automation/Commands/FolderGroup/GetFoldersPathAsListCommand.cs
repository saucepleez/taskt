using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Folder Operation")]
    [Attributes.ClassAttributes.CommandSettings("Get Folders Path As List")]
    [Attributes.ClassAttributes.Description("This command returns a list of folder directories from a specified location")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to return a list of folder directories from a specific location.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetFoldersPathAsListCommand : AFolderExistsFolderPathCommands, ITextCompareProperties, IListResultProperties
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_FolderPath))]
        //public string v_TargetFolderPath { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Folder Name to Filter")]
        [InputSpecification("Folder Name to Filter", true)]
        //[SampleUsage("**hello** or **{{{vFolderName}}}**")]
        [PropertyDetailSampleUsage("**hello**", PropertyDetailSampleUsage.ValueType.Value, "Filter")]
        [PropertyDetailSampleUsage("**{{{vFolderName}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Filter")]
        [PropertyIsOptional(true, " Empty and searched All Folders")]
        [PropertyValidationRule("", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Filter")]
        [PropertyParameterOrder(6000)]
        public string v_SearchFolderName { get; set; }

        [XmlAttribute]
        //[PropertyDetailSampleUsage(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        //[PropertyDescription("Folder Name Search Method")]
        //[PropertyUISelectionOption("Contains")]
        //[PropertyUISelectionOption("Starts with")]
        //[PropertyUISelectionOption("Ends with")]
        //[PropertyUISelectionOption("Exact match")]
        //[PropertyIsOptional(true, "Contains")]
        [PropertyVirtualProperty(nameof(TextCompareSelectMethodControls), nameof(TextCompareSelectMethodControls.v_CompareMethod))]
        [PropertyDescription("Folder Name Compare Method")]
        [PropertyParameterOrder(6100)]
        public string v_CompareMethod { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextCompareSelectMethodControls), nameof(TextCompareSelectMethodControls.v_CaseSensitiveNo))]
        [PropertyParameterOrder(6200)]
        public string v_CaseSensitive { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(TextCompareSelectMethodControls), nameof(TextCompareSelectMethodControls.v_TrimBeforeCompare))]
        [PropertyParameterOrder(6300)]
        public string v_TrimBeforeCompare { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_OutputListName))]
        [PropertyParameterOrder(6400)]
        public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FolderPathControls), nameof(FolderPathControls.v_WaitTime))]
        //public string v_WaitTimeForFolder { get; set; }

        public GetFoldersPathAsListCommand()
        {
            //this.CommandName = "GetFoldersCommand";
            //this.SelectionName = "Get Folders";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //var sourceFolder = FolderPathControls.WaitForFolder(this, nameof(v_TargetFolderPath), nameof(v_WaitTimeForFolder), engine);

            this.FolderAction(engine,
                new Func<string, string>(sourceFolder =>
                {
                    // get folder list
                    var directoriesList = Directory.GetDirectories(sourceFolder).ToList();

                    //var searchFolder = v_SearchFolderName.ExpandValueOrUserVariableAsFolderName(engine);
                    var searchFolder = this.ExpandValueOrUserVariable(nameof(v_SearchFolderName), "Folder Name", engine);

                    var compareFunc = this.GetCompareFunction(engine);

                    var filteredDirectory = new List<string>();
                    foreach (var f in directoriesList)
                    {
                        if (compareFunc(Path.GetFileName(f), searchFolder))
                        {
                            filteredDirectory.Add(f);
                        }
                    }

                    this.StoreListInUserVariable(filteredDirectory, engine);

                    return sourceFolder;
                })
            );   
        }
    }
}
