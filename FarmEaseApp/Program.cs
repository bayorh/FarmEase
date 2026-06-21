using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FarmEaseApp;
using FarmEaseApp.AuthProviders;
using Microsoft.AspNetCore.Components.Authorization;
using FarmEaseApp.Services.Auth;
using FluentValidation;
using FarmEaseApp.Models;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5125")
});
builder.Services.AddScoped<IAuthService, AuthService>();
// Register explicitly to avoid underlying Reflection Scanner crashes completely
builder.Services.AddScoped<IValidator<LoginModel>, LoginModelValidator>();
builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterRequestValidator>();

await builder.Build().RunAsync();
