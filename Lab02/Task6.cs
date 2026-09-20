using System;

System.Threading.Thread.CurrentThread.CurrentCulture =
System.Globalization.CultureInfo.InvariantCulture;



static public class Task6
{
    static public void Run()
    {
        int N = int.Parse(Console.ReadLine());


        int[][] doctors = new int[N][];

        for (int i = 0; i < N; i++)
        {
            int M = int.Parse(Console.ReadLine());

            doctors[i] = new int[M];

            for (int j = 0; j < M; j++)
            {
                doctors[i][j] = int.Parse(Console.ReadLine());
            }
        }

        int maxSum = -1;
        int maxDocIndex = -1;

        for (int i = 0; i < N; i++)
        {
            int M = doctors[i].Length;
            int sum = 0;

            for (int j = 0; j < M; j++)
            {
                sum += doctors[i][j];
            }

            double average = (double)sum / M;

            if (sum > maxSum)
            {
                maxSum = sum;
                maxDocIndex = i;
            }

            string word = "Appointments";
            if (M % 10 == 1 && M % 100 != 11) word = "reception";
            else if (M % 10 >= 2 && M % 10 <= 4 && (M % 100 < 10 || M % 100 >= 20)) word = "receptions";

            Console.WriteLine($"Doctor {i + 1}: {M} {word}, Sum={sum} grn, average={average:F2} grn");
        }

        Console.WriteLine($"Biggest income: Doctor {maxDocIndex + 1} ({maxSum} grn");
    }
}