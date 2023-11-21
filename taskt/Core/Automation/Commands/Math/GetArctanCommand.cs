using System;
using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("Math Commands")]
    [Attributes.ClassAttributes.CommandSettings("Get Arctan")]
    [Attributes.ClassAttributes.Description("This command allows you to get arctan.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to get arctan.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    public class GetArctanCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_Value))]
        public string v_Value { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(NumberControls), nameof(NumberControls.v_OutputNumericalVariableName))]
        public string v_Result { get; set; }

        [XmlAttribute]
        [PropertyVirtualProperty(nameof(MathControls), nameof(MathControls.v_AngleType))]
        public string v_AngleType { get; set; }

        public GetArctanCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            var v = (double)this.ExpandValueOrUserVariableAsDecimal(nameof(v_Value), engine);

            var r = Math.Atan(v);
            switch(this.ExpandValueOrUserVariableAsSelectionItem(nameof(v_AngleType), engine))
            {
                case "radian":
                    r.StoreInUserVariable(engine, v_Result);
                    break;
                case "degree":
                    (r * 180.0 / Math.PI).StoreInUserVariable(engine, v_Result);
                    break;
            }
        }
    }
}