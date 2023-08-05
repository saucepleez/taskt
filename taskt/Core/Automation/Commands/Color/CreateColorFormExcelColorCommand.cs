using System;
using System.Xml.Serialization;
using System.Drawing;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Create Color From Excel Color")]
    [Attributes.ClassAttributes.Description("This command allows you to create Color from Excel Color.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Color from Excel Color.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class CreateColorFromExcelColorCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ColorControls), nameof(ColorControls.v_InputColorVariableName))]
        public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyDescription("Excel Color Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**255**", PropertyDetailSampleUsage.ValueType.Value, "Excel Color")]
        [PropertyDetailSampleUsage("**{{{vExcelColor}}}**", PropertyDetailSampleUsage.ValueType.VariableValue, "Excel Color")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyTextBoxSetting(1, false)]
        [PropertyValidationRule("Excel Color", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Color Value")]
        public string v_ExcelColor { get; set; }

        public CreateColorFromExcelColorCommand()
        {
            //this.CommandName = "CreateColorFromExcelColorCommand";
            //this.SelectionName = "Create Color From Excel Color";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            int color = this.ConvertToUserVariableAsInteger(nameof(v_ExcelColor), engine);

            color &= 0xFFFFFF;
            int r = color & 0xFF;
            color >>= 8;
            int g = color & 0xFF;
            color >>= 8;
            int b = color;

            Color co = Color.FromArgb(255, r, g, b);
            co.StoreInUserVariable(engine, v_Color);
        }
    }
}