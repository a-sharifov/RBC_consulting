using WebApp.Domain.Common.Aggregates;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Projections;
using WebApp.Domain.EmployeeAggregate.ValueObjects;

namespace WebApp.Domain.EmployeeAggregate;

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

    public static Result<Employee> ReconstructFromRaw(
        int id,
        string fullName,
        string position,
        string department,
        DateTime hireDate,
        string? email,
        string? phone,
        decimal? salary,
        DateTime createdAt,
        byte[]? fileBlob = null,
        string? filePath = null)
    {
        var fullNameResult = EmployeeFullName.Create(fullName);
        if (fullNameResult.IsFailure) return Result.Failure<Employee>(fullNameResult.Error);

        var positionResult = Position.Create(position);
        if (positionResult.IsFailure) return Result.Failure<Employee>(positionResult.Error);

        var departmentResult = Department.Create(department);
        if (departmentResult.IsFailure) return Result.Failure<Employee>(departmentResult.Error);

        Email? emailVo = null;
        if (email is not null)
        {
            var r = Email.Create(email);
            if (r.IsFailure) return Result.Failure<Employee>(r.Error);
            emailVo = r.Value;
        }

        Phone? phoneVo = null;
        if (phone is not null)
        {
            var r = Phone.Create(phone);
            if (r.IsFailure) return Result.Failure<Employee>(r.Error);
            phoneVo = r.Value;
        }

        Salary? salaryVo = null;
        if (salary is not null)
        {
            var r = Salary.Create(salary.Value);
            if (r.IsFailure) return Result.Failure<Employee>(r.Error);
            salaryVo = r.Value;
        }

        return Reconstruct(
            new EmployeeId(id),
            fullNameResult.Value,
            positionResult.Value,
            departmentResult.Value,
            hireDate,
            emailVo,
            phoneVo,
            salaryVo,
            createdAt,
            fileBlob,
            filePath);
    }

    public static Result<Employee> ReconstructFromProjection(EmployeeProjection projection) =>
        ReconstructFromRaw(
            projection.Id,
            projection.FullName,
            projection.Position,
            projection.Department,
            projection.HireDate,
            projection.Email,
            projection.Phone,
            projection.Salary,
            projection.CreatedAt,
            null,
            projection.FilePath);

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
