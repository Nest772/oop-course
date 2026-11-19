using System;
using System.Collections.Generic;
using System.Globalization;
using ClinicApp.Comparators;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;
using ClinicApp.Utils;

namespace ClinicApp;

public class Program
{
    public static void Main(string[] args)
    {
        Clinic clinic = new Clinic("Амбулаторія №1");

        DateTime tomorrow = DateTime.Today.AddDays(1);

        clinic.Appointments.Book(1, 1, tomorrow.AddHours(10));
        clinic.Appointments.BookUrgent(2, 2, tomorrow.AddHours(11), "біль у грудях");
        clinic.Appointments.BookSpecialist(3, 3, tomorrow.AddHours(12), "кардіологія", 60);

        UrgentAppointment urgentAppt = new UrgentAppointment(1, 1, tomorrow.AddHours(10), "тест");
        Appointment baseAppt = urgentAppt;

        Console.WriteLine("== new vs override ==");
        Console.WriteLine($"Через Appointment:       {baseAppt.GetDescription()} | пріоритет {baseAppt.GetPriority()}");
        Console.WriteLine($"Через UrgentAppointment: {urgentAppt.GetDescription()} | пріоритет {urgentAppt.GetPriority()}");
        Console.WriteLine();

        while (true)
        {
            ShowMainMenu();
            Console.Write("Оберіть розділ: ");
            string input = Console.ReadLine() ?? "";

            if (!int.TryParse(input, out int choice))
            {
                continue;
            }

            if (choice == 0)
            {
                Console.WriteLine("Завершення роботи.");
                break;
            }

            switch (choice)
            {
                case 1:
                    PatientsMenu(clinic);
                    break;
                case 2:
                    DoctorsMenu(clinic);
                    break;
                case 3:
                    AppointmentsMenu(clinic);
                    break;
                case 4:
                    MedicalRecordsMenu(clinic);
                    break;
                case 5:
                    BillingMenu(clinic);
                    break;
                case 6:
                    WaitingRoomMenu(clinic);
                    break;
                case 7:
                    clinic.GenerateReport();
                    break;
                case 8:
                    AnalyticsMenu(clinic);
                    break;
                case 9:
                    TreatmentPlansMenu(clinic);
                    break;
            }
        }
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║                 МЕДИЧНА КЛІНІКА              ║");
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine("║  1. Пацієнти        — реєстрація, пошук      ║");
        Console.WriteLine("║  2. Лікарі          — персонал, розклад      ║");
        Console.WriteLine("║  3. Записи          — прийоми, скасування    ║");
        Console.WriteLine("║  4. Медична картка  — діагнози, рецепти      ║");
        Console.WriteLine("║  5. Рахунки         — оплата, борги          ║");
        Console.WriteLine("║  6. Черга           — очікування, прийом     ║");
        Console.WriteLine("║  7. Звіт            — загальна статистика    ║");
        Console.WriteLine("║  8. Аналітика       — рейтинги, виручка      ║");
        Console.WriteLine("║  9. Плани лікування — рефлексія, атрибути    ║");
        Console.WriteLine("║  0. Вийти                                    ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝");
    }

    private static void TreatmentPlansMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Плани лікування ────────────────────");
            Console.WriteLine("  1. Показати всі плани");
            Console.WriteLine("  2. Додати план лікування");
            Console.WriteLine("  3. Плани пацієнта");
            Console.WriteLine("  4. Активувати план");
            Console.WriteLine("  5. Завершити план");
            Console.WriteLine("  6. Скасувати план");
            Console.WriteLine("  7. Інформація про тип TreatmentPlan");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть: ");

            string input = Console.ReadLine() ?? "";
            if (!int.TryParse(input, out int choice))
            {
                continue;
            }

            if (choice == 0)
            {
                break;
            }

