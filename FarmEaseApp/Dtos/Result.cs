namespace FarmEaseApp.Dtos;

public class Result : Result<object>
{
    // Utility for non-generic responses
    public static Result Success(string message = "", int statusCode = 200) => 
        new() { IsSuccess = true, Message = message, StatusCode = statusCode };
}

public class Result<T>
{
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
    public List<string>? Errors { get; set; }
    public T? Data { get; set; }
}
