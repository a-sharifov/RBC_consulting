using RBC_consulting.Domain.Common.Aggregates;
using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.EmployeeAggregate.Ids;
using RBC_consulting.Domain.EmployeeAggregate.Projections;
using RBC_consulting.Domain.EmployeeAggregate.ValueObjects;

namespace RBC_consulting.Domain.EmployeeAggregate;

public class Employee : AggregateRoot<EmployeeId>
{
    public EmployeeFullName FullName { get; private set; } = null!;
    public Position Position { get; private set; } = null!;
    public Department Department { get; private set; } = null!;
    public DateTime HireDate { get; private set; }
    public Email? Email { get; private set; }
    public Phone? Phone { get; private set; }
    public Salary? Salary { get; private set; }
    public byte[]? FileBlob { get; private set; }
    public string? FilePath { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public bool HasFile => !string.IsNullOrEmpty(FilePath);

    private Employee() { }

    private Employee(
        EmployeeFullName fullName,
        Position position,
        Department department,
        DateTime hireDate,
        Email? email,
        Phone? phone,
        Salary? salary,
        DateTime createdAt)
    {
        FullName = fullName;
        Position = position;
        Department = department;
        HireDate = hireDate;
        Email = email;
        Phone = phone;
        Salary = salary;
        CreatedAt = createdAt;
    }

    public static Result<Employee> Create(
        EmployeeFullName fullName,
        Position position,
        Department department,
        DateTime hireDate,
        Email? email,
        Phone? phone,
        Salary? salary,
        DateTime createdAt) =>
        Result.Success(new Employee(fullName, position, department, hireDate, email, phone, salary, createdAt));

    public static Employee Reconstruct(
        EmployeeId id,
        EmployeeFullName fullName,
        Position position,
        Department department,
        DateTime hireDate,
        Email? email,
        Phone? phone,
        Salary? salary,
        DateTime createdAt,
        byte[]? fileBlob = null,
        string? filePath = null) =>
        new(fullName, position, department, hireDate, email, phone, salary, createdAt)
        {
            Id = id,
            FileBlob = fileBlob,
            FilePath = filePath
        };

    public static Employee ReconstructFromProjection(EmployeeProjection projection)
    {
        var email = projection.Email is null ? null : Email.Create(projection.Email).Value;
        var phone = projection.Phone is null ? null : Phone.Create(projection.Phone).Value;
        var salary = projection.Salary is null ? null : Salary.Create(projection.Salary.Value).Value;

        return Reconstruct(
            new EmployeeId(projection.Id),
            EmployeeFullName.Create(projection.FullName).Value,
            Position.Create(projection.Position).Value,
            Department.Create(projection.Department).Value,
            projection.HireDate,
            email,
            phone,
            salary,
            projection.CreatedAt,
            null,
            projection.FilePath);
    }

    public Result Update(
        EmployeeFullName fullName,
        Position position,
        Department department,
        DateTime hireDate,
        Email? email,
        Phone? phone,
        Salary? salary)
    {
        FullName = fullName;
        Position = position;
        Department = department;
        HireDate = hireDate;
        Email = email;
        Phone = phone;
        Salary = salary;

        return Result.Success();
    }

    public void SetFile(byte[] blob, string path)
    {
        FileBlob = blob;
        FilePath = path;
    }

    public void ClearFile()
    {
        FileBlob = null;
        FilePath = null;
    }
}
