using AspNetCoreRateLimit;
using CryptoQuoteApi.Api.Middlewares;
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

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

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

// Configure caching
builder.Services.AddMemoryCache();
builder.Services.Configure<CacheSettings>(
    builder.Configuration.GetSection("Caching"));
builder.Services.AddScoped<ICacheService, CacheService>();

// Configure services
builder.Services.AddHttpClient<CoinMarketCapService>();
builder.Services.AddHttpClient<ExchangeRatesService>();
builder.Services.AddScoped<ICryptoQuoteService, CryptoQuoteService>();

// Configure validation
builder.Services.AddScoped<IValidator<CryptoQuoteRequest>, CryptoQuoteRequestValidator>();

// Configure rate limiting
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddInMemoryRateLimiting();

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
// Add error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

app.Run();
