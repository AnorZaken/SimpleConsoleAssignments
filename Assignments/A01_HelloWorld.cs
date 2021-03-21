using System;

namespace ConsoleAssignments.Assignments
{
    // First example implementation:
    sealed record A01_HelloWorld() : Assignment(1, "Hello World!")
    {
        protected override void Implementation() => Console.WriteLine("Hello World!");
    }
}
