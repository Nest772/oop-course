using System;

namespace ClinicApp;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("=== GrowablePatientManager Test ===");
        Console.WriteLine("Adding patients one by one...");

        GrowablePatientManager manager = new GrowablePatientManager();

        for (int i = 1; i <= 20; i++)
        {
            Patient p = new Patient($"Test", $"Patient{i}", new DateTime(2000, 1, 1), "O+", $"05000000{i:D2}");
            manager.Add(p);
        }

        Console.WriteLine("\nSearch test:");
        Patient? found = manager.FindById(10);
        if (found != null)
        {
            Console.WriteLine($"  FindById(10) → {found.FullName}");
        }
        else
        {
            Console.WriteLine("  FindById(10) → not found");
        }

        Patient? notFound = manager.FindById(99);
        if (notFound != null)
        {
            Console.WriteLine($"  FindById(99) → {notFound.FullName}");
        }
        else
        {
            Console.WriteLine("  FindById(99) → not found");
        }

        Console.WriteLine("\nComparison:");
        Console.WriteLine("  PatientManager:          100 slots (fixed)");
        Console.WriteLine($"  GrowablePatientManager:  {manager.Capacity} slots (will grow as needed)");
    }
}