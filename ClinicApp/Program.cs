
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using ClinicApp;
using ClinicApp.Enums;
using ClinicApp.Models;

Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
Console.OutputEncoding = Encoding.UTF8;

Clinic clinic = new Clinic("Medical Clinic");

SeedTestData(clinic);

bool running = true;

while (running)
{
    Console.WriteLine("\n===== MEDICAL CLINIC =====");
    Console.WriteLine("1. Медична картка");
    Console.WriteLine("2. Звіт медичних записів");
    Console.WriteLine("0. Вийти");
    Console.Write("Виберіть пункт меню: ");

    string choice = Console.ReadLine() ?? "";

    switch (choice)
    {
        case "1":
            MedicalRecordsMenu(clinic);
            break;

        case "2":
            ShowReport(clinic);
            break;

        case "0":
            running = false;
            Console.WriteLine("Програму завершено.");
            break;

        default:
            Console.WriteLine("Невідомий пункт меню.");
            break;
    }
}



static void SeedTestData(Clinic clinic)
{
    Patient patient1 = new Patient(
        "Ivan", "Petrenko",
        new DateTime(1990, 5, 14),
        BloodType.APositive, "0501234567");

    Patient patient2 = new Patient(
        "Olena", "Koval",
        new DateTime(1985, 11, 23),
        BloodType.BNegative, "0672345678");

    Patient patient3 = new Patient(
        "Maksym", "Boiko",
        new DateTime(2000, 4, 1),
        BloodType.APositive, "0933456789");

    clinic.Patients.Add(patient1);
    clinic.Patients.Add(patient2);
    clinic.Patients.Add(patient3);

    Doctor doctor1 = new Doctor(
        "Oleh", "Sydorenko", Speciality.Cardiology);

    doctor1.LicenseNumber = "LIC-1001";
    doctor1.Phone = "0501112233";
    doctor1.Schedule = new WorkSchedule(9, 17);

    Doctor doctor2 = new Doctor(
        "Natalia", "Moroz", Speciality.Neurology);

    doctor2.LicenseNumber = "LIC-1002";
    doctor2.Phone = "0671112233";
    doctor2.Schedule = new WorkSchedule(10, 18);

    clinic.Doctors.Add(doctor1);
    clinic.Doctors.Add(doctor2);

    DateTime today = DateTime.Today;

    clinic.MedicalRecords.Add(new Diagnosis(
        1, 1, today.AddDays(-30),
        "I10", "Гіпертонічна хвороба", true));

    clinic.MedicalRecords.Add(new Diagnosis(
        1, 2, today.AddDays(-5),
        "J06.9", "Гострий ринофарингіт", false));

    clinic.MedicalRecords.Add(new LabResult(
        1, 1, today.AddDays(-2),
        "Гемоглобін", 145, "г/л", "120–160", true));

    clinic.MedicalRecords.Add(new LabResult(
        1, 2, today.AddDays(-1),
        "Холестерин", 6.2, "ммоль/л", "< 5.2", false));

    clinic.MedicalRecords.Add(new Prescription(
        1, 1, today.AddDays(-5),
        "Лізиноприл", "10 мг", 30,
        "1 раз на добу вранці"));

    clinic.MedicalRecords.Add(new Prescription(
        2, 2, today.AddDays(-40),
        "Інший препарат", "5 мг", 10,
        "1 раз на добу"));

    clinic.MedicalRecords.Add(new Diagnosis(
        2, 1, today.AddDays(-15),
        "K21.9",
        "Гастроезофагеальна рефлюксна хвороба",
        false));

    clinic.MedicalRecords.Add(new LabResult(
        3, 2, today.AddDays(-3),
        "Глюкоза", 5.1, "ммоль/л", "3.9–5.5", true));

    Console.WriteLine("Тестові дані завантажено.");
}


