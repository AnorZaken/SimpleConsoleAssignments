using System;
using System.Collections.Generic;
using System.Linq;

public class RentalTime
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    public RentalTime(DateTime start, DateTime end)
    {
        this.Start = start;
        this.End = end;
    }
}

public static class CarRental
{
    public static bool CanScheduleAll(IEnumerable<RentalTime> unloadingTimes)
    {
        var orderByStart = unloadingTimes.OrderByDescending(rt => rt.Start);
        bool b = orderByStart.Zip<RentalTime, RentalTime, bool>(orderByStart.Skip(1), (a, b) => a.End <= b.Start).All(true); // Vill vi ha End < Start eller End <= Start - frågan är lite otydlig...
        return b;
    }

    public static void Main(string[] args)
    {
        var format = System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat;

        RentalTime[] unloadingTimes = new RentalTime[]
        {
            new RentalTime(DateTime.Parse("3/4/2019 19:00", format), DateTime.Parse("3/4/2019 20:30", format)),
            new RentalTime(DateTime.Parse("3/4/2019 22:10", format), DateTime.Parse("3/4/2019 22:30", format)),
            new RentalTime(DateTime.Parse("3/4/2019 20:30", format), DateTime.Parse("3/4/2019 22:00", format))
        };

        Console.WriteLine(CarRental.CanScheduleAll(unloadingTimes));
    }
}