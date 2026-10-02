using System;

namespace ClinicApp;

public class AppointmentManager
{
    private const int MaxAppointments = 500;
    private Appointment[] _appointments = new Appointment[MaxAppointments];
    private int _count = 0;

    private PatientManager _patients;
    private DoctorManager _doctors;

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public AppointmentManager(PatientManager patients, DoctorManager doctors)
    {
        _patients = patients;
        _doctors = doctors;
    }

    private Appointment? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].Id == id)
            {
                return _appointments[i];
            }
        }
        return null;
    }

    public bool Book(int patientId, int doctorId, DateTime scheduledAt, int durationMinutes = 30)
    {
        if (_count >= MaxAppointments)
        {
            Console.WriteLine("Error: reached limit of appointments.");
            return false;
        }

        Patient? patient = _patients.FindById(patientId);
        if (patient == null)
        {
            Console.WriteLine($"Error: patient with ID [{patientId}] not found.");
            return false;
        }

        Doctor? doctor = _doctors.FindById(doctorId);
        if (doctor == null)
        {
            Console.WriteLine($"Error: Doctor with ID [{doctorId}] not found.");
            return false;
        }

        Appointment appt = new Appointment(patientId, doctorId, scheduledAt, durationMinutes);
        _appointments[_count] = appt;
        _count++;

        Console.WriteLine($"Appointment [{appt.Id}] created: {patient.FullName} → {doctor.FullName} at {scheduledAt:dd.MM.yyyy HH:mm}");
        return true;
    }

    public bool Cancel(int id, string reason = "")
    {
        Appointment? appt = FindById(id);
        if (appt == null)
        {
            Console.WriteLine($"Appointment with ID [{id}] not found.");
            return false;
        }

        bool success = appt.Cancel(reason);
        if (success)
        {
            Console.WriteLine($"Appointment [{id}] cancelled.");
        }
        else
        {
            Console.WriteLine($"Impossible to cancel [{id}] with status '{appt.Status}'.");
        }
        return success;
    }

    public bool Complete(int id)
    {
        Appointment? appt = FindById(id);
        if (appt == null)
        {
            Console.WriteLine($"Appointment with ID [{id}] not found.");
            return false;
        }

        bool success = appt.Complete();
        if (success)
        {
            Console.WriteLine($"Appointment [{id}] successfully finished.");
        }
        else
        {
            Console.WriteLine($"Impossible to finish appointment [{id}] with status '{appt.Status}'.");
        }
        return success;
    }

    public Appointment[] GetByPatient(int patientId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                matchCount++;
            }
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].PatientId == patientId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetByDoctor(int doctorId)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                matchCount++;
            }
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].DoctorId == doctorId)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetByDate(DateTime date)
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                matchCount++;
            }
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].ScheduledAt.Date == date.Date)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public Appointment[] GetUpcoming()
    {
        int matchCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                matchCount++;
            }
        }

        Appointment[] result = new Appointment[matchCount];
        int index = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_appointments[i].IsUpcoming)
            {
                result[index] = _appointments[i];
                index++;
            }
        }
        return result;
    }

    public void DisplayAppointment(Appointment appt)
    {
        Patient? p = _patients.FindById(appt.PatientId);
        Doctor? d = _doctors.FindById(appt.DoctorId);

        string pName = (p != null) ? p.FullName : $"Patient #{appt.PatientId}";
        string dName = (d != null) ? d.FullName : $"Doctor #{appt.DoctorId}";

        string baseInfo = $"[{appt.Id}] {pName} → {dName} | {appt.ScheduledAt:dd.MM.yyyy HH:mm}–{appt.EndsAt:HH:mm} | {appt.Status}";

        if (appt.Notes != null && appt.Notes.Length > 0)
        {
            Console.WriteLine($"{baseInfo} | {appt.Notes}");
        }
        else
        {
            Console.WriteLine(baseInfo);
        }
    }

    public void DisplayList(Appointment[] list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("No appointments found.");
            return;
        }

        for (int i = 0; i < list.Length; i++)
        {
            DisplayAppointment(list[i]);
        }
    }
}