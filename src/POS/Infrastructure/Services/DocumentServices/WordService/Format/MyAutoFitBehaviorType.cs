namespace POS.Infrastructure.Services.DocumentServices.WordService.Format
{
    public enum MyAutoFitBehaviorType
    {
        /// <summary>
        /// The table is set to a fixed size, regardless of the content, and is not automatically sized.
        /// </summary>
        FixedColumnWidths,
        /// <summary>
        /// The table is automatically sized to fit the content contained in the table.
        /// </summary>
        AutoFitToContents,
        /// <summary>
        /// The table is automatically sized to the width of the active window.
        /// </summary>
        AutoFitToWindow,
    }
}