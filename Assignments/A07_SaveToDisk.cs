using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ConsoleAssignments.Assignments
{
    record A07_SaveToDisk() : Assignment(7, "Save to Disk")
    {
        internal static List<string> RecentFilePaths { get; } = new();

        protected override void Implementation()
        {
            const int MAX_ATTEMPTS = 10;

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
            if (!ConsoleX.TryRead(TryGetNewFileNameFunc, out string filePath, MAX_ATTEMPTS))
                return;

            if (filePath.Length == 0)
            {
                Console.WriteLine("Your input was empty - cancelling.");
                return;
            }
            Console.WriteLine();
            try
            {
                File.WriteAllText(filePath, text, Encoding.UTF8);
                Console.WriteLine("Your text was saved to " + filePath);
                RecentFilePaths.Add(filePath);
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
                Process p = new Process();
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
        }

        private static bool TryGetNewFileNameFunc(out string path, ref string? errorMsg)
        {
            var filename = ConsoleX.PromptRead("> ") ?? string.Empty;

            if (filename.Length == 0)
            {
                path = string.Empty; // Cancel save
                return true;
            }

            if (Path.GetExtension(filename).ToLower() != ".txt") // enforce .txt
                filename += ".txt";

            var dirname = Directory.GetCurrentDirectory();
            path = Path.Combine(dirname, filename);
            var fiin = new FileInfo(path);
            if (!fiin.Exists)
                return true;
            errorMsg = $"File already exists ({path}).\nPlease choose a different filename!";
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
