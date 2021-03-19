using System;

namespace ConsoleApp2
{
    public record CursorPosition(int Left, int Top)
    {
        public CursorPosition() : this(Console.CursorLeft, Console.CursorTop) { }
        public void Restore() => (Console.CursorLeft, Console.CursorTop) = (Left, Top);
        public static implicit operator CursorPosition((int left, int top) tuple) => new(tuple.left, tuple.top);
    }

    public record ConsoleColorSet(ConsoleColor Background, ConsoleColor Foreground)
    {
        public ConsoleColorSet() : this(Console.BackgroundColor, Console.ForegroundColor) { }
        public void Restore() => (Console.BackgroundColor, Console.ForegroundColor) = (Background, Foreground);
    }


    // eXtra helper methods for basic console fun
    public static class ConsoleX
    {
        public static class Cursor
        {
            public static int Left => Console.CursorLeft;
            public static int Top => Console.CursorTop;
            public static CursorPosition Position
            {
                get => new();
                set => value.Restore();
            }
        }

        public static class Colors
        {
            public static ConsoleColor Foreground => Console.ForegroundColor;
            public static ConsoleColor Background => Console.BackgroundColor;
            public static ConsoleColorSet Current
            {
                get => new();
                set => value.Restore();
            }
        }

        public static string EmptyLine => new(' ', Console.BufferWidth);

        public static void ClearRow(int row)
        {
            var pos = Cursor.Position;
            Cursor.Position = (0, row);
            Console.Write(EmptyLine);
            pos.Restore();
        }

        public static void ClearRows(int startRow, int lastRow)
        {
            var pos = Cursor.Position;
            string emptyLine = EmptyLine;
            for (int row = startRow; row <= lastRow; ++row)
            {
                Cursor.Position = (0, row);
                Console.Write(emptyLine);
            }
            pos.Restore();
        }

        public static void PressAnyKey(string message = "Press any key...")
        {
            Console.Write(message);
            Console.ReadKey(true);
        }

        public static void WriteDividerLine(char lineChar = '=', int emptyRowsAfter = 1)
        {
            if (Cursor.Left != 0) // ensure we always start on a new line
                Console.WriteLine();
            int newTop = Cursor.Top + emptyRowsAfter + 1; // ensure we move the correct number of lines
            Console.Write(new string(lineChar, Console.WindowWidth));
            Cursor.Position = (0, newTop);
        }

        public static bool TryReadNumber(out int number, out string? input)
        {
            input = Console.ReadLine();
            return int.TryParse(input, out number);
        }

        public static bool TryReadNumber(string prompt, out int number, ref string? errorMsg, out string? input)
        {
            Console.Write(prompt);
            if (TryReadNumber(out number, out input))
                return true;
            errorMsg = $"Make sure the input is a number (\"{input.Truncate(12)}\")!";
            return false;
        }

        public delegate bool TryReadFunc<T>(out T value, ref string? errorMsg);

        public static bool TryRead<T>(TryReadFunc<T> attemptFunc, out T value, int maxAttempts = 5)
        {
            var errPos = Cursor.Position;
            string? errorMsg = null;
            do
            {
                if (errorMsg != null)
                    Console.WriteLine(errorMsg);
                if (attemptFunc(out value, ref errorMsg))
                {
                    if (errorMsg != null)
                        ClearRow(errPos.Top);
                    return true; // <--
                }
                ClearRows(errPos.Top, Console.CursorTop);
                errPos.Restore();
            }
            while (--maxAttempts > 0);

            Console.WriteLine("Too many attempts!");
            return false;
        }

        public static bool TryAsk<T>(string question, TryReadFunc<T> attemptFunc, out T value, int maxAttempts = 5)
        {
            Console.WriteLine(question);
            if (TryRead(attemptFunc, out value, maxAttempts))
                return true;
            PressAnyKey();
            return false;
        }

        public static string? PromptRead(string prompt = " > ")
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public static bool PromptYesNo(string prompt = "(y/n): ", bool? defaultValue = null)
        {
            Console.Write(prompt);
            if (defaultValue is true)
            {
                Console.Write('Y');
                Console.CursorLeft--;
            }
            else if (defaultValue is false)
            {
                Console.Write('N');
                Console.CursorLeft--;
            }
            do
            {
                var key = Console.ReadKey(true);
                if (key.Key is ConsoleKey.Enter && defaultValue != null)
                {
                    return defaultValue.Value;
                }
                if (key.Key is ConsoleKey.Y)
                {
                    Console.Write('Y');
                    return true;
                }
                if (key.Key is ConsoleKey.N or ConsoleKey.Escape)
                {
                    Console.Write('N');
                    return false;
                }
            }
            while (true);
        }
    }
}
