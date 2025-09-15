using System;

namespace DotnetHospital.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 문자열이 null이거나 공백인지 확인하는 확장 메서드
        /// </summary>
        /// <param name="value">확인할 문자열</param>
        /// <returns>null이거나 공백이면 true, 그렇지 않으면 false</returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }
        
        /// <summary>
        /// 문자열을 지정된 길이로 자르고 필요시 "..."을 추가하는 확장 메서드
        /// </summary>
        /// <param name="value">자를 문자열</param>
        /// <param name="maxLength">최대 길이</param>
        /// <returns>자른 문자열</returns>
        public static string Truncate(this string value, int maxLength)
        {
            if (value == null) return null;
            if (value.Length <= maxLength) return value;
            return value.Substring(0, maxLength - 3) + "...";
        }
        
        /// <summary>
        /// 문자열의 첫 글자를 대문자로 만드는 확장 메서드
        /// </summary>
        /// <param name="value">변환할 문자열</param>
        /// <returns>첫 글자가 대문자인 문자열</returns>
        public static string Capitalize(this string value)
        {
            if (value.IsNullOrWhiteSpace()) return value;
            if (value.Length == 1) return value.ToUpper();
            return char.ToUpper(value[0]) + value.Substring(1).ToLower();
        }
    }
}
