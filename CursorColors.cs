using System;

namespace ConsoleAssignments
{
    public record CursorColors(ConsoleColor Foreground, ConsoleColor Background)
    {
        public CursorColors() : this(Console.ForegroundColor, Console.BackgroundColor) { }

        public CursorColors Apply()
        {
            (Console.BackgroundColor, Console.ForegroundColor) = (Background, Foreground);
            return this;
        }

        public static implicit operator CursorColors((ConsoleColor Foreground, ConsoleColor Background) tuple) => new(tuple.Foreground, tuple.Background);
    }
}
