namespace WebApp.Contracts.UnitOfWork;

public interface IUnitOfWork
{
    Task Commit(CancellationToken cancellationToken = default);
}
