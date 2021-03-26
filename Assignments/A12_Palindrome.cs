using System;
using System.Linq;

namespace ConsoleAssignments.Assignments
{
    record A12_Palindrome() : Assignment(12, "Palindrome?")
    {
        protected override void Implementation()
        {
            Console.Write("Please enter one or more words");
            string input = ConsoleX.PromptLine() ?? "";
            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var palindromes =
                (
                    from word in words
                    where IsPalindrome(word)
                    select word
                )
                .ToArray();

            Console.WriteLine();
            if (words.Length == 1)
            {
                Console.WriteLine("You entered 1 word.");
                Console.WriteLine(palindromes.Length == 0 ? "It is not a a palindrome." : "It is a palindrome!");
            }
            else
            {
                Console.WriteLine($"You entered {words.Length} words.");
                Console.WriteLine(palindromes.Length switch
                {
                    0 => "None of them are palindromes.",
                    1 => $"Out of which the following is a palindrome: {palindromes[0]}",
                    _ => $"Where the following {palindromes.Length} are palindromes: {palindromes.JoinText(", ")}",
                });
            }
        }

        private static bool IsPalindrome(string word)
        {
            return word.Equals(word.Reverse(), StringComparison.CurrentCulture);
        }
    }
}
