
using Api.Extensions;
using Api.MiddleWares;
using Domain.Configs;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Serilog;
namespace Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()             
               .ReadFrom.Configuration(builder.Configuration) 
               .WriteTo.Console()                             
               .WriteTo.File("C:\\Logs\\FarmEaseLog.txt")             
               .CreateLogger();
        builder.Host.UseSerilog();

        // Add services to the container.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.ConfigureSwagger();

        builder.Services.AddInfrastructure();
        builder.Services.ConfigureCors();
        builder.Services.ConfigureVersioning();
        builder.Services.AddControllers();
         

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
        builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));
        builder.Services.AddHttpContextAccessor();
        builder.Services.ConfigureAuthentication(builder.Configuration);
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "FarmEase API v1");
            });
        }

        app.UseMiddleware<ErrorHandlerMiddleware>();
        if (app.Environment.IsProduction())
        {
            app.UseHsts();
        }
        app.UseHttpsRedirection(); 
        app.UseStaticFiles();
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.All
        });
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
