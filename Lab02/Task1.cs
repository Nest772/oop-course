using System;

System.Threading.Thread.CurrentThread.CurrentCulture =
System.Globalization.CultureInfo.InvariantCulture;

public static class Task1
{

    public static void Run()
    {
        int N = int.Parse(Console.ReadLine());
        double[] weights = new double[N];

        for (int i = 0; i < N; i++)
        {
            weights[i] = double.Parse(Console.ReadLine());
        }


        double sum = 0;
        double min = weights[0];
        double max = weights[0];

        foreach (double weight in weights)
        {
            sum += weight;
            if (weight < min) min = weight;
            if (weight > max) max = weight;
        }

        double average = sum / N;


        int countAboveAverage = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            if (weights[i] > average)
            {
                countAboveAverage++;
            }
        }



        Console.WriteLine($"Amount: {N}     / Average weight: {average:F1} kg / Min/Max:{min:F1}/{max:F1} kg / Above average:{countAboveAverage} from {N}");
    }
}