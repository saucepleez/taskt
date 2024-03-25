﻿using System;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Error Handling Commands")]
    [Attributes.ClassAttributes.CommandSettings("Finally")]
    [Attributes.ClassAttributes.Description("This command specifies execution that should occur whether or not an error occured")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to always execute a specific command before leaving the try/catch block")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_try))]
    [Attributes.ClassAttributes.EnableAutomateRender(true, true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class FinallyCommand : ScriptCommand
    {
        public FinallyCommand()
        {
            //this.CommandName = "FinallyCommand";
            //this.SelectionName = "Finally";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine, Script.ScriptAction parentCommand)
        {
            //no execution required, used as a marker by the Automation Engine
        }
    }
}