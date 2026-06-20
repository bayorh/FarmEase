using System.Net;
using System.Net.Http.Json;
using FarmEaseApp.AuthProviders;
using FarmEaseApp.Dtos;
using FarmEaseApp.Models;
using Microsoft.AspNetCore.Components;

namespace FarmEaseApp.Pages;

public partial class Login(HttpClient httpClient) : ComponentBase
{
    private LoginModel loginModel = new();
    private string? errorMessage;
    private bool isSubmitting = false;

    private async Task HandleLogin()
    {
        isSubmitting = true;
        errorMessage = null;

        // 1. Call your API to validate credentials & get the token
        try
        {
            // 1. Post credentials to your backend API
            var response = await httpClient.PostAsJsonAsync("api/Users/login", loginModel);
            if (response.IsSuccessStatusCode)
            {
                // 2. Extract the JWT token from the successful response
                var result = await response.Content.ReadFromJsonAsync<Result<LoginDto>>();

                if (result != null && !string.IsNullOrEmpty(result.Data.Token))
                {
                    // Update the custom AuthenticationStateProvider 
                    // This handles storage and notifies the cascading state UI
                    if (AuthStateProvider is AuthStateProvider authProvider)
                    {
                        await authProvider.MarkUserAsAuthenticated(result.Data.Token);
                    }

                    // Redirect to home or return URL
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    errorMessage = "Invalid server response. Please try again.";
                }
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                errorMessage = "Invalid email or password.";
            }
            else
            {
                errorMessage = "An error occurred on the server. Please try again later.";
            }
        }
        catch (HttpRequestException)
        {
            errorMessage = "Unable to connect to the server. Check your network connection.";
        }
        catch (Exception ex)
        {
            errorMessage = $"An unexpected error occurred: {ex.Message}";
        }
        finally
        {
            isSubmitting = false;
        }
    }
}