namespace BGTG.Web.Infrastructure.Helpers
{
    public static class StringHelper
    {
        private const string BackSlash = "\\";

        public static string RemoveBackslashes(this string str)
        {
            return str.Replace(BackSlash, string.Empty);
        }
    }
}
