using CryptoQuoteApi.Application.Dtos;
using CryptoQuoteApi.Application.Interfaces;
using CryptoQuoteApi.Application.Settings;
using CryptoQuoteApi.Application.Validators;
using CryptoQuoteApi.Infrastructure.Services;
using FluentValidation;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// <-------------------- Register Application Services -------------------->

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CryptoQuote API",
        Version = "v1"
    });
});

// Configure CORS to allow all origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/crypto-quote-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Configure settings
builder.Services.Configure<ExternalApiSettings>(
    builder.Configuration.GetSection("ExternalApis"));

// Configure services
builder.Services.AddHttpClient<CoinMarketCapService>();
builder.Services.AddHttpClient<ExchangeRatesService>();
builder.Services.AddScoped<ICryptoQuoteService, CryptoQuoteService>();

// Configure validation
builder.Services.AddScoped<IValidator<CryptoQuoteRequest>, CryptoQuoteRequestValidator>();

// <-------------------- Build and Configure the App -------------------->

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoQuote API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();
