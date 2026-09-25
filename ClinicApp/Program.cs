using System;

namespace ClinicApp;

internal class Program
{
    private static void Main(string[] args)
    {
        PatientManager patientManager = new PatientManager();
        DoctorManager doctorManager = new DoctorManager();
        AppointmentManager appointmentManager = new AppointmentManager(patientManager, doctorManager);

        patientManager.Add(new Patient("Ivan", "Petrenko", new DateTime(1983, 5, 14), "A+", "0501234567"));
        patientManager.Add(new Patient("Olena", "Koval", new DateTime(1991, 11, 20), "B-", "0672345678"));
        patientManager.Add(new Patient("Maksym", "Boiko", new DateTime(2008, 3, 10), "O+", "0933456789"));

        doctorManager.Add(new Doctor("Oleh", "Sydorenko", "Cardiology", "LIC-001", "0441234567"));
        doctorManager.Add(new Doctor("Nataliia", "Moroz", "Neurology", "LIC-002", "0442345678"));
        doctorManager.Add(new Doctor("Andrii", "Vlasenko", "Pediatrics", "LIC-003", "0443456789"));

        appointmentManager.Book(1, 1, new DateTime(2026, 5, 9, 10, 0, 0), 30);
        appointmentManager.Book(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        appointmentManager.Book(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Console.Clear();
        RunMainMenu(patientManager, doctorManager, appointmentManager);
    }

    private static void RunMainMenu(PatientManager pManager, DoctorManager dManager, AppointmentManager aManager)
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n=== CLINIC MAIN MENU ===");
            Console.WriteLine("1. Patient Management");
            Console.WriteLine("2. Doctor Management");
            Console.WriteLine("3. Appointment Management");
            Console.WriteLine("0. Exit");
            Console.Write("Select an option: ");

            string input = Console.ReadLine()!;

            switch (input)
            {
                case "1":
                    RunPatientMenu(pManager);
                    break;
                case "2":
                    RunDoctorMenu(dManager);
                    break;
                case "3":
                    RunAppointmentMenu(aManager, pManager, dManager);
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

    private static void RunPatientMenu(PatientManager manager)
    {
        Console.WriteLine("\n--- MENU: PATIENTS ---");
        manager.DisplayAll();
    }

    private static void RunDoctorMenu(DoctorManager manager)
    {
        Console.WriteLine("\n--- MENU: DOCTORS ---");
        manager.DisplayAll();
    }

    private static void RunAppointmentMenu(AppointmentManager aManager, PatientManager pManager, DoctorManager dManager)
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
            Console.Write("Select an option: ");

            string input = Console.ReadLine()!;

            switch (input)
            {
                case "1":
                    BookWorkflow(aManager, pManager, dManager);
                    break;

                case "2":
                    Console.WriteLine("\n=== Upcoming Appointments ===");
                    aManager.DisplayList(aManager.GetUpcoming());
                    break;

                case "3":
                    Console.Write("Enter Patient ID: ");
                    if (int.TryParse(Console.ReadLine()!, out int pId))
                    {
                        Console.WriteLine($"\n=== Appointments for Patient #{pId} ===");
                        aManager.DisplayList(aManager.GetByPatient(pId));
                    }
                    break;

                case "4":
                    Console.Write("Enter Doctor ID: ");
                    if (int.TryParse(Console.ReadLine()!, out int dId))
                    {
                        Console.WriteLine($"\n=== Appointments for Doctor #{dId} ===");
                        aManager.DisplayList(aManager.GetByDoctor(dId));
                    }
                    break;

                case "5":
                    Console.Write("Enter Appointment ID to cancel: ");
                    if (int.TryParse(Console.ReadLine()!, out int cancelId))
                    {
                        Console.Write("Enter cancellation reason (optional): ");
                        string reason = Console.ReadLine()!;
                        aManager.Cancel(cancelId, reason);
                    }
                    break;

                case "6":
                    Console.Write("Enter Appointment ID to complete: ");
                    if (int.TryParse(Console.ReadLine()!, out int completeId))
                    {
                        aManager.Complete(completeId);
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

    private static void BookWorkflow(AppointmentManager aManager, PatientManager pManager, DoctorManager dManager)
    {
        Console.WriteLine("\n--- Book Appointment ---");

        pManager.DisplayAll();
        dManager.DisplayAll();

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

        aManager.Book(patientId, doctorId, scheduledAt, duration);
    }
}