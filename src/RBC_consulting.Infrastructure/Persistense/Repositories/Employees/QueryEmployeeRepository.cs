using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RBC_consulting.Domain.Common.Paginations;
using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.Common.Errors;
using RBC_consulting.Domain.EmployeeAggregate.Ids;
using RBC_consulting.Domain.EmployeeAggregate.Projections;
using RBC_consulting.Domain.EmployeeAggregate.Repositories;
using RBC_consulting.Domain.EmployeeAggregate.Enumerations;

namespace RBC_consulting.Infrastructure.Persistense.Repositories.Employees;

internal sealed class QueryEmployeeRepository(
    IOptions<DatabaseSettings> databaseSettings,
    ILogger<QueryEmployeeRepository> logger) : IQueryEmployeeRepository
{
    private readonly string _connectionString = databaseSettings.Value.DefaultConnection;

    private IDbConnection CreateConnection => new SqlConnection(_connectionString);

    public async Task<Result<EmployeeProjection>> GetAsync(EmployeeId id)
    {
        try
        {
            using var connection = CreateConnection;
            var sql = "SELECT EmployeeID as Id, FullName, Position, Department, HireDate, Email, Phone, Salary, FilePath, CreatedAt, CASE WHEN FilePath IS NOT NULL AND FilePath <> '' THEN 1 ELSE 0 END as HasFile FROM Employees WHERE EmployeeID = @Id";
            var employee = await connection.QueryFirstOrDefaultAsync<EmployeeProjection>(sql, new { Id = id.Value });

            if (employee is null)
            {
                logger.LogWarning("Employee {EmployeeId} not found", id.Value);
                return Result.Failure<EmployeeProjection>(Error.NotFound("Employee.NotFound", $"Employee with ID {id.Value} not found."));
            }

            return Result.Success(employee);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to query employee {EmployeeId}", id.Value);
            return Result.Failure<EmployeeProjection>(Error.Failure("Employee.QueryError", ex.Message));
        }
    }

    public async Task<Result<string?>> GetFilePathAsync(EmployeeId id)
    {
        try
        {
            using var connection = CreateConnection;
            var sql = "SELECT FilePath FROM Employees WHERE EmployeeID = @Id";
            var filePath = await connection.QueryFirstOrDefaultAsync<string?>(sql, new { Id = id.Value });
            return Result.Success(filePath);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to query file path for employee {EmployeeId}", id.Value);
            return Result.Failure<string?>(Error.Failure("Employee.FileQueryError", ex.Message));
        }
    }

    public async Task<Result<EmployeeFileData>> GetFileAsync(EmployeeId id)
    {
        try
        {
            using var connection = CreateConnection;
            var sql = "SELECT FilePath, FileBlob FROM Employees WHERE EmployeeID = @Id";
            var data = await connection.QueryFirstOrDefaultAsync<EmployeeFileData>(sql, new { Id = id.Value });

            if (data is null)
            {
                logger.LogWarning("File data not found for employee {EmployeeId}", id.Value);
                return Result.Failure<EmployeeFileData>(Error.NotFound("Employee.NotFound", $"Employee with ID {id.Value} not found."));
            }

            logger.LogInformation("File blob loaded from DB for employee {EmployeeId}", id.Value);
            return Result.Success(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to query file blob for employee {EmployeeId}", id.Value);
            return Result.Failure<EmployeeFileData>(Error.Failure("Employee.FileQueryError", ex.Message));
        }
    }

    public async Task<Result<PagedList<EmployeeProjection>>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        string? sortDir = null)
    {
        try
        {
            pageNumber = Math.Max(1, pageNumber);
            pageSize   = Math.Clamp(pageSize, 1, PagedList<EmployeeProjection>.MaxPageSize);
            if (string.IsNullOrWhiteSpace(searchTerm)) searchTerm = null;

            using var connection = CreateConnection;

            var whereClause = "";
            var parameters = new DynamicParameters();

            if (searchTerm is not null)
            {
                whereClause = @" WHERE FullName LIKE @Term
                                 OR Position LIKE @Term
                                 OR Department LIKE @Term
                                 OR Email LIKE @Term
                                 OR Phone LIKE @Term";
                parameters.Add("Term", $"%{searchTerm}%");
            }

            var countSql = $"SELECT COUNT(*) FROM Employees{whereClause}";
            var totalCount = await connection.ExecuteScalarAsync<int>(countSql, parameters);

            var sortColumn = EmployeeOrderBy.GetNames()
                .FirstOrDefault(n => n.Equals(sortBy, StringComparison.OrdinalIgnoreCase))
                ?? EmployeeOrderBy.EmployeeID.Name;
            var direction = sortDir?.ToLower() == "desc" ? "DESC" : "ASC";

            var sql = $@"
                SELECT EmployeeID as Id, FullName, Position, Department, HireDate, Email, Phone, Salary, FilePath, CreatedAt, CASE WHEN FilePath IS NOT NULL AND FilePath <> '' THEN 1 ELSE 0 END as HasFile
                FROM Employees
                {whereClause}
                ORDER BY {sortColumn} {direction}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

            parameters.Add("Offset", (pageNumber - 1) * pageSize);
            parameters.Add("PageSize", pageSize);

            var items = await connection.QueryAsync<EmployeeProjection>(sql, parameters);
            return Result.Success(new PagedList<EmployeeProjection>(items, totalCount, pageNumber, pageSize));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to query paged employees");
            return Result.Failure<PagedList<EmployeeProjection>>(Error.Failure("Employee.PagedQueryError", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<EmployeeProjection>>> GetAllForExportAsync(
        string? searchTerm = null,
        string? sortBy = null,
        string? sortDir = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm)) searchTerm = null;

            using var connection = CreateConnection;

            var whereClause = "";
            var parameters = new DynamicParameters();

            if (searchTerm is not null)
            {
                whereClause = @" WHERE FullName LIKE @Term
                                 OR Position LIKE @Term
                                 OR Department LIKE @Term
                                 OR Email LIKE @Term
                                 OR Phone LIKE @Term";
                parameters.Add("Term", $"%{searchTerm}%");
            }

            var sortColumn = EmployeeOrderBy.GetNames()
                .FirstOrDefault(n => n.Equals(sortBy, StringComparison.OrdinalIgnoreCase))
                ?? EmployeeOrderBy.EmployeeID.Name;
            var direction = sortDir?.ToLower() == "desc" ? "DESC" : "ASC";

            var sql = $@"
                SELECT EmployeeID as Id, FullName, Position, Department, HireDate, Email, Phone, Salary, FilePath, CreatedAt, CASE WHEN FilePath IS NOT NULL AND FilePath <> '' THEN 1 ELSE 0 END as HasFile
                FROM Employees
                {whereClause}
                ORDER BY {sortColumn} {direction}";

            var items = await connection.QueryAsync<EmployeeProjection>(sql, parameters);
            return Result.Success(items.AsEnumerable());
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to query employees for export");
            return Result.Failure<IEnumerable<EmployeeProjection>>(Error.Failure("Employee.ExportQueryError", ex.Message));
        }
    }

    public async Task<Result<IEnumerable<EmployeeDepartmentStat>>> GetStatisticsAsync()
    {
        try
        {
            using var connection = CreateConnection;
            var stats = await connection.QueryAsync<EmployeeDepartmentStat>(
                "SELECT Department, COUNT(*) as EmployeeCount FROM Employees GROUP BY Department");
            return Result.Success(stats);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to query employee statistics");
            return Result.Failure<IEnumerable<EmployeeDepartmentStat>>(Error.Failure("Employee.StatsError", ex.Message));
        }
    }
}
