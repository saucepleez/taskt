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
    [Attributes.ClassAttributes.Description("This command allows you to Increase Value in Numerical Variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Increase Value in Numerical Variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class IncreaseNumericalVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please specify Numerical Variable")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vNum** or **{{{vNum}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        public string v_VariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify value to increase")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**100** or **{{{vValue}}}**")]
        [Remarks("")]
        [PropertyTextBoxSetting(1, false)]
        [PropertyIsOptional(true, "1")]
        public string v_Value { get; set; }

        public IncreaseNumericalVariableCommand()
        {
            this.CommandName = "IncreaseNumericalVariableCommand";
            this.SelectionName = "Increase Numerical Variable";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            string variableName;
            if (engine.engineSettings.isWrappedVariableMarker(v_VariableName))
            {
                variableName = v_VariableName;
            }
            else
            {
                variableName = engine.engineSettings.wrapVariableMarker(v_VariableName);
            }

            var variableValue = variableName.ConvertToUserVariableAsDecimal("Variable Name", engine);

            if (String.IsNullOrEmpty(v_Value))
            {
                v_Value = "1";
            }
            var add = v_Value.ConvertToUserVariableAsDecimal("Value", engine);

            (variableValue + add).ToString().StoreInUserVariable(engine, variableName);
        }
        public override List<Control> Render(frmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Variable: '" + v_VariableName + "', Add: '" + v_Value + "']";
        }
    }
}