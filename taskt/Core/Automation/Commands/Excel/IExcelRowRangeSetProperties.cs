namespace taskt.Core.Automation.Commands
{
    /// <summary>
    /// for Excel Row Range Set properties
    /// </summary>
    public interface IExcelRowRangeSetProperties : IExcelRowRangeProperties
    {
        /// <summary>
        /// behavior when specified Item(List/Dictionary/DataTable) items not enough
        /// </summary>
        string v_WhenItemNotEnough { get; set; }
    }
}
