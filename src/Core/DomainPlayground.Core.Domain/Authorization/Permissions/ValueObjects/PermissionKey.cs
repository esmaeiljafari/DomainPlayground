using DomainPlayground.Core.Domain.Authorization.Permissions.Errors;
using DomainPlayground.SharedKernel.Abstractions;
using DomainPlayground.SharedKernel.Results;
using System.Text.RegularExpressions;

namespace DomainPlayground.Core.Domain.Authorization.Permissions.ValueObjects
{

    public sealed class PermissionKey : ValueObject
    {
        private static readonly Regex Pattern =
            new(@"^[a-z][a-z0-9]*\.[a-z][a-z0-9]*\.[a-z][a-z0-9]*$", RegexOptions.Compiled);

        public string Value { get; }
        public string Module => Value.Split('.')[0];
        public string Resource => Value.Split('.')[1];
        public string Action => Value.Split('.')[2];

        private PermissionKey(string value) => Value = value;

        public static Result<PermissionKey> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Pattern.IsMatch(value))
                return Result.Failure<PermissionKey>(PermissionErrors.InvalidKey);

            return Result.Success(new PermissionKey(value));
        }

        public override string ToString() => Value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }


}
