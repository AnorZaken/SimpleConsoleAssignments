using System;
using System.Runtime.Versioning;

namespace ConsoleAssignments
{
    public static partial class ConsoleX
    {
        public static class View // System.Console calls this Window, but I find that confusing...
        {
            public static int Left => Console.WindowLeft;
            public static int Top => Console.WindowTop;
            public static int Width => Console.WindowWidth;
            public static int Height => Console.WindowHeight;
            public static ViewArea Area
            {
                get => new();
                [SupportedOSPlatform("windows")]
                set => value.Apply();
            }
            public static ViewPosition Position
            {
                get => new();
                [SupportedOSPlatform("windows")]
                set => value.Apply();
            }
        }
    }
}
