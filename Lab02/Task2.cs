using System;
using System.Globalization;
using System.Threading;

namespace Lab02;

public class Task2
{
    public static void Run()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        
        int N = int.Parse(Console.ReadLine());
        int[] price = new int[N];

        
        for (int i = 0; i < N; i++)
        {
            price[i] = int.Parse(Console.ReadLine());
        }

        string before = string.Join(" ", price);
        Console.WriteLine($"Before: {before}");

        for (int i = 0; i < N - 1; i++)
        {
            for (int j = 0; j < N - 1 - i; j++)
            {
                if (price[j] > price[j + 1])
                {
                    (price[j], price[j + 1]) = (price[j + 1], price[j]);
                }
            }
        }

        string after = string.Join(" ", price);
        Console.WriteLine($"After: {after}");

        Console.WriteLine($"the Cheapest: {price[0]} grn");
        Console.WriteLine($"The most expensive: {price[^1]} grn");
    }
}

        Console.WriteLine($"the Cheapest: {price[0]} grn");
        Console.WriteLine($"The most expensive: {price[^1]} grn");
    }
}
