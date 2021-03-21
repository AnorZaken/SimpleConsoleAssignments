using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAssignments.Assignments
{
    record A08_ReadFile() : Assignment(8, "Read File")
    {
        protected override void Implementation()
        {
            string? filePath = A07_SaveToDisk.RecentFilePaths?[^1]; // fixme: what if empty

            if (filePath != null)
            {

            }
        }
    }
}
