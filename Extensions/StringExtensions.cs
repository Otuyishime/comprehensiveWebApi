using System;
namespace testWebAPI.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var first = input.Substring(0, 1).ToLower();
            return input.Length == 1 ? first : first + input.Substring(1);
        }
    }
}
