using System;
using System.Globalization;
using System.Threading;

namespace Lab02;

public class Task4
{
    public static void Run()
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

        int N = int.Parse(Console.ReadLine());
        int M = int.Parse(Console.ReadLine());

        int[,] matrix = new int[N, M];


        for (int i = 0; i < N; i++)
        {
            string inputLine = Console.ReadLine();
            string[] parts = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int j = 0; j < M; j++)
            {
                matrix[i, j] = int.Parse(parts[j]);
            }
        }


        int maxVal = matrix[0, 0];
        int maxRow = 0;
        int maxCol = 0;


        for (int i = 0; i < N; i++)
        {
            int rowSum = 0;
            for (int j = 0; j < M; j++)
            {
                rowSum += matrix[i, j];

                if (matrix[i, j] > maxVal)
                {
                    maxVal = matrix[i, j];
                    maxRow = i;
                    maxCol = j;
                }
            }
            Console.WriteLine($"Doctor {i + 1}: {rowSum} appointments");
        }

        int[] daySums = new int[M];
        for (int j = 0; j < M; j++)
        {
            for (int i = 0; i < N; i++)
            {
                daySums[j] += matrix[i, j];
            }
        }
        Console.WriteLine($"By days: {string.Join(", ", daySums)}");

        Console.WriteLine($"\nMaximum: {maxVal} (Doctor {maxRow + 1}, Day {maxCol + 1})");
    }
}
