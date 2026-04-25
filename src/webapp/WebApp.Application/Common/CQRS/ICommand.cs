using MediatR;
using WebApp.Domain.Common.Results;

namespace WebApp.Application.Common.CQRS;

public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
