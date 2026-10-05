using FluentValidation;

namespace DomainPlayground.Core.Application.Features.Authorization.UserAccesses.Commands.UpdateUserRoles
{
    public sealed class UpdateUserRolesCommandValidator : AbstractValidator<UpdateUserRolesCommand>
    {
        public UpdateUserRolesCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.RoleIds).NotNull();
        }
    }
}
