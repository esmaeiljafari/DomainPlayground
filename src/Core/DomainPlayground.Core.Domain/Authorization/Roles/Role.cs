using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.Roles.Errors;
using DomainPlayground.Core.Domain.Authorization.Roles.Events;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.SharedKernel.Abstractions;
using DomainPlayground.SharedKernel.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Domain.Authorization.Roles
{
    public sealed class Role : AggregateRoot<RoleId>
    {
        private readonly List<RolePermission> _permissions = [];

        public string Name { get; private set; } = default!;
        public bool IsSystem { get; private set; }
        public bool IsActive { get; private set; } = true;

        public IReadOnlyCollection<RolePermission> Permissions => _permissions.AsReadOnly();

        private Role() { }   // EF

        private Role(string name, bool isSystem)
        {
            Name = name;
            IsSystem = isSystem;
        }

        public static Result<Role> Create(string name, bool isSystem = false) =>
            string.IsNullOrWhiteSpace(name)
                ? Result.Failure<Role>(RoleErrors.NameRequired)
                : Result.Success(new Role(name.Trim(), isSystem));

        public Result Rename(string name)
        {
            if (IsSystem) return Result.Failure(RoleErrors.SystemRoleImmutable);
            if (string.IsNullOrWhiteSpace(name)) return Result.Failure(RoleErrors.NameRequired);

            Name = name.Trim();
            MarkUpdated();
            return Result.Success();
        }

        public Result SetPermissions(IEnumerable<PermissionId> permissionIds)
        {
            if (IsSystem)
                return Result.Failure(RoleErrors.SystemRoleImmutable);

            var target = permissionIds.ToHashSet();
            var current = _permissions.Select(p => p.PermissionId).ToHashSet();

            var toRemove = current.Except(target).ToHashSet();
            var toAdd = target.Except(current).ToList();
            if (toRemove.Count == 0 && toAdd.Count == 0) return Result.Success();

            _permissions.RemoveAll(p => toRemove.Contains(p.PermissionId));
            _permissions.AddRange(toAdd.Select(id => new RolePermission(id)));

            MarkUpdated();
            if (!IsTransient())
                AddDomainEvent(new RolePermissionsChangedDomainEvent(Id));

            return Result.Success();
        }
        public Result Disable()
        {
            if (IsSystem)
                return Result.Failure(RoleErrors.SystemRoleImmutable);
            if (!IsActive)
                return Result.Success();
            IsActive = false;
            MarkUpdated();
            if (!IsTransient()) AddDomainEvent(new RoleDisabledDomainEvent(Id));
            return Result.Success();
        }

        public Result UpdateDetails(string name)
        {
            if (IsSystem) 
                return Result.Failure(RoleErrors.SystemRoleImmutable);
            if (string.IsNullOrWhiteSpace(name)) 
                return Result.Failure(RoleErrors.NameRequired);
            Name = name.Trim();
            MarkUpdated();
            return Result.Success();
        }
    }
}
