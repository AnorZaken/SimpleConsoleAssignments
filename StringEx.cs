using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ConsoleAssignments
{
    static class StringEx
    {
        public static string? Truncate(this string? value, int maxLength, string ellipsis = "...")
        {
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength), $"{nameof(maxLength)} cannot be negative.");
            int length = maxLength - ellipsis.Length;
            if (length <= 0)
                throw new ArgumentException($"{nameof(maxLength)} is shorter than or equal to the length of the {nameof(ellipsis)} string.");
            return !(value?.Length > maxLength) ? value : value[..length] + ellipsis;
        }

        /// <summary>
        /// Aggregates a collection of strings into one big string.
        /// </summary>
        public static string JoinText(this IEnumerable<string> strings, string separator = "")
            => string.Join(separator, strings);

        /// <summary>
        /// Enumerates the content of a string, such that it groups special combinations of characters (Grapheme Clusters), for example é.
        /// </summary>
        public static IEnumerable<string> GraphemeClusters(this string text)
        {
            var iter = StringInfo.GetTextElementEnumerator(text); // 
            while (iter.MoveNext())
                yield return (string)iter.Current;
        }

        /// <summary>
        /// Properly reverses a string using Grapheme Clusters to preserve special character combinations, such as é, in the reversed result.
        /// </summary>
        public static string Reverse(this string text)
        {
            return text.GraphemeClusters().Reverse().JoinText();
                
        }
    }
}
