using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.Enums;
using DomainPlayground.SharedKernel.Abstractions;

namespace DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects
{
    public sealed class UserPermissionOverride : ValueObject
    {
        public PermissionId PermissionId { get; }
        public AccessEffect Effect { get; }

        private UserPermissionOverride() { }   // EF
        public UserPermissionOverride(PermissionId permissionId, AccessEffect effect)
        {
            PermissionId = permissionId;
            Effect = effect;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PermissionId;
            yield return Effect;
        }
    }
}
