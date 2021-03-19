using System;

namespace ConsoleApp2.Assignments
{
    record A04_TodaysDate() : Assignment(4, "Todays Date")
    {
        protected override void Implementation()
        {
            Console.WriteLine("Today's date is " + DateTime.Now.ToLongDateString() + ".");
        }
    }
}