            switch (choice)
            {
                case 1:
                {
                    TreatmentPlan[] allPlans = clinic.TreatmentPlans.GetAll();
                    DisplayTreatmentPlans(allPlans);
                    break;
                }
                case 2:
                {
                    Console.WriteLine("Заповніть дані плану лікування:");
                    TreatmentPlan plan = FormBuilder.Build<TreatmentPlan>();
                    if (clinic.TreatmentPlans.Add(plan))
                    {
                        Console.WriteLine($"План лікування #{plan.Id} успішно додано.");
                    }
                    break;
                }
                case 3:
                {
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        Console.WriteLine("Некоректний ID пацієнта.");
                        break;
                    }

                    TreatmentPlan[] patientPlans = clinic.TreatmentPlans.GetByPatient(patientId);
                    DisplayTreatmentPlans(patientPlans);
                    break;
                }
                case 4:
                {
                    Console.Write("ID плану: ");
                    if (!int.TryParse(Console.ReadLine(), out int planId))
                    {
                        Console.WriteLine("Некоректний ID плану.");
                        break;
                    }

                    TreatmentPlan? plan = clinic.TreatmentPlans.GetById(planId);
                    if (plan == null)
                    {
                        Console.WriteLine("План не знайдено.");
                    }
                    else if (plan.Activate())
                    {
                        Console.WriteLine($"План #{plan.Id} активовано (статус: {plan.Status}).");
                    }
                    else
                    {
                        Console.WriteLine($"Неможливо активувати план #{plan.Id} зі статусом {plan.Status}.");
                    }
                    break;
                }
                case 5:
                {
                    Console.Write("ID плану: ");
                    if (!int.TryParse(Console.ReadLine(), out int planId))
                    {
                        Console.WriteLine("Некоректний ID плану.");
                        break;
                    }

                    TreatmentPlan? plan = clinic.TreatmentPlans.GetById(planId);
                    if (plan == null)
                    {
                        Console.WriteLine("План не знайдено.");
                    }
                    else if (plan.Complete())
                    {
                        Console.WriteLine($"План #{plan.Id} завершено (статус: {plan.Status}).");
                    }
                    else
                    {
                        Console.WriteLine($"Неможливо завершити план #{plan.Id} зі статусом {plan.Status}.");
                    }
                    break;
                }
                case 6:
                {
                    Console.Write("ID плану: ");
                    if (!int.TryParse(Console.ReadLine(), out int planId))
                    {
                        Console.WriteLine("Некоректний ID плану.");
                        break;
                    }

                    TreatmentPlan? plan = clinic.TreatmentPlans.GetById(planId);
                    if (plan == null)
                    {
                        Console.WriteLine("План не знайдено.");
                    }
                    else if (plan.Cancel())
                    {
                        Console.WriteLine($"План #{plan.Id} скасовано (статус: {plan.Status}).");
                    }
                    else
                    {
                        Console.WriteLine($"Неможливо скасувати план #{plan.Id} зі статусом {plan.Status}.");
                    }
                    break;
                }
                case 7:
                {
                    ModelValidator.PrintInfo(typeof(TreatmentPlan));
                    break;
                }
            }
        }
    }

    private static void DisplayTreatmentPlans(TreatmentPlan[] plans)
    {
        if (plans.Length == 0)
        {
            Console.WriteLine("Планів лікування не знайдено.");
            return;
        }

        for (int i = 0; i < plans.Length; i++)
        {
            TreatmentPlan plan = plans[i];
            Console.WriteLine($"[{plan.Id}] Пацієнт #{plan.PatientId} | Діагноз: {plan.Diagnosis} | Тривалість: {plan.DurationDays} дн. | Статус: {plan.Status}");
        }
    }

    private static void AnalyticsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Аналітика ───────────────────");
            Console.WriteLine("  1. Лікарі за навантаженням");
            Console.WriteLine("  2. Лікарі за виручкою");
            Console.WriteLine("  3. Лікарі за іменем");
            Console.WriteLine("  4. Пацієнти за кількістю візитів");
            Console.WriteLine("  5. Пацієнти за витратами");
            Console.WriteLine("  0. Назад");
            Console.Write("Оберіть: ");

            string input = Console.ReadLine() ?? "";
            if (!int.TryParse(input, out int choice))
            {
                continue;
            }

            if (choice == 0)
            {
                break;
            }

            switch (choice)
            {
                case 1:
                {
                    List<DoctorStats> stats = CollectDoctorStats(clinic);
                    stats.Sort();
                    DisplayStatsList(stats);
                    break;
                }
                case 2:
                {
                    List<DoctorStats> stats = CollectDoctorStats(clinic);
                    stats.Sort(new DoctorStatsByRevenue());
                    DisplayStatsList(stats);
                    break;
                }
                case 3:
                {
                    List<DoctorStats> stats = CollectDoctorStats(clinic);
                    stats.Sort(new DoctorStatsByName());
                    DisplayStatsList(stats);
                    break;
                }
                case 4:
                {
                    List<PatientStats> stats = CollectPatientStats(clinic);
                    stats.Sort();
                    DisplayStatsList(stats);
                    break;
                }
                case 5:
                {
                    List<PatientStats> stats = CollectPatientStats(clinic);
                    stats.Sort(new PatientStatsBySpent());
                    DisplayStatsList(stats);
                    break;
                }
            }
        }
    }

    private static List<DoctorStats> CollectDoctorStats(Clinic clinic)
    {
        List<DoctorStats> result = new List<DoctorStats>();
        foreach (DoctorStats item in clinic.Analytics.ComputeDoctorStats())
        {
            result.Add(item);
        }
        return result;
    }

    private static List<PatientStats> CollectPatientStats(Clinic clinic)
    {
        List<PatientStats> result = new List<PatientStats>();
        foreach (PatientStats item in clinic.Analytics.ComputePatientStats())
        {
            result.Add(item);
        }
        return result;
    }

    private static void DisplayStatsList<T>(List<T> items)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("Даних для відображення немає.");
            return;
        }

        for (int i = 0; i < items.Count; i++)
        {
            Console.WriteLine(items[i]);
        }
    }

    private static void WaitingRoomMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Черга ───────────────────────");
            Console.WriteLine("1. Додати пацієнта до черги  2. Прийняти першого  3. Хто перший?  4. Переглянути всю чергу  0. Назад");
            Console.Write("Оберіть: ");

            string input = Console.ReadLine() ?? "";
            if (!int.TryParse(input, out int choice))
            {
                continue;
            }

            if (choice == 0)
            {
                break;
            }

            switch (choice)
            {
                case 1:
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        Console.WriteLine("Некоректний ID.");
                        break;
                    }

                    Patient? patient = clinic.Patients.FindById(patientId);
                    if (patient == null)
                    {
                        Console.WriteLine("Пацієнта не знайдено.");
                    }
                    else
                    {
                        clinic.WaitingRoom.Enqueue(patient);
                        Console.WriteLine($"Пацієнта [{patient.Id}] {patient.FullName} додано до черги.");
                    }
                    break;

                case 2:
                    try
                    {
                        Patient dequeuedPatient = clinic.WaitingRoom.Dequeue();
                        Console.WriteLine($"Прийнято: {dequeuedPatient.FullName}. У черзі лишилось: {clinic.WaitingRoom.Count}");
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Черга порожня.");
                    }
                    break;

                case 3:
                    try
                    {
                        Patient firstPatient = clinic.WaitingRoom.Peek();
                        Console.WriteLine($"Перший у черзі: {firstPatient.FullName}");
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Черга порожня.");
                    }
                    break;

                case 4:
                    Patient[] queueArray = clinic.WaitingRoom.ToArray();
                    if (queueArray.Length == 0)
                    {
                        Console.WriteLine("Черга порожня.");
                    }
                    else
                    {
                        Console.WriteLine("=== Поточний стан черги ===");
                        for (int i = 0; i < queueArray.Length; i++)
                        {
                            Console.WriteLine($"{i + 1}. {queueArray[i].FullName}");
                        }
                    }
                    break;
            }
        }
    }

    private static void BillingMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Рахунки ───────────────────");
            Console.WriteLine("1. Борги пацієнта  2. Всі неоплачені записи  3. Оплатити запис  4. Загальний борг клініки  0. Назад");
            Console.Write("Оберіть: ");

            string input = Console.ReadLine() ?? "";
            if (!int.TryParse(input, out int choice))
            {
                continue;
            }

            if (choice == 0)
            {
                break;
            }

            switch (choice)
            {
                case 1:
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        break;
                    }

                    IPayable[] unpaidByPatient = clinic.Billing.GetUnpaidByPatient(patientId);

                    if (unpaidByPatient.Length > 0)
                    {
                        Console.WriteLine($"Неоплачені записи пацієнта #{patientId}:");
                    }
                    clinic.Billing.DisplayUnpaid(unpaidByPatient);

                    decimal patientDebt = clinic.Billing.GetPatientDebt(patientId);
                    Console.WriteLine($"Борг: {patientDebt.ToString("F2", CultureInfo.InvariantCulture)} грн");
                    break;

                case 2:
                    IPayable[] allUnpaid = clinic.Billing.GetAllUnpaid();
                    clinic.Billing.DisplayUnpaid(allUnpaid);
                    break;

                case 3:
                    Console.Write("ID запису для оплати: ");
                    if (!int.TryParse(Console.ReadLine(), out int apptId))
                    {
                        Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                        break;
                    }

                    bool paid = clinic.Billing.PayAppointment(apptId);
                    if (paid)
                    {
                        Console.WriteLine($"Запис [{apptId}] оплачено.");
                    }
                    else
                    {
                        Console.WriteLine("Не вдалося оплатити: запис не знайдено, вже оплачено або скасовано.");
                    }
                    break;

                case 4:
                    decimal totalDebt = clinic.Billing.GetTotalDebt();
                    Console.WriteLine($"Загальний борг по клініці: {totalDebt.ToString("F2", CultureInfo.InvariantCulture)} грн");
                    break;
            }
        }
    }

    private static void AppointmentsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Записи ───────────────────");
            Console.WriteLine("1. Створити запис  ...  8. Скасувати всі записи пацієнта  9. За типом прийому  0. Назад");
            Console.Write("Оберіть: ");

            string input = Console.ReadLine() ?? "";
            if (!int.TryParse(input, out int choice)) continue;
            if (choice == 0) break;

            switch (choice)
            {
                case 8:
                    Console.Write("ID пацієнта: ");
                    if (!int.TryParse(Console.ReadLine(), out int patientId))
                    {
                        break;
                    }

                    Console.Write("Причина (Enter — без причини): ");
                    string reason = Console.ReadLine() ?? "";

                    Appointment[] patientAppointments = clinic.Appointments.GetByPatient(patientId);
                    int cancelledCount = AppointmentManager.CancelAll(patientAppointments, reason);
                    Console.WriteLine($"Скасовано записів: {cancelledCount}");
                    break;

                case 9:
                    Console.WriteLine("1. Термінові");
                    Console.WriteLine("2. Консультації спеціаліста");
                    Console.WriteLine("3. Звичайні");
                    Console.Write("Оберіть: ");
                    string typeChoice = Console.ReadLine() ?? "";

                    switch (typeChoice)
                    {
                        case "1":
                            clinic.Appointments.DisplayList(clinic.Appointments.GetUrgent());
                            break;
                        case "2":
                            clinic.Appointments.DisplayList(clinic.Appointments.GetSpecialist());
                            break;
                        case "3":
                            clinic.Appointments.DisplayList(clinic.Appointments.GetRegular());
                            break;
                        default:
                            Console.WriteLine("Невірний вибір типу прийому.");
                            break;
                    }
                    break;
            }
        }
    }

    private static void DoctorsMenu(Clinic clinic)
    {
        while (true)
        {
            Console.WriteLine("\n── Лікарі ───────────────────");
            Console.WriteLine("1. Перегляд  ...  5. Вільні години лікаря  0. Назад");
            Console.Write("Оберіть: ");

            string input = Console.ReadLine() ?? "";
            if (!int.TryParse(input, out int choice)) continue;
            if (choice == 0) break;

            switch (choice)
            {
                case 5:
                    Console.Write("ID лікаря: ");
                    if (!int.TryParse(Console.ReadLine(), out int doctorId))
                    {
                        Console.WriteLine("Лікаря не знайдено");
                        break;
                    }

                    Doctor? doctor = clinic.Doctors.FindById(doctorId);
                    if (doctor == null)
                    {
                        Console.WriteLine("Лікаря не знайдено");
                        break;
                    }

                    ISchedulable schedulable = doctor;

                    Console.Write("Дата (dd.MM.yyyy): ");
                    string dateStr = Console.ReadLine() ?? "";
                    if (!DateTime.TryParseExact(dateStr, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                    {
                        Console.WriteLine("Некоректний формат дати.");
                        break;
                    }

                    Console.Write("Скільки слотів: ");
                    if (!int.TryParse(Console.ReadLine(), out int slotCount))
                    {
                        Console.WriteLine("Некоректна кількість слотів.");
                        break;
                    }

                    try
                    {
                        DateTime[] slots = schedulable.GetAvailableSlots(date, slotCount);
                        for (int i = 0; i < slots.Length; i++)
                        {
                            Console.WriteLine(slots[i].ToString("HH:mm"));
                        }
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Помилка: {ex.Message}");
                    }
                    break;
            }
        }
    }

    private static void PatientsMenu(Clinic clinic) { }
    private static void MedicalRecordsMenu(Clinic clinic) { }
}