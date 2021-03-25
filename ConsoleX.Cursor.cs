using System;

namespace ConsoleAssignments
{
    public static partial class ConsoleX
    {
        public static class Cursor
        {
            public static int Left => Console.CursorLeft;
            public static int Top => Console.CursorTop;
            public static CursorPosition Position
            {
                get => new();
                set => value.Apply();
            }
            public static ConsoleColor ForeColor => Console.ForegroundColor;
            public static ConsoleColor BackColor => Console.BackgroundColor;
            public static CursorColors Colors
            {
                get => new();
                set => value.Apply();
            }

            public static bool TryGetVisibility(out bool isVisible)
            {
                if (OperatingSystem.IsWindows())
                {
                    isVisible = Console.CursorVisible;
                    return true;
                }
                isVisible = true;
                return false;
            }
            public static bool TrySetVisibility(bool isVisible)
            {
                if (OperatingSystem.IsWindows())
                {
                    Console.CursorVisible = isVisible;
                    return true;
                }
                return false;
            }

            public static CursorState State
            {
                get => new();
                set => value.Apply();
            }
            public static CursorPosition MoveBackwards(int padLeft = 0, int padRight = 0) => Position.GetRelativePosition(-1, padLeft, padRight).Apply();
            public static CursorPosition MoveForwards(int padLeft = 0, int padRight = 0) => Position.GetRelativePosition(+1, padLeft, padRight).Apply();

            public static CursorPosition Backspace(int padLeft = 0, int padRight = 0)
            {
                var pos = MoveBackwards(padLeft, padRight);
                Console.Write(' ');
                return pos.Apply();
            }

            public static CursorPosition Write(char value, int padLeft = 0, int padRight = 0)
            {
                var pos = Position.GetRelativePosition(1, padLeft, padRight);
                Console.Write(value);
                return pos.Apply();
            }

            //public static CursorPosition Write(string value, int padLeft = 0, int padRight = 0) // extremely unoptimized
            //{
            //    foreach (char c in value)
            //        Write(c, padLeft, padRight);
            //    return Position;
            //}
        }
    }
}
