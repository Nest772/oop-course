using System;

namespace ClinicApp;

internal class Program
{
    private static void Main(string[] args)
    {
        Appointment app1 = new Appointment(1, 1, new DateTime(2026, 5, 9, 10, 0, 0), 30);
        Appointment app2 = new Appointment(2, 2, new DateTime(2026, 5, 9, 11, 0, 0), 45);
        Appointment app3 = new Appointment(3, 3, new DateTime(2026, 5, 10, 9, 0, 0), 20);

        Appointment[] appointments = new Appointment[3] { app1, app2, app3 };

        Console.WriteLine("=== Initial list of appointments ===");
        for (int i = 0; i < appointments.Length; i++)
        {
            if (appointments[i] != null)
            {
                Console.WriteLine(appointments[i].ToString());
            }
        }

        Console.WriteLine("\n--- Change of status ---");

        bool isCancelled = app1.Cancel("Patient didnt came");
        Console.WriteLine($"Cancelleation of reception [1]: {(isCancelled ? "Successfully" : "Rejected")}");

        bool isCompleted = app2.Complete();
        Console.WriteLine($"Finish of reception [2]: {(isCompleted ? "Successfully" : "Rejected")}");

        bool reCancelResult = app1.Cancel("Second try of cancellation");
        Console.WriteLine($"Second cancel [1] (from status Cancelled): {(reCancelResult ? "Successfully" : "Rejected (status not Scheduled)")}");

        Console.WriteLine("\n=== Updated list of appointments ===");
        for (int i = 0; i < appointments.Length; i++)
        {
            if (appointments[i] != null)
            {
                Console.WriteLine(appointments[i].ToString());
            }
        }
    }
}