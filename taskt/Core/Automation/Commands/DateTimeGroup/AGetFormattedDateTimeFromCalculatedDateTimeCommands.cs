using System.Xml.Serialization;
using taskt.Core.Automation.Attributes.PropertyAttributes;
using taskt.Core.Script;

namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Get Formatted DateTime From Calculated Something DateTime commands
    /// </summary>
    public abstract class AGetFormattedDateTimeFromCalculatedDateTimeCommands : ACalculateDateTimeCommands
    {
        /// <summary>
        /// datetime format
        /// </summary>
        [XmlAttribute]
        [PropertyVirtualProperty(nameof(DateTimeControls), nameof(DateTimeControls.v_Format))]
        [PropertyParameterOrder(6500)]
        public virtual string v_Format { get; set; }

        [XmlAttribute]
        [PropertyInstanceType(PropertyInstanceType.InstanceType.none)]
        public override string v_Result { get; set; }

        protected void CommandProcess(ACalculateDateTimeCommands calc, Engine.AutomationEngineInstance engine)
        {
            using (var dt = new InnerScriptVariable(engine))
            {
                calc.v_DateTime = this.v_DateTime;
                calc.v_CalculationMethod = this.v_CalculationMethod;
                calc.v_Value = this.v_Value;
                calc.v_Result = dt.VariableName;
                calc.RunCommand(engine);

                var format = new FormatDateTimeCommand()
                {
                    v_DateTime = dt.VariableName,
                    v_Format = this.v_Format,
                    v_Result = this.v_Result,
                };
                format.RunCommand(engine);
            }
        }
    }
}
