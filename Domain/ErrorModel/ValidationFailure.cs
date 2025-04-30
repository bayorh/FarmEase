namespace Domain.ErrorModel;

public class ValidationFailure
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
}