using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Clinic clinic = new Clinic("Medical Clinic");

        clinic.Patients.Add(new Patient("Ivan", "Petrenko", new DateTime(1985, 5, 10), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Olena", "Koval", new DateTime(1993, 8, 22), BloodType.BNegative, "0672345678"));
        clinic.Patients.Add(new Patient("Maksym", "Boiko", new DateTime(2010, 1, 15), BloodType.OPositive, "0933456789"));
        clinic.Patients.Add(new Patient("Maria", "Tkach")); 

        Doctor d1 = new Doctor("Oleh", "Sydorenko", Speciality.Cardiology);
        Doctor d2 = new Doctor("Natalia", "Moroz", Speciality.Neurology);
        Doctor d3 = new Doctor("Andriy", "Vlasenko", Speciality.Pediatrics);

        clinic.Doctors.Add(d1);
        clinic.Doctors.Add(d2);
        clinic.Doctors.Add(d3);

        Appointment app1 = new Appointment(1, 1, new DateTime(2026, 5, 9, 10, 0, 0));
        Appointment app2 = new Appointment(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);

        Console.WriteLine("\n=== Testing Task 1 ===");

        Console.WriteLine("\n--- Patients Check  ---");
        foreach (Patient p in clinic.Patients.GetAll())
        {
            Console.WriteLine(p);
        }

        Console.WriteLine("\n--- Cardiologists Search ---");
        Doctor[] cardiologists = clinic.Doctors.FindBySpeciality(Speciality.Cardiology);
        foreach (Doctor doc in cardiologists)
        {
            Console.WriteLine(doc);
        }

        Console.WriteLine("\n--- Appointment Status Check ---");
        Console.WriteLine($"Initial status of appointment #1: {app1.Status}");
        app1.Complete();
        Console.WriteLine($"Status after completion: {app1.Status}");

        app2.Cancel("Patient cancelled the visit");
        Console.WriteLine($"Status after cancellation: {app2.Status} | Reason: {app2.Notes}");
    }
}