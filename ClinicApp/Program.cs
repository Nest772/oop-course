using System;

namespace ClinicApp;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Testing Task 4 (Overloading, out parameters, null-operators) ===\n");

        Clinic clinic = new Clinic("Medical Clinic");

        clinic.Patients.Add(new Patient("Ivan", "Petrenko", new DateTime(1985, 5, 10), BloodType.APositive, "0501234567"));
        clinic.Patients.Add(new Patient("Olena", "Koval", new DateTime(1993, 8, 22), BloodType.BNegative, "0672345678"));
        clinic.Patients.Add(new Patient("Maksym", "Boiko", new DateTime(2010, 1, 15), BloodType.APositive, "0933456789"));

        clinic.Doctors.Add(new Doctor("Oleh", "Sydorenko", Speciality.Cardiology));
        clinic.Doctors.Add(new Doctor("Natalia", "Moroz", Speciality.Neurology));

        clinic.Appointments.Add(new Appointment(1, 1, new DateTime(2026, 5, 10, 10, 0, 0)));

        Console.WriteLine("--- Method Overloading: FindBySpeciality ---");
        Doctor[] docsByEnum = clinic.Doctors.FindBySpeciality(Speciality.Cardiology); 
        Doctor[] docsByString = clinic.Doctors.FindBySpeciality("cardio");            

        Console.WriteLine($"Found by enum Speciality.Cardiology: {docsByEnum.Length}");
        Console.WriteLine($"Found by string 'cardio': {docsByString.Length}");

        Console.WriteLine("\n--- Method Overloading: GetByDate ---");
        Appointment[] apps = clinic.Appointments.GetByDate(2026, 5, 10); 
        Console.WriteLine($"Appointments on 2026-05-10: {apps.Length}");

        Console.WriteLine("\n--- FindByBloodType ---");
        Patient[] aPositivePatients = clinic.Patients.FindByBloodType(BloodType.APositive);
        Console.WriteLine($"Patients with A+ blood type: {aPositivePatients.Length}");

        Console.WriteLine("\n--- TryFindById Pattern ---");
        if (clinic.Patients.TryFindById(1, out Patient? foundPatient))
        {
            Console.WriteLine($"Successfully found patient: {foundPatient.FullName}");
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }

        if (clinic.Doctors.TryFindById(99, out Doctor? foundDoctor))
        {
            Console.WriteLine($"Successfully found doctor: {foundDoctor.FullName}");
        }
        else
        {
            Console.WriteLine("Doctor with ID 99 not found.");
        }

        Console.WriteLine("\n--- Testing ?. and ?? Operators ---");
        
        string name1 = clinic.Patients.FindById(1)?.FullName ?? "Patient not found";
        Console.WriteLine($"ID 1: {name1}");

        string name2 = clinic.Patients.FindById(99)?.FullName ?? "Patient not found";
        Console.WriteLine($"ID 99: {name2}");
    }
}