
using System;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;
using ClinicApp.Utils;

Clinic clinic = new Clinic("Medical Clinic");
bool running = true;

while (running)
{
    Console.WriteLine("\n===== MEDICAL CLINIC =====");
    Console.WriteLine("1. Додати пацієнта");
    Console.WriteLine("2. Додати лікаря");
    Console.WriteLine("3. Записати на прийом");
    Console.WriteLine("0. Вийти");
    Console.Write("Виберіть пункт меню: ");

    string choice = Console.ReadLine() ?? "";

    switch (choice)
    {
        case "1":
            AddPatient(clinic);
            break;

        case "2":
            AddDoctor(clinic);
            break;

        case "3":
            BookAppointment(clinic);
            break;

        case "0":
            running = false;
            Console.WriteLine("Програму завершено.");
            break;

        default:
            Console.WriteLine("Невідомий пункт меню. Спробуйте ще раз.");
            break;
    }
}

static void AddPatient(Clinic clinic)
{
    try
    {
        Console.Write("Ім'я: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Прізвище: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Дата народження (дд.мм.рррр): ");
        DateTime dateOfBirth = DateTime.Parse(Console.ReadLine() ?? "");

        BloodType bloodType = ReadEnum<BloodType>("Група крові");

        Console.Write("Телефон (10 цифр): ");
        string phone = Console.ReadLine() ?? "";

        Patient patient = new Patient(
            firstName,
            lastName,
            dateOfBirth,
            bloodType,
            phone);

        clinic.Patients.Add(patient);

        Console.WriteLine($"Пацієнта успішно додано. ID: {patient.Id}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (FormatException ex)
    {
        Console.WriteLine("Некоректний формат введення: " + ex.Message);
    }
    catch (OverflowException ex)
    {
        Console.WriteLine("Введене число завелике: " + ex.Message);
    }
}

static void AddDoctor(Clinic clinic)
{
    try
    {
        Console.Write("Ім'я лікаря: ");
        string firstName = Console.ReadLine() ?? "";

        Console.Write("Прізвище лікаря: ");
        string lastName = Console.ReadLine() ?? "";

        Console.Write("Номер ліцензії: ");
        string licenseNumber = Console.ReadLine() ?? "";

        Console.Write("Телефон (10 цифр): ");
        string phone = Console.ReadLine() ?? "";

        Speciality speciality = ReadEnum<Speciality>("Спеціальність");

        Console.Write("Початок робочого дня (0–23): ");
        int start = int.Parse(Console.ReadLine() ?? "");

        Console.Write("Кінець робочого дня (1–24): ");
        int end = int.Parse(Console.ReadLine() ?? "");

        
        if (string.IsNullOrWhiteSpace(licenseNumber))
        {
            throw new ArgumentException(
                "License number cannot be empty.",
                nameof(Doctor.LicenseNumber));
        }

        ClinicValidator.ValidatePhone(phone);

        WorkSchedule schedule = new WorkSchedule(start, end);

        Doctor doctor = new Doctor(firstName, lastName, speciality);

        doctor.LicenseNumber = licenseNumber;
        doctor.Phone = phone;
        doctor.Schedule = schedule;

        clinic.Doctors.Add(doctor);

        Console.WriteLine($"Лікаря успішно додано. ID: {doctor.Id}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (FormatException ex)
    {
        Console.WriteLine("Некоректний формат введення: " + ex.Message);
    }
    catch (OverflowException ex)
    {
        Console.WriteLine("Введене число завелике: " + ex.Message);
    }
}

static void BookAppointment(Clinic clinic)
{
    try
    {
        Console.Write("ID пацієнта: ");
        int patientId = int.Parse(Console.ReadLine() ?? "");

        Console.Write("ID лікаря: ");
        int doctorId = int.Parse(Console.ReadLine() ?? "");

        Console.Write("Дата й час прийому (дд.мм.рррр гг:хх): ");
        DateTime scheduledAt = DateTime.Parse(Console.ReadLine() ?? "");

        Console.Write("Тривалість прийому в хвилинах: ");
        int durationMinutes = int.Parse(Console.ReadLine() ?? "");

        clinic.Appointments.Book(
            patientId,
            doctorId,
            scheduledAt,
            durationMinutes);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (FormatException ex)
    {
        Console.WriteLine("Некоректний формат введення: " + ex.Message);
    }
    catch (OverflowException ex)
    {
        Console.WriteLine("Введене число завелике: " + ex.Message);
    }
}

static TEnum ReadEnum<TEnum>(string label) where TEnum : struct, Enum
{
    Console.WriteLine(
        $"{label}. Доступні значення: " +
        string.Join(", ", Enum.GetNames(typeof(TEnum))));

    Console.Write("Введіть значення: ");
    string input = Console.ReadLine() ?? "";

    if (!Enum.TryParse<TEnum>(input, true, out TEnum result) ||
        !Enum.IsDefined(typeof(TEnum), result))
    {
        throw new ArgumentException(
            $"Некоректне значення для поля {label}.");
    }

    return result;
}