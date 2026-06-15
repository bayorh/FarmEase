
using Modules.Identities.Extensions;
using Scalar.AspNetCore;
using Shared.Core.Configs;
using Shared.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Log.Logger = new LoggerConfiguration()
//            .ReadFrom.Configuration(builder.Configuration)
//            .WriteTo.Console()
//            .WriteTo.File("C:\\Logs\\FarmEaseLog.txt")
//            .CreateLogger();

//builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddSharedInfrastructure(builder.Configuration);
builder.Services.AddIdentitiesModule(builder.Configuration);
builder.Services.AddOpenApi();
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();             
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
