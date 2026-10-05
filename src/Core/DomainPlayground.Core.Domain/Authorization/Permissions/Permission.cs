using DomainPlayground.Core.Domain.Authorization.Permissions.Errors;
using DomainPlayground.Core.Domain.Authorization.Permissions.Events;
using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.SharedKernel.Abstractions;
using DomainPlayground.SharedKernel.Results;

namespace DomainPlayground.Core.Domain.Authorization.Permissions
{
    public sealed class Permission : AggregateRoot<PermissionId>
    {
        public PermissionKey Key { get; private set; } = default!;
        public string Title { get; private set; } = default!;

        private Permission() { }   // EF

        private Permission(PermissionKey key, string title)
        {
            Key = key;
            Title = title;
        }

        public static Result<Permission> Register(PermissionKey key, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<Permission>(PermissionErrors.DescriptionRequired);

            var permission = new Permission(key, title.Trim());
            permission.AddDomainEvent(new PermissionRegisteredDomainEvent(key.Value));
            return Result.Success(permission);
        }

        public void Rename(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return;
            Title = title.Trim();
            MarkUpdated();
        }
    }
}