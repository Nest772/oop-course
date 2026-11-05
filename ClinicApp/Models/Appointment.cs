using System.Globalization;
using ClinicApp.Enums;
using ClinicApp.Utils;
using ClinicApp.Interfaces;

namespace ClinicApp.Models;

public class Appointment : IPayable, ICancellable
{
    private static int _nextId = 1;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }

    public DateTime ScheduledAt { get; set; }
    private int _durationMinutes;

    public int DurationMinutes
    {
        get => _durationMinutes;
        set
        {
            ClinicValidator.ValidatePositive(
                value,
                nameof(DurationMinutes));

            _durationMinutes = value;
        }
    }
    public AppointmentStatus Status { get; private set; }
    public string Notes { get; private set; }

    public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);

    public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == AppointmentStatus.Scheduled;

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Id = _nextId++;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = AppointmentStatus.Scheduled; 
        Notes = string.Empty;
    }

    public bool IsCancelled => Status == AppointmentStatus.Cancelled;

    public string CancellationReason => IsCancelled ? Notes : "";

    private bool _isPaid = false;

    public virtual decimal GetCost()
    {
        return DurationMinutes * 10m;
    }

    public virtual string GetDescription()
    {
        return "Прийом";
    }

    public int GetPriority()
    {
        return 3;
    }

    public bool IsPaid
    {
        get { return _isPaid; }
    }

    public void MarkPaid()
    {
        if (IsCancelled)
        {
            return;
        }

        _isPaid = true;
    }

    public bool Cancel(string reason = "")
    {
        if (Status != AppointmentStatus.Scheduled)
        {
            return false;
        }

        Status = AppointmentStatus.Cancelled; 
        if (reason != null && reason.Length > 0)
        {
            Notes = reason;
        }

        return true;
    }

    public bool Complete()
    {
        if (Status != AppointmentStatus.Scheduled)
        {
            return false;
        }

        Status = AppointmentStatus.Completed; 
        return true;
    }

    public override string ToString()
    {
        string formattedCost = GetCost().ToString("F2", CultureInfo.InvariantCulture);
        string baseInfo = $"[{Id}] {GetDescription()} | Пацієнт #{PatientId} → Лікар #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status}";

        if (!string.IsNullOrEmpty(Notes))
        {
            return $"{baseInfo} | {Notes} | {formattedCost} грн";
        }

        return $"{baseInfo} | {formattedCost} грн";
    }
}