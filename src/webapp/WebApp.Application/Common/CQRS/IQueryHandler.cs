using MediatR;
using WebApp.Domain.Common.Results;

namespace WebApp.Application.Common.CQRS;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse> { }
