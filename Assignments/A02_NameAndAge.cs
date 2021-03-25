using System;

namespace ConsoleAssignments.Assignments
{
    sealed record A02_NameAndAge() : Assignment(2, "Name and Age")
    {
        private record Names(string First, string Last);

        protected override void Implementation()
        {
            static bool TryReadNameFunc(out Names name, ref string? errorMsg, string prompt)
            {
                Console.Write(prompt);

                var words = Console.ReadLine()?.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

                if (words?.Length == 2)
                {
                    name = new(words[0], words[1]);
                    return true;
                }
                else
                {
                    name = new(string.Empty, string.Empty);
                    errorMsg = "Please enter your first and last name only (Example: \"Bob Horst\")!";
                    return false;
                }
            }

            static bool TryReadAgeFunc(out int age, ref string? errorMsg, string prompt)
            {
                if (!ConsoleX.TryReadNumber(out age, ref errorMsg, prompt))
                    return false;
                if (age >= 0)
                    return true;
                errorMsg = $"Age cannot be negative ({age})!";
                return false;
            }


            if (!ConsoleX.TryAsk("What is your first and last name?", TryReadNameFunc, out Names names))
                return;
            Console.WriteLine();
            
            if (!ConsoleX.TryAsk("What is your age in years?", TryReadAgeFunc, out int age))
                return;
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine(" This is your entered data:");
            Console.WriteLine("----------------------------");
            Console.WriteLine(" FirstName:  " + names.First);
            Console.WriteLine("  LastName:  " + names.Last);
            Console.WriteLine(" Age (yrs):  " + age);
        }
    }
}
