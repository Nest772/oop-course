using System;

namespace ClinicApp;

public class Patient
{
    private static int _nextId = 1;

    public int Id { get; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string BloodType { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; } = string.Empty;

    public string FullName => $"{FirstName} {LastName}";

    public int Age
    {
        get
        {
            int age = DateTime.Today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }
            return age;
        }
    }

    public bool IsAdult => Age >= 18;

    public Patient(string firstName, string lastName, DateTime dob, string bloodType, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
    }

    public Patient(string firstName, string lastName)
        : this(firstName, lastName, DateTime.Today.AddYears(-26), "Unknown", "0000000000")
    {
    }

    public Patient()
        : this("Unknown", "Patient")
    {
    }

    public string GetAgeCategory()
    {
        if (Age < 18)
        {
            return "child";
        }
        if (Age < 60)
        {
            return "adult";
        }
        return "senior";
    }

    public override string ToString()
    {
        return $"[{Id}] {FullName} | Age: {Age} ({GetAgeCategory()}) | Blood: {BloodType} | Phone: {Phone}";
    }
}