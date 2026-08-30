using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Log Data")]
    [Attributes.ClassAttributes.Description("This command logs data to files.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to log custom data to a file for debugging or analytical purposes.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_files))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class LogDataCommand : ScriptCommand, ICanHandleFileName
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Custom Log Name")]
        [InputSpecification("File Name", true)]
        //[PropertyUISelectionOption("Engine Logs")]
        //[PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[SampleUsage("Select 'Engine Logs' or specify your own file")]
        [PropertyDetailSampleUsage("myLog", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("{{{vLogName}}}", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyValidationRule("Custom Log Name", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyIsOptional(true, "Engine Logs")]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyDisplayText(true, "Log Name")]
        [Remarks("Date and Time will be automatically appended to the file name. Logs are all saved in 'Documents\\taskt\\Logs folder'. If 'myLog' is specified, the log file name will be 'taskt **myLog** Logs'.")]
        public string v_LogFile { get; set; }

        [XmlAttribute]
        //[PropertyDescription("Please enter the text to log.")]
        //[PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        //[InputSpecification("Indicate the value of the text to be saved.")]
        //[SampleUsage("Third Step Complete, {{{vVariable}}}, etc.")]
        //[Remarks("")]
        //[PropertyRecommendedUIControl(Attributes.PropertyAttributes.PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        //[PropertyIsVariablesList(true)]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Text to Log")]
        [PropertyDetailSampleUsage("Hello", PropertyDetailSampleUsage.ValueType.Value)]
        [PropertyDetailSampleUsage("{{{vLogValue}}}", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyDisplayText(true, "Text")]
        public string v_LogText { get; set; }

        public LogDataCommand()
        {
            //this.CommandName = "LogDataCommand";
            //this.SelectionName = "Log Data";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            // get text to log and log file name       
            //var textToLog = v_LogText.ExpandValueOrUserVariable(engine);
            var textToLog = this.ExpandValueOrUserVariable(nameof(v_LogText), "Text to Log", engine);

            //var logFile = v_LogFile.ExpandValueOrUserVariable(engine);
            if (string.IsNullOrEmpty(v_LogFile))
            {
                v_LogFile = "Engine Logs";
            }
            var logFile = this.ExpandValueOrUserVariableAsFileName(nameof(v_LogFile), engine);

            // determine log file
            //if (v_LogFile == "Engine Logs")
            if (logFile == "Engine Logs")
            {
                // log to the standard engine logs
                engine.WriteLog(textToLog);
            }
            else
            {
                // create new logger and log to custom file
                using (var logger = new Logging().CreateLogger(logFile, Serilog.RollingInterval.Infinite))
                {
                    logger.Information(textToLog);
                }
            }
        }

        //public override List<Control> Render(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    //create standard group controls
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LogFile", this, editor));
        //    RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LogText", this, editor));

        //    return RenderedControls;
        //}


        //public override string GetDisplayValue()
        //{
        //    string logFileName;
        //    if (v_LogFile == "Engine Logs")
        //    {
        //        logFileName = "taskt Engine Logs.txt";
        //    }
        //    else
        //    {
        //        logFileName = "taskt " + v_LogFile + " Logs.txt";
        //    }


        //    return base.GetDisplayValue() + " [Write Log to 'taskt\\Logs\\" + logFileName + "']";
        //}

        //public override bool IsValidate(UI.Forms.ScriptBuilder.CommandEditor.frmCommandEditor editor)
        //{
        //    base.IsValidate(editor);

        //    if (String.IsNullOrEmpty(this.v_LogFile))
        //    {
        //        this.validationResult += "Log file is empty.\n";
        //        this.IsValid = false;
        //    }

        //    return this.IsValid;
        //}
    }
}