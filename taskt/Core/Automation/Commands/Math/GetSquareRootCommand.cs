﻿using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Square Root")]
    [Attributes.ClassAttributes.Description("This command allows you to get Square Root.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get Square Root.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetSquareRootCommand : AMathValueResultCommands
    {
        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        //public string v_Value { get; set; }

        //[XmlAttribute]
        //[PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        //public string v_Result { get; set; }

        public GetSquareRootCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var v = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Value), engine);

            Math.Sqrt(v).StoreInUserVariable(engine, v_Result);
        }
    }
}