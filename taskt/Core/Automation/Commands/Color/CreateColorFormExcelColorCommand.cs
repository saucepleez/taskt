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
    [Attributes.ClassAttributes.Description("This command allows you to create Color from Excel Color.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Color from Excel Color.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    public class CreateColorFromExcelColorCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Please select a Color Variable Name")]
        [InputSpecification("")]
        [SampleUsage("**vColor** or **{{{vColor}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyRecommendedUIControl(PropertyRecommendedUIControl.RecommendeUIControlType.ComboBox)]
        [PropertyIsVariablesList(true)]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.Color, true)]
        [PropertyParameterDirection(PropertyParameterDirection.ParameterDirection.Output)]
        [PropertyValidationRule("Variable", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_Color { get; set; }

        [XmlAttribute]
        [PropertyDescription("Please specify Excel Color Value")]
        [PropertyUIHelper(PropertyUIHelper.UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("")]
        [SampleUsage("**255** or **{{{vColor}}}**")]
        [Remarks("")]
        [PropertyShowSampleUsageInDescription(true)]
        [PropertyValidationRule("Excel Color", PropertyValidationRule.ValidationRuleFlags.Empty)]
        public string v_ExcelColor { get; set; }

        public CreateColorFromExcelColorCommand()
        {
            this.CommandName = "CreateColorFromExcelColorCommand";
            this.SelectionName = "Create Color From Excel Color";
            this.CommandEnabled = true;
            this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (Engine.AutomationEngineInstance)sender;

            int color = v_ExcelColor.ConvertToUserVariableAsInteger("Excel Color", engine);
            color &= 0xFFFFFF;
            int r = color & 0xFF;
            color >>= 8;
            int g = color & 0xFF;
            color >>= 8;
            int b = color;

            Color co = Color.FromArgb(255, r, g, b);
            co.StoreInUserVariable(engine, v_Color);
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Name: '" + v_Color + "', From: '" + v_ExcelColor + "']";
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