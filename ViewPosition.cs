using System;
using System.Runtime.Versioning;

namespace ConsoleAssignments
{
    // System.Console calls this Window, but that's just confusing...
    public record ViewPosition(int Left, int Top)
    {
        public ViewPosition() : this(Console.WindowLeft, Console.WindowTop) { }

        [SupportedOSPlatform("windows")]
        public ViewPosition Apply()
        {
            Console.SetWindowPosition(Left, Top);
            return this;
        }

        public static implicit operator ViewPosition((int Left, int Top) tuple) => new(tuple.Left, tuple.Top);
    }
}
