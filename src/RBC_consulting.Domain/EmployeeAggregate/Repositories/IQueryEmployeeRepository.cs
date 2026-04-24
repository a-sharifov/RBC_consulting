using RBC_consulting.Domain.Common.Paginations;
using RBC_consulting.Domain.Common.Results;
using RBC_consulting.Domain.EmployeeAggregate.Ids;
using RBC_consulting.Domain.EmployeeAggregate.Projections;

namespace RBC_consulting.Domain.EmployeeAggregate.Repositories;

public interface IQueryEmployeeRepository
{
    public Task<Result<EmployeeProjection>> GetAsync(EmployeeId id);
    public Task<Result<string?>> GetFilePathAsync(EmployeeId id);
    public Task<Result<EmployeeFileData>> GetFileAsync(EmployeeId id);
    public Task<Result<PagedList<EmployeeProjection>>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm = null,
        string? sortBy = null,
        string? sortDir = null);

    public Task<Result<IEnumerable<EmployeeProjection>>> GetAllForExportAsync(
        string? searchTerm = null,
        string? sortBy = null,
        string? sortDir = null);

    public Task<Result<IEnumerable<EmployeeDepartmentStat>>> GetStatisticsAsync();
}
