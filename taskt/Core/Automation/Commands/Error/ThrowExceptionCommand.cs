﻿using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.CommandSettings("Throw Exception")]
    [Attributes.ClassAttributes.Description("This command allows you to throw an exception error.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to throw an exception error")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_try))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class ThrowExceptionCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_MultiLinesTextBox))]
        [PropertyDescription("Exception Message")]
        [InputSpecification("Message", true)]
        [PropertyIsOptional(true)]
        [PropertyValidationRule("Message", PropertyValidationRule.ValidationRuleFlags.None)]
        [PropertyDisplayText(true, "Message")]
        public string v_Message { get; set; }

        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

        public ThrowExceptionCommand()
        {
            //this.CommandName = "ThrowExceptionCommand";
            //this.SelectionName = "Throw Exception";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            string message;
            if (String.IsNullOrEmpty(v_Message))
            {
                message = "A Error has Occured.";
            }
            else
            {
                message = v_Message.ExpandValueOrUserVariable(engine);
            }

            throw new Exception(message);
        }
    }
}