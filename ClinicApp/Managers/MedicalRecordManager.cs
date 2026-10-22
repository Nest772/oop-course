
using System;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class MedicalRecordManager
{
    private const int MaxRecords = 1000;

    private readonly MedicalRecord[] _records =
        new MedicalRecord[MaxRecords];

    private int _count;

    public int Count => _count;

    public void Add(MedicalRecord record)
    {
        if (record == null)
        {
            throw new ArgumentNullException(nameof(record));
        }

        if (_count >= MaxRecords)
        {
            Console.WriteLine(
                "Неможливо додати запис: досягнуто ліміту у 1000 записів."
            );
            return;
        }

        _records[_count] = record;
        _count++;

        Console.WriteLine(
            $"Запис [{record.Id}] {record.GetRecordType()} додано."
        );
    }

    public MedicalRecord? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_records[i].Id == id)
            {
                return _records[i];
            }
        }

        return null;
    }

    public MedicalRecord[] GetByPatient(int patientId)
    {
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                matches++;
            }
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_records[i].PatientId == patientId)
            {
                result[index] = _records[i];
                index++;
            }
        }

        return result;
    }

    public MedicalRecord[] GetByDoctor(int doctorId)
    {
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                matches++;
            }
        }

        MedicalRecord[] result = new MedicalRecord[matches];
        int index = 0;

        for (int i = 0; i < _count; i++)
        {
            if (_records[i].DoctorId == doctorId)
            {
                result[index] = _records[i];
                index++;
            }
        }

        return result;
    }
    
public Diagnosis[] GetDiagnoses(int patientId)
{
    int matches = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is Diagnosis)
        {
            matches++;
        }
    }

    Diagnosis[] result = new Diagnosis[matches];
    int index = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is Diagnosis diagnosis)
        {
            result[index] = diagnosis;
            index++;
        }
    }

    return result;
}

public LabResult[] GetLabResults(int patientId)
{
    int matches = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is LabResult)
        {
            matches++;
        }
    }

    LabResult[] result = new LabResult[matches];
    int index = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is LabResult labResult)
        {
            result[index] = labResult;
            index++;
        }
    }

    return result;
}

public Prescription[] GetPrescriptions(int patientId)
{
    int matches = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is Prescription)
        {
            matches++;
        }
    }

    Prescription[] result = new Prescription[matches];
    int index = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is Prescription prescription)
        {
            result[index] = prescription;
            index++;
        }
    }

    return result;
}
public Prescription[] GetActivePrescriptions(int patientId)
{
    int count = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is Prescription prescription &&
            prescription.IsActive())
        {
            count++;
        }
    }

    Prescription[] result = new Prescription[count];
    int index = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is Prescription prescription &&
            prescription.IsActive())
        {
            result[index] = prescription;
            index++;
        }
    }

    return result;
}

public Diagnosis[] GetChronicDiagnoses(int patientId)
{
    int matches = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is Diagnosis diagnosis &&
            diagnosis.IsChronic)
        {
            matches++;
        }
    }

    Diagnosis[] result = new Diagnosis[matches];
    int index = 0;

    for (int i = 0; i < _count; i++)
    {
        if (_records[i].PatientId == patientId &&
            _records[i] is Diagnosis diagnosis &&
            diagnosis.IsChronic)
        {
            result[index] = diagnosis;
            index++;
        }
    }

    return result;
}


public void DisplayPatientSummary(int patientId)
{
    MedicalRecord[] patientRecords = GetByPatient(patientId);

    if (patientRecords.Length == 0)
    {
        Console.WriteLine(
            $"У пацієнта #{patientId} немає медичних записів."
        );
        return;
    }

    int diagnosisCount = 0;
    int labResultCount = 0;
    int prescriptionCount = 0;

    for (int i = 0; i < patientRecords.Length; i++)
    {
        if (patientRecords[i] is Diagnosis)
        {
            diagnosisCount++;
        }
        else if (patientRecords[i] is LabResult)
        {
            labResultCount++;
        }
        else if (patientRecords[i] is Prescription)
        {
            prescriptionCount++;
        }
    }

    Console.WriteLine(
        $"=== Медична картка пацієнта #{patientId} ==="
    );

    Console.WriteLine(
        $"Всього записів: {patientRecords.Length} " +
        $"(діагнозів: {diagnosisCount}, " +
        $"аналізів: {labResultCount}, " +
        $"рецептів: {prescriptionCount})"
    );

    Diagnosis[] chronicDiagnoses =
        GetChronicDiagnoses(patientId);

    if (chronicDiagnoses.Length > 0)
    {
        Console.WriteLine(
            $"Хронічні діагнози ({chronicDiagnoses.Length}):"
        );

        for (int i = 0; i < chronicDiagnoses.Length; i++)
        {
            Console.WriteLine($"  {chronicDiagnoses[i]}");
        }
    }

    Prescription[] activePrescriptions =
        GetActivePrescriptions(patientId);

    if (activePrescriptions.Length > 0)
    {
        Console.WriteLine(
            $"Активні рецепти ({activePrescriptions.Length}):"
        );

        for (int i = 0; i < activePrescriptions.Length; i++)
        {
            Prescription prescription = activePrescriptions[i];

            Console.WriteLine(
                $"  {prescription} | " +
                $"до {prescription.ExpiresAt:dd.MM.yyyy}"
            );
        }
    }
}

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Медичних записів немає.");
            return;
        }

        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_records[i]);
        }
    }

    public void DisplayList(MedicalRecord[] records)
    {
        if (records == null)
        {
            throw new ArgumentNullException(nameof(records));
        }

        if (records.Length == 0)
        {
            Console.WriteLine("Список медичних записів порожній.");
            return;
        }

        for (int i = 0; i < records.Length; i++)
        {
            Console.WriteLine(records[i]);
        }
    }

    public MedicalRecord? this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                return null;
            }

            return _records[index];
        }
    }
}