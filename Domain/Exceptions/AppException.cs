using Domain.ErrorModel;

namespace Domain.Exceptions;

public class AppException: Exception
{
    
    public AppException(ErrorDetails errorDetails): base(errorDetails.Message) 
    {
        ErrorDetails = errorDetails;
    } 
    public ErrorDetails ErrorDetails { get; set; }  
}
