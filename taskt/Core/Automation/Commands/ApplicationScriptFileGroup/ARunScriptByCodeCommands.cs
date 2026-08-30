using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    public abstract class ARunScriptByCodeCommands : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Script Code")]
        [PropertyValidationRule("Script Code", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(false, "Script Code")]
        [PropertyParameterOrder(5000)]
        public virtual string v_ScriptCode { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ApplicationScriptControls), nameof(ApplicationScriptControls.v_Arguments))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Arguments")]
        //[InputSpecification("Arguments", true)]
        //[PropertyDetailSampleUsage("**1**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**Hello**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**1 2 3**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**{{{vArgs}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Arguments")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Arguments", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        [PropertyParameterOrder(7000)]
        public virtual string v_Arguments { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Expand taskt Variables In Code")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyValidationRule("Expand Variables", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Expand Variables")]
        [PropertyParameterOrder(7000)]
        public virtual string v_ReplaceScriptVariables { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ApplicationScriptControls), nameof(ApplicationScriptControls.v_Result))]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Receive the Output")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(true, "Result")]
        [PropertyParameterOrder(8000)]
        public virtual string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        [PropertyDescription("Delete Script File After Execute")]
        [PropertyIsOptional(true, "Yes")]
        [PropertyValidationRule("Delete Script File", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(10000)]
        public virtual string v_DeleteScriptFile { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Folder to Save Temporary Script File")]
        [PropertyUISelectionOption("taskt Temporary Folder")]
        [PropertyUISelectionOption("User Temporary Folder")]
        [PropertyDetailSampleUsage("taskt Temporary Folder", "Generally **Document\\taskt\\Temporary**")]
        [PropertyDetailSampleUsage("User Temporary Folder", "Generally **C:\\Users\\UserName\\AppData\\Local\\Temp\\taskt**")]
        [PropertyIsOptional(true, "User Temporary Folder")]
        [PropertyValidationRule("Temporary", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Temporary Folder")]
        [PropertyParameterOrder(11000)]
        public virtual string v_TemporaryScriptFolder { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        [PropertyDescription("Variable Name to Store Temporary Script File Path")]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Temporary Script File Path", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "Temporary Script File Path")]
        [PropertyParameterOrder(12000)]
        public string v_ScriptFilePath { get; set; }

        /// <summary>
        /// run script action
        /// </summary>
        /// <param name="extensionAction"></param>
        /// <param name="engine"></param>
        protected void RunScriptAction(Func<string> extensionAction, ARunScriptFileCommands runScriptCommand, Engine.AutomationEngineInstance engine)
        {
            string tempFolderPath = "";
            switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_TemporaryScriptFolder), engine))
            {
                case "taskt temporary folder":
                    tempFolderPath = IO.Folders.GetTasktTemporaryFolderPath();
                    break;
                case "user temporary folder":
                    tempFolderPath = IO.Folders.GetUserTemporaryFolderPath();
                    break;
            }
            string scriptFilePath;
            using (var tempFile = new InnerScriptVariable(engine))
            {
                string scriptExtension = extensionAction();

                var getTempFile = new GetRandomFilePathCommand()
                {
                    v_TargetFolderPath = tempFolderPath,
                    v_Extension = scriptExtension,
                    v_Result = tempFile.VariableName,
                };
                getTempFile.RunCommand(engine);
                scriptFilePath = tempFile.VariableValue.ToString();
            }

            string code;
            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_ReplaceScriptVariables), engine))
            {
                code = this.ExpandValueOrUserVariable(nameof(v_ScriptCode), "Script", engine);
            }
            else
            {
                code = this.v_ScriptCode;
            }
            var writeScript = new WriteTextFileCommand()
            {
                v_TextToWrite = code,
                v_FilePath = scriptFilePath,
                v_ReplaceScriptVariables = this.v_ReplaceScriptVariables,
            };
            writeScript.RunCommand(engine);

            runScriptCommand.v_TargetFilePath = scriptFilePath;
            runScriptCommand.RunCommand(engine);

            if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_DeleteScriptFile), engine))
            {
                var deleteScript = new DeleteFileCommand()
                {
                    v_TargetFilePath = scriptFilePath,
                };
                deleteScript.RunCommand(engine);
            }

            if (!string.IsNullOrEmpty(v_ScriptFilePath))
            {
                scriptFilePath.StoreInUserVariable(engine,v_ScriptFilePath);
            }
        }
    }
}
