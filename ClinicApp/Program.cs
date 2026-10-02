using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Testing Task 2 ===\n");

        WorkSchedule morning = new WorkSchedule(8, 16);
        WorkSchedule evening = new WorkSchedule(14, 22);

        Console.WriteLine($"Morning schedule: {morning}");
        Console.WriteLine($"Is morning schedule active now? {morning.IsNow}");
        Console.WriteLine($"Does morning schedule accept at 10:00? {morning.Contains(10)}");
        Console.WriteLine($"Does morning schedule accept at 17:00? {morning.Contains(17)}");

        Console.WriteLine("\n--- Value Type Copying Experiment ---");
        WorkSchedule originalSchedule = new WorkSchedule(9, 17);
        WorkSchedule copiedSchedule = originalSchedule; 

        Console.WriteLine($"Original: {originalSchedule}");
        Console.WriteLine($"Copied:   {copiedSchedule}");

        copiedSchedule = new WorkSchedule(10, 19);
        Console.WriteLine("\nAfter changing copiedSchedule variable:");
        Console.WriteLine($"Original (unchanged): {originalSchedule}");
        Console.WriteLine($"Copied (new value):   {copiedSchedule}");

        Console.WriteLine("\n--- Doctors with WorkSchedule ---");
        Doctor d1 = new Doctor("Oleh", "Sydorenko", Speciality.Cardiology, morning);
        Doctor d2 = new Doctor("Natalia", "Moroz", Speciality.Neurology, evening);

        Console.WriteLine(d1);
        Console.WriteLine(d2);
    }
}