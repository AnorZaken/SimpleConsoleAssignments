using System;
using System.Runtime.Versioning;

namespace ConsoleAssignments
{
    // System.Console calls this Window, but that's just confusing...
    public record ViewArea(int Left, int Top, int Width, int Height) : ViewPosition(Left, Top)
    {
        public ViewArea() : this(Console.WindowLeft, Console.WindowTop, Console.WindowWidth, Console.WindowHeight) { }
        
        [SupportedOSPlatform("windows")]
        public new ViewArea Apply()
        {
            Console.SetWindowSize(1, 1); // shrink before changing position because:
            Console.SetWindowPosition(Left, Top); // <-- risk of going out-of-range.
            Console.SetWindowSize(Width, Height);
            return this;
        }
    }
}
