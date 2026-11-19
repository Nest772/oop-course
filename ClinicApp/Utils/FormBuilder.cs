using System;
using System.Collections.Generic;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class FormBuilder
{
    public static T Build<T>() where T : new()
    {
        T obj = new T();
        Type type = typeof(T);
        PropertyInfo[] properties = type.GetProperties();

        for (int i = 0; i < properties.Length; i++)
        {
            PropertyInfo prop = properties[i];

            if (prop.GetIndexParameters().Length > 0)
            {
                continue;
            }

            if (prop.GetSetMethod() == null)
            {
                continue;
            }

            string prompt = BuildPrompt(prop);

            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine() ?? string.Empty;

                try
                {
                    object? converted;
                    Type? underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
                    Type targetType = underlyingType ?? prop.PropertyType;

                    if (underlyingType != null && string.IsNullOrWhiteSpace(input))
                    {
                        converted = null;
                    }
                    else if (targetType.IsEnum)
                    {
                        converted = Enum.Parse(targetType, input, ignoreCase: true);
                    }
                    else
                    {
                        converted = Convert.ChangeType(input, targetType);
                    }

                    prop.SetValue(obj, converted);
                    break;
                }
                catch
                {
                    Console.WriteLine("  Некоректне значення, спробуйте ще раз.");
                }
            }
        }

        return obj;
    }

    private static string BuildPrompt(PropertyInfo prop)
    {
        List<string> hints = new List<string>();

        if (prop.GetCustomAttribute<RequiredAttribute>() != null)
        {
            hints.Add("обов'язкове");
        }

        MaxLengthAttribute? maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
        if (maxLengthAttr != null)
        {
            hints.Add($"макс. {maxLengthAttr.Length}");
        }

        MinValueAttribute? minValueAttr = prop.GetCustomAttribute<MinValueAttribute>();
        if (minValueAttr != null)
        {
            hints.Add($"мін. {minValueAttr.Min}");
        }

        if (hints.Count > 0)
        {
            return $"  {prop.Name} ({string.Join(", ", hints)}): ";
        }

        return $"  {prop.Name}: ";
    }
}