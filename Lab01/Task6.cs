using System;


int cardNumber = int.Parse(Console.ReadLine());

string department = (cardNumber % 10) switch
{
    0 or 1 => "general therapy",
    2 or 3=> " surgery",
    4 or 5 => " cardiology",
    6 or 7 => " neurology",
    8 or 9 => " ophthalmology",
    _ => " incorret",
};

string discount = (cardNumber % 2 == 0) ? "Yes" : "No";
string checkup = (cardNumber % 3 == 0 || cardNumber % 10 <= 5) ? "Yes" : "No";

Console.WriteLine($"department: {department}");
Console.WriteLine($"concession map: {discount}");
Console.WriteLine($"routine checkup: {checkup}");