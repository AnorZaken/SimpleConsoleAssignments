using System;

namespace ConsoleApp2
{
    abstract record Assignment(int Number, string Name)
    {
        public void Run()
        {
            WriteHeader();
            Implementation();
            WriteExit();
        }

        protected virtual void WriteHeader()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"  Assignment {Number:d2} : {Name}");
            ConsoleX.WriteDividerLine(emptyRowsAfter: 1);
        }

        protected virtual void WriteExit()
        {
            Console.WriteLine();
            ConsoleX.PressAnyKey("Press any key to return to the menu...");
        }

        protected abstract void Implementation();
    }
}
