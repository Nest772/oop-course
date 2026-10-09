namespace ClinicApp.Utils;

public static class ClinicValidator
{
    public static void ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(
                $"{fieldName} cannot be empty.",
                fieldName);
        }

        if (value.Length > 50)
        {
            throw new ArgumentException(
                $"{fieldName} cannot exceed 50 characters.",
                fieldName);
        }
    }

    public static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException(
                "Phone cannot be empty.",
                nameof(phone));
        }

        if (phone.Length != 10)
        {
            throw new ArgumentException(
                "Phone must contain exactly 10 digits.",
                nameof(phone));
        }

        for (int i = 0; i < phone.Length; i++)
        {
            if (phone[i] < '0' || phone[i] > '9')
            {
                throw new ArgumentException(
                    "Phone must contain only digits.",
                    nameof(phone));
            }
        }
    }

    public static void ValidateDate(DateTime value, string fieldName)
    {
        if (value > DateTime.Today)
        {
            throw new ArgumentOutOfRangeException(
                fieldName,
                $"{fieldName} cannot be in the future.");
        }

        if (value < new DateTime(1900, 1, 1))
        {
            throw new ArgumentOutOfRangeException(
                fieldName,
                $"{fieldName} cannot be earlier than 1900.");
        }
    }

    public static void ValidatePositive(int value, string fieldName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(
                fieldName,
                $"{fieldName} must be greater than zero.");
        }
    }
}