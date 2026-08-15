namespace Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

public class DependencyNotFoundException : Exception
{
    public DependencyNotFoundException(string message) : base(message) { }
}

public class ValidationAppException : Exception
{
    public IReadOnlyList<(string Field, string Message)> Errors { get; }

    public ValidationAppException(IReadOnlyList<(string Field, string Message)> errors)
        : base("Validation failed.")
    {
        Errors = errors;
    }
}
