
using Domain.ErrorModel;
using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class BadRequestException(string message): AppException(new ErrorDetails { Message = message, StatusCode = StatusCodes.Status400BadRequest});
public class AlreadyExistException(string message): AppException(new ErrorDetails { Message = message, StatusCode = StatusCodes.Status400BadRequest });
public class UnauthorizedException(string message) : AppException(new ErrorDetails { Message = message, StatusCode = StatusCodes.Status401Unauthorized });