static void MedicalRecordsMenu(Clinic clinic)
{
    while (true)
    {
        Console.WriteLine("\n── Медична картка ────────────");
        Console.WriteLine("1. Картка пацієнта");
        Console.WriteLine("2. Усі записи пацієнта");
        Console.WriteLine("3. Додати діагноз");
        Console.WriteLine("4. Додати аналіз");
        Console.WriteLine("5. Додати рецепт");
        Console.WriteLine("6. Записи лікаря");
        Console.WriteLine("0. Назад");
        Console.Write("Оберіть: ");

        string choice = Console.ReadLine() ?? "";

        switch (choice)
        {
            case "1":
                if (TryReadExistingPatient(clinic, out int patientId1))
                {
                    clinic.MedicalRecords.DisplayPatientSummary(patientId1);
                }
                break;

            case "2":
                if (TryReadExistingPatient(clinic, out int patientId2))
                {
                    DisplayPatientRecords(clinic, patientId2);
                }
                break;

            case "3":
                AddDiagnosis(clinic);
                break;

            case "4":
                AddLabResult(clinic);
                break;

            case "5":
                AddPrescription(clinic);
                break;

            case "6":
                DisplayDoctorRecords(clinic);
                break;

            case "0":
                return;

            default:
                Console.WriteLine("Невідомий пункт меню.");
                break;
        }
    }
}


static bool TryReadInt(string prompt, out int value)
{
    Console.Write(prompt);

    if (!int.TryParse(Console.ReadLine(), out value))
    {
        Console.WriteLine("Некоректне число.");
        return false;
    }

    return true;
}

static bool TryReadYesNo(string prompt, out bool result)
{
    Console.Write(prompt);
    string answer = (Console.ReadLine() ?? "").Trim();

    if (answer == "1")
    {
        result = true;
        return true;
    }

    if (answer == "0")
    {
        result = false;
        return true;
    }

    result = false;
    Console.WriteLine("Введіть 1 (так) або 0 (ні).");
    return false;
}

static bool TryReadExistingPatient(
    Clinic clinic, out int patientId)
{
    patientId = 0;

    Console.WriteLine("\nПацієнти:");
    clinic.Patients.DisplayAll();

    if (!TryReadInt("ID пацієнта: ", out patientId))
    {
        return false;
    }

    if (clinic.Patients.FindById(patientId) == null)
    {
        Console.WriteLine("Пацієнта з таким ID не знайдено.");
        return false;
    }

    return true;
}

static bool TryReadPatientAndDoctor(
    Clinic clinic, out int patientId, out int doctorId)
{
    patientId = 0;
    doctorId = 0;

    Console.WriteLine("\nПацієнти:");
    clinic.Patients.DisplayAll();

    if (!TryReadInt("ID пацієнта: ", out patientId))
    {
        return false;
    }

    if (clinic.Patients.FindById(patientId) == null)
    {
        Console.WriteLine("Пацієнта з таким ID не знайдено.");
        return false;
    }

    Console.WriteLine("\nЛікарі:");
    clinic.Doctors.DisplayAll();

    if (!TryReadInt("ID лікаря: ", out doctorId))
    {
        return false;
    }

    if (clinic.Doctors.FindById(doctorId) == null)
    {
        Console.WriteLine("Лікаря з таким ID не знайдено.");
        return false;
    }

    return true;
}

