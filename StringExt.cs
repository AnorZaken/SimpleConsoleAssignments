using System;

namespace ConsoleAssignments
{
    static class StringExt
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
    }
}
