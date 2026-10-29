using System;
using System.Globalization;
using ClinicApp.Interfaces;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class BillingManager
{
    private readonly AppointmentManager _appointmentManager;

    public BillingManager(AppointmentManager appointmentManager)
    {
        _appointmentManager = appointmentManager
            ?? throw new ArgumentNullException(nameof(appointmentManager));
    }

    public IPayable[] GetAllUnpaid()
    {
        return FilterUnpaid(_appointmentManager.GetAll());
    }

    public IPayable[] GetUnpaidByPatient(int patientId)
    {
        return FilterUnpaid(
            _appointmentManager.GetByPatient(patientId)
        );
    }

    private IPayable[] FilterUnpaid(Appointment[] appointments)
    {
        int count = 0;

        for (int i = 0; i < appointments.Length; i++)
        {
            if (appointments[i] != null &&
                appointments[i] is IPayable payable &&
                !payable.IsPaid &&
                appointments[i] is ICancellable cancellable &&
                !cancellable.IsCancelled)
            {
                count++;
            }
        }

        IPayable[] result = new IPayable[count];
        int index = 0;

        for (int i = 0; i < appointments.Length; i++)
        {
            if (appointments[i] != null &&
                appointments[i] is IPayable payable &&
                !payable.IsPaid &&
                appointments[i] is ICancellable cancellable &&
                !cancellable.IsCancelled)
            {
                result[index] = payable;
                index++;
            }
        }

        return result;
    }

    public decimal GetTotalDebt()
    {
        IPayable[] unpaid = GetAllUnpaid();
        decimal total = 0m;

        for (int i = 0; i < unpaid.Length; i++)
        {
            total += unpaid[i].GetCost();
        }

        return total;
    }

    public decimal GetPatientDebt(int patientId)
    {
        IPayable[] unpaid = GetUnpaidByPatient(patientId);
        decimal total = 0m;

        for (int i = 0; i < unpaid.Length; i++)
        {
            total += unpaid[i].GetCost();
        }

        return total;
    }

    public bool PayAppointment(int appointmentId)
    {
        Appointment[] appointments = _appointmentManager.GetAll();

        for (int i = 0; i < appointments.Length; i++)
        {
            Appointment appointment = appointments[i];

            if (appointment == null || appointment.Id != appointmentId)
            {
                continue;
            }

            if (appointment is IPayable payable &&
                appointment is ICancellable cancellable &&
                !payable.IsPaid &&
                !cancellable.IsCancelled)
            {
                payable.MarkPaid();
                return payable.IsPaid;
            }

            return false;
        }

        return false;
    }

    public void DisplayUnpaid(IPayable[] items)
    {
        if (items.Length == 0)
        {
            Console.WriteLine("Немає неоплачених записів.");
            return;
        }

        for (int i = 0; i < items.Length; i++)
        {
            string cost = items[i]
                .GetCost()
                .ToString("F2", CultureInfo.InvariantCulture);

            if (items[i] is Appointment appointment)
            {
                Console.WriteLine(
                    $"{appointment} | Сума: {cost} грн"
                );
            }
            else
            {
                Console.WriteLine(
                    $"[{i + 1}] | Сума: {cost} грн"
                );
            }
        }
    }
}