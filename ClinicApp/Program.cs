using System;

namespace ClinicApp;

internal class Program
{
    private static void Main(string[] args)
    {
        DoctorManager doctorManager = new DoctorManager();

        Doctor d1 = new Doctor("Oleg", "Sidorenko", "Cardiology", "LIC-001", "0441234567");
        
        Doctor d2 = new Doctor("Nataliya", "Moroz", "Neurology", "LIC-002", "0442345678");
        d2.WorkStartHour = 9;
        d2.WorkEndHour = 18;

        Doctor d3 = new Doctor("Andriy", "Vlasenko", "Pediatrics", "LIC-003", "0443456789");

        doctorManager.Add(d1);
        doctorManager.Add(d2);
        doctorManager.Add(d3);

        Console.WriteLine();
        RunDoctorMenu(doctorManager);
    }

    private static void RunDoctorMenu(DoctorManager manager)
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- MENU: DOCTORS ---");
            Console.WriteLine("1. Show all");
            Console.WriteLine("2. Add doctor");
            Console.WriteLine("3. Find by speciality");
            Console.WriteLine("4. Check availability by ID and hour");
            Console.WriteLine("5. Delete by ID");
            Console.WriteLine("6. Statistic");
            Console.WriteLine("0. Exit");
            Console.Write("Choose action: ");

            string input = Console.ReadLine()!;

            switch (input)
            {
                case "1":
                    Console.WriteLine();
                    manager.DisplayAll();
                    break;

                case "2":
                    AddDoctorWorkflow(manager);
                    break;

                case "3":
                    Console.Write("Enter speciality for search: ");
                    string searchSpec = Console.ReadLine()!;
                    Doctor[] found = manager.FindBySpeciality(searchSpec);

                    Console.WriteLine($"\nFound doctors: {found.Length}");
                    for (int i = 0; i < found.Length; i++)
                    {
                        Console.WriteLine(found[i].ToString());
                    }
                    break;

                case "4":
                    CheckAvailabilityWorkflow(manager);
                    break;

                case "5":
                    Console.Write("Enter doctor`s ID  for removal: ");
                    if (int.TryParse(Console.ReadLine()!, out int removeId))
                    {
                        bool removed = manager.Remove(removeId);
                        if (removed)
                        {
                            Console.WriteLine($"Doctor with ID [{removeId}] delted.");
                        }
                        else
                        {
                            Console.WriteLine($"Doctor with ID [{removeId}] not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong format of ID.");
                    }
                    break;

                case "6":
                    Console.WriteLine();
                    manager.DisplayStats();
                    break;

                case "0":
                    running = false;
                    break;

                default:
                    Console.WriteLine("Unknown command. Try again.");
                    break;
            }
        }
    }

    private static void AddDoctorWorkflow(DoctorManager manager)
    {
        Console.WriteLine("\n--- Adding new doctor ---");
        Console.Write("Name: ");
        string firstName = Console.ReadLine()!;

        Console.Write("Secondname: ");
        string lastName = Console.ReadLine()!;

        Console.Write("Speciality: ");
        string speciality = Console.ReadLine()!;

        Console.Write("Number of licensy: ");
        string license = Console.ReadLine()!;

        Console.Write("Phone: ");
        string phone = Console.ReadLine()!;

        Doctor newDoctor = new Doctor(firstName, lastName, speciality, license, phone);

        Console.Write("Working shift (0–23) [Enter = 8]: ");
        string startInput = Console.ReadLine()!;
        if (int.TryParse(startInput, out int startHour) && startHour >= 0 && startHour <= 23)
        {
            newDoctor.WorkStartHour = startHour;
        }

        Console.Write("Working shift end (0–23) [Enter = 17]: ");
        string endInput = Console.ReadLine()!;
        if (int.TryParse(endInput, out int endHour) && endHour >= 0 && endHour <= 23)
        {
            newDoctor.WorkEndHour = endHour;
        }

        manager.Add(newDoctor);
    }

    private static void CheckAvailabilityWorkflow(DoctorManager manager)
    {
        Console.Write("Enter doctors ID : ");
        if (int.TryParse(Console.ReadLine()!, out int id))
        {
            Doctor? doctor = manager.FindById(id);
            if (doctor == null)
            {
                Console.WriteLine($"Doctor with ID [{id}] not found.");
                return;
            }

            Console.Write("Enter hour to check (0–23): ");
            if (int.TryParse(Console.ReadLine()!, out int hour) && hour >= 0 && hour <= 23)
            {
                bool canAccept = doctor.CanAcceptAt(hour);
                string status = canAccept ? "available" : "not available";
                Console.WriteLine($"Doctor {doctor.FullName} at {hour:D2}:00 — {status}.");
            }
            else
            {
                Console.WriteLine("Wrong hour.");
            }
        }
        else
        {
            Console.WriteLine("Wrong format of ID.");
        }
    }
}