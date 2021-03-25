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

            var text = ConsoleX.PromptLine("> ") ?? string.Empty;
            if (text.Length == 0)
            {
                Console.WriteLine("Your input was empty - cancelling.");
                return;
            }

            Console.WriteLine("Ok, now please enter a filename, so we can save the text:");
            string fileName = ConsoleX.PromptFilename(
                out string filePath,
                maxLength: 32,
                preValidationProcessing: SetFileNameExtensionToTxt,
                validateAndGetPath: ValidateFileNameAndGetPath
                );

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
            if (ConsoleX.PromptYesNo(defaultValue: true) == true)
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
            Console.Write("Do you want to keep the file on disk?");
            var inputPos = ConsoleX.Cursor.Position;
            Console.WriteLine("\n(Recomended: Keep it for testing A08)");
            var outputPos = ConsoleX.Cursor.Position;
            inputPos.Apply();
            bool keep = ConsoleX.PromptYesNo(defaultValue: true) ?? true;
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

        private static void SetFileNameExtensionToTxt(ref string fileName)
        {
            if (fileName.Length != 0 && Path.GetExtension(fileName).ToLower() != ".txt") // enforce .txt
                fileName = Path.GetFileNameWithoutExtension(fileName) + ".txt";
        }

        private static bool ValidateFileNameAndGetPath(string fileName, out string filePath, out string? errorMsg)
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
}
