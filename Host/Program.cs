using Microsoft.OpenApi.Models;
using Shared.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

    builder.Services.AddSharedInfrastructure(builder.Configuration);
   // builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" }); });


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
