using System;

namespace ClinicApp;

public class PatientManager
{
    private const int MaxPatients = 100;
    private Patient[] _patients = new Patient[MaxPatients];
    private int _count = 0;

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public void Add(Patient patient)
    {
        if (patient == null)
        {
            return;
        }

        if (_count >= MaxPatients)
        {
            Console.WriteLine("reached maximum capacity of patients");
            return;
        }

        _patients[_count] = patient;
        _count++;
        Console.WriteLine($"Patient [{patient.Id}] {patient.FullName} added.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public Patient[] FindByName(string name)
    {
        if (name == null)
        {
            name = string.Empty;
        }

        string lowerName = name.ToLower();
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            string fullNameLower = _patients[i].FullName.ToLower();
            if (fullNameLower.Contains(lowerName))
            {
                matches++;
            }
        }

        Patient[] result = new Patient[matches];
        int resultIndex = 0;

        for (int i = 0; i < _count; i++)
        {
            string fullNameLower = _patients[i].FullName.ToLower();
            if (fullNameLower.Contains(lowerName))
            {
                result[resultIndex] = _patients[i];
                resultIndex++;
            }
        }

        return result;
    }

    public bool Remove(int id)
    {
        int targetIndex = -1;

        for (int i = 0; i < _count; i++)
        {
            if (_patients[i].Id == id)
            {
                targetIndex = i;
                break;
            }
        }

        if (targetIndex == -1)
        {
            return false;
        }

        for (int i = targetIndex; i < _count - 1; i++)
        {
            _patients[i] = _patients[i + 1];
        }

        _patients[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("Patient list is empty.");
            return;
        }

        Console.WriteLine($"=== Patients ({_count} / {MaxPatients}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_patients[i].ToString());
        }
        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("=== Patient`s statisic ===");
            Console.WriteLine("No data for statistic.");
            Console.WriteLine("============================");
            return;
        }

        int totalAge = 0;
        int adultCount = 0;
        int youngestIndex = 0;
        int oldestIndex = 0;

        for (int i = 0; i < _count; i++)
        {
            int currentAge = _patients[i].Age;
            totalAge += currentAge;

            if (_patients[i].IsAdult)
            {
                adultCount++;
            }

            if (currentAge < _patients[youngestIndex].Age)
            {
                youngestIndex = i;
            }

            if (currentAge > _patients[oldestIndex].Age)
            {
                oldestIndex = i;
            }
        }

        double averageAge = (double)totalAge / _count;

        Console.WriteLine("=== Patient`s statistic ===");
        Console.WriteLine($"Overall:       {_count}");
        Console.WriteLine($"Average age: {averageAge:F1} р.");
        Console.WriteLine($"Youngest:  {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Oldest:   {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Adults:     {adultCount} from {_count}");
        Console.WriteLine("============================");
    }
}