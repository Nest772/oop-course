using System;

namespace ClinicApp;

public class Appointment
{
    private static int _nextId = 1;

    public int Id { get; }
    public int PatientId { get; }
    public int DoctorId { get; }

    public DateTime ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }

    public string Status { get; private set; }
    public string Notes { get; private set; }

    public DateTime EndsAt => ScheduledAt.AddMinutes(DurationMinutes);

    public bool IsUpcoming => ScheduledAt > DateTime.Now && Status == "Scheduled";

    public Appointment(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        Id = _nextId++;
        PatientId = patientId;
        DoctorId = doctorId;
        ScheduledAt = scheduledAt;
        DurationMinutes = durationMinutes;
        Status = "Scheduled";
        Notes = string.Empty;
    }

    public bool Cancel(string reason = "")
    {
        if (Status != "Scheduled")
        {
            return false;
        }

        Status = "Cancelled";
        if (reason != null && reason.Length > 0)
        {
            Notes = reason;
        }

        return true;
    }

    public bool Complete()
    {
        if (Status != "Scheduled")
        {
            return false;
        }

        Status = "Completed";
        return true;
    }

    public override string ToString()
    {
        string baseInfo = $"[{Id}] Patient #{PatientId} → Doctor #{DoctorId} | {ScheduledAt:dd.MM.yyyy HH:mm}–{EndsAt:HH:mm} | {Status}";

        if (Notes != null && Notes.Length > 0)
        {
            return $"{baseInfo} | {Notes}";
        }

        return baseInfo;
    }
}