
using Domain.Entities;
using Domain.ErrorModel;
using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class NotFoundException : AppException 
{
    
    public NotFoundException(string message) : base(new ErrorDetails()
    {
        StatusCode = StatusCodes.Status404NotFound,
        Message = message,
    })
    { }
}
