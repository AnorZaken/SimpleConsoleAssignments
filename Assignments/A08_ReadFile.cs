using System;
using System.IO;

namespace ConsoleAssignments.Assignments
{
    record A08_ReadFile() : Assignment(8, "Read File")
    {
        protected override void Implementation() // TODO: a file-prompt for using whatever file you want.
        {
            Console.WriteLine("This assignment is about reading (text) files.");
            do
            {
                Console.WriteLine();
                if (!A07_SaveToDisk.RecentFilePaths.TryPeek(out var filePath))
                {
                    Console.WriteLine("Please run Assignment-07 to create a file first!");
                    return;
                }
                
                string fileName = Path.GetFileName(filePath);
                Console.Write($"Do you want to open file {fileName} from Assignment-07? ");
                if (ConsoleX.PromptYesNo(defaultValue: true) != true)
                    return;

                string fileContent;
                try
                {
                    fileContent = File.ReadAllText(filePath);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unfortunately reading the file failed :(");
                    Console.WriteLine(e.Message.Truncate(100)); // todo: TruncateLines(3) or something...
                    A07_SaveToDisk.RecentFilePaths.TryPop(out _);
                    Console.WriteLine("(File removed from A07's recent file history)");
                    continue;
                }
                Console.WriteLine();
                Console.WriteLine("This was the content written in the file:");
                ConsoleX.WriteDividerLine();

                Console.WriteLine(fileContent);

                Console.WriteLine();
                ConsoleX.WriteDividerLine(emptyRowsAfter: 0);
                return;
            }
            while (true);
        }
    }
}
