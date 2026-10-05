using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.SharedKernel.Abstractions;

namespace DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects
{
    public sealed class RolePermission : ValueObject
    {
        public PermissionId PermissionId { get; }

        private RolePermission() { }   // EF
        public RolePermission(PermissionId permissionId) => PermissionId = permissionId;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return PermissionId;
        }
    }
}
