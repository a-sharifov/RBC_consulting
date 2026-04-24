namespace RBC_consulting.Contracts.UnitOfWork;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken = default);
}
