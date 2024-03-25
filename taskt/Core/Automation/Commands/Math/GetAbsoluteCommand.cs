﻿using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Absolute")]
    [Attributes.ClassAttributes.Description("This command allows you to get abs.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get abs.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetAbsoluteCommand : AMathValueResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        //public string v_Value { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        //public string v_Result { get; set; }

        public GetAbsoluteCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var v = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Value), engine);

            Math.Abs(v).StoreInUserVariable(engine, v_Result);
        }
    }
}