using System;
using System.Collections.Generic;
using System.Text;

namespace DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects
{
    public readonly record struct UserAccessId(Guid Value)
    {
        public static UserAccessId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
