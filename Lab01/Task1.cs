using System;

double weight = double.Parse(Console.ReadLine());
double height = double.Parse(Console.ReadLine());

double bmi = weight / (height * height);

Console.WriteLine($"BMI: {bmi:F} kg/m^2");