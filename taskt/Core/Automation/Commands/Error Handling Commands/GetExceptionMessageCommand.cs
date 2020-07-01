using System;
using System.Collections.Generic;
using System.Linq;
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
    [Group("Error Handling Commands")]
    [Description("This command retrieves the most recent error in the engine and stores it in the defined variable.")]
    public class GetExceptionMessageCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Output Exception Message Variable")]
        [InputSpecification("Select or provide a variable from the variable list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("If you have enabled the setting **Create Missing Variables at Runtime** then you are not required " +
                 "to pre-define your variables, however, it is highly recommended.")]
        public string v_OutputUserVariableName { get; set; }

        public GetExceptionMessageCommand()
        {
            CommandName = "GetExceptionMessageCommand";
            SelectionName = "Get Exception Message";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var error = engine.ErrorsOccured.OrderByDescending(x => x.LineNumber).FirstOrDefault();
            string errorMessage = string.Empty;
            if (error != null)
                errorMessage = $"Source: {error.SourceFile}, Line: {error.LineNumber}, " +
                    $"Exception Type: {error.ErrorType}, Exception Message: {error.ErrorMessage}";
            errorMessage.StoreInUserVariable(sender, v_OutputUserVariableName);
        }

        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(
                CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor)
                );

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Exception Message in '{v_OutputUserVariableName}']";
        }
    }
}