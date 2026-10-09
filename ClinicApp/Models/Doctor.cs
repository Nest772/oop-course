using ClinicApp.Enums;
using ClinicApp.Utils;
namespace ClinicApp.Models;

public class Doctor
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
private string _licenseNumber = "";

public string LicenseNumber
{
    get => _licenseNumber;
    set
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "License number cannot be empty.",
                nameof(LicenseNumber));

        _licenseNumber = value;
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
    public Speciality Speciality { get; set; }

    public WorkSchedule Schedule { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public bool IsAvailableNow => Schedule.IsNow;

    public Doctor(string firstName, string lastName, Speciality speciality, string licenseNumber, string phone, WorkSchedule schedule)
    {
        Id = _nextId++;
        FirstName = firstName;
        LastName = lastName;
        Speciality = speciality;
        LicenseNumber = licenseNumber;
        Phone = phone;
        Schedule = schedule;
    }

    public Doctor(string firstName, string lastName, Speciality speciality, WorkSchedule schedule)
        : this(firstName, lastName, speciality, "LIC-000", "0000000000", schedule)
    {
    }

    public Doctor(string firstName, string lastName, Speciality speciality)
        : this(firstName, lastName, speciality, new WorkSchedule(8, 17)) 
    {
    }

    public Doctor()
        : this("Unknown", "Doctor", Speciality.General)
    {
    }

    public bool CanAcceptAt(int hour)
    {
        return Schedule.Contains(hour);
    }

    public override string ToString()
    {
        string status = IsAvailableNow ? "available now" : "outside working hours";
        string formattedSpec = ClinicFormatter.FormatSpeciality(Speciality);
        string formattedPhone = ClinicFormatter.FormatPhone(Phone);
        return $"[{Id}] {FullName} | {Speciality} | {LicenseNumber} | Tel: {Phone} | {Schedule} | {status}";
    }
}