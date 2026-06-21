using FarmEaseApp.AuthProviders;
using FarmEaseApp.Models;
using FarmEaseApp.Services.Auth;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace FarmEaseApp.Pages;

public partial class Login : ComponentBase
{
    [Inject] protected IAuthService AuthService { get; set; } = default!;
    [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; } = default!;
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;

    protected LoginModel LoginModel { get; set; } = new();
    protected string? ErrorMessage { get; set; }
    protected List<string>? ApiErrors { get; set; }
    protected bool IsSubmitting { get; set; }


    protected async Task HandleLogin()
    {
        IsSubmitting = true;
        ErrorMessage = null;
        ApiErrors = null;

        var validator = new LoginModelValidator();
        var validationResult = await validator.ValidateAsync(LoginModel);

        if (!validationResult.IsValid)
        {
            // Map validation errors directly to the UI list
            ApiErrors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            IsSubmitting = false;
            return;
        }

        // Process credentials through the Service Layer
        var result = await AuthService.LoginAsync(LoginModel);

        if (result.IsSuccess && result.Data?.Token != null)
        {
            // Update secure browser state with custom provider downcast pattern
            if (AuthStateProvider is AuthStateProvider customProvider)
            {
                await customProvider.MarkUserAsAuthenticated(result.Data.Token);
            }

            NavigationManager.NavigateTo("/");
        }
        else
        {
            // Bind the custom Result envelope failures to the UI tracking properties
            ErrorMessage = result.Message;
            ApiErrors = result.Errors;
            IsSubmitting = false;
        }
    }
}
