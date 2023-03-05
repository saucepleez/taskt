using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("List Commands")]
    [Attributes.ClassAttributes.SubGruop("Math")]
    [Attributes.ClassAttributes.CommandSettings("Get Min From List")]
    [Attributes.ClassAttributes.Description("This command allows you to get min value from a list.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get min value from a list.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public class GetMinFromListCommand : ScriptCommand
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

        public GetMinFromListCommand()
        {
            //this.CommandName = "GetMinFromListCommand";
            //this.SelectionName = "Get Min From List";
            //this.CommandEnabled = true;
            //this.CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            ListControls.MathProcess(this, nameof(v_IfValueIsNotNumeric), v_InputList, engine,
                new Func<List<decimal>, decimal>((lst) =>
                {
                    return lst.Min();
                })
            ).StoreInUserVariable(engine, v_Result);
        }
    }
}