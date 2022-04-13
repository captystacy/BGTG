using POS.Infrastructure.Services.DocumentServices.WordService.Format;

namespace POS.Infrastructure.AppConstants
{
    public static partial class Constants
    {
        public static readonly MyParagraphFormat DefaultParagraph = new()
        {
            HorizontalAlignment = MyParagraphAlignment.Justify,
            FirstLineIndent = 43,
        };

        public static readonly MyCharacterFormat DefaultFontSize = new()
        {
            FontSize = 14
        };

        public static readonly MyParagraphFormat ParagraphHorizontalCentered = new()
        {
            HorizontalAlignment = MyParagraphAlignment.Center,
        };

        public static readonly MyTableFormat TableHorizontalCentered = new()
        {
            HorizontalAlignment = MyTableAlignment.Center,
        };

        public static readonly MyCellFormat VerticalCentered = new()
        {
            VerticalAlignment = MyVerticalAlignment.Middle,
        };

        public static readonly MyCellFormat BottomToTopCentered = new()
        {
            VerticalAlignment = MyVerticalAlignment.Middle,
            TextDirection = MyTextDirection.LeftToRight
        };

        public static readonly MyCellFormat BottomBorderCleared = new()
        {
            VerticalAlignment = MyVerticalAlignment.Middle,
            Border = new MyBorder
            {
                Bottom = MyBorderStyle.Cleared,
                Top = MyBorderStyle.Single,
                Left = MyBorderStyle.Single,
                Right = MyBorderStyle.Single,
            },
        };

        public static readonly MyCellFormat BottomBorderSingle = new()
        {
            VerticalAlignment = MyVerticalAlignment.Middle,
            Border = new MyBorder
            {
                Bottom = MyBorderStyle.Single,
                Top = MyBorderStyle.Cleared,
                Left = MyBorderStyle.Cleared,
                Right = MyBorderStyle.Cleared,
            },
        };

        public static readonly MyCellFormat TopBorderCleared = new()
        {
            VerticalAlignment = MyVerticalAlignment.Middle,
            Border = new MyBorder
            {
                Bottom = MyBorderStyle.Single,
                Top = MyBorderStyle.Cleared,
                Left = MyBorderStyle.Single,
                Right = MyBorderStyle.Single,
            },
        };

        public static readonly MyCharacterFormat Underlined = new()
        {
            UnderlineStyle = MyUnderlineStyle.Single,
        };
    }
}