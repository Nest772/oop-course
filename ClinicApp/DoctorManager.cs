using System;

namespace ClinicApp;

public class DoctorManager
{
    private const int MaxDoctors = 50;
    private Doctor[] _doctors = new Doctor[MaxDoctors];
    private int _count = 0;

    public int Count
    {
        get
        {
            return _count;
        }
    }

    public void Add(Doctor doctor)
    {
        if (doctor == null)
        {
            return;
        }

        if (_count >= MaxDoctors)
        {
            Console.WriteLine("reached maximum capacity of doctors");
            return;
        }

        _doctors[_count] = doctor;
        _count++;
        Console.WriteLine($"Doctor [{doctor.Id}] {doctor.FullName} added.");
    }

    public Doctor? FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
            {
                return _doctors[i];
            }
        }
        return null;
    }

    public Doctor[] FindBySpeciality(string speciality)
    {
        if (speciality == null)
        {
            speciality = string.Empty;
        }

        string lowerSpec = speciality.ToLower();
        int matches = 0;

        for (int i = 0; i < _count; i++)
        {
            string doctorSpecLower = _doctors[i].Speciality.ToLower();
            if (doctorSpecLower.Contains(lowerSpec))
            {
                matches++;
            }
        }

        Doctor[] result = new Doctor[matches];
        int resultIndex = 0;

        for (int i = 0; i < _count; i++)
        {
            string doctorSpecLower = _doctors[i].Speciality.ToLower();
            if (doctorSpecLower.Contains(lowerSpec))
            {
                result[resultIndex] = _doctors[i];
                resultIndex++;
            }
        }

        return result;
    }

    public Doctor[] GetAll()
    {
        Doctor[] copy = new Doctor[_count];
        for (int i = 0; i < _count; i++)
        {
            copy[i] = _doctors[i];
        }
        return copy;
    }

    public bool Remove(int id)
    {
        int targetIndex = -1;

        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].Id == id)
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
            _doctors[i] = _doctors[i + 1];
        }

        _doctors[_count - 1] = null!;
        _count--;

        return true;
    }

    public void DisplayAll()
    {
        if (_count == 0)
        {
            Console.WriteLine("List of doctors is empty.");
            return;
        }

        Console.WriteLine($"=== Лікарі ({_count} / {MaxDoctors}) ===");
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_doctors[i].ToString());
        }
        Console.WriteLine("────────────────────────────────────────────────────────────");
    }

    public void DisplayStats()
    {
        if (_count == 0)
        {
            Console.WriteLine("=== Doctor`s statistic ===");
            Console.WriteLine("No data for statistic.");
            Console.WriteLine("==========================");
            return;
        }

        int availableNowCount = 0;
        for (int i = 0; i < _count; i++)
        {
            if (_doctors[i].IsAvailableNow)
            {
                availableNowCount++;
            }
        }

        Console.WriteLine("=== Doctor`s statistic ===");
        Console.WriteLine($"Overall:         {_count}");
        Console.WriteLine($"Available now: {availableNowCount}");
        Console.WriteLine("By speciality:");

        for (int i = 0; i < _count; i++)
        {
            string currentSpec = _doctors[i].Speciality;
            bool isAlreadyProcessed = false;

            for (int j = 0; j < i; j++)
            {
                if (_doctors[j].Speciality.ToLower() == currentSpec.ToLower())
                {
                    isAlreadyProcessed = true;
                    break;
                }
            }

            if (isAlreadyProcessed == false)
            {
                int specCount = 0;
                for (int k = 0; k < _count; k++)
                {
                    if (_doctors[k].Speciality.ToLower() == currentSpec.ToLower())
                    {
                        specCount++;
                    }
                }
                Console.WriteLine($"  {currentSpec}: {specCount}");
            }
        }

        Console.WriteLine("==========================");
    }
}