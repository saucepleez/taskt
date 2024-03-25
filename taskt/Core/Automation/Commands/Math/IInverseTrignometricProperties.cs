namespace taskt.Core.Automation.Commands
{
    public interface IInverseTrignometricProperties : ITrignometricProperties
    {
        /// <summary>
        /// behavior when value is out of range
        /// </summary>
        string v_WhenValueIsOutOfRange { get; set; }
    }
}
