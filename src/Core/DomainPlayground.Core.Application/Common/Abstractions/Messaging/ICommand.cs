using DomainPlayground.SharedKernel.Results;
using MediatR;

namespace DomainPlayground.Core.Application.Common.Abstractions.Messaging;


public interface ICommandMarker { }

public interface ICommand : IRequest<Result>, ICommandMarker { }

public interface ICommand<TResponse> : IRequest<Result<TResponse>>, ICommandMarker { }

public interface IQuery<TResponse> : IRequest<Result<TResponse>> { }

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{ }

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{ }

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{ }