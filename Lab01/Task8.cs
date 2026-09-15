using System;

public static class Task8
{
    public static void Run()
    {
       
        double weight = double.Parse(Console.ReadLine());
        double height = double.Parse(Console.ReadLine());
        double price = double.Parse(Console.ReadLine());
        int quantity = int.Parse(Console.ReadLine());
        int sale = int.Parse(Console.ReadLine());
        int birthdate = int.Parse(Console.ReadLine());
        int systolic = int.Parse(Console.ReadLine());
        int diastolic = int.Parse(Console.ReadLine());

        
        double bmi = calculateBMI(weight, height);
        string bmiCategory = getBMICategory(bmi);
        Console.WriteLine($"BMI: {bmi:F2} kg/m^2 ({bmiCategory})");

        double totalCost = calculateCost(price, quantity, sale);
        Console.WriteLine($"Sum: {totalCost:F2}");

        int age = 2026 - birthdate;
        string ageCategory = getAgeCategory(age);
        Console.WriteLine(age);
        Console.WriteLine(ageCategory);

        
        string pressureStatus = getPressureStatus(systolic, diastolic);
        Console.WriteLine($"{systolic}/{diastolic} {pressureStatus}");
    }

    

    public static double calculateBMI(double weight, double height)
    {
        return weight / (height * height);
    }

    public static string getBMICategory(double bmi)
    {
        if (bmi < 18.5) return "недостатня вага";
        if (bmi < 25.0) return "норма";
        if (bmi < 30.0) return "надмірна вага";
        return "ожиріння";
    }

    public static double calculateCost(double price, int quantity, int sale)
    {
        return price * quantity * (1 - (double)sale / 100);
    }

    public static string getAgeCategory(int age)
    {
        if (age <= 17) return "Child";
        if (age <= 59) return "Adult";
        return "Pensioner";
    }

    public static string getPressureStatus(int systolic, int diastolic)
    {
        if (systolic < 120 && diastolic < 80) return "norm";
        if (systolic < 130 && diastolic < 80) return "high";
        if (systolic < 140 || diastolic < 90) return "stage 1 hypertension";
        return "stage 2 hypertension";
    }
}
