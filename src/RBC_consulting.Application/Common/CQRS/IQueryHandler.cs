using MediatR;
using RBC_consulting.Domain.Common.Results;

namespace RBC_consulting.Application.Common.CQRS;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse> { }
