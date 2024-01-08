namespace taskt.Core.Automation.Commands
{
    public interface ITrignometricProperties
    {
        /// <summary>
        /// target value to convert
        /// </summary>
        string v_Value {  get; set; }

        /// <summary>
        /// agnle type Radian or Degree
        /// </summary>
        string v_AngleType { get; set; }

        /// <summary>
        /// result variable name
        /// </summary>
        string v_Result { get; set; }
    }
}
