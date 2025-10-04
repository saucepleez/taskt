using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run Batch Script By Code")]
    [Attributes.ClassAttributes.Description("This command allows you to run a batch script by code.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to run a batch script by code.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_script))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RunBatchScriptByCodeCommand : ARunScriptByCodeCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        //[PropertyDescription("Batch Script Code")]
        [PropertyDetailSampleUsage("**dir**", PropertyDetailSampleUsage.ValueType.Value, "Batch Script")]
        [PropertyDetailSampleUsage("**{{{vCode}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Batch Script")]
        //[PropertyParameterOrder(5000)]
        public override string v_ScriptCode { get; set; }

        //[XmlAttribute]
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
        //[PropertyParameterOrder(6000)]
        //public string v_Arguments { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Receive the Output")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(7000)]
        //public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Script File Type")]
        [PropertyUISelectionOption("Batch")]
        [PropertyUISelectionOption("VBScript")]
        [PropertyUISelectionOption("JScript")]
        [PropertyUISelectionOption("Windows Script Host")]
        [PropertyIsOptional(true, "Batch")]
        [PropertyValidationRule("Script File Type", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(false, "")]
        [PropertyParameterOrder(9000)]
        public string v_ScriptType { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(SelectionItemsControls), nameof(SelectionItemsControls.v_YesNoComboBox))]
        //[PropertyDescription("Delete Script File After Execute")]
        //[PropertyIsOptional(true, "Yes")]
        //[PropertyFirstValue("Yes")]
        //[PropertyValidationRule("Delete Script File", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(9000)]
        //public string v_DeleteScriptFile { get; set; }

        public RunBatchScriptByCodeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //string scriptFilePath;
            //using (var tempFolder = new InnerScriptVariable(engine))
            //{
            //    var getTempFolder = new GetSpecialFolderPathCommand()
            //    {
            //        v_FolderType = "temporary",
            //        v_Result = tempFolder.VariableName,
            //    };
            //    getTempFolder.RunCommand(engine);

            //    using (var tempFile = new InnerScriptVariable(engine))
            //    {
            //        string scriptExtension = "";
            //        switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ScriptType), engine))
            //        {
            //            case "batch":
            //                scriptExtension = "bat";
            //                break;
            //            case "vbscript":
            //                scriptExtension = "vbs";
            //                break;
            //            case "jscript":
            //                scriptExtension = "js";
            //                break;
            //            case "windows script host":
            //                scriptExtension = "wsf";
            //                break;
            //        }

            //        var getTempFile = new GetRandomFilePathCommand()
            //        {
            //            v_TargetFolderPath = tempFolder.VariableValue.ToString(),
            //            v_Extension = scriptExtension,
            //            v_Result = tempFile.VariableName,
            //        };
            //        getTempFile.RunCommand(engine);
            //        scriptFilePath = tempFile.VariableValue.ToString();
            //    }
            //}

            //var code = this.ExpandValueOrUserVariable(nameof(v_ScriptCode), "Script", engine);
            //var writeScript = new WriteTextFileCommand()
            //{
            //    v_TextToWrite = code,
            //    v_FilePath = scriptFilePath,
            //};
            //writeScript.RunCommand(engine);

            //var runScript = new RunBatchScriptFileCommand()
            //{
            //    v_TargetFilePath = scriptFilePath,
            //    v_Arguments = this.v_Arguments,
            //    v_Result = this.v_Result,
            //};
            //runScript.RunCommand(engine);

            //if (this.ExpandValueOrUserVariableAsYesNo(nameof(v_DeleteScriptFile), engine))
            //{
            //    var deleteScript = new DeleteFileCommand()
            //    {
            //        v_TargetFilePath = scriptFilePath,
            //    };
            //    deleteScript.RunCommand(engine);
            //}

            this.RunScriptAction(
                new Func<string>(() =>
                {
                    string ret = "";
                    switch (this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_ScriptType), engine))
                    {
                        case "batch":
                            ret = "bat";
                            break;
                        case "vbscript":
                            ret = "vbs";
                            break;
                        case "jscript":
                            ret = "js";
                            break;
                        case "windows script host":
                            ret = "wsf";
                            break;
                    }
                    return ret;
                }), 
                new RunBatchScriptFileCommand()
                {
                    v_Arguments = this.v_Arguments,
                    v_Result = this.v_Result,
                },
                engine);
        }
    }
}
