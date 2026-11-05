using System;
using System.Globalization;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;

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

        /*
        ВІДПОВІДІ НА ЗАПИТАННЯ:

        1. Чому GetDescription() через змінну типу Appointment дає опис термінового прийому?
           Оскільки метод GetDescription() є віртуальним (virtual у батьківському класі та override у підкласі),
           C# застосовує динамічний поліморфізм. Виклик методу визначається реальним типом об'єкта в пам'яті (UrgentAppointment),
           а не типом змінної.

        2. Чому GetPriority() через ту саму змінну повертає 3, хоча об'єкт — UrgentAppointment?
           Метод GetPriority() не є віртуальним у базовому класі. У підкласі UrgentAppointment він оголошений з модифікатором new,
           що лише приховує (shadows) метод батька, а не перевизначає його. При виклику невіртуального або прихованого через new методу
           C# використовує статичне зв'язування за типом змінної (Appointment), тому викликається версія базового класу.

        3. Що треба змінити в Appointment і UrgentAppointment, щоб GetPriority() теж працював поліморфно?
           У класі Appointment слід позначити метод як віртуальний: public virtual int GetPriority() => 3;
           А в класі UrgentAppointment замінити модифікатор new на override: public override int GetPriority() => 1;
        */

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
                    clinic.GenerateReport();
                    break;
            }
        }
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine("╔══════════════════════════════════════════════╗");
        Console.WriteLine("║            МЕДИЧНА КЛІНІКА                   ║");
        Console.WriteLine("╠══════════════════════════════════════════════╣");
        Console.WriteLine("║  1. Пацієнти       — реєстрація, пошук       ║");
        Console.WriteLine("║  2. Лікарі         — персонал, розклад       ║");
        Console.WriteLine("║  3. Записи         — прийоми, скасування     ║");
        Console.WriteLine("║  4. Медична картка — діагнози, рецепти       ║");
        Console.WriteLine("║  5. Рахунки        — оплата, борги           ║");
        Console.WriteLine("║  6. Звіт           — загальна статистика     ║");
        Console.WriteLine("║  0. Вийти                                    ║");
        Console.WriteLine("╚══════════════════════════════════════════════╝");
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