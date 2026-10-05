using DomainPlayground.Core.Domain.Authorization.UserAccesses.Enums;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.Events;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using DomainPlayground.SharedKernel.Abstractions;
using DomainPlayground.SharedKernel.Results;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.Errors;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;

namespace DomainPlayground.Core.Domain.Authorization.UserAccesses
{
    public sealed class UserAccess : AggregateRoot<UserAccessId>
    {
        private readonly List<UserRole> _roles = [];
        private readonly List<UserPermissionOverride> _overrides = [];

        public UserId UserId { get; private set; }
        public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();
        public IReadOnlyCollection<UserPermissionOverride> Overrides => _overrides.AsReadOnly();

        private UserAccess() { }// EF

        private UserAccess(UserAccessId id, UserId userId) : base(id) => UserId = userId;

        public static UserAccess Create(UserId userId) => new(UserAccessId.New(), userId);

        public Result AssignRole(RoleId roleId)
        {
            if (_roles.Any(r => r.RoleId == roleId))
                return Result.Failure(UserAccessErrors.RoleAlreadyAssigned);

            _roles.Add(new UserRole(roleId));
            MarkUpdated();
            AddDomainEvent(new UserRoleAssignedDomainEvent(UserId, roleId));
            return Result.Success();
        }

        public Result RemoveRole(RoleId roleId)
        {
            var role = _roles.FirstOrDefault(r => r.RoleId == roleId);
            if (role is null)
                return Result.Failure(UserAccessErrors.RoleNotAssigned);

            _roles.Remove(role);
            MarkUpdated();
            AddDomainEvent(new UserRoleRemovedDomainEvent(this.UserId, roleId));
            return Result.Success();
        }

        public void SetOverride(PermissionId permissionId, AccessEffect effect)
        {
            _overrides.RemoveAll(o => o.PermissionId == permissionId);
            _overrides.Add(new UserPermissionOverride(permissionId, effect));

            MarkUpdated();
            AddDomainEvent(new UserOverridesChangedDomainEvent(UserId));
        }

        public void RemoveOverride(PermissionId permissionId)
        {
            if (_overrides.RemoveAll(o => o.PermissionId == permissionId) == 0) return;

            MarkUpdated();
            AddDomainEvent(new UserOverridesChangedDomainEvent(UserId));
        }
        public void SetRoles(IEnumerable<RoleId> roleIds)
        {
            var target = roleIds.ToHashSet();
            var current = _roles.Select(r => r.RoleId).ToHashSet();

            var toRemove = current.Except(target).ToHashSet();
            var toAdd = target.Except(current).ToList();
            if (toRemove.Count == 0 && toAdd.Count == 0) return;

            _roles.RemoveAll(r => toRemove.Contains(r.RoleId));
            _roles.AddRange(toAdd.Select(id => new UserRole(id)));

            MarkUpdated();
            AddDomainEvent(new UserRolesChangedDomainEvent(UserId));
        }
    }
}
