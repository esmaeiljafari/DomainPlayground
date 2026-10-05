namespace DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects
{
    public readonly record struct PermissionId(int Value)
    {
        public override string ToString() => Value.ToString();
    }


}
