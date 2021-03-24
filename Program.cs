﻿// C#9 Top-level statements:

using ConsoleAssignments;
using ConsoleAssignments.Assignments;

//using System;
//// PLAYING AROUND: console scrolling...
//for (int i = 0; i < 100; ++i)
//    Console.WriteLine(i);

//Console.WriteLine();
//Console.WriteLine();

//while (true)
//{
//    var pos = ConsoleX.Cursor.Position;
//    Console.Write($"Coords: {Console.GetCursorPosition()}");
//    pos.Apply();
//    switch (Console.ReadKey(true).Key)
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
};

new AssignmentMenu(assignments).Run();
