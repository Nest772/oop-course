using System;

namespace ClinicApp.Models;

public struct WorkSchedule
{
    public int Start { get; }
    public int End { get; }

    public int HoursPerDay => End - Start;

    public string Display => $"{Start:D2}:00–{End:D2}:00";

    public bool IsNow => Contains(DateTime.Now.Hour);

public WorkSchedule(int start, int end)
{
    if (start < 0 || start > 23)
        throw new ArgumentOutOfRangeException(
            nameof(start),
            "Start must be between 0 and 23.");

    if (end < 1 || end > 24)
        throw new ArgumentOutOfRangeException(
            nameof(end),
            "End must be between 1 and 24.");

    if (start >= end)
        throw new ArgumentException(
            "Start must be earlier than end.",
            nameof(start));

    Start = start;
    End = end;
}

    public bool Contains(int hour)
    {
        return hour >= Start && hour < End;
    }

    public override string ToString()
    {
        return $"{Display} ({HoursPerDay} hours)";
    }
}