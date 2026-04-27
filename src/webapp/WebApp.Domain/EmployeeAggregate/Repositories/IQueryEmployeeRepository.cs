using WebApp.Domain.Common.Paginations;
using WebApp.Domain.Common.Results;
using WebApp.Domain.EmployeeAggregate.Ids;
using WebApp.Domain.EmployeeAggregate.Projections;

namespace WebApp.Domain.EmployeeAggregate.Repositories;

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

    public Task<Result<IEnumerable<EmployeeExportProjection>>> GetForExportPdfAsync(
        string? searchTerm = null,
        string? sortBy = null,
        string? sortDir = null);

    public Task<Result<IEnumerable<EmployeeDepartmentStat>>> GetStatisticsAsync();
}
