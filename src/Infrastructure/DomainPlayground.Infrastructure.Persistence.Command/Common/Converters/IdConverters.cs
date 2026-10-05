using DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects;
using DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Infrastructure.Persistence.Commands.Common.Converters
{
    public sealed class UserIdConverter() : ValueConverter<UserId, Guid>(v => v.Value, v => new UserId(v));
    public sealed class UserAccessIdConverter() : ValueConverter<UserAccessId, Guid>(v => v.Value, v => new UserAccessId(v));
    public sealed class RoleIdConverter() : ValueConverter<RoleId, int>(v => v.Value, v => new RoleId(v));
    public sealed class PermissionIdConverter() : ValueConverter<PermissionId, int>(v => v.Value, v => new PermissionId(v));
}
