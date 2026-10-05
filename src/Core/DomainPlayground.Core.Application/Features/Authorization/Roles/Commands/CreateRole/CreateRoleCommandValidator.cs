using FluentValidation;

namespace DomainPlayground.Core.Application.Features.Authorization.Roles.Commands.CreateRole;

public sealed class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator() =>
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
}
