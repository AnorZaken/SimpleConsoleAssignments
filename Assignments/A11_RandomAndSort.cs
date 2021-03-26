using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAssignments.Assignments
{
    record A11_RandomAndSort() : Assignment(11, "Random & Sort")
    {
        protected override void Implementation()
        {
            int[] random = TakeRandom(count:10, maxExclusive:1000).ToArray();
            int[] sorted = random.OrderBy(i => i).ToArray();

            Console.Write("Random numbers (in an array):  ");
            Console.WriteLine(string.Join(',', random));
            Console.Write("Sorted numbers (in an array):  ");
            Console.WriteLine(string.Join(',', sorted));
        }

        private IEnumerable<int> TakeRandom(int count, int maxExclusive)
        {
            var r = new Random();
            while (--count >= 0) yield return r.Next(maxExclusive);
        }
    }
}
