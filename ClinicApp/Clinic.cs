using System;

namespace ClinicApp;

public class Clinic
{
    public string Name { get; }
    public PatientManager Patients { get; }
    public DoctorManager Doctors { get; }
    public AppointmentManager Appointments { get; }

    public Clinic(string name)
    {
        Name = name;
        Patients = new PatientManager();
        Doctors = new DoctorManager();
        
        Appointments = new AppointmentManager(Patients, Doctors);
    }

    public void DisplaySchedule(DateTime date)
    {
        Console.WriteLine($"\n=== Schedule on {date:dd.MM.yyyy} ===");
        Appointment[] dailyAppointments = Appointments.GetByDate(date);
        Appointments.DisplayList(dailyAppointments);
    }

    public void GenerateReport()
    {
        Console.WriteLine("\n╔══════════════════════════════════════════════╗");
        Console.WriteLine($"║  Report — {Name}");
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine($"║  Patient:          {Patients.Count}");
        Console.WriteLine($"║  Doctors:            {Doctors.Count}");

        Appointment[] upcoming = Appointments.GetUpcoming();
        Console.WriteLine($"║  Future appointments:  {upcoming.Length}");
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine("║  Workload of doctors (upcoming appointments):");

        Doctor[] allDoctors = Doctors.GetAll();
        for (int i = 0; i < allDoctors.Length; i++)
        {
            int count = 0;
            for (int j = 0; j < upcoming.Length; j++)
            {
                if (upcoming[j].DoctorId == allDoctors[i].Id)
                {
                    count++;
                }
            }
            Console.WriteLine($"║    {allDoctors[i].FullName} ({allDoctors[i].Speciality}): {count} appointments");
        }
        Console.WriteLine("╚══════════════════════════════════════════════╝\n");
    }
}