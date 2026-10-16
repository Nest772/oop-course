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
        ClinicValidator.ValidateName(value, nameof(FirstName));
        _firstName = value;
    }
}

  private string _lastName = "";

public string LastName
{
    get => _lastName;
    set
    {
        ClinicValidator.ValidateName(value, nameof(LastName));
        _lastName = value;
    }
}

private DateTime _dateOfBirth;

public DateTime DateOfBirth
{
    get => _dateOfBirth;
    set
    {
        ClinicValidator.ValidateDate(value, nameof(DateOfBirth));
        _dateOfBirth = value;
    }
}

  private string _phone = "";

public string Phone
{
    get => _phone;
    set
    {
        ClinicValidator.ValidatePhone(value);
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