namespace DomainPlayground.Core.Application.Common.Abstractions.Security;

[AttributeUsage(AttributeTargets.Class)]
public sealed class HasPermissionAttribute(string moduleCode, string permissionCode, string permissionTitle) : Attribute
{
    public string ModuleCode { get; } = moduleCode;
    public string PermissionCode { get; } = permissionCode;
    public string PermissionTitle { get; } = permissionTitle;
}