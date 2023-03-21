using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.UI.CustomControls;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Application/Script Commands")]
    [Attributes.ClassAttributes.SubGruop("Application")]
    [Attributes.ClassAttributes.CommandSettings("Stop Application")]
    [Attributes.ClassAttributes.Description("This command allows you to stop a program or a process.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command to close an application by its name such as 'chrome'. Alternatively, you may use the Close Window or Thick App Command instead.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements 'Process.CloseMainWindow'.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class StopApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_DisallowNewLine_OneLineTextBox))]
        [PropertyDescription("Application Name")]
        [InputSpecification("Application Name", true)]
        [PropertyDetailSampleUsage("**notepad**", "Stop Notepad")]
        [PropertyDetailSampleUsage("**myapp**", PropertyDetailSampleUsage.ValueType.Value, "Application Name")]
        [PropertyDetailSampleUsage("**{{{vProcess}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Application Name")]
        [Remarks("Provide the **Program Process Name** as it appears as a process in Windows Task Manager, **NOT** Window Name. The program name may vary from the actual process name.  You can use Thick App commands instead to close an application window.")]
        [PropertyCustomUIHelper("Show Process Selector", nameof(lnkProcessSelector_Click))]
        [PropertyValidationRule("Application Name", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Application")]
        public string v_ProgramShortName { get; set; }

        public StopApplicationCommand()
        {
            //this.CommandName = "StopProgramCommand";
            //this.SelectionName = "Stop Process";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string shortName = v_ProgramShortName.ConvertToUserVariable(engine);
            var processes = System.Diagnostics.Process.GetProcessesByName(shortName);

            foreach (var prc in processes)
            {
                prc.CloseMainWindow();
            }
        }

        private void lnkProcessSelector_Click(object sender, EventArgs e)
        {
            var txt = (TextBox)((CommandItemControl)sender).Tag;

            var ps = System.Diagnostics.Process.GetProcesses()
                .OrderByDescending(p => p.Id).Select(p => p.ProcessName).ToList();
            using (var fm = new UI.Forms.Supplemental.frmItemSelector(ps))
            {
                if (fm.ShowDialog() == DialogResult.OK)
                {
                    txt.Text = fm.selectedItem.ToString();
                }
            }
        }
    }
}