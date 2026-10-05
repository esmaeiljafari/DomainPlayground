using DomainPlayground.Core.Application.Common.Abstractions.Messaging;
using DomainPlayground.Core.Application.Common.Behaviors;
using DomainPlayground.Core.Application.Common.Interfaces.Persistence;
using DomainPlayground.SharedKernel.Errors;
using DomainPlayground.SharedKernel.Results;
using Moq;
using Xunit;

namespace DomainPlayground.Core.Application.UnitTests.Behaviors;

public class UnitOfWorkBehaviorTests
{
    [Fact]
    public async Task Handle_WhenHandlerSucceeds_CallsSaveChanges()
    {
        var uow = new Mock<IUnitOfWork>();
        var behavior = new UnitOfWorkBehavior<TestCommand, Result>(uow.Object);

        await behavior.Handle(
            new TestCommand(),
            _ => Task.FromResult(Result.Success()),
            default);

        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenHandlerFails_DoesNotCallSaveChanges()
    {
        var uow = new Mock<IUnitOfWork>();
        var behavior = new UnitOfWorkBehavior<TestCommand, Result>(uow.Object);
        var error = new Error("Test.Error", ErrorType.Failure);

        await behavior.Handle(
            new TestCommand(),
            _ => Task.FromResult(Result.Failure(error)),
            default);

        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    private sealed record TestCommand : ICommand;
}
