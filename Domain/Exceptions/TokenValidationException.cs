

using Domain.ErrorModel;
using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class TokenValidationException : AppException
{
    public TokenValidationException(string message) : base(new ErrorDetails()
    {
        StatusCode = StatusCodes.Status406NotAcceptable,
        Message = message
    })
    {
        
    }
}
