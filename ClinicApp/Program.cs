using System;

namespace ClinicApp;

internal class Program
{
    private static void Main(string[] args)
    {
        Clinic clinic = new Clinic("Medical Clinic");

        clinic.Patients.Add(new Patient("Ivan", "Petrenko", new DateTime(1983, 5, 14), "A+", "0501234567"));
        clinic.Patients.Add(new Patient("Olena", "Koval", new DateTime(1991, 11, 20), "B-", "0672345678"));
        clinic.Patients.Add(new Patient("Maksym", "Boiko", new DateTime(2008, 3, 10), "O+", "0933456789"));

        clinic.Doctors.Add(new Doctor("Oleh", "Sydorenko", "Cardiology", "LIC-001", "0441234567"));
        clinic.Doctors.Add(new Doctor("Nataliia", "Moroz", "Neurology", "LIC-002", "0442345678"));
        clinic.Doctors.Add(new Doctor("Andrii", "Vlasenko", "Pediatrics", "LIC-003", "0443456789"));

        clinic.Appointments.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0), 30);
        clinic.Appointments.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        clinic.Appointments.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.Clear();
        RunMainMenu(clinic);
    }

    private static void RunMainMenu(Clinic clinic)
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine($"\n=== MAIN MENU: {clinic.Name} ===");
            Console.WriteLine("1. Patient Management");
            Console.WriteLine("2. Doctor Management");
            Console.WriteLine("3. Appointment Management");
            Console.WriteLine("4. View schedule for a date");
            Console.WriteLine("5. Generate report");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            string input = Console.ReadLine()!;

            switch (input)
            {
                case "1":
                    RunPatientMenu(clinic);
                    break;
                case "2":
                    RunDoctorMenu(clinic);
                    break;
                case "3":
                    RunAppointmentMenu(clinic);
                    break;
                case "4":
                    Console.Write("Enter date (yyyy-MM-dd): ");
                    if (DateTime.TryParse(Console.ReadLine()!, out DateTime date))
                    {
                        clinic.DisplaySchedule(date);
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format.");
                    }
                    break;
                case "5":
                    clinic.GenerateReport();
                    break;
                case "0":
                    running = false;
                    break;
                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }

    private static void RunPatientMenu(Clinic clinic)
    {
        Console.WriteLine("\n--- MENU: PATIENTS ---");
        clinic.Patients.DisplayAll();
    }

    private static void RunDoctorMenu(Clinic clinic)
    {
        Console.WriteLine("\n--- MENU: DOCTORS ---");
        clinic.Doctors.DisplayAll();
    }

    private static void RunAppointmentMenu(Clinic clinic)
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- MENU: APPOINTMENTS ---");
            Console.WriteLine("1. Book an appointment");
            Console.WriteLine("2. Show all upcoming appointments");
            Console.WriteLine("3. Search by patient");
            Console.WriteLine("4. Search by doctor");
            Console.WriteLine("5. Cancel appointment");
            Console.WriteLine("6. Complete appointment");
            Console.WriteLine("0. Back to main menu");
            Console.Write("Select an action: ");

            string input = Console.ReadLine()!;

            switch (input)
            {
                case "1":
                    BookWorkflow(clinic);
                    break;

                case "2":
                    Console.WriteLine("\n=== Upcoming Appointments ===");
                    clinic.Appointments.DisplayList(clinic.Appointments.GetUpcoming());
                    break;

                case "3":
                    Console.Write("Enter Patient ID: ");
                    if (int.TryParse(Console.ReadLine()!, out int pId))
                    {
                        Console.WriteLine($"\n=== Appointments for Patient #{pId} ===");
                        clinic.Appointments.DisplayList(clinic.Appointments.GetByPatient(pId));
                    }
                    break;

                case "4":
                    Console.Write("Enter Doctor ID: ");
                    if (int.TryParse(Console.ReadLine()!, out int dId))
                    {
                        Console.WriteLine($"\n=== Appointments for Doctor #{dId} ===");
                        clinic.Appointments.DisplayList(clinic.Appointments.GetByDoctor(dId));
                    }
                    break;

                case "5":
                    Console.Write("Enter Appointment ID to cancel: ");
                    if (int.TryParse(Console.ReadLine()!, out int cancelId))
                    {
                        Console.Write("Enter cancellation reason (optional): ");
                        string reason = Console.ReadLine()!;
                        clinic.Appointments.Cancel(cancelId, reason);
                    }
                    break;

                case "6":
                    Console.Write("Enter Appointment ID to complete: ");
                    if (int.TryParse(Console.ReadLine()!, out int completeId))
                    {
                        clinic.Appointments.Complete(completeId);
                    }
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Unknown command.");
                    break;
            }
        }
    }

    private static void BookWorkflow(Clinic clinic)
    {
        Console.WriteLine("\n--- Book Appointment ---");

        // Display lists through the single clinic object
        clinic.Patients.DisplayAll();
        clinic.Doctors.DisplayAll();

        Console.Write("Enter Patient ID: ");
        if (!int.TryParse(Console.ReadLine()!, out int patientId))
        {
            Console.WriteLine("Invalid Patient ID.");
            return;
        }

        Console.Write("Enter Doctor ID: ");
        if (!int.TryParse(Console.ReadLine()!, out int doctorId))
        {
            Console.WriteLine("Invalid Doctor ID.");
            return;
        }

        Console.Write("Enter date and time (yyyy-MM-dd HH:mm): ");
        if (!DateTime.TryParse(Console.ReadLine()!, out DateTime scheduledAt))
        {
            Console.WriteLine("Invalid date and time format.");
            return;
        }

        Console.Write("Duration in minutes [Enter = 30]: ");
        string durationInput = Console.ReadLine()!;
        int duration = 30;
        if (durationInput.Length > 0)
        {
            int.TryParse(durationInput, out duration);
        }

        clinic.Appointments.Book(patientId, doctorId, scheduledAt, duration);
    }
}