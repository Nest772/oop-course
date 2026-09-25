using System;

namespace ClinicApp;

internal class Program
{
    private static void Main(string[] args)
    {
        PatientManager manager = new PatientManager();

        manager.Add(new Patient("Ivan", "Petrenko", new DateTime(1983, 5, 14), "A+", "0501234567"));
        manager.Add(new Patient("Olena", "Koval", new DateTime(1991, 11, 20), "B-", "0672345678"));
        manager.Add(new Patient("Maxim", "Boyko", new DateTime(2008, 3, 10), "O+", "0933456789"));
        manager.Add(new Patient("Maria", "Tkach"));

        Console.WriteLine();
        RunPatientMenu(manager);
    }

    private static void RunPatientMenu(PatientManager manager)
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n--- MENU: PATIENTS ---");
            Console.WriteLine("1. Show all");
            Console.WriteLine("2. Add patient");
            Console.WriteLine("3. Find by name/secondname");
            Console.WriteLine("4. Delete by ID");
            Console.WriteLine("5. Statistic");
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
                    AddPatientWorkflow(manager);
                    break;

                case "3":
                    Console.Write("Enter name or secondname to search: ");
                    string searchName = Console.ReadLine()!;
                    Patient[] found = manager.FindByName(searchName);
                    
                    Console.WriteLine($"\nFound patients: {found.Length}");
                    for (int i = 0; i < found.Length; i++)
                    {
                        Console.WriteLine(found[i].ToString());
                    }
                    break;

                case "4":
                    Console.Write("Enter ID for removal: ");
                    if (int.TryParse(Console.ReadLine()!, out int removeId))
                    {
                        bool removed = manager.Remove(removeId);
                        if (removed)
                        {
                            Console.WriteLine($"Patient with ID [{removeId}] deleted.");
                        }
                        else
                        {
                            Console.WriteLine($"Patient with ID [{removeId}] not found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong format of ID.");
                    }
                    break;

                case "5":
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

    private static void AddPatientWorkflow(PatientManager manager)
    {
        Console.WriteLine("\n--- Add new patient ---");
        Console.Write("Name: ");
        string firstName = Console.ReadLine()!;

        Console.Write("Secondname: ");
        string lastName = Console.ReadLine()!;

        Console.Write("Date of birth (yyyy-mm-dd) or Enter by default: ");
        string dobInput = Console.ReadLine()!;

        if (dobInput == string.Empty)
        {
            manager.Add(new Patient(firstName, lastName));
        }
        else if (DateTime.TryParse(dobInput, out DateTime dob))
        {
            Console.Write("Blood type  (example, A+): ");
            string blood = Console.ReadLine()!;

            Console.Write("Phone: ");
            string phone = Console.ReadLine()!;

            manager.Add(new Patient(firstName, lastName, dob, blood, phone));
        }
        else
        {
            Console.WriteLine("Wrong date of birth. Cancellation of making.");
        }
    }
}