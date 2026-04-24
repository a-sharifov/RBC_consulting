using RBC_consulting.Domain.Common.Errors;

namespace RBC_consulting.Domain.EmployeeAggregate.Errors;

public static class PhoneErrors
{
    public static readonly Error TooLong = Error.Validation(
        "Phone.TooLong",
        "Phone cannot exceed 20 characters");

    public static readonly Error Invalid = Error.Validation(
        "Phone.Invalid",
        "Phone format is invalid. Use E.164 format (e.g., +1234567890)");

    public static readonly Error Empty = Error.Validation(
        "Phone.Empty",
        "Phone cannot be empty");

}
