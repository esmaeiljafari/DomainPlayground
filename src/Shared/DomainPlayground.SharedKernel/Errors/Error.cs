namespace DomainPlayground.SharedKernel.Errors;

public enum ErrorType { Failure, Validation, NotFound, Conflict, Unauthorized, Forbidden }
public interface IError
{
    string Code { get; }
    ErrorType Type { get; }
}

public sealed record Error(string Code, ErrorType Type = ErrorType.Failure) : IError
{
    public static readonly Error None = new(string.Empty);
}