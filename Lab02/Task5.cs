using System;

System.Threading.Thread.CurrentThread.CurrentCulture =
System.Globalization.CultureInfo.InvariantCulture;


public static class Task5
{
    public static void Run ()
    {
        int N = int.Parse(Console.ReadLine());

        int[,] matrix = new int[N, N];


        for (int i = 0; i < N; i++)
        {
            string inputLine = Console.ReadLine();
            string[] parts = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int j = 0; j < N; j++)
            {
                matrix[i, j] = int.Parse(parts[j]);
            }
        }


        int[] mainDiag = new int[N];
        int[] antiDiag = new int[N];
        int mainDSum = 0;
        int antiDSum = 0;


        for (int i = 0; i < N; i++)
        {
            mainDiag[i] = matrix[i, i];
            mainDSum += matrix[i, i];

            antiDiag[i] = matrix[i, N - 1 - i];
            antiDSum += matrix[i, N - 1 - i];

        }


        Console.WriteLine($"Main diagonal: {string.Join(", ", mainDiag)} (Sum = {mainDSum})");

        Console.WriteLine($"Anti diagonal: {string.Join(", ", antiDiag)} (Sum = {antiDSum})");
    }
}