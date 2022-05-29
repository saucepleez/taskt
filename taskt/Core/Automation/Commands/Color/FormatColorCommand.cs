﻿using System;
using System.Linq;
using System.Xml.Serialization;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.UI.CustomControls;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.Description("This command allows you to get Format Color Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Format Color Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class FormatColorCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a Color Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**{{{vColor}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Color, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please Select Color Format")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyUISelectionOption("Hex")]
        [PropertyUISelectionOption("CSS RGB")]
        [PropertyUISelectionOption("CSS RGBA")]
        [PropertyUISelectionOption("Excel Color")]
        [PropertyUISelectionOption("Red")]
        [PropertyUISelectionOption("Green")]
        [PropertyUISelectionOption("Blue")]
        [PropertyUISelectionOption("Alpha")]
        [PropertyUISelectionOption("HSL")]
        [PropertyUISelectionOption("CMYK")]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Format { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Variable Name to Store Result")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**vResult** or **{{{vResult}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyIsVariablesList(true)]
        [PropertyValidationRule("Result", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Result { get; set; }

        public FormatColorCommand()
        {
            this.CommandName = "FormatColorCommand";
            this.SelectionName = "Format Color";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            Color co = v_Color.GetColorVariable(engine);
            string format = v_Format.GetUISelectionValue("v_Format", this, engine);

            string res = "";
            switch (format)
            {
                case "hex":
                    res = string.Format("{0:X2}{1:X2}{2:X2}", co.R, co.G, co.B);
                    break;
                case "css rgb":
                    res = "rgb(" + co.R + "," + co.G + "," + co.B + ")";
                    break;
                case "css rgba":
                    res = "rgba(" + co.R + "," + co.G + "," + co.B + ", " + (co.A / 255.0) + ")";
                    break;
                case "excel color":
                    res = (co.R + (co.G * 256) + (co.B * 65536)).ToString();
                    break;
                case "red":
                    res = co.R.ToString();
                    break;
                case "green":
                    res = co.G.ToString();
                    break;
                case "blue":
                    res = co.B.ToString();
                    break;
                case "alpha":
                    res = co.A.ToString();
                    break;

                case "hsl":
                    var hsl = new Dictionary<string, string>()
                    {
                        { "Hue", co.GetHue().ToString() },
                        { "Saturation", co.GetSaturation().ToString() },
                        { "Lightness", co.GetBrightness().ToString() }
                    };
                    hsl.StoreInUserVariable(engine, v_Result);
                    return;

                case "cmyk":
                    double r = co.R / 255.0;
                    double g = co.G / 255.0;
                    double b = co.B / 255.0;

                    double max = r;
                    if (max < g)
                    {
                        max = g;
                    }
                    if (max < b)
                    {
                        max = b;
                    }

                    double k = 1 - max;
                    double ki = (1 - k);
                    double c = (1 - r - k) / ki;
                    double m = (1 - g - k) / ki;
                    double y = (1 - b - k) / ki;

                    var cmyk = new Dictionary<string, string>()
                    {
                        { "Cyan", c.ToString() },
                        { "Magenta", m.ToString()  },
                        { "Yellow", y.ToString()  },
                        { "Key", k.ToString() }
                    };
                    cmyk.StoreInUserVariable(engine, v_Result);
                    return;
            }
            res.StoreInUserVariable(engine, v_Result);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Name: '" + v_Color + "', Format: '" + v_Format + "', Store: '" + v_Result + "']";
        }

        public override List<Control> Render(UI.Forms.frmCommandEditor editor)
        {
            //custom rendering
            base.Render(editor);

            var ctrls = CommandControls.MultiCreateInferenceDefaultControlGroupFor(this, editor);
            RenderedControls.AddRange(ctrls);

            return RenderedControls;
        }
    }
}