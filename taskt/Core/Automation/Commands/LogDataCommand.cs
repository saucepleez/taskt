using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Data Commands")]
    [Attributes.ClassAttributes.Description("This command logs data to files.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to log custom data to a file for debugging or analytical purposes.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Thread.Sleep' to achieve automation.")]
    public class LogDataCommand : ScriptCommand
    {


        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Select existing log file or enter a custom name.")]
        [Attributes.PropertyAttributes.PropertyUISelectionOption("Engine Logs")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the file name where logs should be appended to")]
        [Attributes.PropertyAttributes.SampleUsage("Select 'Engine Logs' or specify your own file")]
        [Attributes.PropertyAttributes.Remarks("Date and Time will be automatically appended to the file name.  Logs are all saved in taskt Root\\Logs folder")]
        public string v_LogFile { get; set; }

        [XmlAttribute]
        [Attributes.PropertyAttributes.PropertyDescription("Please enter the text to log.")]
        [Attributes.PropertyAttributes.PropertyUIHelper(Attributes.PropertyAttributes.PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [Attributes.PropertyAttributes.InputSpecification("Indicate the value of the text to be saved.")]
        [Attributes.PropertyAttributes.SampleUsage("Third Step Complete, [vVariable], etc.")]
        [Attributes.PropertyAttributes.Remarks("")]
        public string v_LogText { get; set; }

        public LogDataCommand()
        {
            this.CommandName = "LogDataCommand";
            this.SelectionName = "Log Data";
            this.CommandEnabled = true;
            this.CustomRendering = true;
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
                var engine = (Core.Automation.Engine.AutomationEngineInstance)sender;
                engine.engineLogger.Information(textToLog);
            }
            else
            {
                //create new logger and log to custom file
                using (var logger = new Core.Logging().CreateLogger(logFile, Serilog.RollingInterval.Infinite))
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
                logFileName = "taskt Engine Logs.txt";
            }
            else
            {
                logFileName = "taskt " + v_LogFile + " Logs.txt";
            }


            return base.GetDisplayValue() + " [Write Log to 'taskt\\Logs\\" + logFileName + "']";
        }
    }
}