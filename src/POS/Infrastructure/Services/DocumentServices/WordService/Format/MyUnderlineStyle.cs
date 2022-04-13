namespace POS.Infrastructure.Services.DocumentServices.WordService.Format
{
    public enum MyUnderlineStyle
    {
        None = 0,
        /// <summary>No underlining.</summary>
        Empty = 1,
        /// <summary>Normal single underline.</summary>
        Single = 2,
        /// <summary>Underline words only.</summary>
        Words = 3,
        /// <summary>Double underline.</summary>
        Double = 4,
        DotDot = 5,
        /// <summary>Dotted underline.</summary>
        Dotted = 6,
        /// <summary>Heavy underline.</summary>
        Thick = 7,
        /// <summary>Dashed underline.</summary>
        Dash = 8,
    }
}