using System;
using System.Linq;

namespace ConsoleAssignments.Assignments
{
    record A15_CsvSum() : Assignment(15, "CSV Sum")
    {
        protected override void Implementation()
        {
            if (!A14_CsvOddEven.AskForSomeCommaSeparatedIntegers(out var numbers, out _))
                return;

            Console.WriteLine();
            Console.WriteLine("These were the integers: " + numbers.Select(num => num.ToString()).JoinText(", "));
            Console.WriteLine($"The sum of those integers is: {numbers.Sum()}");
        }
    }
}
