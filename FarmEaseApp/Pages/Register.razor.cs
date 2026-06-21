using FarmEaseApp.Models;
using FarmEaseApp.Services.Auth;
using Microsoft.AspNetCore.Components;

namespace FarmEaseApp.Pages;

public partial class Register
{
    [Inject] protected IAuthService AuthService { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    protected RegisterRequest RegisterModel { get; set; } = new();
    protected string? GlobalMessage { get; set; }
    protected List<string>? ApiErrors { get; set; }
    protected bool IsSubmitting { get; set; }
    protected bool IsSuccess { get; set; }
    protected async Task HandleRegistration()
    {
        IsSubmitting = true;
        GlobalMessage = null;
        ApiErrors = null;

        var validator = new RegisterRequestValidator();
        var validationResult = await validator.ValidateAsync(RegisterModel);

        if (!validationResult.IsValid)
        {
            // Map validation errors directly to the UI list
            ApiErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            IsSubmitting = false;
            return;
        }

        var result = await AuthService.RegisterAsync(RegisterModel);

        if (result.IsSuccess)
        {
            IsSuccess = true;
            StateHasChanged();
            await Task.Delay(2000);
            NavigationManager.NavigateTo("/login");
        }
        else
        {
            GlobalMessage = result.Message;
            ApiErrors = result.Errors;
            IsSubmitting = false;
        }
    }
}