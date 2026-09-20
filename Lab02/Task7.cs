using System;

System.Threading.Thread.CurrentThread.CurrentCulture =
System.Globalization.CultureInfo.InvariantCulture;


public static class Task7
{
    public static void Run()
    {
        int N = int.Parse(Console.ReadLine());


        string[] names = new string[N];
        double[] bmis = new double[N];


        for (int i = 0; i < N; i++)
        {
            names[i] = Console.ReadLine();
            bmis[i] = double.Parse(Console.ReadLine());
        }


        for (int i = 0; i < N - 1; i++)
        {
            for (int j = 0; j < N - 1 - i; j++)
            {
                if (bmis[j] < bmis[j + 1])
                {
                    (bmis[j], bmis[j + 1]) = (bmis[j + 1], bmis[j]);

                    (names[j], names[j + 1]) = (names[j + 1], names[j]);
                }
            }
        }

        Console.WriteLine("=== BMI RATING ===");
        for (int i = 0; i < N; i++)
        {
            Console.WriteLine($"#{i + 1} {names[i]}: {bmis[i]:F2}");
        }
    }
}
