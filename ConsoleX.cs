using System;
using System.Collections.Generic;
using System.Runtime.Versioning;

namespace ConsoleAssignments
{
    // eXtra helper methods for basic console fun
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
            public static CursorPosition MoveForwards(int padLeft = 0, int padRight = 0) => Position.GetRelativeMove(+1, padLeft, padRight).Apply();
            public static CursorPosition MoveBackwards(int padLeft = 0, int padRight = 0) => Position.GetRelativeMove(-1, padLeft, padRight).Apply();
        }

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

        public static string EmptyLine => new(' ', Console.BufferWidth);

        public static void ClearRow(int row)
        {
            var pos = Cursor.Position;
            Cursor.Position = (0, row);
            Console.Write(EmptyLine);
            pos.Apply();
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
            pos.Apply();
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

        //[SupportedOSPlatform("windows")]
        //public static void WriteAtPosition(CursorPosition position, dynamic value)
        //{
        //    var oldView = View.Position;
        //    var oldCursor = Cursor.Position;
        //    position.Apply();
        //    Console.Write(value);
        //    oldView.Apply();
        //    oldCursor.Apply();
        //}

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
                errPos.Apply();
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

        public static IEnumerable<PromptUpdate> PromptAdvanced(CharFilter? charFilter = null, int maxLength = 255, int padLeft = -1, int padRight = 0)
        {
            var pos = Cursor.Position;
            if (padLeft == -1)
                padLeft = pos.Left;
            else if (padLeft > pos.Left)
            {
                pos = pos with { Left = padLeft };
                pos.Apply();
            }
            CharFilter filter = charFilter ?? CharFilter.All;

            PromptUpdate update;
            string text = "";
            do
            {
                pos.Apply();
                var keyInfo = Console.ReadKey(true);
                bool hasModifiersOtherThanShift = (keyInfo.Modifiers & ~ConsoleModifiers.Shift) != 0;
                update = hasModifiersOtherThanShift
                    ? new PromptUpdate(text, text, pos, keyInfo, PromptUpdateCause.InputHasModifier)
                    : char.IsControl(keyInfo.KeyChar)
                        ? keyInfo.Key switch
                        {
                            ConsoleKey.Escape => EscapeHelper(keyInfo),
                            ConsoleKey.Enter => EnterHelper(keyInfo),
                            ConsoleKey.Backspace => BackspaceHelper(keyInfo),
                            _ => new(text, pos, keyInfo, PromptUpdateCause.InputNonPrintable),
                        }
                        : TextInputHelper(keyInfo);
                yield return update;
            }
            while (!update.IsFinalUpdate);

            PromptUpdate EscapeHelper(ConsoleKeyInfo keyInfo) => new(text, pos, keyInfo, PromptUpdateCause.Escape);
            PromptUpdate EnterHelper(ConsoleKeyInfo keyInfo) => new(text, pos, keyInfo, PromptUpdateCause.Enter);
            PromptUpdate BackspaceHelper(ConsoleKeyInfo keyInfo)
            {
                if (text.Length == 0)
                    return new(text, pos, keyInfo, PromptUpdateCause.BackspaceOnEmpty);
                string oldText = text;
                text = text[..^1];
                //pos.Apply();
                pos = TextEdit.Backspace(padLeft, padRight);
                return new(text, oldText, pos, keyInfo, PromptUpdateCause.Backspace);
            }
            PromptUpdate TextInputHelper(ConsoleKeyInfo keyInfo)
            {
                if (!filter.IsValid(keyInfo.KeyChar))
                    return new(text, pos, keyInfo, PromptUpdateCause.InputRejectedByFilter);
                if (text.Length >= maxLength)
                    return new(text, pos, keyInfo, PromptUpdateCause.TextMaxLength);
                return InputAppendedHelper(keyInfo);
            }
            PromptUpdate InputAppendedHelper(ConsoleKeyInfo keyInfo)
            {
                string oldText = text;
                text += keyInfo.KeyChar;
                //pos.Apply();
                pos = TextEdit.Write(keyInfo.KeyChar, padLeft, padRight);
                return new(text, oldText, pos, keyInfo, PromptUpdateCause.InputAppended);
            }
        }

        public static class TextEdit
        {
            public static CursorPosition Backspace(int padLeft = 0, int padRight = 0)
            {
                var pos = Cursor.MoveBackwards(padLeft, padRight);
                Console.Write(' ');
                return pos.Apply();
            }

            public static CursorPosition Write(char value, int padLeft = 0, int padRight = 0)
            {
                var pos = Cursor.Position.GetRelativeMove(1, padLeft, padRight);
                Console.Write(value);
                return pos.Apply();
            }

            public static CursorPosition Write(string value, int padLeft = 0, int padRight = 0) // extremely unoptimized
            {
                foreach (char c in value)
                    Write(c, padLeft, padRight);
                return Cursor.Position;
            }
        }
    }
}
