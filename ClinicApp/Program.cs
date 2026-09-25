using System;

namespace ClinicApp;

internal class Program
{
    private static void Main(string[] args)
    {

        Patient p1 = new Patient("Ivan", "Petrenko", new DateTime(1983, 5, 14), "A+", "0501234567");
        Patient p2 = new Patient("Olena", "Koval", new DateTime(1991, 11, 20), "B-", "0672345678");
        Patient p3 = new Patient("Maxim", "Boyko", new DateTime(2008, 3, 10), "O+", "0933456789");

        Patient p4 = new Patient();

        Patient p5 = new Patient("Maria", "Tkach");

        Patient[] patients = new Patient[5] { p1, p2, p3, p4, p5 };

        for (int i = 0; i < patients.Length; i++)
        {
            if (patients[i] != null)
            {
                Console.WriteLine(patients[i].ToString());
            }
        }
    }
}
