using System;

namespace ConsoleApp2.Assignments
{
    record A05_LargestValue() : Assignment(5, "Largest Value")
    {
        protected override void Implementation()
        {
            // Interestingly the assignment does not specify that the values are numbers...
            Console.WriteLine("Please provide two values:");
            string val1 = ConsoleX.PromptRead("Value1 > ") ?? string.Empty;
            string val2 = ConsoleX.PromptRead("Value2 > ") ?? string.Empty;

            // still seems sane to compare as numbers if it's possible
            if (double.TryParse(val1, out var d1) && double.TryParse(val2, out var d2))
            {
                Console.WriteLine("It seems that you have entered two numbers...");
                Console.WriteLine(
                    d1 == d2 ? $" and they are numerically equal ({d1})."
                    : d1 > d2 ? $" and the first value is larger ({d1})."
                    /*else*/ : $" and the second value is larger ({d2})."
                    );
            }
            else
            {
                Console.WriteLine("Treating both values as text...");
                if (val1.Length == val2.Length) // compare as text, based on lexical sort
                {
                    Console.WriteLine(" and since both entries are of equal length, lexical sort is looked at,");
                    Console.WriteLine(string.Compare(val1, val2) switch
                    {
                        0 => " however the values are in fact lexically equivalent.",
                        <= 0 => $" and thus the first value ({val1.Truncate(11)}) preceedes the second ({val2.Truncate(11)}).",
                        >= 0 => $" and thus the second value ({val2.Truncate(11)}) preceeded the first ({val1.Truncate(11)}).",
                    });
                }
                else // compare as text, purely on length
                {
                    Console.WriteLine(val1.Length > val2.Length
                        ? $" and the first value is longer ({val1.Length}) than the second value ({val2.Length})"
                        : $" and the second value is longer ({val2.Length}) than the first value ({val1.Length})"
                        );
                }
            }
        }
    }
}
