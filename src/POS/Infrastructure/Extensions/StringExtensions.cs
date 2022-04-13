namespace POS.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string MakeFirstLetterUppercase(this string text)
        {
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}