using System;

namespace ClinicApp;

internal class Program
{
    private static void Main(string[] args)
    {

        Doctor d1 = new Doctor("Oleh", "Sydorenko", "Cardiology", "LIC-001", "0441234567");
        Doctor d2 = new Doctor("Natalia", "Moroz", "Neurology", "LIC-002", "0442345678");

        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;

        Doctor d3 = new Doctor("Andriy", "Vlasenko", "Pediatrics");

        Doctor d4 = new Doctor();

        Doctor[] doctors = new Doctor[4] { d1, d2, d3, d4 };

        Console.WriteLine($"Current time: {DateTime.Now:HH:mm}\n");

        for (int i = 0; i < doctors.Length; i++)
        {
            if (doctors[i] != null)
            {
                Console.WriteLine(doctors[i].ToString());
            }
        }

        int testHour = 14;
        Console.WriteLine($"\n--- Availability check at {testHour:D2}:00 ---");
        for (int i = 0; i < doctors.Length; i++)
        {
            if (doctors[i] != null)
            {
                bool canAccept = doctors[i].CanAcceptAt(testHour);
                string availability = canAccept ? "Available" : "Not available";
                Console.WriteLine($"Dr. {doctors[i].FullName} ({doctors[i].Speciality}): {availability}");
            }
        }
    }
}