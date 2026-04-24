using MediatR;
using RBC_consulting.Domain.Common.Results;

namespace RBC_consulting.Application.Common.CQRS;

public interface ICommand : IRequest<Result> { }
public interface ICommand<TResponse> : IRequest<Result<TResponse>> { }
