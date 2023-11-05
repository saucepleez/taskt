﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Sin")]
    [Attributes.ClassAttributes.Description("This command allows you to get sin.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get sin.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetSinCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_Number { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_angleType))]
        public string v_AngleType { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        public GetSinCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var value = NumberControls.ConvertAngleToRadian(this, nameof(v_Number), nameof(v_AngleType), engine);

            Math.Sin(value).StoreInUserVariable(engine, v_Result);
        }
    }
}