using System;

namespace ConsoleAssignments.Assignments
{
    record A06_GuessingNumber() : Assignment(6, "Guess the Number")
    {
        private readonly Random rng = new Random();

        protected override void Implementation()
        {
            const int MAX_ATTEMPTS = 10;
            int number = rng.Next(1, 101);
            int attempts = 0; // we are doing capture on this
            Console.WriteLine($"A random number between 1-100 has been generated. Can you guess it within {MAX_ATTEMPTS} attempts?");
            ConsoleX.TryReadManyAttempts(
                TryGuessFunc,
                out int guess,
                maxAttempts: MAX_ATTEMPTS
                );

            Console.WriteLine();
            if (guess == number)
                Console.WriteLine($"Congratulations! You managed to guess the correct number in {attempts} attempts.");
            else
                Console.WriteLine($"Better luck next time... (The correct answer was {number}).");

            bool TryGuessFunc(out int guess, ref string? hintMsg, string prompt)
            {
                ++attempts;
                if (!ConsoleX.TryReadNumber(out guess, ref hintMsg, prompt))
                    return false;
                
                int diff = number - guess;
                hintMsg = diff switch
                {
                    0 => null,
                    < -33 => $"The number {guess} is much too large.",
                    > +33 => $"The number {guess} is much too small.",
                    < 0 => $"The number {guess} is too large.",
                    > 0 => $"The number {guess} is too small.",
                };
                return diff == 0;
            }
        }

    }
}
