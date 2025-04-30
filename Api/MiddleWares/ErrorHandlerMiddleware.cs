using Domain.ErrorModel;
using Domain.Exceptions;
using FluentValidation;
using System.Net;


namespace Api.MiddleWares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;
    private readonly IWebHostEnvironment _env;
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger,IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch(ValidationException ex)
        {

            var response = context.Response;
            response.StatusCode = StatusCodes.Status400BadRequest;
            response.ContentType = "application/json";
            var errorMessage = "One or more validation errors occured";
            await response.WriteAsync(new ErrorDetails()
            {
                StatusCode = response.StatusCode,
                Message = errorMessage ,
                ValidationErrors = ex.Errors.Select(x => $" {x.ErrorMessage}").ToList()

            }.ToString());

        }
        catch(AppException ex)
        {
            _logger.LogError(ex, ex.Message);
            var response = context.Response;
            response.StatusCode = ex.ErrorDetails.StatusCode;
            response.ContentType = "application/json";
            await response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            }.ToString());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.InternalServerError; 
            response.ContentType = "application/json";
            var message = string.Empty;
            if (_env.IsDevelopment())
            {
                message = ex.Message + " " + ex.StackTrace;
            }
            else
            {
                message = "An error occured" + " " + ex.StackTrace;
            }
            await response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }

}
