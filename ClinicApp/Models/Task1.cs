using ClinicApp.Enums;
using ClinicApp.Utils;
namespace ClinicApp.Models;

public class Patient
{
    private static int _nextId = 1;

    public int Id { get; }

private string _firstName = "";

public string FirstName
{
    get => _firstName;
    set
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "First name cannot be empty.",
                nameof(FirstName));

        if (value.Length > 50)
            throw new ArgumentException(
                "First name cannot exceed 50 characters.",
                nameof(FirstName));

        _firstName = value;
    }
}

private string _lastName = "";

public string LastName
{
    get => _lastName;
    set
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Last name cannot be empty.",
                nameof(LastName));

        if (value.Length > 50)
            throw new ArgumentException(
                "Last name cannot exceed 50 characters.",
                nameof(LastName));

        _lastName = value;
    }
}

private DateTime _dateOfBirth;

public DateTime DateOfBirth
{
    get => _dateOfBirth;
    set
    {
        if (value > DateTime.Today)
            throw new ArgumentOutOfRangeException(
                nameof(DateOfBirth),
                "Date of birth cannot be in the future.");

        if (value < new DateTime(1900, 1, 1))
            throw new ArgumentOutOfRangeException(
                nameof(DateOfBirth),
                "Date of birth cannot be earlier than 1900.");

        _dateOfBirth = value;
    }
}

private string _phone = "";

public string Phone
{
    get => _phone;
    set
    {
        if (value == null || value.Length != 10)
            throw new ArgumentException(
                "Phone must contain exactly 10 digits.",
                nameof(Phone));

        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] < '0' || value[i] > '9')
                throw new ArgumentException(
                    "Phone must contain only digits.",
                    nameof(Phone));
        }

        _phone = value;
    }
}
    public BloodType BloodType { get; set; }
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

    public Patient(string firstName, string lastName, DateTime dob, BloodType bloodType, string phone)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        BloodType = bloodType;
        Phone = phone;
    }

    public Patient(string firstName, string lastName)
        : this(firstName, lastName, DateTime.Today.AddYears(-26), BloodType.Unknown, "0000000000")
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
    string formattedAge = ClinicFormatter.FormatAge(Age);
    string formattedBlood = ClinicFormatter.FormatBloodType(BloodType);
    string formattedPhone = ClinicFormatter.FormatPhone(Phone);

    return $"[{Id}] {FullName} | Age: {formattedAge} ({GetAgeCategory()}) | Bloodtype: {formattedBlood} | Тел: {formattedPhone}";
}
}