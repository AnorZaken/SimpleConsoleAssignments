using System;

namespace ConsoleAssignments.Assignments
{
    using static Math;

    record A09_MathPower() : Assignment(9, "Math Power")
    {
        protected override void Implementation()
        {
            Console.Write($"Please enter a decimal number (e.g. {2.41}): "); // the example is good because it informs the user what the correct decimal symbol is.
            if (!ConsoleX.TryReadManyAttempts(ConsoleX.TryReadNumber, out double number, prompt: ""))
                return;

            Console.WriteLine("-------------------------");
            Console.WriteLine($" Sqrt({number}) = {Sqrt(number):#.##}");
            Console.WriteLine($" Pow'2({number}) = {Pow(number, 2):#.##}");
            Console.WriteLine($" Pow'10({number}) = {Pow(number, 10):n2}");
        }
    }
}
