using System;
using System.Globalization;
using ClinicApp.Interfaces;
using ClinicApp.Managers;
using ClinicApp.Models;	

PatientManager patients = new PatientManager();
DoctorManager doctors = new DoctorManager();


AppointmentManager appointments = new AppointmentManager(patients, doctors);

DateTime tomorrow = DateTime.Today.AddDays(1);

appointments.Book(1, 1, tomorrow.AddHours(10));
appointments.BookUrgent(2, 2, tomorrow.AddHours(11), "біль у грудях");
appointments.BookSpecialist(3, 3, tomorrow.AddHours(12), "кардіологія", 60);

UrgentAppointment urgentAppt = new UrgentAppointment(1, 1, tomorrow.AddHours(10), "тест");
Appointment baseAppt = urgentAppt;

Console.WriteLine("== new vs override ==");
Console.WriteLine($"Через Appointment:       {baseAppt.GetDescription()} | пріоритет {baseAppt.GetPriority()}");
Console.WriteLine($"Через UrgentAppointment: {urgentAppt.GetDescription()} | пріоритет {urgentAppt.GetPriority()}");

/*
ВІДПОВІДІ НА ЗАПИТАННЯ:

1. Чому GetDescription() через змінну типу Appointment дає опис термінового прийому?
   Оскільки метод GetDescription() є віртуальним (позначений як virtual у батьківському класі та override у підкласі),
   C# застосовує динамічний поліморфізм. Виклик методу визначається реальним типом об'єкта в пам'яті (UrgentAppointment),
   а не типом змінної.

2. Чому GetPriority() через ту саму змінну повертає 3, хоча об'єкт — UrgentAppointment?
   Метод GetPriority() не є віртуальним у базовому класі. У підкласі UrgentAppointment він оголошений з модифікатором new,
   що лише приховує (shadows) метод батька, а не перевизначає його. При виклику невіртуального або прихованого через new методу
   C# використовує статичне зв'язування за типом змінної (Appointment), тому викликається версія базового класу.

3. Що треба змінить в Appointment і UrgentAppointment, щоб GetPriority() теж працював поліморфно?
   У класі Appointment слід позначити метод як віртуальний: public virtual int GetPriority() => 3;
   А в класі UrgentAppointment замінити модифікатор new на override: public override int GetPriority() => 1;
*/