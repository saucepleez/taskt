﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Logarithm")]
    [Attributes.ClassAttributes.Description("This command allows you to get log.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get log.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetLogarithmCommand : ScriptCommand
    {

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]

        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        [PropertyDescription("Base")]
        [InputSpecification("Base", true)]
        [PropertyDisplayText(true, "Base")]
        [PropertyValidationRule("Base", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyIsOptional(true, "10")]
        [PropertyFirstValue("10")]
        public string v_Base { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public GetLogarithmCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var b = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Base), engine);
            var v = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Value), engine);

            Math.Log(v, b).StoreInUserVariable(engine, v_Result);
        }
    }
}