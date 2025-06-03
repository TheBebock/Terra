using System.Text.RegularExpressions;

namespace Terra.Extensions
{
    public static class StringExtensions 
    {
        public static string RemoveWhiteSpace(this string input)
        {
            return string.IsNullOrEmpty(input) ? input : Regex.Replace(input, "\\s+", string.Empty);
        }
        public static bool IsNullOrWhitespace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
