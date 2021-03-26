using System;
using System.Linq;

namespace ConsoleAssignments.Assignments
{
    using static Math;

    record A13_Range() : Assignment(13, "Range")
    {
        protected override void Implementation()
        {
            Console.WriteLine("Please enter two numbers:");
            if (!ConsoleX.TryReadManyAttempts(ConsoleX.TryReadNumber, out int val1, " Value1 > ") ||
                !ConsoleX.TryReadManyAttempts(ConsoleX.TryReadNumber, out int val2, " Value2 > "))
                return;

            Console.WriteLine();
            Console.WriteLine("Range:");
            int start = Min(val1, val2);
            int final = Max(val1, val2);
            int count = final - start + 1;
            Console.WriteLine(Enumerable.Range(start, count).Select(n => n.ToString()).JoinText(", "));
        }
    }
}
