using System.Text.Json.Serialization;

namespace Shared.Core.Dtos;

public enum ResultStatusCode
{
    Success = 200,
    Created = 201,
    Accepted = 202,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    Conflict = 409,
    ValidationError = 422,
    InternalServerError = 500
}


public  record Result
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonNumberEnumConverter<ResultStatusCode>))]
    public ResultStatusCode StatusCode { get; set; }
    public List<string>? Errors { get; set; }

    public static Result Success(string message)
    {
        return new Result
        {
            IsSuccess = true,
            Message = message,
            StatusCode = ResultStatusCode.Success
        };
    }
    public static Result Created(string message = "Created")
    {
        return new Result()
        {
            IsSuccess = true,
            Message = message,
            StatusCode = ResultStatusCode.Created
        };
    }

    public static Result Accepted(string message = "Accepted")
    {
        return new Result()
        {
            IsSuccess = true,
            Message = message,
            StatusCode = ResultStatusCode.Accepted
        };
    }


    public static Result Failure(string message, ResultStatusCode statusCode, List<string>? errors = null)
    {
        return new Result()
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode,
            Errors = errors
        };
    }

    public static Result ValidationFailure(IEnumerable<object> errors)
    {
        return Failure(
            $"One or more Validation error(s) occured: {string.Join("; ", errors)}",
            ResultStatusCode.ValidationError);
    }
}
public record Result<T>(T? data) : Result
{
    public static Result<T> Success(T data, string message = "Successful")
    {
        return new Result<T>(data)
        {
            IsSuccess = true,
            Message = message,
            StatusCode = ResultStatusCode.Success
        };
    }

    public static Result<T> Created(T data, string message = "Created")
    {
        return new Result<T>(data)
        {
            IsSuccess = true,
            Message = message,
            StatusCode = ResultStatusCode.Created
        };
    }

    public static Result<T> Accepted(T data, string message = "Accepted")
    {
        return new Result<T>(data)
        {
            IsSuccess = true,
            Message = message,
            StatusCode = ResultStatusCode.Accepted
        };
    }

    public static Result<T> Failure(string message, ResultStatusCode statusCode, List<string>? errors = null)
    {
        return new Result<T>(null)
        {
            IsSuccess = false,
            Message = message,
            StatusCode = statusCode,
            Errors = errors
        };
    }

    public static Result<T> ValidationFailure(IEnumerable<object> errors)
    {
        return Failure(
            $"One or more Validation error(s) occured: {string.Join("; ", errors)}",
            ResultStatusCode.ValidationError);
    }
}
