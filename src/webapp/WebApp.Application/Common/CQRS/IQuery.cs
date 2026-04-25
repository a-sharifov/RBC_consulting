using MediatR;
using WebApp.Domain.Common.Results;

namespace WebApp.Application.Common.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
