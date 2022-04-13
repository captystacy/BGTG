namespace POS.Infrastructure.Services.DocumentServices.WordService.Format
{
    public enum MyTextDirection
    {
        None = 0,
        /// <summary>
        /// Specifies that text in the parent object shall flow from left to right horizontally,
        /// then top to bottom vertically on the page.
        /// This means that horizontal lines are filled before the text expands vertically.
        /// </summary>
        TopToBottom = 1,
        /// <summary>
        /// Specifies that text in the parent object shall flow from top to bottom vertically,
        /// then right to left horizontally on the page.
        /// This means that horizontal lines are filled before the text expands vertically.
        /// This flow is also rotated such that all text is rotated 90 degrees when displayed on a page.
        /// </summary>
        RightToLeftRotated = 2,
        /// <summary>
        /// Specifies that text in the parent object shall flow from bottom to top vertically,
        /// then from left to right horizontally on the page.
        /// </summary>
        LeftToRight = 3,
        /// <summary>
        /// Specifies that text in the parent object shall flow from right to left horizontally,
        /// then top to bottom vertically on the page.
        /// This means that horizontal lines are filled before the text expands vertically.
        /// </summary>
        RightToLeft = 4,
        /// <summary>
        /// Specifies that text in the parent object shall flow from left to right horizontally,
        /// then top to bottom vertically on the page.
        /// This means that horizontal lines are filled before the text expands vertically.
        /// This flow is also rotated such that any East Asian text shall be rotated 270 degrees when displayed on a page.
        /// </summary>
        TopToBottomRotated = 5,
        /// <summary>
        /// Specifies that text in the parent object shall flow from top to bottom vertically,
        /// then left to right horizontally on the page.
        /// This means that horizontal lines are filled before the text expands vertically.
        /// This flow is also rotated such that all text is rotated 90 degrees when displayed on a page.
        /// </summary>
        LeftToRightRotated = 6,
    }
}