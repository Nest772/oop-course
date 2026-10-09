using System;
using System.Linq;

namespace ClinicApp;

public static class ClinicFormatter
{
    public static string FormatBloodType(BloodType bt) => bt switch
    {
        BloodType.APositive => "A+",
        BloodType.ANegative => "A-",
        BloodType.BPositive => "B+",
        BloodType.BNegative => "B-",
        BloodType.ABPositive => "AB+",
        BloodType.ABNegative => "AB-",
        BloodType.OPositive => "O+",
        BloodType.ONegative => "O-",
        _ => "Unknown"
    };

    public static string FormatSpeciality(Speciality s) => s switch
    {
        Speciality.General => "General practice",
        Speciality.Cardiology => "Cardiology",
        Speciality.Neurology => "Neurology",
        Speciality.Pediatrics => "Pediatrics",
        Speciality.Surgery => "Surgery",
        Speciality.Orthopedics => "Orthopedics",
        Speciality.Dermatology => "Dermatology",
        Speciality.Emergency => "Emergency care",
        _ => "Unknown"
    };

    public static string FormatAge(int age)
    {
        int mod100 = age % 100;
        int mod10 = age % 10;

        string word;
        if (mod100 >= 11 && mod100 <= 19)
        {
            word = "years";
        }
        else if (mod10 == 1)
        {
            word = "year";
        }
        else if (mod10 >= 2 && mod10 <= 4)
        {
            word = "years";
        }
        else
        {
            word = "years";
        }

        return $"{age} {word}";
    }

    public static string FormatPhone(string phone)
    {
        if (string.IsNullOrEmpty(phone))
        {
            return string.Empty;
        }

        if (phone.Length == 10 && phone.All(char.IsDigit))
        {
            return $"({phone.Substring(0, 3)}) {phone.Substring(3, 3)}-{phone.Substring(6, 4)}";
        }

        return phone;
    }
}