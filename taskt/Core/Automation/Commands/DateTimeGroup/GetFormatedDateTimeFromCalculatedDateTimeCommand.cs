using System;

namespace taskt.Core.Automation.Commands
{
    [Serializable]
    [Attributes.ClassAttributes.Group("DateTime")]
    [Attributes.ClassAttributes.SubGruop("Calculate")]
    [Attributes.ClassAttributes.CommandSettings("Get Formatted DateTime From Calculated DateTime")]
    [Attributes.ClassAttributes.Description("This command allows you to Get Formatted DateTime Text From Calculated DateTime Value.")]
    [Attributes.ClassAttributes.UsesDescription("Use this command when you want to Get Formatted DateTime Text From Calculated DateTime Value.")]
    [Attributes.ClassAttributes.ImplementationDescription("")]
    [Attributes.ClassAttributes.CommandIcon(nameof(Properties.Resources.command_function))]
    [Attributes.ClassAttributes.EnableAutomateRender(true)]
    [Attributes.ClassAttributes.EnableAutomateDisplayText(true)]
    public sealed class GetFormattedDateTimeFromCalculatedDateTimeCommand : AGetFormattedDateTimeFromCalculatedDateTimeCommands
    {
        public GetFormattedDateTimeFromCalculatedDateTimeCommand()
        {
        }

        public override void RunCommand(Engine.AutomationEngineInstance engine)
        {
            //using (var dt = new InnerScriptVariable(engine))
            //{
            //    var calc = new CalculateDateTimeCommand()
            //    {
            //        v_DateTime = this.v_DateTime,
            //        v_CalculationMethod = this.v_CalculationMethod,
            //        v_Value = this.v_Value,
            //        v_Result = dt.VariableName,
            //    };
            //    calc.RunCommand(engine);

            //    var fmt = new FormatDateTimeCommand()
            //    {
            //        v_DateTime = dt.VariableName,
            //        v_Format = this.v_Format,
            //        v_Result = this.v_Result,
            //    };
            //    fmt.RunCommand(engine);
            //}
            this.CommandProcess(
                new CalculateDateTimeCommand(),
                engine
            );
        }
    }
}
