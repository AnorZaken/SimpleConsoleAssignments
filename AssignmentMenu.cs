using System;

namespace ConsoleAssignments
{
    class AssignmentMenu
    {
        private readonly Assignment[] assignments;

        public AssignmentMenu(Assignment[] assignments) => this.assignments = assignments;


        public void Run()
        {
            do
            {
                Console.Clear();
                WriteHeader();
                WriteAssignments();
                Console.WriteLine("Select assignment(enter number)");
                ConsoleX.TryReadManyAttempts(TryReadIndexFunc, out int index);
                
                if (index == -1) 
                    break; // Exit

                assignments[index].Run();
            }
            while (true);
        }


        private void WriteHeader()
        {
            Console.WriteLine("-- List of completed Assignments:");
            Console.WriteLine();
        }

        private void WriteAssignments()
        {
            foreach (var a in assignments)
                Console.WriteLine($" - {a.Number:d2} : {a.Name} ");

            Console.WriteLine();
            Console.WriteLine(" - 00 : Exit ");
        }

        private bool TryReadIndexFunc(out int index, ref string? errorMsg, string prompt)
        {
            index = -1;
            if (ConsoleX.TryReadNumber(out int? number, ref errorMsg, prompt))
            {
                if (number == 0 /* Exit */ || TryFindAssignmentIndex(number.Value, out index))
                    return true;
                errorMsg = $"No such assignment number found ({number})!";
                return false;
            }
            return number is null;

            bool TryFindAssignmentIndex(int assignmentNumber, out int index)
            {
                index = Array.FindIndex(assignments, a => a.Number == assignmentNumber);
                return index != -1;
            }
        }
    }
}
