using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleAssignments
{
    // eXtra helper methods for basic console fun
    public static partial class ConsoleX
    {
        private const string DEFAULT_PROMPT = " > ";

        public static string EmptyLine => new(' ', Console.BufferWidth);

        public static void ClearRow(int row)
        {
            var pos = Cursor.Position;
            Cursor.Position = (0, row);
            Console.Write(EmptyLine);
            pos.Apply();
        }

        public static void ClearRows(int firstRow, int lastRow) // both values are inclusive
        {
            var pos = Cursor.Position;
            string emptyLine = EmptyLine;
            int diffSign = (lastRow - firstRow) >> 31 | 1; // makes argument order irrelevant (-1 or +1)
            for (int row = firstRow; row <= lastRow; row += diffSign)
            {
                Cursor.Position = (0, row);
                Console.Write(emptyLine);
            }
            pos.Apply();
        }


        public static ConsoleKeyInfo PressAnyKey(string message = "Press any key...")
        {
            Console.Write(message);
            return Console.ReadKey(true);
        }


        public static void WriteDividerLine(char lineChar = '=', int emptyRowsAfter = 1) // Issue: Can cause horizontally View movement if View.Width < Buffer.Width
        {
            // ensure cursor is at the start of a line
            if (Cursor.Left != 0)
                Console.WriteLine();

            // newTop ensures that the cursor moves the correct number of lines
            int newTop = Console.CursorTop + emptyRowsAfter + 1;
            Console.Write(new string(lineChar, Console.WindowWidth));
            Cursor.Position = (0, newTop);
        }

        // supports cancel with ESC (that's why the number type is nullable: If (return: false && number is null) then ESC cancel occurred)
        public static bool TryReadNumber([NotNullWhen(true)] out int? number, ref string? errorMsg, string prompt = DEFAULT_PROMPT)
            => TryReadNumber(int.TryParse, out number, ref errorMsg, prompt, true);
        public static bool TryReadNumber([NotNullWhen(true)] out float? number, ref string? errorMsg, string prompt = DEFAULT_PROMPT)
            => TryReadNumber(float.TryParse, out number, ref errorMsg, prompt, true);
        public static bool TryReadNumber([NotNullWhen(true)] out decimal? number, ref string? errorMsg, string prompt = DEFAULT_PROMPT)
            => TryReadNumber(decimal.TryParse, out number, ref errorMsg, prompt, true);
        public static bool TryReadNumber([NotNullWhen(true)] out double? number, ref string? errorMsg, string prompt = DEFAULT_PROMPT)
            => TryReadNumber(double.TryParse, out number, ref errorMsg, prompt, true);
        // overloads that does not support cancellation:
        public static bool TryReadNumber(out int number, ref string? errorMsg, string prompt = DEFAULT_PROMPT)
            => TryReadNumber(int.TryParse, out number, ref errorMsg, prompt);
        public static bool TryReadNumber(out float number, ref string? errorMsg, string prompt = DEFAULT_PROMPT)
            => TryReadNumber(float.TryParse, out number, ref errorMsg, prompt);
        public static bool TryReadNumber(out decimal number, ref string? errorMsg, string prompt = DEFAULT_PROMPT)
            => TryReadNumber(decimal.TryParse, out number, ref errorMsg, prompt);
        public static bool TryReadNumber(out double number, ref string? errorMsg, string prompt = DEFAULT_PROMPT)
            => TryReadNumber(double.TryParse, out number, ref errorMsg, prompt);

        private delegate bool TryParseFunc<T>(string? input, out T output) where T : notnull;

        private static bool TryReadNumber<T>(TryParseFunc<T> tryParseFunc, out T number, ref string? errorMsg, string prompt) where T : struct
        {
            bool success = TryReadNumber(tryParseFunc, out T? numberNull, ref errorMsg, prompt, false);
            number = numberNull ?? default;
            return success;
        }

        private static bool TryReadNumber<T>(TryParseFunc<T> tryParseFunc, [NotNullWhen(true)] out T? number, ref string? errorMsg, string prompt, bool allowEscCancel)
            where T : struct // could also constrain to: "notnull, new()"
        {
            Console.Write(prompt);
            bool success = TryReadNumber(tryParseFunc, out number, out string? input, allowEscCancel);
            if (success == false && number != null)
                errorMsg = $"Make sure the input is a number (\"{input.Truncate(12)}\")!";
            return success;

            static bool TryReadNumber(TryParseFunc<T> tryParseFunc, [NotNullWhen(true)] out T? number, out string? input, bool allowEscCancel)
            {
                input = "";
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = Console.ReadKey(true);
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Escape:
                            if (allowEscCancel)
                            {
                                number = null;
                                return false;
                            }
                            break;
                        case ConsoleKey.Backspace:
                            if (input.Length > 0)
                            {
                                input = input[..^1];
                                //Console.Write(keyInfo.KeyChar); // writing a backspace just moves the cursor, it doesn't erase any buffer content.
                                Cursor.Backspace();
                            }
                            break;
                        case ConsoleKey.Enter:
                            break;
                        default:
                            if (!keyInfo.HasModifiersOtherThanShift() && !keyInfo.IsAControlChar())
                            {
                                input += keyInfo.KeyChar;
                                Console.Write(keyInfo.KeyChar);
                            }
                            break;
                    }
                } while (keyInfo.Key != ConsoleKey.Enter);

                bool success = tryParseFunc(input, out T num);
                number = num;
                return success;
            }
        }

        public delegate bool TryReadFunc<T>(out T value, ref string? errorMsg, string prompt);

        // A wrapper around a TryReadFunc that handles errorMsg output, and allows specifying maxAttempts.
        public static bool TryReadWithError<T>(TryReadFunc<T> tryReadFunc, out T value, string prompt = DEFAULT_PROMPT, int maxAttempts = 8)
        {
            var errPos = Cursor.Position;
            string? errorMsg = null;
            do
            {
                if (errorMsg != null)
                    Console.WriteLine(errorMsg);
                if (tryReadFunc(out value, ref errorMsg, prompt))
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

        public static bool TryAsk<T>(string question, TryReadFunc<T> attemptFunc, out T value, string prompt = DEFAULT_PROMPT, int maxAttempts = 8)
        {
            Console.WriteLine(question);
            if (TryReadWithError(attemptFunc, out value, prompt, maxAttempts))
                return true;
            //PressAnyKey();
            return false;
        }

        public static string? PromptLine(string prompt = " > ")
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        public static bool? PromptYesNo(string prompt = "(y/n): ", bool? defaultValue = null, bool allowEscapeCancel = false)
        {
            Console.Write(prompt);
            var pos = Cursor.Position;
            bool? value = PrintValue(defaultValue);
            do
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Escape:
                        if (!allowEscapeCancel)
                            goto case ConsoleKey.N;
                        value = PrintValue(null);
                        return null;
                    case ConsoleKey.Enter:
                        if (value != null)
                            return value;
                        break;
                    case ConsoleKey.Y:
                        value = PrintValue(true);
                        break;
                    case ConsoleKey.N:
                        value = PrintValue(false);
                        break;
                }
            }
            while (true);

            bool? PrintValue(bool? value)
            {
                pos.Apply();
                Console.Write(value == null ? '-' : value.Value ? 'Y' : 'N');
                pos.Apply();
                return value;
            }
        }

        public static IEnumerable<PromptUpdate> PromptAdvanced(CharFilter? charFilter = null, string prompt = DEFAULT_PROMPT, int maxLength = 255, int padLeft = -1, int padRight = 0)
        {
            charFilter ??= CharFilter.All;

            Console.Write(prompt);
            var pos = Cursor.Position;
            if (padLeft == -1)
                padLeft = pos.Left;
            else if (padLeft > pos.Left)
            {
                pos = pos with { Left = padLeft };
                pos.Apply();
            }

            PromptUpdate update;
            string text = "";
            do
            {
                pos.Apply();
                var keyInfo = Console.ReadKey(true);
                update = keyInfo.HasModifiersOtherThanShift()
                    ? new PromptUpdate(text, text, pos, keyInfo, PromptUpdateCause.InputHasModifier)
                    : keyInfo.IsAControlChar()
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
                pos = Cursor.Backspace(padLeft, padRight);
                return new(text, oldText, pos, keyInfo, PromptUpdateCause.Backspace);
            }
            PromptUpdate TextInputHelper(ConsoleKeyInfo keyInfo)
            {
                if (!charFilter.IsValid(keyInfo.KeyChar))
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
                pos = Cursor.Write(keyInfo.KeyChar, padLeft, padRight);
                return new(text, oldText, pos, keyInfo, PromptUpdateCause.InputAppended);
            }
        }

        public delegate void PreValidationProcessing(ref string fileName);
        public delegate bool ValidateFileNameAndGetPath(string fileName, out string filePath, out string? errorMsg);

        // Assumes that the current cursor position when called is the errorMessage position.
        public static string PromptFilename(out string filePath, ValidateFileNameAndGetPath validateAndGetPath, PreValidationProcessing? preValidationProcessing = null, string prompt = DEFAULT_PROMPT, int maxLength = 255)
        {
            CursorState? errCursor = Cursor.State with { Colors = (ConsoleColor.Red, ConsoleColor.Black), IsVisible = false };
            Console.WriteLine();
            string fileName;
            string? errorMsg = null;
            do
            {
                ClearRows(errCursor.Top, Console.CursorTop); // (error AND input)
                PrintError(errorMsg); // (error from validateAndGetPath)
                var inputPos = Cursor.Position;

                fileName = string.Empty;
                foreach (var update in PromptAdvanced(charFilter: CharFilter.FileName, prompt: prompt, maxLength: maxLength)) // <- ReadKey inside
                {
                    ClearRows(errCursor.Top, inputPos.Top - 1); // (error only)

                    switch (update.Cause)
                    {
                        case PromptUpdateCause.Backspace:
                        case PromptUpdateCause.BackspaceOnEmpty:
                        case PromptUpdateCause.InputAppended:
                        case PromptUpdateCause.InputHasModifier:
                        case PromptUpdateCause.InputNonPrintable:
                            continue;
                        case PromptUpdateCause.Escape:
                            filePath = string.Empty; // Cancel by specifying empty path.
                            return string.Empty; // <--
                        case PromptUpdateCause.Enter:
                            fileName = update.Text;
                            continue;
                        case PromptUpdateCause.InputRejectedByFilter:
                            PrintError("Invalid filename character!");
                            break;
                        case PromptUpdateCause.TextMaxLength:
                            PrintError("Filename too long...");
                            break;
                    }
                }
                if (preValidationProcessing != null)
                    preValidationProcessing(ref fileName);
            }
            while (!validateAndGetPath(fileName, out filePath, out errorMsg));
            return fileName;

            void PrintError(string? errorMsg)
            {
                var colors = Cursor.Colors;
                errCursor.Apply();
                Console.WriteLine(errorMsg);
                colors.Apply();
                Cursor.TrySetVisibility(true);
            }
        }
    }
}
