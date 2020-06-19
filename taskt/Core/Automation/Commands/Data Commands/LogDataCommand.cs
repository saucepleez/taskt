using Serilog;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.ClassAttributes;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Automation.Engine;
using taskt.Core.Utilities.CommonUtilities;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command logs text data to either an engine file or a custom file.")]
    public class LogDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Write Log To")]
        [InputSpecification("Specify the corresponding logging option to save logs to Engine Logs or to a custom File.")]
        [SampleUsage("Engine Logs || Custom File Name || {vFileVariable}")]
        [Remarks("Selecting 'Engine Logs' will result in writing execution logs in the 'Engine Logs'. " +
            "The current Date and Time will be automatically appended to a local file if a custom file name is provided. " +
            "Logs are all saved in the TaskT Root Folder in the 'Logs' folder.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LogFile { get; set; }

        [XmlAttribute]
        [PropertyDescription("Log Text")]
        [InputSpecification("Specify the log text.")]
        [SampleUsage("Third Step is Complete || {vLogText}")]
        [Remarks("Provide only text data.")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LogText { get; set; }

        public LogDataCommand()
        {
            CommandName = "LogDataCommand";
            SelectionName = "Log Data";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get text to log and log file name       
            var textToLog = v_LogText.ConvertToUserVariable(sender);
            var logFile = v_LogFile.ConvertToUserVariable(sender);

            //determine log file
            if (v_LogFile == "Engine Logs")
            {
                //log to the standard engine logs
                var engine = (AutomationEngineInstance)sender;
                engine.EngineLogger.Information(textToLog);
            }
            else
            {
                //create new logger and log to custom file
                using (var logger = new Logging().CreateLogger(logFile, RollingInterval.Infinite))
                {
                    logger.Information(textToLog);
                }
            }
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LogFile", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_LogText", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            string logFileName;
            if (v_LogFile == "Engine Logs")
            {
                logFileName = "taskt_Engine_Logs.txt";
            }
            else
            {
                logFileName = $"taskt_{v_LogFile}_Logs.txt";
            }

            return base.GetDisplayValue() + $" [Write Log '{v_LogText}' to 'taskt\\Logs\\{logFileName}']";
        }
    }
}