using System;

public static class Task7
{
    public static void Run()
    {
        
        int N = int.Parse(Console.ReadLine());
        decimal[] prices = new decimal[N];

        for (int i = 0; i < N; i++)
        {
            prices[i] = decimal.Parse(Console.ReadLine());
        }

        
        decimal sum = 0;
        decimal min = prices[0];
        decimal max = prices[0];

        foreach (decimal price in prices)
        {
            sum += price;
            if (price < min) min = price;
            if (price > max) max = price;
        }

        decimal average = sum / N;

        
        int countAboveAverage = 0;
        for (int i = 0; i < prices.Length; i++)
        {
            if (prices[i] > average)
            {
                countAboveAverage++;
            }
        }

        
        int index = 0;
        int expensiveIndex = -1;

        while (index < prices.Length)
        {
            if (prices[index] > 1000)
            {
                expensiveIndex = index;
                break;
            }
            index++;
        }

        
        Console.WriteLine("=== Appointment report ===");
        Console.WriteLine($"number of appointments: {N}");
        Console.WriteLine($"overall sum: {sum:F2}");
        Console.WriteLine($"Average: {average:F2}");
        Console.WriteLine($"Minimum: {min:F2}");
        Console.WriteLine($"Maximum: {max:F2}");
        Console.WriteLine($"Above Average: {countAboveAverage}");

        if (expensiveIndex != -1)
        {
            
            Console.WriteLine($"First > 1000: №{expensiveIndex + 1} ({prices[expensiveIndex]:F2})");
        }
        else
        {
            Console.WriteLine("First > 1000: none");
        }
    }
}
