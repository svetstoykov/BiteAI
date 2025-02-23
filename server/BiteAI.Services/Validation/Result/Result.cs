using BiteAI.Services.Validation.Errors;

namespace BiteAI.Services.Validation.Result;

public class Result
{
    private readonly List<OperationError> _errors = new();
        
    public bool IsSuccess => this._errors.Count == 0;
    
    public bool IsFailure => !this.IsSuccess;
    
    public IReadOnlyList<OperationError> Errors => this._errors.AsReadOnly();
    
    public string? SuccessMessage { get; protected set; }

    protected Result(string? message = null)
    {
        this.SuccessMessage = message;
    }

    private void AddError(OperationError operationError) => this._errors.Add(operationError);

    public static Result Success(string? message = null) => new(message);

    public static Result<T> Success<T>(T value, string? message = null) => new(value, message);

    public static Result Fail(OperationError operationError)
    {
        var result = new Result();
        result.AddError(operationError);
        return result;
    }
    
    public static Result<T> Fail<T>(OperationError operationError)
    {
        var result = new Result<T>(default);
        result.AddError(operationError);
        return result;
    }
    
    public static Result<T> ErrorFromResult<T>(Result input)
    {
        var result = new Result<T>(default);

        foreach (var error in input.Errors) 
            result.AddError(error);
        
        return result;
    }
    
    public static Result ErrorFromResult(Result input)
    {
        var result = new Result();

        foreach (var error in input.Errors) 
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