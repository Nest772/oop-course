using System;
using System.Collections.Generic;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class AnalyticsManager
{
    private readonly AppointmentManager _appointments;
    private readonly DoctorManager _doctors;
    private readonly PatientManager _patients;

    public AnalyticsManager(
        AppointmentManager appointments,
        DoctorManager doctors,
        PatientManager patients)
    {
        _appointments = appointments ?? throw new ArgumentNullException(nameof(appointments));
        _doctors = doctors ?? throw new ArgumentNullException(nameof(doctors));
        _patients = patients ?? throw new ArgumentNullException(nameof(patients));
    }

    public IEnumerable<DoctorStats> ComputeDoctorStats()
    {
        Doctor[] doctors = _doctors.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        for (int i = 0; i < doctors.Length; i++)
        {
            Doctor doctor = doctors[i];
            int count = 0;
            decimal totalRevenue = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < appointments.Length; j++)
            {
                Appointment appointment = appointments[j];
                if (appointment.DoctorId == doctor.Id)
                {
                    count++;
                    totalRevenue += appointment.GetCost();

                    if (appointment.ScheduledAt > lastDate)
                    {
                        lastDate = appointment.ScheduledAt;
                    }
                }
            }

            yield return new DoctorStats(
                doctor.Id,
                doctor.FullName,
                count,
                totalRevenue,
                lastDate);
        }
    }

    public IEnumerable<PatientStats> ComputePatientStats()
    {
        Patient[] patients = _patients.GetAll();
        Appointment[] appointments = _appointments.GetAll();

        for (int i = 0; i < patients.Length; i++)
        {
            Patient patient = patients[i];
            int count = 0;
            decimal totalSpent = 0m;
            DateTime lastDate = DateTime.MinValue;

            for (int j = 0; j < appointments.Length; j++)
            {
                Appointment appointment = appointments[j];
                if (appointment.PatientId == patient.Id)
                {
                    count++;
                    totalSpent += appointment.GetCost();

                    if (appointment.ScheduledAt > lastDate)
                    {
                        lastDate = appointment.ScheduledAt;
                    }
                }
            }

            yield return new PatientStats(
                patient.Id,
                patient.FullName,
                count,
                totalSpent,
                lastDate);
        }
    }
}