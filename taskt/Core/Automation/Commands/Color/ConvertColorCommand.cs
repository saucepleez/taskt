using System;
using System.Xml.Serialization;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using System.Windows.Forms;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Color Commands")]
    [Attributes.ClassAttributes.SubGruop("")]
    [Attributes.ClassAttributes.CommandSettings("Convert Color")]
    [Attributes.ClassAttributes.Description("This command allows you to get convert Color Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get convert Color Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ConvertColorCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Color Variable Name")]
        [InputSpecification("")]
        [PropertyDetailSampleUsage("**{{{vColor}}}**", PropertyDetailSampleUsage.ValueType.VariableValue)]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Color, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Input)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Variable")]
        public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyDescription("Color Format")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertySecondaryLabel(true)]
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
        [PropertyUISelectionOption("RGBA Dictionary")]
        [PropertyUISelectionOption("RGBA DataTable")]
        [PropertyAddtionalParameterInfo("Hex", "Convert to Hex value, like **11FFAA**")]
        [PropertyAddtionalParameterInfo("CSS RGB", "Convert to CSS RGB value, like **rgb(255, 64, 0)**")]
        [PropertyAddtionalParameterInfo("CSS RGBA", "Convert to CSS RGB value, like **rgba(255, 64, 0, 0.6)**")]
        [PropertyAddtionalParameterInfo("Excel Color", "Convert to Excel Color Value like **25312**")]
        [PropertyAddtionalParameterInfo("RGBA Dictionary", "Convert to Dictionary. Key names are R, G, B, A.")]
        [PropertyAddtionalParameterInfo("RGBA DataTable", "Convert to DataTable. Column names are R, G, B, A.")]
        [PropertySelectionChangeEvent(nameof(cmbFormatSelectionChange))]
        [PropertyValidationRule("Format", PropertyValidationRule.ValidationRuleFlags.Empty)]
        [PropertyDisplayText(true, "Format")]
        public string v_Format { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public ConvertColorCommand()
        {
            //this.CommandName = "FormatColorCommand";
            //this.SelectionName = "Format Color";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            Color co = v_Color.GetColorVariable(engine);
            string format = this.GetUISelectionValue(nameof(v_Format), engine);

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
                        { "H", co.GetHue().ToString() },
                        { "S", co.GetSaturation().ToString() },
                        { "L", co.GetBrightness().ToString() }
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
                        { "C", c.ToString() },
                        { "M", m.ToString()  },
                        { "Y", y.ToString()  },
                        { "K", k.ToString() }
                    };
                    cmyk.StoreInUserVariable(engine, v_Result);
                    return;

                case "rgba dictionary":
                    var rgbaDic = new Dictionary<string, string>()
                    {
                        { "R", co.R.ToString() },
                        { "G", co.G.ToString() },
                        { "B", co.B.ToString() },
                        { "A", co.A.ToString() }
                    };
                    rgbaDic.StoreInUserVariable(engine, v_Result);
                    return;

                case "rgba datatable":
                    var rgbaDT = new DataTable();
                    rgbaDT.Columns.Add("R");
                    rgbaDT.Columns.Add("G");
                    rgbaDT.Columns.Add("B");
                    rgbaDT.Columns.Add("A");
                    rgbaDT.Rows.Add();
                    rgbaDT.Rows[0][0] = co.R;
                    rgbaDT.Rows[0][1] = co.G;
                    rgbaDT.Rows[0][2] = co.B;
                    rgbaDT.Rows[0][3] = co.A;
                    rgbaDT.StoreInUserVariable(engine, v_Result);
                    return;
            }
            res.StoreInUserVariable(engine, v_Result);
        }

        private void cmbFormatSelectionChange(object sender, EventArgs e)
        {
            ComboBox cmb = (ComboBox)sender;

            //var labelDic = (Dictionary<string, string>)(ControlsList["lbl_" + nameof(v_Format)].Tag);
            //Label lbl = (Label)ControlsList["lbl2_" + nameof(v_Format)];

            //var labelDic = ControlsList.Get2ndLabelText(nameof(v_Format));
            //var lbl = ControlsList.GetPropertyControl2ndLabel(nameof(v_Format));

            //var searchKey = cmb.SelectedItem.ToString();
            //if (labelDic.ContainsKey(searchKey))
            //{
            //    lbl.Text = labelDic[searchKey];
            //}
            //else
            //{
            //    lbl.Text = "";
            //}

            ControlsList.SecondLabelProcess(nameof(v_Format), nameof(v_Format), cmb.SelectedItem.ToString());
        }

        public override void AddInstance(InstanceCounter counter)
        {
            var co = (string.IsNullOrEmpty(v_Color)) ? "" : v_Color;
            counter.addInstance(co, new PropertyInstanceType(PropertyInstanceType.InstanceType.Color, true), true);

            var format = (string.IsNullOrEmpty(v_Format) ? "" : v_Format.ToLower());
            var ins = (string.IsNullOrEmpty(v_Result) ? "" : v_Result);
            switch (format)
            {
                case "hsl":
                case "cmyk":
                case "rgba dictioanry":
                    var dicProp = new PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary, true);
                    counter.addInstance(ins, dicProp, false);
                    counter.addInstance(ins, dicProp, true);
                    break;
                case "rgba datatable":
                    var dtProp = new PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable, true);
                    counter.addInstance(ins, dtProp, false);
                    counter.addInstance(ins, dtProp, true);
                    break;
            }
        }

        public override void RemoveInstance(InstanceCounter counter)
        {
            var co = (string.IsNullOrEmpty(v_Color)) ? "" : v_Color;
            counter.removeInstance(co, new PropertyInstanceType(PropertyInstanceType.InstanceType.Color, true), true);

            var format = (string.IsNullOrEmpty(v_Format) ? "" : v_Format.ToLower());
            var ins = (string.IsNullOrEmpty(v_Result) ? "" : v_Result);
            switch (format)
            {
                case "hsl":
                case "cmyk":
                case "rgba dictioanry":
                    var dicProp = new PropertyInstanceType(PropertyInstanceType.InstanceType.Dictionary, true);
                    counter.removeInstance(ins, dicProp, false);
                    counter.removeInstance(ins, dicProp, true);
                    break;
                case "rgba datatable":
                    var dtProp = new PropertyInstanceType(PropertyInstanceType.InstanceType.DataTable, true);
                    counter.removeInstance(ins, dtProp, false);
                    counter.removeInstance(ins, dtProp, true);
                    break;
            }
        }
    }
}