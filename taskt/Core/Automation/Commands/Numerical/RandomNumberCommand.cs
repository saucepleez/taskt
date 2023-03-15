using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.CommandSettings("Random Number")]
    [Attributes.ClassAttributes.Description("This command allows you to get Random Number.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Random Number.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RandomNumberCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_ComboBox))]
        [PropertyDescription("Random Type")]
        [PropertyIsOptional(true, "Integer Number")]
        [PropertyUISelectionOption("Integer Number")]
        [PropertyUISelectionOption("Real Number")]
        [PropertyDisplayText(true, "Random Type")]
        public string v_RandomType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public RandomNumberCommand()
        {
            //this.CommandName = "RandomNumberCommand";
            //this.SelectionName = "Random Number";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            //var t = v_RandomType.GetUISelectionValue("v_RandomType", this, engine);
            //var t = this.GetUISelectionValue(nameof(v_RandomType), "Random Type", engine);
            var t = this.GetUISelectionValue(nameof(v_RandomType), engine);

            Random rand = new Random();

            decimal res = 0;
            switch (t)
            {
                case "integer number":
                    res = rand.Next();
                    break;
                case "real number":
                    res = (decimal)rand.NextDouble();
                    break;
            }

            res.ToString().StoreInUserVariable(engine, v_Result);
        }
    }
}