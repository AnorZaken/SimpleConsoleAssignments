using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ConsoleAssignments.Assignments
{
    record A07_SaveToDisk() : Assignment(7, "Save to Disk")
    {
        internal static Stack<string> RecentFilePaths { get; } = new();

        protected override void Implementation()
        {
            Console.WriteLine("Lets write some text to disk.");
            Console.WriteLine("First lets get some text input:");
            Console.WriteLine("(Enter an empty sting to cancel)");
            Console.WriteLine();

            var text = ConsoleX.PromptRead("> ") ?? string.Empty;
            if (text.Length == 0)
            {
                Console.WriteLine("Your input was empty - cancelling.");
                return;
            }

            Console.WriteLine("Ok, now please enter a filename, so we can save the text:");
            // New:
            string fileName = PromptFilename(out string filePath, maxLength: 32);
            // Old:
            //if (!ConsoleX.TryRead(TryGetNewFileNameFunc, out string filePath, 10))
            //    return;

            if (filePath.Length == 0)
            {
                Console.WriteLine("Saving was cancelled.");
                return;
            }

            Console.WriteLine();
            try
            {
                File.WriteAllText(filePath, text, Encoding.UTF8);
                Console.WriteLine("Your text was saved to " + filePath);
                RecentFilePaths.Push(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Saving failed :(");
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine();
            Console.Write("Do you want to open the file now to have a look at it? ");
            if (ConsoleX.PromptYesNo(defaultValue: true))
            {
                Process p = new();
                p.StartInfo = new ProcessStartInfo { UseShellExecute = true, FileName = filePath };

                try
                {
                    p.Start();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unfortunately the file failed to open :(");
                    Console.WriteLine(e.Message);
                }
            }
            
            Console.WriteLine();
            Console.Write("Do you want to keep the file on disk? ");
            var inputPos = ConsoleX.Cursor.Position;
            Console.WriteLine("\n(Hint: Keep it for testing A08)");
            var outputPos = ConsoleX.Cursor.Position;
            inputPos.Apply();
            bool keep = ConsoleX.PromptYesNo(defaultValue: true);
            outputPos.Apply();
            if (!keep)
            {
                try
                {
                    File.Delete(filePath);
                    Console.WriteLine($"File {fileName} was deleted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unfortunately file deletion failed :(");
                    Console.WriteLine(e.Message);
                    return;
                }
                finally
                {
                    RecentFilePaths.TryPop(out _);
                }
            }
        }

        private static string PromptFilename(out string filePath, int maxLength = 255)
        {
            CursorState? errCursor = OperatingSystem.IsWindows()
                ? ConsoleX.Cursor.State with { Colors = (ConsoleColor.Red, ConsoleColor.Black), IsVisible = false }
                : ConsoleX.Cursor.State with { Colors = (ConsoleColor.Red, ConsoleColor.Black) };
            Console.WriteLine();
            string fileName;
            string? errorMsg = null;
            do
            {
                ConsoleX.ClearRows(errCursor.Top, ConsoleX.Cursor.Top); // Clears previous error AND input.
                PrintError(errorMsg); // Prints errors from ValidateFileName
                Console.Write(" > ");

                fileName = string.Empty;
                foreach (var update in ConsoleX.PromptAdvanced(charFilter: CharFilter.FileName, maxLength: maxLength)) // <- ReadKey inside
                {
                    ConsoleX.ClearRows(errCursor.Top, update.CursorPosition.Top - 1); // Clears previous error.

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
                SetFileNameExtensionToTxt(ref fileName);
            }
            while (!ValidateFileName(fileName, out filePath, out errorMsg));
            return fileName;

            void PrintError(string? errorMsg)
            {
                var colors = ConsoleX.Cursor.Colors;
                errCursor.Apply();
                Console.WriteLine(errorMsg);
                colors.Apply();
                ConsoleX.Cursor.TrySetVisibility(true);
            }
        }

        //private static bool TryGetNewFileNameFunc(out string path, ref string? errorMsg)
        //{
        //    var filename = ConsoleX.PromptRead("> ") ?? string.Empty;
        //    return AdjustAndValidateFileName(filename, out path, ref errorMsg);
        //}

        private static void SetFileNameExtensionToTxt(ref string fileName)
        {
            if (fileName.Length != 0 && Path.GetExtension(fileName).ToLower() != ".txt") // enforce .txt
                fileName = Path.GetFileNameWithoutExtension(fileName) + ".txt";
        }

        private static bool ValidateFileName(string fileName, out string filePath, out string? errorMsg)
        {
            if (fileName.Length != 0)
            {
                var dirname = Directory.GetCurrentDirectory();
                var tempPath = Path.Combine(dirname, fileName);
                var fiin = new FileInfo(tempPath);
                if (!fiin.Exists)
                {
                    errorMsg = null;
                    filePath = tempPath;
                    return true;
                }
                errorMsg = $"File already exists ({tempPath}).\nPlease choose a different filename!";
            }
            else
            {
                errorMsg = $"File name cannot be empty!";
            }
            filePath = string.Empty;
            return false;
        }
    }

    /* TODO...
    public record MonoSpacedTextArea(int Width)
    {
        public record CursorPosition(int Row, int Col);

        private List<List<string>> Text { get; } = new();
        public int Rows => Text.Count;
        public int Cols() => Cols(Cursor.Row);
        public int Cols(int row) => unchecked((uint)Text.Count > (uint)row) ? Text[row].Count : 0;
        public CursorPosition Cursor { get; private set; } = new(0,0);

        public bool ArrowLeft()
        {
            if (Cursor.Col == 0)
            {
                if (Cursor.Row == 0)
                    return false;
                int row = Cursor.Row - 1;
                int col = Cols(row) - 1;
                Cursor = new(row, col);
            }
        }
    }
    */
}
