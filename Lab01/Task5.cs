using System;

public static class Task5
{
    public static void Run()
    {
        int day = int.Parse(Console.ReadLine());

        string schedule = day switch
        {
            1 => "monday 08:00 - 18:00",
            2 => "tuesday 08:00 - 18:00",
            3 => "wednesday 08:00 - 18:00",
            4 => "thursday 08:00 - 18:00",
            5 => "friday 08:00 - 18:00",
            6 => "saturday 08:00 - 18:00",
            7 => "sunday - day off",
            _ => "incorrect number of day"
        };

        Console.WriteLine(schedule);
    }
}
