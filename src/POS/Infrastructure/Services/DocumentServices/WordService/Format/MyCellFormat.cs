namespace POS.Infrastructure.Services.DocumentServices.WordService.Format
{
    public class MyCellFormat
    {
        public MyVerticalAlignment? VerticalAlignment { get; set; }

        public MyTextDirection? TextDirection { get; set; }

        public MyBorder? Border { get; set; } = null!;
    }
}