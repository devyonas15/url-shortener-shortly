using System.Diagnostics.CodeAnalysis;
using Application;
using HealthChecks.UI.Client;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Serilog;
using UrlShortenerApi.Configurations;
using UrlShortenerApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add options services to the container by using IOption Pattern.
builder.Services.AddOptionsConfiguration(builder.Configuration);


// Add Authorization and Authentication
builder.Services.AddAuthConfiguration();

// Add Health check configuration
builder.Services.AddHealthCheckConfiguration();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddPersistenceServices();
builder.Services.AddLogging();

// Add serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

// Add Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("all", corsPolicyBuilder => corsPolicyBuilder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("all");
app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

// Add Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Register the health endpoint
app.MapHealthChecks(
    "api/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}