using System;

namespace DotnetHospital.Extensions
{
    /// <summary>
    /// Extension methods for string operations
    /// Provides utility methods for common string manipulations
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Extension method to check if string is null or whitespace
        /// </summary>
        /// <param name="value">String to check</param>
        /// <returns>True if null or whitespace, false otherwise</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        
        /// <summary>
        /// Extension method to truncate string to specified length with ellipsis
        /// </summary>
        /// <param name="value">String to truncate</param>
        /// <param name="maxLength">Maximum length</param>
        /// <returns>Truncated string with ellipsis if needed</returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (value == null) return null;
            if (value.Length <= maxLength) return value;
            return value.Substring(0, maxLength - 3) + "...";
        }
        
        /// <summary>
        /// Extension method to capitalize the first letter of a string
        /// </summary>
        /// <param name="value">String to capitalize</param>
        /// <returns>String with first letter capitalized</returns>
        public static string Capitalize(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return value;
            if (value.Length == 1) return value.ToUpper();
            return char.ToUpper(value[0]) + value.Substring(1).ToLower();
        }
    }
}