static void AddDiagnosis(Clinic clinic)
{
    if (!TryReadPatientAndDoctor(
        clinic, out int patientId, out int doctorId))
    {
        return;
    }

    Console.Write("Код діагнозу (напр. J06.9): ");
    string code = Console.ReadLine() ?? "";

    Console.Write("Опис: ");
    string description = Console.ReadLine() ?? "";

    if (!TryReadYesNo(
        "Хронічне? (1=так, 0=ні): ", out bool chronic))
    {
        return;
    }

    try
    {
        Diagnosis diagnosis = new Diagnosis(
            patientId,
            doctorId,
            DateTime.Today,
            code.Trim(),
            description.Trim(),
            chronic);

        clinic.MedicalRecords.Add(diagnosis);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
}

static void AddLabResult(Clinic clinic)
{
    if (!TryReadPatientAndDoctor(
        clinic, out int patientId, out int doctorId))
    {
        return;
    }

    Console.Write("Назва аналізу: ");
    string testName = Console.ReadLine() ?? "";

    Console.Write("Значення (число з крапкою): ");
    string valueText = (Console.ReadLine() ?? "").Trim();

    if (!double.TryParse(
        valueText,
        NumberStyles.Float,
        CultureInfo.InvariantCulture,
        out double value)
        || double.IsNaN(value)
        || double.IsInfinity(value))
    {
        Console.WriteLine(
            "Некоректне число. Приклад правильного вводу: 6.2");
        return;
    }

    Console.Write("Одиниці виміру: ");
    string unit = Console.ReadLine() ?? "";

    Console.Write("Норма (напр. 4.0–9.0): ");
    string referenceRange = Console.ReadLine() ?? "";

    if (!TryReadYesNo(
        "В нормі? (1=так, 0=ні): ", out bool isNormal))
    {
        return;
    }

    try
    {
        LabResult result = new LabResult(
            patientId,
            doctorId,
            DateTime.Today,
            testName.Trim(),
            value,
            unit.Trim(),
            referenceRange.Trim(),
            isNormal);

        clinic.MedicalRecords.Add(result);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
}

static void AddPrescription(Clinic clinic)
{
    if (!TryReadPatientAndDoctor(
        clinic, out int patientId, out int doctorId))
    {
        return;
    }

    Console.Write("Препарат: ");
    string medicationName = Console.ReadLine() ?? "";

    Console.Write("Дозування (напр. 10 мг): ");
    string dosage = Console.ReadLine() ?? "";

    if (!TryReadInt("Кількість днів: ", out int durationDays))
    {
        return;
    }

    Console.Write("Інструкція (Enter — пропустити): ");
    string instructions = Console.ReadLine() ?? "";

    try
    {
        Prescription prescription = new Prescription(
            patientId,
            doctorId,
            DateTime.Today,
            medicationName.Trim(),
            dosage.Trim(),
            durationDays,
            instructions.Trim());

        clinic.MedicalRecords.Add(prescription);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine("Помилка: " + ex.Message);
    }
}

static void DisplayPatientRecords(Clinic clinic, int patientId)
{
    MedicalRecord[] records =
        clinic.MedicalRecords.GetByPatient(patientId);

    if (records.Length == 0)
    {
        Console.WriteLine("У пацієнта немає медичних записів.");
        return;
    }

    Console.WriteLine($"\nМедичні записи пацієнта #{patientId}:");

    for (int i = 0; i < records.Length; i++)
    {
        Console.WriteLine(
            $"{records[i].GetRecordType()}: {records[i].GetSummary()}");
    }
}

static void DisplayDoctorRecords(Clinic clinic)
{
    Console.WriteLine("\nЛікарі:");
    clinic.Doctors.DisplayAll();

    if (!TryReadInt("ID лікаря: ", out int doctorId))
    {
        return;
    }

    if (clinic.Doctors.FindById(doctorId) == null)
    {
        Console.WriteLine("Лікаря з таким ID не знайдено.");
        return;
    }

    MedicalRecord[] records =
        clinic.MedicalRecords.GetByDoctor(doctorId);

    Console.WriteLine($"\nЗаписи лікаря #{doctorId}:");
    clinic.MedicalRecords.DisplayList(records);
}


static void ShowReport(Clinic clinic)
{
    Console.WriteLine("\n===== ЗВІТ МЕДИЧНИХ ЗАПИСІВ =====");
    Console.WriteLine($"Кількість записів: {clinic.MedicalRecords.Count}");

    clinic.MedicalRecords.DisplayAll();
}