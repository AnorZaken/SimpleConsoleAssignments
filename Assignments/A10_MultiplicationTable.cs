using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAssignments.Assignments
{
    record A10_MultiplicationTable() : Assignment(10, "Multiplication Table")
    {
        protected override void Implementation()
        {
            // could have simply done two nested for-loops, and that would have been better, but this was more fun:
            foreach (var row in CreateMultiplicationTable(rows: 10, columns: 10))
                Console.WriteLine(row);
        }

        private static IEnumerable<string> CreateMultiplicationTable(int rows, int columns)
        {
            long max = rows * (long)columns;
            int padding = $"{(max):F0}".Length + 1; // the +1 is for the TAB character

            var multTable = Enumerable
                .Repeat(Enumerable.Range(1, columns), rows)
                .Select(
                    (range, rowIndex) => range
                    .Select(num => num * (rowIndex + 1))
                    .Select(num => (num + "\t").PadLeft(padding, ' ')) // padding to right-align numbers
                    .Aggregate((row, elem) => row + elem)
                );

            return multTable;
        }
    }
}
