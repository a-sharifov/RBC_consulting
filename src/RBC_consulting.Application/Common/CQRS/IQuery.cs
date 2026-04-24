using MediatR;
using RBC_consulting.Domain.Common.Results;

namespace RBC_consulting.Application.Common.CQRS;

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }
