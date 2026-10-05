using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Domain.Authorization.Roles.ValueObjects
{
    public readonly record struct RoleId(int Value)
    {
        public override string ToString() => Value.ToString();
    }
}
