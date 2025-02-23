using BiteAI.Services.Validation.Errors;

namespace BiteAI.Services.Validation.Result;

public class Result
{
    private readonly List<Error> _errors = new();
        
    public bool IsSuccess => this._errors.Count == 0;
    
    public bool IsFailure => !this.IsSuccess;
    
    public IReadOnlyList<Error> Errors => this._errors.AsReadOnly();
    
    public string? SuccessMessage { get; protected set; }

    protected Result(string? message = null)
    {
        this.SuccessMessage = message;
    }

    private void AddError(Error error) => this._errors.Add(error);

    public static Result Success(string? message = null) => new(message);

    public static Result<T> Success<T>(T value, string? message = null) => new(value, message);

    public static Result Fail(Error error)
    {
        var result = new Result();
        result.AddError(error);
        return result;
    }
    
    public static Result<T> Fail<T>(Error error)
    {
        var result = new Result<T>(default);
        result.AddError(error);
        return result;
    }
}

public class Result<T> : Result
{
    public T? Data { get; }

    protected internal Result(T? value, string? message = null) 
        : base(message)
    {
        this.Data = value;
    }

    public static implicit operator Result<T>(T value) => new(value);
}