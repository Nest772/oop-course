using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ClinicApp.Attributes;

namespace ClinicApp.Utils;

public static class ModelValidator
{
    public static ValidationResult Validate(object obj)
    {
        ArgumentNullException.ThrowIfNull(obj);

        ValidationResult result = new ValidationResult();
        Type type = obj.GetType();
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo prop in properties)
        {
            if (prop.GetIndexParameters().Length > 0)
            {
                continue;
            }

            object? value = prop.GetValue(obj);

            RequiredAttribute? requiredAttr = prop.GetCustomAttribute<RequiredAttribute>();
            if (requiredAttr != null)
            {
                bool isEmpty = value == null || (value is string str && string.IsNullOrWhiteSpace(str));
                if (isEmpty)
                {
                    result.AddError($"{prop.Name}: {requiredAttr.ErrorMessage}");
                }
            }

            MaxLengthAttribute? maxLengthAttr = prop.GetCustomAttribute<MaxLengthAttribute>();
            if (maxLengthAttr != null && value is string text)
            {
                if (text.Length > maxLengthAttr.Length)
                {
                    result.AddError($"{prop.Name}: {maxLengthAttr.ErrorMessage}");
                }
            }

            MinValueAttribute? minValueAttr = prop.GetCustomAttribute<MinValueAttribute>();
            if (minValueAttr != null && value != null)
            {
                try
                {
                    double numericValue = Convert.ToDouble(value);
                    if (numericValue < minValueAttr.Min)
                    {
                        result.AddError($"{prop.Name}: {minValueAttr.ErrorMessage}");
                    }
                }
                catch (Exception ex) when (ex is InvalidCastException or FormatException or OverflowException)
                {
                    // Ігноруємо, якщо атрибут застосовано до нечислового типу
                }
            }
        }

        return result;
    }

    public static void PrintInfo(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        Console.WriteLine($"=== Клас: {type.Name} ===");
        PropertyInfo[] properties = type.GetProperties();

        foreach (PropertyInfo prop in properties)
        {
            if (prop.GetIndexParameters().Length > 0)
            {
                continue;
            }

            List<Attribute> attrs = prop.GetCustomAttributes().ToList();
            string attrInfo = attrs.Count > 0
                ? string.Join(", ", attrs.Select(FormatAttribute))
                : "без атрибутів";

            Console.WriteLine($"  {prop.Name} ({prop.PropertyType.Name}) — {attrInfo}");
        }
    }

    private static string FormatAttribute(Attribute attr)
    {
        string name = attr.GetType().Name.Replace("Attribute", "");

        return attr switch
        {
            MaxLengthAttribute max => $"[{name}({max.Length})]",
            MinValueAttribute min => $"[{name}({min.Min})]",
            RequiredAttribute => $"[{name}]",
            _ => $"[{name}]"
        };
    }
}