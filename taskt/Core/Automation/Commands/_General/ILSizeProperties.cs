namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// general size properties
    /// </summary>
    public interface ILSizeProperties : ILExpandableProperties
    {
        /// <summary>
        /// width
        /// </summary>
        string v_Width { get; set; }

        /// <summary>
        /// height
        /// </summary>
        string v_Height { get; set;}
    }
}
