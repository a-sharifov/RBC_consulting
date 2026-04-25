using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApp.Domain.Common.Errors;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Repositories;
using VO = WebApp.Domain.EmployeeAggregate.ValueObjects;

namespace WebApp.Infrastructure.Persistense.Repositories.Employees;

internal sealed class CommandEmployeeRepository(
    IOptions<DatabaseSettings> databaseSettings,
    ILogger<CommandEmployeeRepository> logger) : ICommandEmployeeRepository
{
    private readonly string _connectionString = databaseSettings.Value.DefaultConnection;

    private IDbConnection CreateConnection => new SqlConnection(_connectionString);

    public async Task<Result<EmployeeId>> AddAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection;
        const string sql = """
            INSERT INTO Employees (FullName, Position, Department, HireDate, Email, Phone, Salary, FileBlob, FilePath, CreatedAt)
            VALUES (@FullName, @Position, @Department, @HireDate, @Email, @Phone, @Salary, @FileBlob, @FilePath, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() as int);
            """;

        var id = await connection.ExecuteScalarAsync<int>(sql, ToParameters(entity));
        logger.LogInformation("Employee created with ID {EmployeeId}", id);
        return Result.Success(new EmployeeId(id));
    }

    public Task<Result> DeleteAsync(Employee entity, CancellationToken cancellationToken = default) =>
        DeleteAsync(entity.Id, cancellationToken);

    public async Task<Result> DeleteAsync(EmployeeId id, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection;
        var affected = await connection.ExecuteAsync(
            "DELETE FROM Employees WHERE EmployeeID = @Id",
            new { Id = id.Value });

        if (affected > 0)
        {
            logger.LogInformation("Employee {EmployeeId} deleted", id.Value);
            return Result.Success();
        }

        logger.LogWarning("Delete: employee {EmployeeId} not found", id.Value);
        return Result.Failure(Error.NotFound("Employee.NotFound", $"Employee with ID {id.Value} was not found."));
    }

    public async Task<Result<Employee>> GetAsync(EmployeeId id, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection;
        const string sql = """
            SELECT EmployeeID, FullName, Position, Department, HireDate, Email, Phone, Salary, FileBlob, FilePath, CreatedAt
            FROM Employees WHERE EmployeeID = @Id
            """;

        var row = await connection.QueryFirstOrDefaultAsync<EmployeeRow>(sql, new { Id = id.Value });
        if (row is null)
        {
            logger.LogWarning("Employee {EmployeeId} not found", id.Value);
            return Result.Failure<Employee>(Error.NotFound("Employee.NotFound", $"Employee with ID {id.Value} not found."));
        }

        return Employee.ReconstructFromRaw(
            row.EmployeeID,
            row.FullName,
            row.Position,
            row.Department,
            row.HireDate,
            row.Email,
            row.Phone,
            row.Salary,
            row.CreatedAt,
            row.FileBlob,
            row.FilePath);
    }

    public async Task<bool> IsExistAsync(EmployeeId id, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection;
        var count = await connection.ExecuteScalarAsync<int>(
            "SELECT COUNT(1) FROM Employees WHERE EmployeeID = @Id",
            new { Id = id.Value });
        return count > 0;
    }

    public async Task<Result> UpdateAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection;
        const string sql = """
            UPDATE Employees SET
                FullName = @FullName,
                Position = @Position,
                Department = @Department,
                HireDate = @HireDate,
                Email = @Email,
                Phone = @Phone,
                Salary = @Salary,
                FileBlob = @FileBlob,
                FilePath = @FilePath
            WHERE EmployeeID = @Id
            """;

        var affected = await connection.ExecuteAsync(sql, ToParameters(entity, includeId: true));
        if (affected > 0)
        {
            logger.LogInformation("Employee {EmployeeId} updated", entity.Id.Value);
            return Result.Success();
        }

        logger.LogWarning("Update: employee {EmployeeId} not found", entity.Id.Value);
        return Result.Failure(Error.NotFound("Employee.NotFound", $"Employee with ID {entity.Id.Value} not found for update."));
    }

    private static object ToParameters(Employee entity, bool includeId = false) => includeId
        ? new
        {
            Id = entity.Id.Value,
            FullName = entity.FullName.Value,
            Position = entity.Position.Value,
            Department = entity.Department.Value,
            entity.HireDate,
            Email = entity.Email?.Value,
            Phone = entity.Phone?.Value,
            Salary = entity.Salary?.Value,
            entity.FileBlob,
            entity.FilePath,
            entity.CreatedAt,
        }
        : new
        {
            FullName = entity.FullName.Value,
            Position = entity.Position.Value,
            Department = entity.Department.Value,
            entity.HireDate,
            Email = entity.Email?.Value,
            Phone = entity.Phone?.Value,
            Salary = entity.Salary?.Value,
            entity.FileBlob,
            entity.FilePath,
            entity.CreatedAt,
        };

    private sealed class EmployeeRow
    {
        public int EmployeeID { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public decimal? Salary { get; set; }
        public byte[]? FileBlob { get; set; }
        public string? FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
