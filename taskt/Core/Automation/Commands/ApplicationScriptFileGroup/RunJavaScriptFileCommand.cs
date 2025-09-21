using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script")]
    [Attributes.ClassAttributes.SubGruop("Windows Script File")]
    [Attributes.ClassAttributes.CommandSettings("Run JavaScript File")]
    [Attributes.ClassAttributes.Description("This command allows you to execute JavaScript.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command executes JavaScript using Edge")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_web))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class RunJavaScriptFileCommand : ARunScriptFileCommands
    {
        [XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_NoSample_FilePath))]
        [PropertyDescription("JavaScript File Path")]
        [PropertyDetailSampleUsage("**C:\\temp\\myscript.js**", PropertyDetailSampleUsage.ValueType.Value, "Script File")]
        [PropertyDetailSampleUsage("**{{{vScriptPath}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Script File")]
        [PropertyFilePathSetting(false, PropertyFilePathSetting.ExtensionBehavior.RequiredExtensionAndExists, PropertyFilePathSetting.FileCounterBehavior.NoSupport, "js")]
        public override string v_TargetFilePath { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        //[PropertyDescription("Arguments")]
        //[InputSpecification("Arguments", true)]
        //[PropertyDetailSampleUsage("**0**", PropertyDetailSampleUsage.ValueType.Value, "Arguments")]
        //[PropertyDetailSampleUsage("**{{{vValue}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Arguments")]
        //[Remarks("The value of the argument can be obtained with 'arguments[0]' in code.")]
        //[PropertyIsOptional(true)]
        //[PropertyDisplayText(false, "")]
        //[PropertyParameterOrder(6000)]
        //public string v_Arguments { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        //[PropertyDescription("Variable Name to Recieve Result Value")]
        //[PropertyIsOptional(true)]
        //[PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.None)]
        //[PropertyParameterOrder(7000)]
        //public string v_Result { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(FilePathControls), nameof(FilePathControls.v_WaitTime))]
        //public string v_WaitTimeForFile { get; set; }

        public RunJavaScriptFileCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var filePath = this.WaitForFile(engine);

            using (var myInstance = new InnerScriptVariable(engine))
            {
                myInstance.VariableValue = engine.GetNewAppInstanceName();
                var instanceVar = VariableNameControls.GetWrappedVariableName(myInstance.VariableName, engine);
                var webInstance = new SeleniumBrowserCreateWebBrowserInstanceCommand()
                {
                    v_InstanceName = instanceVar,
                    v_EngineType = "Edge",
                    v_HeadlessMode = "Yes",
                };
                webInstance.RunCommand(engine);

                var executeJS = new SeleniumBrowserExecuteScriptCommand()
                {
                    v_InstanceName = instanceVar,
                    v_CodeType = "File",
                    v_ScriptCode = filePath,
                    v_Args = this.v_Arguments,
                    v_userVariableName = this.v_Result,
                };
                executeJS.RunCommand(engine);

                var closeInsntace = new SeleniumBrowserCloseWebBrowserInstanceCommand()
                {
                    v_InstanceName = instanceVar,
                };
                closeInsntace.RunCommand(engine);
            }
        }
    }
}