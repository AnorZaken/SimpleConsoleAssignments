// C#9 Top-level statements:

using ConsoleAssignments;
using ConsoleAssignments.Assignments;

/* - Playing around with console scrolling - */
//using System;
//while (true)
//{
//    var pos = ConsoleX.Cursor.Position;
//    Console.Write($"Coords: {Console.GetCursorPosition()}");
//    pos.Apply();
//    switch (Console.ReadKey(true).Key) // (not checking bounds on any of these!)
//    {
//        case ConsoleKey.UpArrow:
//            --Console.CursorTop;
//            break;
//        case ConsoleKey.DownArrow:
//            ++Console.CursorTop;
//            break;
//        case ConsoleKey.LeftArrow:
//            --Console.CursorLeft;
//            break;
//        case ConsoleKey.RightArrow:
//            ++Console.CursorLeft;
//            break;
//        default: break;
//    }
//}

var assignments = new Assignment[]
{
    new A01_HelloWorld(),
    new A02_NameAndAge(),
    new A03_ColorChanger(),
    new A04_TodaysDate(),
    new A05_LargestValue(),
    new A06_GuessingNumber(),
    new A07_SaveToDisk(),
    new A08_ReadFile(),
    new A09_MathPower(),
    new A10_MultiplicationTable(),
    new A11_RandomAndSort(),
};

new AssignmentMenu(assignments).Run();
