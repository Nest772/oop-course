using System;

public static class Task4
{
    public static void Run()
    {
        int systolicpressure = int.Parse(Console.ReadLine());
        int dialosticpressure = int.Parse(Console.ReadLine());

        if (systolicpressure < 120 && dialosticpressure < 80)
        {
            Console.WriteLine($"{systolicpressure}/{dialosticpressure} norm");
        }
        else if (systolicpressure < 130 && dialosticpressure < 80)
        {
            Console.WriteLine($"{systolicpressure}/{dialosticpressure} high");
        }
        else if (systolicpressure < 140 || dialosticpressure < 90)
        {
            Console.WriteLine($"{systolicpressure}/{dialosticpressure} stage 1 hypertension");
        }
        else
        {
            Console.WriteLine($"{systolicpressure}/{dialosticpressure} stage 2 hypertension");
        }
    }
}
