using System;


Run();

void Run()
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

    string pressureStatus = findPressureStatus(systolic, diastolic);
    Console.WriteLine($"{systolic}/{diastolic} {pressureStatus}");
}


double calculateBMI(double weight, double height)
{
    return weight / (height * height);
}

string getBMICategory(double bmi)
{
    if (bmi < 18.5) return "not enough weight";
    if (bmi < 25.0) return "norm";
    if (bmi < 30.0) return "overweight";
    return "obese";
}

double calculateCost(double price, int quantity, int sale)
{
    return price * quantity * (1 - (double)sale / 100);
}

string getAgeCategory(int age)
{
    if (age <= 17) return "Child";
    if (age <= 59) return "Adult";
    return "Pensioner";
}

string findPressureStatus(int systolic, int diastolic)
{
    if (systolic < 120 && diastolic < 80) return "norm";
    if (systolic < 130 && diastolic < 80) return "high";
    if (systolic < 140 || diastolic < 90) return "stage 1 hypertension";
    return "stage 2 hypertension";
}