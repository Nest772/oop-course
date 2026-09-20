using System;
using System.Globalization;
using System.Threading;

namespace Lab02;

public class Task3
{
    public static void Run()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        string[] days = { "Md", "Td", "Wd", "Thd", "Fd", "Std", "Sd" };
        int[] counts = new int[7];


        for (int i = 0; i < 7; i++)
        {
            counts[i] = int.Parse(Console.ReadLine());
        }


        int total = 0;
        int maxIdx = 0;
        int minIdx = 0;

        for (int i = 0; i < 7; i++)
        {
            total += counts[i];

            if (counts[i] > counts[maxIdx])
            {
                maxIdx = i;
            }

            if (counts[i] < counts[minIdx])
            {
                minIdx = i;
            }
        }


        Console.WriteLine($"{"Day",-10} : {"Patients",-10}");


        for (int i = 0; i < 7; i++)
        {
            Console.WriteLine($"{days[i],-10} : {counts[i],-10}");
        }




        Console.WriteLine($"Overall: {total}");
        Console.WriteLine($"Busiest: {days[maxIdx]} ({counts[maxIdx]})");
        Console.WriteLine($"Quietest: {days[minIdx]} ({counts[minIdx]})");
    }
}($"Busiest: {days[maxIdx]} ({counts[maxIdx]})");
        Console.WriteLine($"Quietest: {days[minIdx]} ({counts[minIdx]})");
    }
}
