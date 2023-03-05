using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Word Commands")]
    [Attributes.ClassAttributes.Description("This command returns existance of Word instance.")]
    [Attributes.ClassAttributes.CommandSettings("Check Word Instance Exists")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to check Word instance.")]
    [Attributes.ClassAttributes.ImplementationDescription("This command implements Word Interop to achieve automation.")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class WordCheckWordInstanceExistsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(WordControls), nameof(WordControls.v_InstanceName))]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(BooleanControls), nameof(BooleanControls.v_Result))]
        [Remarks("When Word Instance exists, Result is **True**")]
        public string v_applyToVariableName { get; set; }

        public WordCheckWordInstanceExistsCommand()
        {
            //this.CommandName = "CheckWordInstanceExistsCommand";
            //this.SelectionName = "Check Word Instance Exists";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;

            //this.v_InstanceName = "";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            try
            {
                var _ = v_InstanceName.GetWordInstance(engine);
                true.StoreInUserVariable(engine, v_applyToVariableName);
            }
            catch
            {
                false.StoreInUserVariable(engine, v_applyToVariableName);
            }
        }
    }
}