// C#9 Top-level statements:

using ConsoleApp2;
using ConsoleApp2.Assignments;

var assignments = new Assignment[]
{
    new A01_HelloWorld(),
    new A02_NameAndAge(),
    new A03_ColorChanger(),
    new A04_TodaysDate(),
    new A05_LargestValue(),
    new A06_GuessingNumber(),
    new A07_SaveToDisk(),
};

new AssignmentMenu(assignments).Run();
