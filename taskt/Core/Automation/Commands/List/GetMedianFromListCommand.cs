﻿using System;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Math")]
    [Attributes.ClassAttributes.CommandSettings("Get Median From List")]
    [Attributes.ClassAttributes.Description("This command allows you to get median value from a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get median value from a list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetMedianFromListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_InputListName))]
        public string v_InputList { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(GeneralPropertyControls), nameof(GeneralPropertyControls.v_Result))]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(ListControls), nameof(ListControls.v_WhenValueIsNotNumeric))]
        public string v_IfValueIsNotNumeric { get; set; }

        public GetMedianFromListCommand()
        {
            //this.CommandName = "GetMedianFromListCommand";
            //this.SelectionName = "Get Median From List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            ListControls.MathProcess(this, nameof(v_IfValueIsNotNumeric), v_InputList, engine,
                new Func<System.Collections.Generic.List<decimal>, decimal>((lst) =>
                {
                    decimal med;
                    if (lst.Count() % 2 == 0)
                    {
                        int center = lst.Count() / 2;
                        med = (lst[center - 1] + lst[center]) * (decimal)0.5;
                    }
                    else
                    {
                        med = lst[lst.Count() / 2];
                    }
                    return med;
                })
            ).StoreInUserVariable(engine, v_Result);
        }
    }
}