using System;

public static class Task3
{
    public static void Run()
    {
        int birthdate = int.Parse(Console.ReadLine());

        double age = 2026 - birthdate;

        Console.WriteLine(age);

        if (age <= 17)
        {
            Console.WriteLine("Child");
        }
        else if (age <= 59)
        {
            Console.WriteLine("Adult");
        }
        else
        {
            Console.WriteLine("Pensioner");
        }
    }
}
