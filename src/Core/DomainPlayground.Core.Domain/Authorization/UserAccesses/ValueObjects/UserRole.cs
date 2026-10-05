using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.SharedKernel.Abstractions;

namespace DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects
{
    public sealed class UserRole : ValueObject
    {
        public RoleId RoleId { get; }

        private UserRole() { }   // EF
        public UserRole(RoleId roleId) => RoleId = roleId;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return RoleId;
        }
    }
}
