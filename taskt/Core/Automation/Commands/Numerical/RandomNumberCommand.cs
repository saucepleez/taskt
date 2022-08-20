using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.Forms;
using taskt.UI.CustomControls;
using System.Data;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.Description("This command allows you to get Random Number.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Random Number.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class RandomNumberCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select Random Type")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyIsOptional(true, "Integer Number")]
        [PropertyShowSampleUsageInDescription(false)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Integer Number")]
        [PropertyUISelectionOption("Real Number")]
        [PropertyDisplayText(true, "Random Type")]
        public string v_RandomType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vValue** or **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Store")]
        public string v_Result { get; set; }

        public RandomNumberCommand()
        {
            this.CommandName = "RandomNumberCommand";
            this.SelectionName = "Random Number";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            var t = v_RandomType.GetUISelectionValue("v_RandomType", this, engine);

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
        //public override List<Control> Render(frmCommandEditor editor)
        //{
        //    base.Render(editor);

        //    RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

        //    return RenderedControls;
        //}

        //public override string GetDisplayValue()
        //{
        //    return base.GetDisplayValue() + " [Type: '" + v_RandomType + "', Result: '" + v_Result + "']";
        //}
    }
}