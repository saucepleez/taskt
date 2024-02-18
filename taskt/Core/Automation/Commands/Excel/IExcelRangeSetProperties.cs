namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// excel range Set properties
    /// </summary>
    public interface IExcelRangeSetProperties
    {
        /// <summary>
        /// behavior when specified Item(List/Dictionary/DataTable) items not enough
        /// </summary>
        string v_WhenItemNotEnough { get; set; }
    }
}
