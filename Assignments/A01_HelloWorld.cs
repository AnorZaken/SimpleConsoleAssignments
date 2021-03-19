using System;

namespace ConsoleApp2.Assignments
{
    // First example implementation:
    sealed record A01_HelloWorld() : Assignment(1, "Hello World!")
    {
        protected override void Implementation() => Console.WriteLine("Hello World!");
    }
}
