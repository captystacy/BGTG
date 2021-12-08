namespace POSWeb.Helpers
{
    public static class StringHelper
    {
        private const string _backSlash = "\\";

        public static string RemoveBackslashes(this string str)
        {
            return str.Replace(_backSlash, string.Empty);
        }
    }
}
