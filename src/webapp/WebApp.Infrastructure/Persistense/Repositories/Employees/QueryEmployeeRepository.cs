using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebApp.Domain.Common.Paginations;
using WebApp.Domain.Common.Results;
using WebApp.Domain.Common.Errors;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Projections;
using WebApp.Domain.EmployeeAggregate.Repositories;
using WebApp.Domain.EmployeeAggregate.Enumerations;

namespace WebApp.Infrastructure.Persistense.Repositories.Employees;

internal sealed class QueryEmployeeRepository(
    IOptions<DatabaseSettings> databaseSettings,
    ILogger<QueryEmployeeRepository> logger) : IQueryEmployeeRepository
{
    private readonly string _connectionString = databaseSettings.Value.DefaultConnection;

    private IDbConnection CreateConnection => new SqlConnection(_connectionString);

    public async Task<Result<EmployeeProjection>> GetAsync(EmployeeId id)
    {
        using var connection = CreateConnection;
        const string sql = "SELECT EmployeeID as Id, FullName, Position, Department, HireDate, Email, Phone, Salary, FilePath, CreatedAt, CASE WHEN FilePath IS NOT NULL AND FilePath <> '' THEN 1 ELSE 0 END as HasFile FROM Employees WHERE EmployeeID = @Id";
        var employee = await connection.QueryFirstOrDefaultAsync<EmployeeProjection>(sql, new { Id = id.Value });

        if (employee is null)
        {
            logger.LogWarning("Employee {EmployeeId} not found", id.Value);
            return Result.Failure<EmployeeProjection>(Error.NotFound("Employee.NotFound", $"Employee with ID {id.Value} not found."));
        }

        return Result.Success(employee);
    }

    public async Task<Result<string?>> GetFilePathAsync(EmployeeId id)
    {
        using var connection = CreateConnection;
        const string sql = "SELECT FilePath FROM Employees WHERE EmployeeID = @Id";
        var filePath = await connection.QueryFirstOrDefaultAsync<string?>(sql, new { Id = id.Value });
        return Result.Success(filePath);
    }

    public async Task<Result<EmployeeFileData>> GetFileAsync(EmployeeId id)
    {
        using var connection = CreateConnection;
        const string sql = "SELECT FilePath, FileBlob FROM Employees WHERE EmployeeID = @Id";
        var data = await connection.QueryFirstOrDefaultAsync<EmployeeFileData>(sql, new { Id = id.Value });

        if (data is null)
        {
            logger.LogWarning("File data not found for employee {EmployeeId}", id.Value);
            return Result.Failure<EmployeeFileData>(Error.NotFound("Employee.NotFound", $"Employee with ID {id.Value} not found."));
        }

        logger.LogInformation("File blob loaded from DB for employee {EmployeeId}", id.Value);
        return Result.Success(data);
    }

    public async Task<Result<PagedList<EmployeeProjection>>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        string? sortDir = null)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize   = Math.Clamp(pageSize, 1, PagedList<EmployeeProjection>.MaxPageSize);
        if (string.IsNullOrWhiteSpace(searchTerm)) searchTerm = null;

        using var connection = CreateConnection;

        var (whereClause, parameters) = BuildSearchClause(searchTerm);
        var orderClause = BuildOrderClause(sortBy, sortDir);

        var countSql = $"SELECT COUNT(*) FROM Employees{whereClause}";
        var totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

        var sql = $@"
            SELECT EmployeeID as Id, FullName, Position, Department, HireDate, Email, Phone, Salary, FilePath, CreatedAt, CASE WHEN FilePath IS NOT NULL AND FilePath <> '' THEN 1 ELSE 0 END as HasFile
            FROM Employees
            {whereClause}
            {orderClause}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

        parameters.Add("Offset", (pageNumber - 1) * pageSize);
        parameters.Add("PageSize", pageSize);

        var items = await connection.QueryAsync<EmployeeProjection>(sql, parameters);
        return Result.Success(new PagedList<EmployeeProjection>(items, totalCount, pageNumber, pageSize));
    }

    public async Task<Result<IEnumerable<EmployeeExportProjection>>> GetForExportPdfAsync(
        string? searchTerm = null,
        string? sortBy = null,
        string? sortDir = null)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) searchTerm = null;

        using var connection = CreateConnection;

        var (whereClause, parameters) = BuildSearchClause(searchTerm);
        var orderClause = BuildOrderClause(sortBy, sortDir);

        var sql = $@"
            SELECT EmployeeID as Id, FullName, Position, Department, HireDate, Email, Phone, Salary
            FROM vw_EmployeesForExport
            {whereClause}
            {orderClause}";

        var items = await connection.QueryAsync<EmployeeExportProjection>(sql, parameters);
        return Result.Success(items.AsEnumerable());
    }

    public async Task<Result<IEnumerable<EmployeeDepartmentStat>>> GetStatisticsAsync()
    {
        using var connection = CreateConnection;
        var stats = await connection.QueryAsync<EmployeeDepartmentStat>(
            "SELECT Department, COUNT(*) as EmployeeCount FROM Employees GROUP BY Department");
        return Result.Success(stats);
    }

    private static (string whereClause, DynamicParameters parameters) BuildSearchClause(string? searchTerm)
    {
        var parameters = new DynamicParameters();
        if (searchTerm is null) return (string.Empty, parameters);

        const string whereClause = @" WHERE FullName LIKE @Term
                                       OR Position LIKE @Term
                                       OR Department LIKE @Term
                                       OR Email LIKE @Term
                                       OR Phone LIKE @Term";
        parameters.Add("Term", $"%{searchTerm}%");
        return (whereClause, parameters);
    }

    private static string BuildOrderClause(string? sortBy, string? sortDir)
    {
        var sortColumn = EmployeeOrderBy.GetNames()
            .FirstOrDefault(n => n.Equals(sortBy, StringComparison.OrdinalIgnoreCase))
            ?? EmployeeOrderBy.EmployeeID.Name;
        var direction = string.Equals(sortDir, "desc", StringComparison.OrdinalIgnoreCase) ? "DESC" : "ASC";
        return $"ORDER BY {sortColumn} {direction}";
    }
}
