using DomainPlayground.SharedKernel.Abstractions;

namespace DomainPlayground.Core.Domain.Authorization.UserAccesses.ValueObjects
{
    public readonly record struct UserId(Guid Value) 
    {
        public static UserId New() => new(Guid.NewGuid());
        public static UserId Empty => new(Guid.Empty);
        public override string ToString() => Value.ToString();
    }
}
