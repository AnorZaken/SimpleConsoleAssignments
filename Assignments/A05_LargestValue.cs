using System;

namespace ConsoleAssignments.Assignments
{
    record A05_LargestValue() : Assignment(5, "Largest Value")
    {
        protected override void Implementation()
        {
            // Interestingly the assignment does not specify that the values are numbers...
            Console.WriteLine("Please provide two values:");
            string val1 = ConsoleX.PromptLine("Value1 > ") ?? string.Empty;
            string val2 = ConsoleX.PromptLine("Value2 > ") ?? string.Empty;
            Console.WriteLine();

            // still seems sane to compare as numbers if it's possible
            if (double.TryParse(val1, out var d1) && double.TryParse(val2, out var d2))
            {
                Console.Write("You have entered two numbers, and ");
                Console.WriteLine(
                    d1 == d2 ? $"they are numerically equal ({d1})."
                    : d1 > d2 ? $"the first value is larger ({d1})."
                    /*else*/ : $"the second value is larger ({d2})."
                    );
            }
            else
            {
                Console.WriteLine("Treating both inputs as text since at least one of them isn't a number...");
                Console.Write("Thus lexical sort is looked at, and ");
                Console.WriteLine(string.Compare(val1, val2) switch
                {
                    0 => "both values are lexically equivalent.",
                    <= 0 => $"the first value ({val1.Truncate(11)}) preceedes the second ({val2.Truncate(11)}).",
                    >= 0 => $"the second value ({val2.Truncate(11)}) preceeded the first ({val1.Truncate(11)}).",
                });
            }
        }
    }
}
