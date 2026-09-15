using System;

public static class Task2
{
    public static void Run()
    {
        double price = double.Parse(Console.ReadLine());
        int quantity = int.Parse(Console.ReadLine());
        int sale = int.Parse(Console.ReadLine());

        double sum = price * quantity * (1 - (double)sale / 100);

        Console.WriteLine($"Sum: {sum:F2}");
    }
}
