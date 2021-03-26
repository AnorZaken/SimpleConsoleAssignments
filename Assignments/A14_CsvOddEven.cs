using System;
using System.Linq;

namespace ConsoleAssignments.Assignments
{
    record A14_CsvOddEven() : Assignment(14, "CSV Odd-Even")
    {
        protected override void Implementation()
        {
            Console.Write("Please enter a few integers, comma separated:");
            var input = ConsoleX.PromptLine();
            var values = input?.Split(',', StringSplitOptions.TrimEntries);
            var numbers =
                (
                    from val in values
                    let numNull = TryParse(val)
                    where numNull != null
                    let number = numNull.Value
                    orderby number
                    select number
                )
                .ToArray();

            if (numbers.Length == 0)
            {
                Console.WriteLine("You did not enter comma-separated numbers!");
                return;
            }
            if (numbers.Length < values?.Length)
            {
                Console.WriteLine("You have entered a mix of things... some of which are numbers.");
            }

            Console.WriteLine();
            Console.Write("The odd numbers are (in order): ");
            Console.WriteLine(numbers.Where(IsOdd).Select(num => num.ToString()).JoinText(", "));
            Console.Write("The even numbers are (in order): ");
            Console.WriteLine(numbers.Where(IsEven).Select(num => num.ToString()).JoinText(", "));

            int? TryParse(string val) => int.TryParse(val, out int num) ? num : (int?)null;
            bool IsOdd(int val) => (val & 1) == 1;
            bool IsEven(int val) => (val & 1) == 0;
        }
    }
}
