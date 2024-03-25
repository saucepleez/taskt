﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Numerical Commands")]
    [Attributes.ClassAttributes.CommandSettings("Create Numerical Variable")]
    [Attributes.ClassAttributes.Description("This command allows you to create Number Variable.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to create Number Variable.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class CreateNumericalVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        public string v_VariableName { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_NumberValue { get; set; }

        public CreateNumericalVariableCommand()
        {
            //this.CommandName = "CreateNumberVariableCommand";
            //this.SelectionName = "Create Number Variable";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            string numString = v_NumberValue.ExpandValueOrUserVariable(engine);
            if (decimal.TryParse(numString, out _))
            {
                numString.StoreInUserVariable(engine, v_VariableName);
            }
            else
            {
                throw new Exception("Number Value '" + v_NumberValue + "' is not number");
            }
        }
    }
}