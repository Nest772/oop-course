using System;
using System.Globalization;
using System.Threading;

namespace Lab02;

public class Task8
{
    public static void Run()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        int D = int.Parse(Console.ReadLine());
        int W = int.Parse(Console.ReadLine());

        int[,,] data = new int[D, W, 2];

        for (int d = 0; d < D; d++)
        {
            for (int w = 0; w < W; w++)
            {
                for (int s = 0; s < 2; s++)
                {
                    data[d, w, s] = int.Parse(Console.ReadLine());
                }
            }
        }


        int[] depTotals = new int[D];
        int maxDepIdx = 0;

        for (int d = 0; d < D; d++)
        {
            Console.WriteLine($"Department{d + 1}:");
            int depTotal = 0;

            for (int w = 0; w < W; w++)
            {
                int morning = data[d, w, 0];
                int evening = data[d, w, 1];
                int weekTotal = morning + evening;

                depTotal += weekTotal;

                Console.WriteLine($"week {w + 1}: morning{morning}, evening {evening} -> together {weekTotal}");
            }

            depTotals[d] = depTotal;
            Console.WriteLine($"  Together: {depTotal} patients");


            if (depTotals[d] > depTotals[maxDepIdx])
            {
                maxDepIdx = d;
            }
        }


        Console.WriteLine($"Busiest: Department {maxDepIdx + 1} ({depTotals[maxDepIdx]} patients)");
    }
}
