namespace DomainPlayground.SharedKernel.Abstractions;

public static class ModuleCodes
{
    public const string Authorization = "Authorization";
    public const string User = "User";

    private static readonly Dictionary<string, string> Titles = new(StringComparer.OrdinalIgnoreCase)
    {
        [Authorization] = "احراز هویت",
        [User] = "کاربران"
    };

    public static string GetTitle(string moduleCode) =>
        Titles.TryGetValue(moduleCode, out var title) ? title : moduleCode;
}