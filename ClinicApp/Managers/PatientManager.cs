using System;
using System.Collections.Generic;
using ClinicApp.Enums;
using ClinicApp.Models;

namespace ClinicApp.Managers;

public class PatientManager
{
    private List<Patient> _patients = new List<Patient>();

    public int Count
    {
        get
        {
            return _patients.Count;
        }
    }

    public Patient? this[int index]
    {
        get
        {
            if (index < 0 || index >= _patients.Count)
            {
                return null;
            }
            return _patients[index];
        }
    }

    public void Add(Patient patient)
    {
        if (patient == null)
        {
            return;
        }

        _patients.Add(patient);
        Console.WriteLine($"Patient [{patient.Id}] {patient.FullName} added.");
    }

    public Patient? FindById(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                return _patients[i];
            }
        }
        return null;
    }

    public bool TryFindById(int id, out Patient? patient)
    {
        patient = FindById(id);
        return patient != null;
    }

    public Patient[] FindByName(string name)
    {
        if (name == null)
        {
            name = string.Empty;
        }

        string lowerName = name.ToLower();
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            string fullNameLower = _patients[i].FullName.ToLower();
            if (fullNameLower.Contains(lowerName))
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public Patient[] FindByBloodType(BloodType bloodType)
    {
        List<Patient> result = new List<Patient>();

        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].BloodType == bloodType)
            {
                result.Add(_patients[i]);
            }
        }

        return result.ToArray();
    }

    public bool Remove(int id)
    {
        for (int i = 0; i < _patients.Count; i++)
        {
            if (_patients[i].Id == id)
            {
                _patients.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public Patient[] GetAll()
    {
        return _patients.ToArray();
    }

    public void DisplayAll()
    {
        if (_patients.Count == 0)
        {
            Console.WriteLine("Patient list is empty.");
            return;
        }

        Console.WriteLine($"=== Patients ({_patients.Count}) ===");
        for (int i = 0; i < _patients.Count; i++)
        {
            Console.WriteLine(_patients[i].ToString());
        }
        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_patients.Count == 0)
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

        for (int i = 0; i < _patients.Count; i++)
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

        double averageAge = (double)totalAge / _patients.Count;

        Console.WriteLine("=== Patient`s statistic ===");
        Console.WriteLine($"Overall:       {_patients.Count}");
        Console.WriteLine($"Average age: {averageAge:F1} р.");
        Console.WriteLine($"Youngest:   {_patients[youngestIndex].FullName} ({_patients[youngestIndex].Age} р.)");
        Console.WriteLine($"Oldest:    {_patients[oldestIndex].FullName} ({_patients[oldestIndex].Age} р.)");
        Console.WriteLine($"Adults:      {adultCount} from {_patients.Count}");
        Console.WriteLine("============================");
    }
}